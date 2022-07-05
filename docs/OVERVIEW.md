<div id="top"></div>

<br />


<!-- Getting Started, a walkthrough -->
## Getting Started Guide - Overview

Getting started will walk you through the process to setup Sensors, a Devicegateway, IoT communication and Visualisation.

To get an idea about the architecture of this project have a look at the following picture.

![IoT SensorBase][Architecture]

Python is used to connect to various sensors and gather data.<br />
Sensordata is stored in a Mongo-DB.<br />

A DeviceGateway was created in C# to connect to various inbound-Channels.<br />
In a first step only a MongoDbInboundChannes was created.<br />
Data read by DaviceGateway are forwarded to an outbound-Channel.<br />
In this case the outbound-Channel connects to AWS-Cloud.<br />


### HardwareSelection

Since a Raspberry was already provided by the FH-Wels, there was no need to look for an alternative here.

The selection of sensors and LTE modul was done by Andrej Pervan

### Prerequisites

MongoDB 5.0 under Raspberry Pi OS (64-bit) 
[https://andyfelong.com/2021/08/mongodb-4-4-under-raspberry-pi-os-64-bit-raspbian64/]

To run the C# .NET Code built on Visual Studio 2017 we used MONO:
   ```sh
   sudo apt install mono-complete
   ```

@Cloud-Crew: what do we need from your side to setup this project

<p align="right">(<a href="#top">back to top</a>)</p>

### install the Sensors

@Andrej: please tell about your experience about installation of sensors

### communication between MicroServices with Sensors

@Andrej: please tell about your experience about communication with sensors

### Document in MongoDB

### configuration of DeviceGateway

You will find a config.xml in your binaries folder.<br />

Devicegateway needs at least one inbound and one outbound channel to run.<br />
To read data from Mongo-Db and send to AWS-cloud the configuration could look like this:<br />

![example configuration][config-image]

In order to connect to MQTT-broker you need some certificates.
@Bergmair Thomas: please add some information how to get these files.

Once both connections are established DeviceGateway will start to transfer data.

### configuration of AWS-cloud services
@Cloud-Crew: please tell about your work


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[Architecture]: images/IoT-SensorBase.png
[config-image]: images/config.PNG
