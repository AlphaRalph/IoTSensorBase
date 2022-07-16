<div id="top"></div>

<br />

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#Acquisition">Data Acquisition</a></li>
	<li><a href="#Transportation">Data Transportation</a></li>
    <li><a href="#Cloudstorage">Store data in cloud</a></li>    
	<li><a href="#CloudProcessing">Process data in cloud</a></li>   
  </ol>
</details>

## Acquisition

To make the solution expandable microservices shall be used to acquire data from sensor.
Every sensor gets its own microservice.
There are two tasks for these micro-services:
* to acquire data from sensor
* write data into database

There are no restrictions about programming language for these services.
Find more information <a href="ACQUISITION_OV.md">here</a>.

<p align="right">(<a href="#top">back to top</a>)</p>

## Transportation

The transportation of data from source to destination is handled by a C# program called DeviceGateway.
This DeviceGateway has to coordinate InboundChannels and OutboundChannels defined in a config.xml.
In case there is no config.xml when DeviceGateway starts the first time, a dummy config.xml will be created.
This should be a starting point for your own configuration.

Find more information <a href="TRANSPORTATION_OV.md">here</a>.

<p align="right">(<a href="#top">back to top</a>)</p>
  
## Cloudstorage

Various measurement data received via MQTT must be stored in a database.
The database is designed to accommodate all data regardless of sensor type. In this solution we store the data from a temperature/humidity sensor and a CO2 sensor. 

In this solution, the data is not only written to a database, but the MQTT broker also sends the data to the CloudWatch at the same time to monitor all sent data at the beginning to avoid erroneous data in the cloud. Do not forget to disable sending all data to the CloudWatch (high cost).

Translated with www.DeepL.com/Translator (free version)

Find more information <a href="CLOUDSTORAGE_OV.md">here</a>.

<p align="right">(<a href="#top">back to top</a>)</p>

## CloudProcessing

In order to visualize the measured data, which is located in the AWS DynamoDB, Grafana was used in this project, which has to be installed first. 

[https://grafana.com/grafana/download/8.4.6?platform=windows]

Grafana does not have an interface to read the data from the database, so an additional plugin is needed. 

[https://github.com/TLV-PMOP/grafana-dynamodb-datasource]

For querying the data primary key values are required: a partition key attribute and a sort key attribute.
The sort key attribute is the timestamp (SensorTimestamp), the primary key value is the sensor name (SensorName). 

Find more information <a href="CLOUDPROCESSING_OV.md">here</a>.

<p align="right">(<a href="#top">back to top</a>)</p>
