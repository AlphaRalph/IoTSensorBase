<div id="top"></div>

<br />

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#Acquisition">Data Acquisition</a></li>
	<li><a href="#Transportation">Data Transportation</a></li>
		<ul>
	        <li><a href="#Interfaces">Interfaces</a></li>
	        <li><a href="#ChannelProvider">ChannelProvider</a></li>
	        <li><a href="#MongoDBInboundChannel">Mongo Inbound Channel</a></li>
	        <li><a href="#AwsOutboundChannel">AwsOutboundChannel</a></li>
	   </ul>
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

DataSource could be
* MongoDB
* Text-File
* TCP-Port
* and much more

Receiver could be
* MQTT
* Database
* File
* and much more

For each source or receiver type you need to implement a Channel!

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

The first version includes an inbound channel for <a href="#MongoDBInboundChannel">Mongo-DB</a>,
and one outbound channel for <a href="#AwsOutboundChannel">AWS-Cloud</a>

<p align="right">(<a href="#top">back to top</a>)</p>

### MongoDBInboundChannel

Be aware of the following topics:
* The configuration contains the connection-string to connect to.
* The connection-string is case sensitiv!
* This database must contain a Collection 'Values'! ( case sensitiv again )
* In the first version, this collectionname is hard coded.

To get data from collection the following code is executed:

```csharp
	var allDoc = db.GetCollection<BsonDocument>("Values");
	var filter = Builders<BsonDocument>.Filter.Eq("Status", 1);
	var documents = allDoc.Find(filter).ToList();
```
This means all documents from collection "Values" are read, but those with "Status" not equal 1 are skipped.
The returned documents have the type:
```csharp
	List<BsonDocument>
```
These documents can easily be converted into a JSON document.
But for this step you have to remove the original _id property. I got exeptions when converting this property into JSON.
The oriId is used later when updating the set of data in SourceDB.

```csharp
	// save the document ID before removing from BSON
	string docId = doc.GetValue("_id").ToString();
	doc.Remove("_id");
	// add original ID to JSON                
	JObject newJsonDoc = JObject.Parse(doc.ToJson());
	newJsonDoc.Add("oriId", docId);
```

<p align="right">(<a href="#top">back to top</a>)</p>

### AwsOutboundChannel

Be aware of the following topics:
* for X509Certificate we require a file called AmazonRootCA1.crt in binaries folder
* for X509Certificate2 we require a file called certificate.cert.pfx in binaries folder
* to create the MQTT Client we use this protocol: MqttSslProtocols.TLSv1_2
* documents are sent one by one , without compression


When sending documents in a first step data not necessary in cloud are removed from JSON-document
```csharp
	private void removeOrgData(JObject oneMessageToSend)
	{
	    oneMessageToSend.Remove("IOT-Topic");
	    oneMessageToSend.Remove("Status");
	    oneMessageToSend.Remove("oriId");
	}
```

IoT-topic is removed from dokument and stored in local variable because it is used at mqttClient:
```csharp
    topic = oneMessage.GetValue("IOT-Topic").ToString();
	// remove fields not needed in cloud
    removeOrgData(oneMessage);
	mqttClient.Publish(topic, Encoding.UTF8.GetBytes($"{oneMessage}"));
```
After sending without exception we raise an event to inform IInboundChannels 
```csharp
	SentDataEventArgs sentDataEventArgs = new SentDataEventArgs();
	var sentDataID = new JObject();
	sentDataID.Add("oriId", sOriId);
	sentDataEventArgs.SentData = sentDataID;
	sentDataEventArgs.IsSuccessful = true;
	onSendCompleted(sentDataEventArgs);
```
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