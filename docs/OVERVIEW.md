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

Getting started will walk you through the process to setup Sensors, a Devicegateway, IoT communication and Visualization.

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

Then click on "create thing".<br />
IMPORTANT: A window will appear instructing you to download the certificates. Download all the certificates according to the image and click on "Done". The "Thing" is sucessfully created!

![IoT SensorBase][Zertifikate]

To enable the connection with the outputcannel from the gateway you need to take the certificates and perform the following steps: 

Device certificate - This file usually ends with ".pem.crt". When you download this it will save as .txt file extension in windows. Save it in your ninary directory as 'bin\certificate.cert.pem' and make sure that it is of file type '.pem', not 'txt' or '.crt'

Device public key - This file usually ends with ".pem" and is of file type ".key". Save this file as 'bin\certificate.public.key'.

Device private key - This file usually ends with ".pem" and is of file type ".key". Save this file as 'bin\certificate.private.key'. Make sure that this file is referred with suffix ".key" in the code while making MQTT connection to AWS IoT.

Root certificate - Save this file to 'bin\AmazonRootCA1.crt'

Converting Device Certificate from .pem to .pfx
In order to establish an MQTT connection with the AWS IoT platform, the root CA certificate, the private key of the thing, and the certificate of the thing/device are needed. The .NET cryptographic APIs can understand root CA (.crt), device private key (.key) out-of-the-box. It expects the device certificate to be in the .pfx format, not the .pem format. Hence we need to convert the device certificate from .pem to .pfx.

The easiest way to do this is via an online converter. Like: https://rvssl.com/ssl-converter/ 

If you have followed all the steps correctly and the certificates are correctly placed in the binaries folder, the DeviceGateway should be able to connect to AWS IOT Core.x

# Visualization 

To visualize the measurement data, the open source platform Grafana is used. 
Download Grafana from the homepage. If necessary, see also the installation instructions. 

Get Grafana [https://grafana.com/grafana/download?platform=windows]
	
Installation guides [https://www.tutorialandexample.com/grafana-tutorial
	
The Grafana documentation is available at [https://grafana.com/docs/].

### Plugins 
Since there is no native support for AWS DynamoDB, a plugin is needed. It is recommended to use grafana-cli for this. 
[https://github.com/TLV-PMOP/grafana-dynamodb-datasource]

grafana-cli --pluginUrl https://github.com/TLV-PMOP/grafana-dynamodb-datasource/dynamodb-datasource_1.0.0.zip plugins install dynamodb-datasource 

if the use of grafana-cli does not work or is not wanted, the unzipped folder can be saved here '\GrafanaLabs\grafana-8.4.6\data\plugins'

To use this plugin, the following change must be made in custom.ini (see also [https://grafana.com/docs/grafana/latest/setup-grafana/configure-grafana/#allow_loading_unsigned_plugins]): allow_loading_unsigned_plugins = dynamodb-datasource

The Grafana server must be restarted at this point. 
Verify the plugin was installed.

![IoT SensorBase][PlugIn]

To check if the installation was done correctly, you have to log in to your Grafana account and click on Configuration Settings -> Data Sources 

![IoT SensorBase][PlugIn2]
![IoT SensorBase][PlugIn32]

This leads to the configuration page, where you can search for the installed plugin in the plugins teb. 

![IoT SensorBase][PlugIn3]



<p align="right">(<a href="#top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[Architecture]: images/IoT-SensorBase.png
[config-image]: images/config.PNG
[CreateNewThing1]: images/CreateNewThing1.PNG
[CreateNewThing2]: images/CreateNewThing2.PNG
[policy1]: images/Policy1.PNG
[Zertifikate]: images/Zertifikate.PNG
[PlugIn]: images/VerifyPlugin.PNG
[PlugIn2]: images/DataSources.png
[PlugIn3]: images/ConfigurationPlugin.png
[PlugIn32]: images/DataSources2.png

