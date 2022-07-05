<div id="top"></div>

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]


<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/AlphaRalph/IoTSensorBase">
    <img src="images/screenshot_1.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">IOT Sensor Base</h3>

  <p align="center">
    A project at FH-Wels.
    <br />
    <a href="https://github.com/AlphaRalph/IoTSensorBase"><strong>Explore the docs »</strong></a>
    <br />
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#about-the-project">About The Project</a></li>
	<li><a href="#discussion">Discussion</a></li>
    <li><a href="#getting-started">Getting Started</a></li>    
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project

![IoT SensorBase][product-screenshot]

The aim is to develop a sensor platform, which is independent of the measured variables, records data and sends it to a cloud service.
In cloud-services this data is analysed and visualised.

Project scope:
* Familiarisation with current IoT technologies
* Creation of a universal software architecture
* Realisation of a Showcase, e.g."Environmental measurement"

Fields of activity:
* Softwaredevelopment/programming with .NET, Python
* Database and SQL
* Cloud Services
* Sensors
* Communication ( WIFI,LTE,etc...)

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- discussion -->
## Discussion

What did we discuss about before the project started.

<a href="docs/DISCUSSION.md">Discussions</a> done at project start.

<!-- Getting Started -->
## Getting Started

<a href="docs/OVERVIEW.md">Getting Started Guide</a> includes a description of how to get the project run.

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- Roadmap -->
## Roadmap

In our project we identified four main topics:

* **reading data from various sensors and write data to database**
- [x] implement template for micro service 
	- [x] write data to MongoDB at localhost
	- [x] acquire temperature from sensor
	- [x] acquire CO2 level from sensor
- [x] autostart micro services at startup
- [ ] ...

* **establish a connection to cloud and send data in correct format**
- [x] implement a DeviceGateway to connect to source DB and MQTT-Broker
- [ ] handle KEY Files for authentification and authorisation
	- [x] creation of KEY Files is done manually
	- [ ] fully authomated handling of new devices
- [x] retriev data from the source DB cyclically
- [x] forward data to configured outbound channel
- [x] IoT-topic can be configured ( could be a serial number )
- [x] outbound channes sends all data one by one 
	- [ ] outbound channel can be configured to send all data in a bulk
	- [ ] outbound channel can be configured to compress data
- [x] multiple channes are configurable
- [x] inbound data is sent to all outbound channels
- [x] after exception connections are restored

* **store received data in cloud database**
- [ ] how to handle message from MQTT broker
- [ ] which database should be used
- [ ] how to store in Database

* **visualize data**
- [ ] what tool can be used for visualisation
- [ ] what database can be used as source for visualisation
- [ ] how to realise dashboards

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
* 


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


<p align="right">(<a href="#top">back to top</a>)</p>


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/alphaRalph/IoTSensorBase.svg?style=for-the-badge
[contributors-url]: https://github.com/alphaRalph/IoTSensorBase/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/alphaRalph/IoTSensorBase.svg?style=for-the-badge
[forks-url]: https://github.com/alphaRalph/IoTSensorBase/network/members
[stars-shield]: https://img.shields.io/github/stars/alphaRalph/IoTSensorBase.svg?style=for-the-badge
[stars-url]: https://github.com/alphaRalph/IoTSensorBase/stargazers
[issues-shield]: https://img.shields.io/github/issues/alphaRalph/IoTSensorBase.svg?style=for-the-badge
[issues-url]: https://github.com/alphaRalph/IoTSensorBase/issues
[license-shield]: https://img.shields.io/github/license/alphaRalph/IoTSensorBase.svg?style=for-the-badge
[license-url]: https://github.com/alphaRalph/IoTSensorBase/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/alphaRalph
[product-screenshot]: images/screenshot_1.png
[dwg-image]:images/DWG.png
