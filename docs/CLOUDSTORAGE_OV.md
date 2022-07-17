<div id="top"></div>

<br />

# Details AWS Cloud Services

Before going deeper into the subject, here is an overview picture of the structure. Complementary to the picture in the Getting Started chapter. In this chapter, however, we will only talk about the AWS cloud services.

![IoT SensorBase][UbersichtAWS]


## IOT Core
[Here is the link to AWS IOT Core](https://aws.amazon.com/iot-core/?nc1=h_ls)

AWS IOT Core serves as the MQTT broker. It has the task of distributing the incoming messages. This works with the publish/subscribe model. Rules are used to describe what should happen with the incoming messages. 

In our case the rule is very simple. It says that all entries in the message are pushed into the database. With the function "Split message into multiple columns of a DynamoDB table (DynamoDBv2)" we make sure that the messages are stored in the individual columns of the table. More about this in the DynamoDB section. 

Our rule is: 
```csharp
SELECT SensorData,SensorName,SensorType,SensorTimestamp,SensorUnit, cast(topic(2) AS String) as DeviceID, timestamp() as databaseTimestamp FROM 'device/+/data'
```

Let's consider the JASON file sent over the MQTT protocol from the gateway. 
```csharp
{
    "SensorData": 1557,
    "SensorName": "MH-Z19C",
    "SensorType": "CO2",
    "SensorTimestamp": 1658016804.1572983,
    "SensorUnit": "ppm",
    "DeviceID": "10000000ffa294b5",
    "DatenbankTimestamp": 1658016826060
}
```
You can see that in our Rule we dim the "DeviceID" as MQTT Topic and also we create an Timestamp for the time we recived the message. The timestamp SensorTimestamp is the time, the sensor measured. If you want to send more data you can adapt the Gateway and then adapt the rule. 

For example you want to send the GPS data from the IOT-Device and store the data in the DynamoDB. So you have to adapt the message you send. Add e.g. GPSData = 'here are data'.

The JASON file would then look like this: 

```csharp
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
```

To store the data in the DynamoDB you have to select the GPSDATA in the rule. It would look like this: 

```csharp
SELECT GPSData, SensorData,SensorName,SensorType,SensorTimestamp,SensorUnit, cast(topic(2) AS String) as DeviceID, timestamp() as databaseTimestamp FROM 'device/+/data'
```

Once you selected all the data you need, you have to define what to do with the data. To do this aws IOT Core uses actions. In our case we use the action "Split message into multiple columns of a DynamoDB table (DynamoDBv2)". The DynamoDBv2 action allows you to write all or part of an MQTT message to a DynamoDB table. Each attribute in the payload is written to a separate column in the DynamoDB database. Messages processed by this action must be in the JSON format. But there are even other actions and you can write your own action with Lamdafunctions.

For more information look here: 
https://docs.aws.amazon.com/iot/latest/developerguide/iot-ddb-rule.html
https://docs.aws.amazon.com/iot/latest/developerguide/iot-rule-actions.html

## DynamoDB

The creation of DynamoDB is very fast and simple. There is not very much to set. Important here is the SortKey and the Partition key and Sort key. Similar to other database systems, DynamoDB stores data in tables. DynamoDB tables are schemaless—other than the primary key, you do not need to define any extra attributes or data types when you create a table.

Partition key
The partition key is part of the table's primary key. It is a hash value that is used to retrieve items from your table and allocate data across hosts for scalability and availability.

Sort key - optional
You can use a sort key as the second part of a table's primary key. The sort key allows you to sort or search among all items sharing the same partition key.

In our case you maybe will geht a warning by running our system. The waring is: 
ApplicationInsights/ApplicationInsights-Test/AWS/DynamoDB/ConsumedWriteCapacityUnits/IOTSensorBase2/

This is because we send all the data on specific points. DynamoDB has auto scaling mode. Amazon DynamoDB auto scaling uses the AWS Application Auto Scaling service to dynamically adjust provisioned throughput capacity on your behalf, in response to actual traffic patterns. The peaks in data transmission then exceed the expected data transmission. To avoid the error, turn off this auto scaling mode. 

For more information look here: 
https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Introduction.html

## CloudWatch

Amazon CloudWatch monitors your Amazon Web Services (AWS) resources and the applications you run on AWS in real time. You can use CloudWatch to collect and track metrics, which are variables you can measure for your resources and applications.
CloudWatch is very suitable to check the incoming data and thus also evaluate the created rule in IOT Core. However, monitoring and storing all data becomes relatively expensive in the long run. So don't forget to deactivate the continuous monitoring of messages after the test phase. During operation, it is usually sufficient to trace only error messages.

For more information look here: 
https://docs.aws.amazon.com/AmazonCloudWatch/latest/monitoring/WhatIsCloudWatch.html




[UbersichtAWS]: images/UbersichtAWS.PNG


