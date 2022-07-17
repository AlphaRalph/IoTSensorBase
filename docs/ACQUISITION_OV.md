<details>
<summary>Table of Contents</summary>   
<li><a href="#Layout">Layout</a></li>
<li><a href="#MongoDB">MongoDB</a></li>
<li><a href="#Combination Of All Services">Combination Of All Services</a></li>
<li><a href="#Autostart">Autostart</a></li>
<li><a href="#Troubleshotting">Troubleshotting</a></li>
</details>

## Layout
Once everything is set up and tested, the individual components can be connected. The assignment of the individual pins remains the same as in the <a href="OVERVIEW.md">Install Sensor</a> chapter. Thus, the overall layout of the hardware looks like this:
![IoT SensorBase/docs][LayoutGesamt]

<p align="right">(<a href="#top">back to top</a>)</p>

## MongoDB
For the connection between MongoDB and the Python script there is the following possibility:
```python
from pymongo import MongoClient # library for MongoDB

database = MongoClient('mongodb://127.0.0.1:27017/')['measuredValues'] # connect to MongoDB
collection = database['Values'] # transmit data to collection Values

while True:
    list = [
        {data1}, {data2},{data3}
    ] # list format for MongoDB

    collection.insert_many(list) # send data to MongoDB
```
It is important that the value `measuredValues` is named the same as the value in the `config.xml` file of the device gateway. If this is not the case, the device gateway cannot establish a connection and thus a data transfer to the cloud is not possible. For comparison, here you can see the line in the `config.xml` file:
```csharp
<ConnectionString>mongodb:/localhost:27017/measuredValues</ConnectionString>
```

<p align="right">(<a href="#top">back to top</a>)</p>

## Combination Of All Services
If you now combine all the parts that you have worked out so far, you get a Python file that combines all the functions, except for the device gateway, and thus represents the master document.
```python
import mh_z19 # library for CO2 sensor
import adafruit_dht # library for temp and humidity sensor
import board # library for pins on board
import json # library for json strings
import time 
from pymongo import MongoClient # library for MongoDB

dhtDevice = adafruit_dht.DHT22(board.D4, use_pulseio=False) # read data on pin 4

timestamp = "SensorTimestamp"
name = "SensorName"
typ = "SensorType"
data = "SensorData"
sensorTemp = "DHT22_Temperature"
sensorHum = "DHT22_Humidity"
sensorCO2 = "MH-Z19C"
typeTemp = "Temperature"
typeHum = "Humidity"
typeCO2 = "CO2"
SensorUnit = "SensorUnit"
Status = "Status"

database = MongoClient('mongodb://127.0.0.1:27017/')['measuredValues'] # connect to MongoDB
collection = database['Values'] # transmit data to collection Values

while True:
    try:
        dataTemp = dhtDevice.temperature # read temperature
        dataHum = dhtDevice.humidity # read humidity
        dataCO2 = json.dumps(mh_z19.read()).split() # sensor returns a dict format, converting to JSON and split it into a list
        dataCO2 = dataCO2[1] # use first element in list
        dataCO2 = float(dataCO2[:-1]) # delete last letter in string, convert it to float

    except RuntimeError as error:
        # errors happen fairly often, DHT's are hard to read, just keep going
        print(error.args[0])
        time.sleep(5.0)
        continue
    except Exception as error:
        dhtDevice.exit()
        raise error

    time.sleep(5.0) # maximum time for DHT22 = 2.0

    list = [
        {   #data format for temperature
            timestamp: time.time(), 
            name: sensorTemp,
            typ: typeTemp,
            data: dataTemp,
            SensorUnit: "C", 
            Status: 1
        },
        {   # data format for humidity
            timestamp: time.time(), 
            name: sensorHum,
            typ: typeHum,
            data: dataHum,
            SensorUnit: "%", 
            Status: 1
        },
        {   # data format for CO2 concentration
            timestamp: time.time(),
            name: sensorCO2,
            typ: typeCO2,
            data: dataCO2,
            SensorUnit: "ppm", 
            Status: 1
        }
    ] # list format for MongoDB

    collection.insert_many(list) # send data to MongoDB
```
The following functions are covered chronologically:
* Establish connection with the local database
* Read in sensor data
* Process sensor data and prepare for database
* Write sensor data to database

<p align="right">(<a href="#top">back to top</a>)</p>

## Autostart 
To enable "headless" operation, it is necessary that the programs are executed automatically. Thus, the programs are executed immediately after switching on the power supply and thus enable a completely periphery-less operation of the Raspberry Pi.

### MongoDB
To start MongoDB automatically when the Raspberry Pi is turned on, you only need a few commands:
```sh
sudo service mongodb start
sudo service mongodb status
sudo systemctl enable mongodb
```

### Service Combination
To start the combination of services automatically when the Raspberry Pi is turned on, you only need a few commands:
```sh
sudo crontab -e
```
This opens the program cron, which is already stored in the autostart. At the bottom of the program add the following line:
```sh
@reboot /usr/bin/python /home/pi/iot.py &
```
With `CTRL+O`, `Enter` and `CTRL+X` you save and exit the editor and end up in the terminal again. It is important that on the one hand the absolute path of the interpreter and on the other hand the absolute path of the program to be executed are specified. The `&` at the end puts the process in the background and thus prevents incorrect behavior or enables subsequent programs to be executed correctly. After that, the file must be defined executable.
```sh
sudo chmod +x /home/pi/iot.py
```

### Device Gateway
To start the device gateway automatically when the Raspberry Pi is turned on, you will need a little script for the execution:
```sh
sudo nano gatewayStart.sh
```
In the script you now write the things that should be executed at startup. A general guide to scripts can be found [here](https://www.elektronik-kompendium.de/sites/raspberry-pi/2006091.htm). Since the file to be executed is not in the general home directory, but one level below it, we must first change the directory in the file and then use a utility to execute the `.exe` file. In addition, the correct "shebang" must be inserted, which depends on the interpreter. In our case we use `bash` and the file looks like this:
```bash
#!/bin/bash
cd DeviceGateway
mono DeviceGateway.exe
```
With `CTRL+O`, `Enter` and `CTRL+X` you save and exit the editor and end up in the terminal again. Afterwards it is necessary to add another `crontab`.
```sh
sudo crontab -e
```
This opens the program cron, which is already stored in the autostart. At the bottom of the program add the following line:
```sh
@reboot /bin/bash /home/pi/gatewayStart.sh
```
With `CTRL+O`, `Enter` and `CTRL+X` you save and exit the editor and end up in the terminal again. If you now restart the Raspberry Pi, you can see in the database that every few seconds values are read in and written to the database. Every 30 seconds this data is sent from the device gateway to the cloud and its status is changed from one to two.

<p align="right">(<a href="#top">back to top</a>)</p>

## Troubleshooting
In the course of the project, problems arose again and again, which could only be solved in a time-consuming manner. Some of these problems are listed here and should serve a faster problem solution.

### Error Data Reading
While reading in the data, there were repeated errors in the values of temperature and humidity. The solution was that the sampling time was set higher and the maximum time for the DHT22 is two seconds. Anything less than two seconds does not provide meaningful or erroneous values.

### Short Circuit MH-Z19
The CO2 sensor kept shorting out when the supply voltage was plugged in, forcing the Raspberry Pi into current limiting. The solution to this was to not use just any GND connection, but the one directly under the 5V connection. Unfortunately, there is no reasonable explanation for this phenomenon.

### No Internet Connection
When assigning the static IP address, it happened in the beginning that the Raspberry Pi is addressable via the network, but internet-dependent functions like updating the package sources were no longer available. The problem was that the DNS server could not resolve the IP address correctly and therefore there was no internet connection. The solution was a change in the file `/etc/resolv.conf` where the resolve server had to be set.
```sh
sudo nano /etc/resolv.conf
```
Change the nameserver to the following:
```sh
# Generated by resolvconf
nameserver 8.8.4.4
nameserver 8.8.8.8
```
With `CTRL+O`, `Enter` and `CTRL+X` you save and exit the editor and end up in the terminal again.

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[LayoutGesamt]: images/LayoutGesamt.png