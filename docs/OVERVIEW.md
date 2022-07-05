<div id="top"></div>

<br />


<!-- Getting Started, a walkthrough -->
## Getting Started Guide - Overview

Getting started will walk you through the process to setup Sensors, a Devicegateway, IoT communication and Visualisation.

To get an idea about the architecture of this project have a look at the following picture.

![IoT SensorBase][Architecture]

Python is used to connect to various sensors and gather data.
Sensordata is stored in a Mongo-DB.

A DeviceGateway was created in C# to connect to various inbound-Channels.
In a first step only a MongoDbInboundChannes was created.
Data read by DaviceGateway are forwarded to an outbound-Channel.
In this case the outbound-Channel connects to AWS-Cloud.


### HardwareSelection

Since a Raspberry was already provided by the FH-Wels, there was no need to look for an alternative here.

The selection of sensors and LTE modul was done by Andrej Pervan

<p align="right">(<a href="#top">back to top</a>)</p>


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[Architecture]: images/IoT-SensorBase.png
