<div id="top"></div>

<br />

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#Data Acquisition">Data Acquisition</a></li>
	<li><a href="#Data Transportation">Data Transportation</a></li>
    <li><a href="#Cloud Storage">Store data in cloud</a></li>    
	<li><a href="#Cloud Processing">Process data in cloud</a></li>   
  </ol>
</details>

## Data Acquisition
To make the solution expandable micro services shall be used to acquire data from sensor. Every sensor gets its own micro service. There are two tasks for these micro services:
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
  
## Cloud Storage

Various measurement data received via MQTT must be stored in a database.
The database is designed to accommodate all data regardless of sensor type. In this solution we store the data from a temperature/humidity sensor and a CO2 sensor. 

In this solution, the data is not only written to a database, but the MQTT broker also sends the data to the CloudWatch at the same time to monitor all sent data at the beginning to avoid erroneous data in the cloud. Do not forget to disable sending all data to the CloudWatch (high cost).

Find more information <a href="CLOUDSTORAGE_OV.md">here</a>.

<p align="right">(<a href="#top">back to top</a>)</p>

## Cloud Processing

In order to visualize the measured data, which is located in the AWS DynamoDB, Grafana was used in this project. 

Find more information <a href="CLOUDPROCESSING_OV.md">here</a>.

<p align="right">(<a href="#top">back to top</a>)</p>
