  <details>
  <summary>DeviceGateway Components</summary>
  <ol>
	        <li><a href="#Interfaces">Interfaces</a></li>
	        <li><a href="#ChannelProvider">ChannelProvider</a></li>
	        <li><a href="#MongoDBInboundChannel">Mongo Inbound Channel</a></li>
	        <li><a href="#AwsOutboundChannel">AwsOutboundChannel</a></li>
	        <li><a href="#DeviceGateway">DeviceGateway</a></li>
	        <li><a href="#DeviceGatewayTest">DeviceGateway.Test</a></li>
  </ol>
</details>	

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
<p align="right">(<a href="#top">back to top</a>)</p>

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

### DeviceGateway

#### When creating a new instance of DeviceGateway you need the following:

```csharp
public DeviceGateway ( List<IInboundChannel> inboundChannels, 
					   List<IOutboundChannel> outboundChannels , string sDeviceGatewayName, int iSleeptime)
	{
		//some code
	}
```
* List of inbound Channels has to be defined in config.xml and will be created by  <a href="#ChannelProvider">ChannelProvider</a>
* List of outbound Channels has to be defined in config.xml and will be created <a href="#ChannelProvider">ChannelProvider</a>
* DeviceGatewayname is used as IoT-topic later and can be used from config.xml
* iSleeptime can be used from config.xml


#### After creating an instance of DeviceGateway the synchronisation can be started:
```csharp
var task = deviceGateway.Synchronize(token);
```
DeviceGateway will iterate through all the IInboundChannels to get all the available data.<br/>All available Inbound-Documents will be distributed to all configured IOutboundChannels.<br/>After sending a document the ***IOutboundChannel.sentDataEventHandler*** will call the connected method ***DeviceGateway.updateReceivedData***.<br/>This method calls every ***IInboundChannel.updateDataToDone(JObject oToUpdate)*** .<br />This leads to an update of documents in InboundChannel so the document can not be sent again.<br />In case of MongoDbInboundChannel the status will be set to '2'.

<p align="right">(<a href="#top">back to top</a>)</p>

### DeviceGatewayTest

For testing a DeviceGateway.Test solution was created. <br/>
When first tests should be integrated we discovered some problems.
Architecture of DeviceGateway solution was not built for testing.
So the solution has to be refactored to enable adequat testing.

<p align="right">(<a href="#top">back to top</a>)</p>

[dwg-image]:images/DWG.png