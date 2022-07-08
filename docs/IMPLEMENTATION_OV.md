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

To make the solution expandable microservices are used to acquire data from sensor.
Every sensor has its own microservice.
There are two tasks for these micro-services:
* to acquire data from sensor
* write data into database

There are no restrictions about programming language for these services.
@andrejpervan - please insert some more details.

<p align="right">(<a href="#top">back to top</a>)</p>

## Transportation

The transportation of data from source to destination is handled by a C# program called DeviceGateway.
This DeviceGateway has to coordinate InboundChannels and OutboundChannels defined in a config.xml.
In case there is no config.xml when DeviceGateway starts the first time, a dummy config.xml will be created.
This should be a starting point for your own configuration.

Here we have a schema of Device Gateway:
![schema DeviceGateway][dwg-image]

### Interfaces

To make the solution flexible two interfaces are implemented
* IInboundChannel
```csharp
    public interface IInboundChannel
    {
        List<JObject> getInboundData();
        void updateDataToDone(JObject oToUpdate);
        void deleteData(object oToDelete );
        void doConnect(string ConnectionString);
    }
```

* IOutboundChannel
```csharp
    public interface IOutboundChannel
    {
        bool sendData(List<JObject> toSend);
        void doConnect(string ConnectionString);
        void onSendCompleted(EventArgs e);
        event EventHandler sentDataEventHandler;
    }
```

### ChannelProvider

The static class ChannelProvicer returns lists of channels built from config-settings. <br />
The configured channel has to implement the IInboundChannel or IOutboundChannel interface. <br />
When creating the channels doConnect is called, this means the channels are ready to use.

```csharp
	public static class ChannelProvider
	{
		public static List<IInboundChannel> getInboundChannels(){}
		public static List<IOutboundChannel> getOutboundChannels(){}
	}
```

How to create channel from config-string?

ChannelProvider calls the following line:
```csharp
	IInboundChannel newChannel = (IInboundChannel)Activator.CreateInstance(Type.GetType(channelConf.ChannelName));
```
ChannelName in config.xml has to be equal to Namespace + Classname of channel-implementation:
* example from dummy config.xml
```xml
  <OutboundChannelConfigurations>
    <OutboundChannelConfiguration>
      <ChannelName>DeviceGateway.OutboundChannels.AwsOutboundChannel</ChannelName>
      <ConnectionString>your-connection-string</ConnectionString>
    </OutboundChannelConfiguration>
  </OutboundChannelConfigurations>
```

* example for corresponding namespace and classname
```csharp
	namespace DeviceGateway.OutboundChannels
	{
		class AwsOutboundChannel : IOutboundChannel
		{
			// some code
		}
	}
```


The first version includes an inbound channel for MONGO-DB,
and one outbound channel for AWS-Cloud

<p align="right">(<a href="#top">back to top</a>)</p>

### read data from MONGO inbound-channel

Be aware of the following topics:
* The configuration contains the connection-string to connect to.
* The connection-string is case sensitiv!
* This database must contain a Collection 'Values'! ( case sensitiv again )
  In the first version, this collectionname is hard coded.

<p align="right">(<a href="#top">back to top</a>)</p>
  
## Cloudstorage

Various measurement data received via MQTT must be stored in a database.
@BergmairThomas Please insert the result of your research here.

<p align="right">(<a href="#top">back to top</a>)</p>

## CloudProcessing

In order to visualize the measured data, which is located in the AWS DynamoDB, Grafana was used in this project, which has to be installed first. 

[https://grafana.com/grafana/download/8.4.6?platform=windows]

Grafana does not have an interface to read the data from the database, so an additional plugin is needed. 

[https://github.com/TLV-PMOP/grafana-dynamodb-datasource]

For querying the data primary key values are required: a partition key attribute and a sort key attribute.
The sort key attribute is the timestamp (SensorTimestamp), the primary key value is the sensor name (SensorName). 

<p align="right">(<a href="#top">back to top</a>)</p>

[dwg-image]:images/DWG.png