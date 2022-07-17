<div id="top"></div>

<br />

# Details AWS Cloud Services

Before going deeper into the subject, here is an overview picture of the structure. Complementary to the picture in the Getting Started chapter. In this chapter, however, we will only talk about the AWS cloud services.

![IoT SensorBase][UbersichtAWS]


## IOT Core
[Here is the link to AWS IOT Core](https://aws.amazon.com/iot-core/?nc1=h_ls)

AWS IOT Core serves as the MQTT broker. It has the task of distributing the incoming messages. This works with the publish/subscribe model. Rules are used to describe what should happen with the incoming messages. 

In our case the rule is very simple. It says that all entries in the message are pushed into the database. With the function "Split blabbal" we make sure that the messages are stored in the individual columns of the table. More about this in the DynamoDB section. 

Our rule is: 
SELECT SensorData,SensorName,SensorType,SensorTimestamp,SensorUnit, cast(topic(2) AS String) as DeviceID, timestamp() as databaseTimestamp FROM 'device/+/data'

Let's consider the JASON file sent over the MQTT protocol from the gateway. 

{
    "SensorData": 1557,
    "SensorName": "MH-Z19C",
    "SensorType": "CO2",
    "SensorTimestamp": 1658016804.1572983,
    "SensorUnit": "ppm",
    "DeviceID": "10000000ffa294b5",
    "DatenbankTimestamp": 1658016826060
}

You can see that in our Rule we dim the "DeviceID" as MQTT Topic and also we create an Timestamp for the time we recived the message. The timestamp SensorTimestamp is the time, the sensor measured. If you want to send more data you can adapt the Gateway and then adapt the rule. 

For example you want to send the GPS data from the IOT-Device and store the data in the DynamoDB. So you have to adapt the message you send. Add e.g. GPSData = 'here are data'.

The JASON file would then look like this: 

{
    "GPSData": 'here are data',
    "SensorData": 1557,
    "SensorName": "MH-Z19C",
    "SensorType": "CO2",
    "SensorTimestamp": 1658016804.1572983,
    "SensorUnit": "ppm",
    "DeviceID": "10000000ffa294b5",
    "DatenbankTimestamp": 1658016826060
}

To store the data in the DynamoDB you have to select the GPSDATA in the rule. It would look like this: 
SELECT GPSData, SensorData,SensorName,SensorType,SensorTimestamp,SensorUnit, cast(topic(2) AS String) as DeviceID, timestamp() as databaseTimestamp FROM 'device/+/data'



[UbersichtAWS]: images/UbersichtAWS.PNG


