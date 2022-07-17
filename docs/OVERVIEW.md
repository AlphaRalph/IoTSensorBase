<div id="top"></div>

<br />

<!-- TABLE OF CONTENTS -->
<details>
<summary>Table of Contents</summary>   
<li><a href="#Getting Started Guide - Overview">Overview</a></li>
<li><a href="#Hardware Selection">Hardware Selection</a></li>
<li><a href="#Prerequisites">Prerequisites</a></li>
<li><a href="#Install Sensors">Install Sensors</a></li>
<li><a href="#Micro Services General">Micro Services General</a></li>
<li><a href="#MongoDB">MongoDB</a></li>
<li><a href="#Device Gateway">Device Gateway</a></li>
<li><a href="#Cloud Services">Cloud Services</a></li>
<li><a href="#Visualization">Visualization</a></li>
</details>


<!-- Getting Started Guide - Overview -->
## Getting Started Guide - Overview

Getting started will walk you through the process to setup the Raspberry Pi, various sensors, a device gateway, IoT communication and the visualization.

To get an idea about the architecture of this project have a look at the following picture.

![IoT SensorBase][Architecture]

Python is used to connect to various sensors and gather data.<br />
Sensor data is stored in a locally installed MongoDB.<br />

A device gateway was created in C# to connect to various inbound channels.<br />
In a first step only a MongoDbInboundChannes was created.<br />
Data read by device gateway are forwarded to an outbound channel.<br />
In this case the outbound channel connects to AWS cloud.<br />

<p align="right">(<a href="#top">back to top</a>)</p>

## Hardware Selection
Various devices and components are necessary for the elementary tasks. The complete list of components is as follows:
* [Raspberry Pi 4 Model B](https://www.raspberrypi.com/products/raspberry-pi-4-model-b/) 4GB
  * Power supply with at least 15 watts output power
  * USB-C cable for the connection between power supply and Raspberry Pi
  * Micro SD card with at least 16GB storage capacity
  * Network cable for the connection between computer and Raspberry Pi
* Sensor for temperature and humidity [DHT22](https://www.reichelt.at/at/de/entwicklerboards-temperatur-feuchtigkeitssensor-dht22-debo-dht-22-p224218.html?PROVID=2807&gclid=CjwKCAjww8mWBhABEiwAl6-2ReQO3vYPiNHrR649THn-6HLbpCPvgqA7vQPz0gEjFMpD8jH_2U874xoC2z0QAvD_BwE)
* Sensor for CO2 [MH-Z19C](https://www.reichelt.de/de/de/infrarot-co2-sensor-mh-z19c-pinleiste-rm-2-54-co2-mh-z19c-ph-p297320.html?r=1&gclid=CjwKCAjww8mWBhABEiwAl6-2RW_JbI0Y-NaGWL5Y9Eb1-wMTT53rWzYfBefaIPCa8fqqE1RBcy8AtxoCa7YQAvD_BwE)
* Various Resistors
* [T-Type breakout board set](https://www.reichelt.at/at/de/raspberry-pi-gpio-pinboard-t-typ-set-rpi-gpio-t-type-p282705.html?PROVID=2807&gclid=CjwKCAjww8mWBhABEiwAl6-2RdIoqnCXQQ9rfV1jxi7AKwFQ33lawaRIiw1MCyQWM_EOn3bcRJnlqRoCr1cQAvD_BwE)
* Computer

<p align="right">(<a href="#top">back to top</a>)</p>

## Prerequisites
Some preliminary work is necessary for the project in order to later integrate the individual components into the overall system in an executable manner.

### Installing Raspberry OS
Since it was decided in advance that a noSQL database or MongoDB will be used, it is necessary that Raspberry OS is installed in 64-bit format, otherwise an installation of MongoDB locally on the Raspberry Pi is not possible. The exact and official sequence of steps required for the installation is available and can be read [here](https://www.raspberrypi.com/documentation/computers/getting-started.html#sd-cards), so we will not go into it further in this post. After the installation, it is important that the SD card is still in the computer and that you create a file there called "SSH". Open the editor on Windows and save this empty file without file extension under the name "SSH" on the SD card. SSH (secure shell) is a network protocol, which allows the communication between computer and Raspberry Pi and thus enables a "headless" operation, where the Raspberry Pi does not need any additional displays or input devices. Finally, the SD card can be removed from the computer and inserted into the Raspberry Pi.

### First Steps with Raspberry Pi
For the first connection between laptop and Raspberry Pi, the two devices are connected via a network cable. Assuming you don't have any peripherals available, this is a simple and efficient way to work with the Raspberry Pi. Further, use a terminal program such as Windows Power Shell. Since you don't know the IP address of the newly installed Raspberry Pi, you can establish a connection with `pi@raspberry.local`. The credentials are the default values and are pi as username and raspberry as password. After that you are successfully connected to the Raspberry Pi.

Commands in the Raspberry Pi are entered in the terminal as bash scripts. The first time you should update the Raspberry Pi, so that all packages and the operating system are updated. To do this use
```sh
sudo apt-get update
sudo apt-get upgrade
```

### Network Settings
To avoid the need for a network cable every time you connect, we assign the Raspberry Pi a static IP address, which you can use to address the Raspberry Pi wirelessly on your home network. If you execute the `ipconfig` command on the Windows computer in another terminal, the default gateway of the wireless LAN adapter is output, where the trailing sequence of numbers is interesting (in this case it was 192.168.0.1). To do this, execute the following command in the terminal:
```sh
sudo nano /etc/dhcpcd.conf
```
This changes on the one hand from the home directory to the directory `etc` and on the other hand opens an editor (in this case `nano`) with which you can edit the file `dhcpcd.conf`. `sudo` is a prefix for commands that are executed with root privileges. The following lines are added at the very bottom of the document:
```sh
interface wlan0
static ip_address=192.168.0.101/24
static routers=192.168.0.1
static domain_name_servers=8.8.4.4 8.8.8.8
static domain_search=
```
The first three numbers are the numbers of the standard gateway and the last number determines a single participant. The Raspberry Pi can now be addressed with the number 101 in the home network. The other numbers will not be discussed further here, since they are not relevant for the actual project. With `CTRL+O`, `Enter` and `CTRL+X` you save and exit the editor and end up in the terminal again. The Raspberry Pi has now been assigned a static IP address, which can now be used for communication. To activate the settings, you have to restart the Raspberry Pi, but the network cable must be removed immediately after the reboot, otherwise there will be complications with the selection of the standard gateway.
```sh
sudo reboot
```
If you let the Raspberry Pi boot up in peace, you can log in again via the Windows Power Shell in the next step. This time, however, the static IP address is used again, with which the Raspberry Pi can now be addressed in the home network. The credentials are the default values and are pi as username and raspberry as password. After that you are successfully connected to the Raspberry Pi.
```sh
ssh pi@192.168.0.101
```

### Other Stuff
For the execution of the device gateway on the Raspberry Pi a utility is necessary, because `.exe` cannot be executed natively by the Raspberry Pi. Thus, the code written in C# can be executed on the Raspberry Pi.
```sh
sudo apt install mono-complete
```

To run the IoT sensor base, we rely on AWS cloud services. Specifically, the following services are required to build the IoT sensor base: 
*aws IOT-CORE
*aws DynamoDB
*aws CloudWatch

In addition an [account](https://portal.aws.amazon.com/billing/signup?nc2=h_ct&src=header_signup&refid=c25dd0aa-ac63-4039-9735-8633c6c683f6&redirect_url=https%3A%2F%2Faws.amazon.com%2Fregistration-confirmation&language=de_de#/start/email) in AWS cloud must be created. 

<p align="right">(<a href="#top">back to top</a>)</p>

## Install Sensors
In the project, three different characteristic values are recorded via two sensors and written to the local database. A temperature sensor, which records the values for temperature and humidity, and a CO2 sensor, which records the current CO2 content in the air, provide values that form the basis for the subsequent visualization.

### Temperature and Humidity Sensor
For temperature measurement and humidity measurement, the DHT22 sensor is used, which on the one hand provides accurate values and on the other hand can be purchased very cheaply. A german [manual](https://tutorials-raspberrypi.de/raspberry-pi-luftfeuchtigkeit-temperatur-messen-dht11-dht22/) was used as a basis for the use, which describes the setup and the further individual steps very precisely and in detail.

#### Required Components
* Temperature and humidity sensor [DHT22](https://www.reichelt.at/at/de/entwicklerboards-temperatur-feuchtigkeitssensor-dht22-debo-dht-22-p224218.html?PROVID=2807&gclid=CjwKCAjww8mWBhABEiwAl6-2ReQO3vYPiNHrR649THn-6HLbpCPvgqA7vQPz0gEjFMpD8jH_2U874xoC2z0QAvD_BwE)
* 10k resistor
* Breadboard and jumper cables

#### Layout
![IoT SensorBase/images][DHT22]

#### Software
To use the sensor properly, some packages must be installed first:
```sh
sudo apt-get update
sudo apt-get install build-essential python-dev-is-python2 git
```
Now the library for the sensors can be loaded. Here a prefabricated library from Adafruit, which supports different sensors, is used:
```sh
sudo pip3 install adafruit-circuitpython-dht
sudo apt-get install libgpiod2
```
All necessary software is thus installed for the sensor.

### CO2 Sensor
The CO2 sensor is many times more expensive than the DHT22, but on the one hand it can be used to determine the CO2 content in the air and on the other hand it is relatively easy to connect to the Raspberry Pi and read out the individual sensor data. Furthermore, other values such as temperature are also recorded by this sensor, but only the CO2 value is taken into account in this project. A german [manual](https://tutorials-raspberrypi.de/raspberry-pi-co2-sensor-mh-z19-tutorial/) was used as a basis for the use, which describes the setup and the further individual steps very precisely and in detail.

#### Definition CO2 Concentration
In order to be able to interpret the values of the CO2 concentration correctly, the following table shows the individual definitions of the respective concentrations.
air quality | CO2 in ppm
----------  | ----------
high | <800
medium | 800-1000
moderate  | 1000-1400
low | >1400
#### Required Components
* CO2 sensor [MH-Z19C](https://www.reichelt.de/de/de/infrarot-co2-sensor-mh-z19c-pinleiste-rm-2-54-co2-mh-z19c-ph-p297320.html?r=1&gclid=CjwKCAjww8mWBhABEiwAl6-2RW_JbI0Y-NaGWL5Y9Eb1-wMTT53rWzYfBefaIPCa8fqqE1RBcy8AtxoCa7YQAvD_BwE)
* Breadboard and jumper cables
#### Layout
![IoT SensorBase/images][MHZ19]

#### Software
To use the sensor properly, some packages must be installed first:
```sh
git clone https://github.com/UedaTakeyuki/mh-z19
cd mh-z19
```
On the one hand, this clones a project from GitHub to the Raspberry Pi, and on the other hand, it changes to `mh-z19` in the Raspberry Pi's directory. Afterwards, we now run the setup script that installs and enables the necessary packages.
```sh
./setup.sh
```
All necessary software is thus installed for the sensor.

<p align="right">(<a href="#top">back to top</a>)</p>

## Micro Services General
For the communication between Raspberry Pi and the respective sensors, small own programs were developed, which communicate independently with the sensors and thus record data. The programs are primarily used to test the functionality of the individual sensors. Nevertheless, individual components of the programs are subsequently used for the actual project. The advantage of micro services is that, regardless of the number of sensors, only small building blocks need to be generated again and again, which are then implemented back into the overall system.

### Temperature and Humidity Sensor
To check the functionality, a new program must first be created. The editor `nano` is used for this and the file extension `.py` indicates that it is a program in Python.
```sh
sudo nano ms_dht22.py
```
```python
import time
import board # library for pins on board
import adafruit_dht # library for temperature and humidity sensor

dhtDevice = adafruit_dht.DHT22(board.D17, use_pulseio=False) # read data on pin 17
while True:
    try:
        dataTemp = dhtDevice.temperature # read temperature
        dataHum = dhtDevice.humidity # read humidity
        print(dataTemp, dataHum)
    except RuntimeError as error:
        # Errors happen fairly often, DHT's are hard to read, just keep going
        print(error.args[0])
        time.sleep(5.0)
        continue
    except Exception as error:
        dhtDevice.exit()
        raise error
    time.sleep(5.0) # maximum time for DHT22 = 2.0
```
With `CTRL+O`, `Enter` and `CTRL+X` you save and exit the editor and end up in the terminal again. To start the program, the following is executed in the terminal:
```sh
sudo python ms_dht22.py
```
The values for temperature and humidity are now displayed in the terminal and read out cyclically. If you want to end the current process, you can do this with the key combination `CTRL+C`.

### CO2 Sensor
To check the functionality, a new program must first be created. The editor `nano` is used for this and the file extension `.py` indicates that it is a program in Python.
```sh
sudo nano ms_mhz19.py
```
```python
import time
import json # library for json strings
import mh_z19 # library for CO2 sensor

while True:
    try:  
        dataCO2 = json.dumps(mh_z19.read()).split() # sensor returns a dict format, converting to JSON and split it into a list
        dataCO2 = dataCO2[1] # use first element in list
        dataCO2 = float(dataCO2[:-1]) # delete last letter in string, convert it to float
        print(dataCO2)
    except:
        print(error.args[0])
        time.sleep(5.0)
        continue
    time.sleep(5.0)
```
With `CTRL+O`, `Enter` and `CTRL+X` you save and exit the editor and end up in the terminal again. To start the program, the following is executed in the terminal:
```sh
sudo python ms_mhz19.py
```
The values for CO2 concentration are now displayed in the terminal and read out cyclically. If you want to end the current process, you can do this with the key combination `CTRL+C`.

The value of the CO2 content can alternatively be read out via the ready-made Python program, which comes from the manufacturer. Thereby one uses
```sh
sudo python -m mh_z19
```
The rest of the procedure is identical to the other two measurements.

<p align="right">(<a href="#top">back to top</a>)</p>

## MongoDB
The choice between SQL or noSQL database was made in favor of the noSQL database, because the individual data have no relevant connection to each other and thus the core idea of an SQL database (dependency between individual data) is not fulfilled. For the installation of MongoDB, we used Andy Felong's [instructions](https://andyfelong.com/2021/08/mongodb-4-4-under-raspberry-pi-os-64-bit-raspbian64/), which are very detailed and accurate. Therefore, the topic will not be discussed further here.

In addition to the installation on the Raspberry Pi, it is recommended that you download and install [MongoDBCompass](https://www.mongodb.com/products/compass) on the Windows computer, since it is a graphical management program and thus the handling with the database becomes easier. After installation, select the advanced connection options when the program is open. Then select Proxy/SSH and fill in the remaining fields. The hostname is the static IP address, the port is 22, the username is pi and the password is raspberry.

Lastly, `pymongo` needs to be installed, as this package is needed for the connection between Python and MongoDB.
```sh
sudo python -m pip install pymongo
```

<p align="right">(<a href="#top">back to top</a>)</p>

## Device Gateway

You will find a config.xml in your binaries folder.<br />

Device gateway needs at least one inbound and one outbound channel to run.<br />
To read data from MongoDB and send to AWS cloud the configuration could look like this:<br />

![example configuration][config-image]

In order to connect to MQTT broker you need some certificates.<br />
The certificates has to be located in the binaries folder.

* Device certificate
* Device public key 
* Device private key
* Root certificate 

You get this certificates by creating a new "THING" in AWS. Look here for the detail walkthrough.

To use the device gateway on the Raspberry Pi, it must first be copied to the Raspberry Pi. The easiest way to do this is to use a graphical utility like `WinSCP`, because the data can be moved by drag and drop. Afterwards the folder has to be unpacked on the Raspberry Pi.
```sh
unzip DeviceGateway.zip
cd DeviceGateway/
```
Next, the serial number of the Raspberry Pi should be stored in `config.xml` so that it is also clear which participant is communicating with the cloud. Thus, the file must be opened first.
```sh
sudo nano config.xml
```
You can get the serial number with the following command in the terminal:
```sh
cat /proc/cpuinfo | grep Serial | cut -d' ' -f2
```
The serial number (xx) must now be inserted in the following line:
```csharp
<DWGName>device/xx/data</DWGName>
```
With `CTRL+O`, `Enter` and `CTRL+X` you save and exit the editor and end up in the terminal again.
The gateway can now be started via mono:
```sh
sudo mono DeviceGateway.exe
```
Once both connections are established the device gateway will start to transfer data.

<p align="right">(<a href="#top">back to top</a>)</p>

## CloudServices
First of all make sure that you have chosen the right region. In our case it is eu-central.<br />
![IoT SensorBase][PlugIn5]

### Create a thing
We start the process by creating a new Thing. Go to IOT Core. Click on "Manage" and then on "Things". To start the creation click on "Create a new Thing". 
For the IOTSensorBase we create a "SingleThing".<br />

![IoT SensorBase][CreateNewThing1]<br />

Type in a name of the "Thing" and click on "next". We don't need to set up the additional settings and don't need an device shadow. <br />

![IoT SensorBase][CreateNewThing2]<br />

To get the right certificates please click "Auto-generate a new certificte (recommended)".<br />
Then create a new policy like the following sample and attach it to the "Thing". This policy allows each device to do everything. If you want restrictions, specify this in the policy. 

![IoT SensorBase][policy1]

Then click on "create thing".<br />
IMPORTANT: <br /> A window will appear instructing you to download the certificates. Download all the certificates according to the image and click on "Done". The "Thing" is sucessfully created!

![IoT SensorBase][Zertifikate]

To enable the connection with the outputcannel from the gateway you need to take the certificates and perform the following steps: 

Device certificate<br />
This file usually ends with ".pem.crt". When you download this it will save as .txt file extension in windows. Save it in your ninary directory as 'bin\certificate.cert.pem' and make sure that it is of file type '.pem', not 'txt' or '.crt'

Device public key<br />
This file usually ends with ".pem" and is of file type ".key". Save this file as 'bin\certificate.public.key'.

Device private key<br />
This file usually ends with ".pem" and is of file type ".key". Save this file as 'bin\certificate.private.key'. Make sure that this file is referred with suffix ".key" in the code while making MQTT connection to AWS IoT.

Root certificate<br />
Save this file to 'bin\AmazonRootCA1.crt'

Converting Device Certificate from .pem to .pfx<br />
In order to establish an MQTT connection with the AWS IoT platform, the root CA certificate, the private key of the thing, and the certificate of the thing/device are needed. The .NET cryptographic APIs can understand root CA (.crt), device private key (.key) out-of-the-box. It expects the device certificate to be in the .pfx format, not the .pem format. Hence we need to convert the device certificate from .pem to .pfx.

The easiest way to do this is via an online converter. Like: https://rvssl.com/ssl-converter/ 

If you have followed all the steps correctly and the certificates are correctly placed in the binaries folder, the DeviceGateway should be able to connect to AWS IOT Core.

### Create Trace Database
Next, we create a log database to track and review incoming messages before loading them into the database. <br /> This prevents unnecessary or erroneous data in the database. 
To do this, search "Cloud Watch" - Click on "Log groups" and then click "new log group". Click on "create" <br />

![IoT SensorBase][CloudWatch]<br />

Now the LogDatebase is created. You can repeat this step to create an Log for errors. Name it "IOTError"<br />

### Create DynamoDB

Now we will create the actual database in which the data will finally be stored. To do this, search for "DynamoDB". Click on "Tabels" and then on "create Table".
Assign a name for the table. It is important that you enter in the PartationKey field the name "SensorName" with the unit "string" and the SortKey the name "SensorTimestamp" with unit "number". Make sure that there are no spelling mistakes. Also upper and lower case is important. <br />

![IoT SensorBase][DynamoDB]<br />

Then click on "create table". Now the database is ready to be filled. <br />

### Connect IOT CORE / Cloud Watch / DynamoDB
The last step in setting up the aws Cloud is to link the individual services together. This is done via the MQTT Broker (IOT Core). To do this, search for "IOT Core".  Click on "Act" and then on "Rules". To create a rule click on "create". 

At the beginning, you assign a name for the rule. The name does not matter. Next, click on "edit" at Rule query statement and add the following code:<br />

To store the data in the DynamoDB you have to select the GPSDATA in the rule. It would look like this: 

```csharp
SELECT SensorData,SensorName,SensorType,SensorTimestamp,SensorUnit, cast(topic(2) AS String) as DeviceID, timestamp() as DatenbankTimestamp FROM 'device/+/data'
```
<br />
![IoT SensorBase][Rule1]<br />

Next, select "add new Action" from "Actions". Click on "Send message data to CloudWatch logs" and select your created CloudWatch for logging.<br />
![IoT SensorBase][Rule3]<br />

For logging errors, you can either use Cloudwatch from before, or if you have created a standalone Cloudwatch, you can use this.<br />
![IoT SensorBase][Rule4]<br />

Now it would be a great time for testing. Send Data from your client and check in the CloudWatch if Data arrive. <br /><br />
Last but not least we have to connect or DynamoDB to the MQTTClient. Click on "add Action" and then on "Split message into multiple columns of a DynamoDB table (DynamoDBv2)" choose the DynamoDB we created in the list. <br />
![IoT SensorBase][Rule2]<br />

At the end it should look like this:<br />

![IoT SensorBase][Rule5]<br />

The aws Cloudservice is now set up!!<br />

## Visualization 

To visualize the measurement data, the open source platform Grafana is used. 
Download Grafana from the homepage. If necessary, see also the installation instructions. 

Get Grafana [https://grafana.com/grafana/download?platform=windows]

Make sure to download the installer and run it. 
	
Installation guides [https://www.tutorialandexample.com/grafana-tutorial

When trying to start grafana-server.exe make sure you allow the file as trusted.
To do this, open the file properties by right-clicking the file in Windows Explorer and selecting 'Properties'. On the 'General' tab of the dialog that appears, you will find a note about security in the last section. Click on the adjacent 'Allow' button.

The Grafana documentation is available at [https://grafana.com/docs/]

### Plugins 
Since there is no native support for AWS DynamoDB, a plugin is needed. 

To download the pluigin, click on 'Code', then 'Download ZIP'
[https://github.com/TLV-PMOP/grafana-dynamodb-datasource]

![IoT SensorBase][PlugIn1]

You have to move the folder 'src' from the main folder 'grafana-dynamodb-datasource-master' into the folder 'dist'. After that move the files 'module' and 'module.js.map' from the folder 'dist' into the folder 'src'.

XXX\grafana\data\plugins\grafana-dynamodb-datasource-master\dist\src should now look like this: 

![IoT SensorBase][PlugIn7]

To use this plugin, the following change must be made in custom.ini (see also [https://grafana.com/docs/grafana/latest/setup-grafana/configure-grafana/#allow_loading_unsigned_plugins]): allow_loading_unsigned_plugins = tlv-co-ltd-dynamodb-datasource

![IoT SensorBase][PlugIn6]

The Grafana server must be restarted at this point. 
Verify the plugin was installed.

![IoT SensorBase][PlugIn]

To check if the installation was done correctly, you have to log in to your Grafana account and click on Configuration Settings -> Data Sources 

![IoT SensorBase][PlugIn2]

This leads to the configuration page, where you can search for the installed plugin in the plugins tab. 

![IoT SensorBase][PlugIn3]

To add this datasource, under configuration page -> Data sources, click the button 'Add Data source' and select the added plugin. 
This leads to the configuration page where the region and API keys must be added: 

![IoT SensorBase][PlugIn4]

Since the Frankfurt region was selected for the DynamoDB database, the AWS Region field must contain: eu-central-1. 
You can find your personal private key and AccessKey ID in your AWS account. [https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/SettingUp.DynamoWebService.html]

Click on 'Save and test'. You can now start with the visualization. 

To create a new dashboard, click on 'dashboards' and 'new dashboard'.

![IoT SensorBase][PlugIn9]

To display the values of the table, the fields must be filled in: 
1. Select the dynamodb-datasource plugin
2. Under 'Table name' the desired table can be selected. 
3. The 'Time Field' contains the Sort Key - in our case 'SensorTimestamp'. 
4. $ 5.  Under 'Key condition expression' the partition key is entered - here 'SensorName'. This is used to filter the data of the respective sensor. See the example in the picture.
6. Value Fields contain the data - SensorData.
7. Finally, the time period must be selected 

![IoT SensorBase][PlugIn8]

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[Architecture]: images/IoT-SensorBase.png
[config-image]: images/config.PNG
[CreateNewThing1]: images/CreateNewThing1.PNG
[CreateNewThing2]: images/CreateNewThing2.PNG
[policy1]: images/Policy1.PNG
[Zertifikate]: images/Zertifikate.PNG
[PlugIn]: images/VerifyPlugin.PNG
[PlugIn1]: images/DownloadPlugin.PNG
[PlugIn2]: images/DataSources2.PNG
[PlugIn3]: images/ConfigurationPlugin.png
[PlugIn4]: images/DataSourcesPlugin.png
[PlugIn5]: images/AWSFrankfurt.PNG
[PlugIn6]: images/AllowUnsignedPlugins.PNG
[PlugIn7]: images/srcfolder.PNG
[PlugIn8]: images/TableFillIn.PNG
[PlugIn9]: images/NewDashboard.PNG
[CloudWatch]: images/CloudWatch.PNG
[DynamoDB]: images/DynamoDB.PNG
[Rule1]: images/Rule1.PNG
[Rule2]: images/Rule2.PNG
[Rule3]: images/Rule3.PNG
[Rule4]: images/Rule4.PNG
[Rule5]: images/Rule5.PNG
[DHT22]: images/DHT22.png
[MHZ19]: images/MHZ19.jpg
