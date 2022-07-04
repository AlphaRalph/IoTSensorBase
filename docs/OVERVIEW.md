<div id="top"></div>

<br />


<!-- ABOUT THE PROJECT -->
## About The Project
This project can be devided into three parts.
* programming Python
* programming C# <br />
  open topics: max batchsize <br />
               timestamp when data was sent
* working in AWS-Cloud

![IoT SensorBase][Architecture]

Python is used to connect to various sensors and gather data.
Sensordata is stored in a Mongo-DB.

A DeviceGateway was created in C# to connect to various inbound-Channels.
In a first step only a MongoDbInboundChannes was created.
Data read by DaviceGateway are forwarded to an outbound-Channel.
In this case the outbound-Channel connects to AWS-Cloud.



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[Architecture]: images/IoT-SensorBase.png
