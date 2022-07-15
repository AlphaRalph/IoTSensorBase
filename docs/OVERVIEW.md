<div id="top"></div>

<br />

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>    
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
	    <li><a href="#HardwareSelection">Selection of suitable hardware</a></li>
        <li><a href="#Prerequisites">Prerequisites</a></li>
		<li><a href="#InstallSensors">Install Sensors</a></li>
		<li><a href="#MicroServices">Communication between MicroServices with Sensors</a></li>
		<li><a href="#MongoDB">Setup MongoDB</a></li>
		<li><a href="#DeviceGateway">Configuration of DeviceGateway</a></li>
		<li><a href="#CloudServices">Configuration of AWS-cloud services</a></li>
      </ul>
    </li>
  </ol>
</details>


<!-- Getting Started Guide - Overview -->
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

<p align="right">(<a href="#top">back to top</a>)</p>

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
To run the IOT sensor base, we rely on AWS cloud services. Specifically, the following services are required to build the IOT Sensor Base: 
aws IOT-CORE<br />
aws DynamoDB<br />
aws CloudWatch<br />

First, you need to create an account in Amazon AWS Cloud. 
[https://portal.aws.amazon.com/billing/signup?nc2=h_ct&src=header_signup&refid=c25dd0aa-ac63-4039-9735-8633c6c683f6&redirect_url=https%3A%2F%2Faws.amazon.com%2Fregistration-confirmation&language=de_de#/start/email]


<p align="right">(<a href="#top">back to top</a>)</p>


### InstallSensors

@Andrej: please tell about your experience about installation of sensors

<p align="right">(<a href="#top">back to top</a>)</p>

### MicroServices

@Andrej: please tell about your experience about communication with sensors

<p align="right">(<a href="#top">back to top</a>)</p>

### MongoDB

<p align="right">(<a href="#top">back to top</a>)</p>

### DeviceGateway

You will find a config.xml in your binaries folder.<br />

Devicegateway needs at least one inbound and one outbound channel to run.<br />
To read data from Mongo-Db and send to AWS-cloud the configuration could look like this:<br />

![example configuration][config-image]

In order to connect to MQTT-broker you need some certificates.<br />
The certificates has to be located in the binaries folder.

Device certificate<br />
Device public key <br />
Device private key <br />
Root certificate <br />

You get this certificates by creating a new "THING" in AWS. Look here for the detail walkthrough.

@Andrej: could you add some information how to start the DeviceGateway.exe on raspberry.

Once both connections are established DeviceGateway will start to transfer data.

<p align="right">(<a href="#top">back to top</a>)</p>

### CloudServices
We start the process by creating a new Thing. Go to IOT Core. Click on "Manage" and then on "Things". To start the creation click on "Create a new Thing". 
For the IOTSensorBase we create a "SingleThing".

![IoT SensorBase][CreateNewThing1]

Type in a name of the "Thing" and click on "next". We don't need to set up the additional settings and don't need an device shadow. 

![IoT SensorBase][CreateNewThing2]

To get the right certificates please click "Auto-generate a new certificte (recommended)"
Then create a new policy like the following sample and attach it to the "Thing". This policy allows each device to do everything. If you want restrictions, specify this in the policy. 

![IoT SensorBase][policy1]

Then click on "create thing". IMPORTANT: A window will appear instructing you to download the certificates. Download all the certificates according to the image and click on "Done". The "Thing" is sucessfully created!

![IoT SensorBase][Zertifikate]


<p align="right">(<a href="#top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[Architecture]: images/IoT-SensorBase.png
[config-image]: images/config.PNG
[CreateNewThing1]: images/CreateNewThing1.PNG
[CreateNewThing2]: images/CreateNewThing2.PNG
[policy1]: images/policy1.png
[Zertifikate]:images/Zertifikate.png
