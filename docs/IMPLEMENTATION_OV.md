<div id="top"></div>

<br />

### Data acquisition

To make the solution expandable microservices are used to acquire data from sensor.
Every sensor has its own microservice.
There are two tasks for these micro-services:
* to acquire data from sensor
* write data into database

There are no restrictions about programming language for these services.
@andrejpervan - please insert some more details.

### Data transportation

The transportation of data from source to destination is handled by a C# program called DeviceGateway.
This DeviceGateway has to coordinate InboundChannels and OutboundChannels defined in a config.xml.
In case there is no config.xml when DeviceGateway starts the first time, a dummy config.xml will be created.
This should be a starting point for your own configuration.

Here we have a schema of Device Gateway:
![schema DeviceGateway][dwg-image]

The first version includes an inbound channel for MONGO-DB,
and one outbound channel for AWS-Cloud

#### read data from MONGO inbound-channel

Be aware of the following topics:
* The configuration contains the connection-string to connect to.
* The connection-string is case sensitiv!
* This database must contain a Collection 'Values'! ( case sensitiv again )
  In the first version, this collectionname is hard coded.
  
### Data storage in cloud

Various measurement data received via MQTT must be stored in a database.
@BergmairThomas Please insert the result of your research here.

### Data processing

In order to visualize the measured data, which is located in the AWS DynamoDB, Grafana was used in this project, which has to be installed first. 

[https://grafana.com/grafana/download/8.4.6?platform=windows]

Grafana does not have an interface to read the data from the database, so an additional plugin is needed. 

[https://github.com/TLV-PMOP/grafana-dynamodb-datasource]

For querying the data primary key values are required: a partition key attribute and a sort key attribute.
The sort key attribute is the timestamp (SensorTimestamp), the primary key value is the sensor name (SensorName). 