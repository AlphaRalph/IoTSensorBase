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