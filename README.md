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
    <li>
      <a href="#about-the-project">About The Project</a>      
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
	    <li><a href="#HardwareSelection">Selection of suitable hardware</a></li>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#discussion">Discussion</a></li>
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

<!-- Getting Started -->
## Getting Started

### HardwareSelection

Since a Raspberry was already provided by the FH-Wels, there was no need to look for an alternative here.

The selection of sensors and LTE modul was done by Andrej Pervan

<p align="right">(<a href="#top">back to top</a>)</p>

### Prerequisites

MongoDB 5.0 under Raspberry Pi OS (64-bit) 
[https://andyfelong.com/2021/08/mongodb-4-4-under-raspberry-pi-os-64-bit-raspbian64/]

To run the C# .NET Code built on Visual Studio 2017 we used MONO:
   ```sh
   sudo apt install mono-complete
   ```

### Installation

How to install IOTSensorBase and get KEY for AWS-Cloud

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- discussion -->
## Discussion

### How to read data from sensor

In the first discussion a decision should be made on how the sensors should be implemented.
The choice was between an implementation directly in the C# gateway or as a microservice outside of the gateway programming.

* Using a C# implementation inside the gateway solution should avoid persistence of measured values on Raspberry-Pi.
* Implementing a micro-service for each sensor would make the solutuion more flexible.
  Micro services can be implemented in any programming language.

We have decided to implement the sensors as a microService.
We defined to program the microservices in Python as one team member has experience with this programming language.


### How to transfer data from MicroService to Cloud - Step 1

For the purpose of data security, measured values must be persistently buffered.
This works easiest in a lean database.
At the beginning of the discussion, we wanted to use MariaDB.
It can be used with simple SQL statements. 
Advantages of this database are 
* Easy installation on Raspberry
* Easy to use in C# code as there are many well-documented application examples.

However, it is an SQL database, which does not bring any advantage with the simplicity of our data.
We do not need any relations between sets of data.
For this reason, we went looking for a NO-SQL database.
In combination with Raspberry-Pi we decided to use a MONGO-DB.
While using MONGO-DB we got aware of some difficuties.
* Old Mongo DBs no longer work with the current C# drivers.
* new Mongo DBs cannot be easily installed on the latest Raspberry OS versions
* retrieving data from Mongo-DB does not work with the well known syntax of SQL-Selects.

### How to transfer data from MicroService to Cloud - Step 2

A gateway has to be implemented to establish the connection between sensor implementation and the cloud.
In order to remain as flexible as possible in the future several inbound and outbound channels should be parameterisable.

An Inbound-channel has to be able to do the following:
* connect to a datasource
* read values from datasource
* deliver data to a gateway
* mark data as processed when the gateway has completed the process

An Outbound-channel has to be able to do the following:
* connect to a cloud-service
* send data
* inform gateway about process-state



<p align="right">(<a href="#top">back to top</a>)</p>

<!-- Roadmap -->
## Roadmap

In our project we identified four main topics:

* reading data from various sensors and write data to database
- [x] implement template for micro service 
	- [x] write data to MongoDB at localhost
	- [x] acquire temperature from sensor
	- [x] acquire CO2 level from sensor
- [x] autostart micro services at startup
- [] 

* establish a connection to cloud and send data in correct format
- [x] implement a DeviceGateway to connect to source DB and MQTT-Broker
- [] handle KEY Files for authentification and authorisation
	- [x] creation of KEY Files is done manually
	- [] fully authomated handling of new devices
- [x] retriev data from the source DB cyclically
- [x] forward data to configured outbound channel
- [x] IoT-topic can be configured ( could be a serial number )
- [x] outbound channes sends all data one by one 
	- [] outbound channel can be configured to send all data in a bulk
	- [] outbound channel can be configured to compress data
- [x] multiple channes are configurable
- [x] inbound data is sent to all outbound channels
- [x] after exception connections are restored

* store received data in cloud database
- [] how to handle message from MQTT broker
- [] which database should be used
- [] how to store in Database

* visualize data
- [] what tool can be used for visualisation
- [] what database can be used as source for visualisation
- [] how to realise dashboards



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

Devicegateway needs at least one inbound and one outbound channel to run.
At config.xml this could look like this:
![example configuration][config-image]

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

Measurment data has to be visualized.
@Hatidza0612 Please insert the result of your research here.

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
[config-image]:images/config.png