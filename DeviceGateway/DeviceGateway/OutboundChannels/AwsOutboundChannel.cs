using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using uPLibrary.Networking.M2Mqtt;

namespace DeviceGateway.OutboundChannels
{
    class AwsOutboundChannel : IOutboundChannel
    {        

        public string connectionData { get; set; } // "a2eowo00nh8hc6-ats.iot.eu-central-1.amazonaws.com"
        public int brokerPort = 8883;
        public event EventHandler sentDataEventHandler;
        string topic = "Test4";

        private X509Certificate caCert;
        private X509Certificate2 clientCert;
        private MqttClient mqttClient;

        /**
         * empty constructor needed to create instance by typename
         */
        public AwsOutboundChannel(){}
        public AwsOutboundChannel(string iotEndpoint)
        {
            connectionData = iotEndpoint;
            doConnect(connectionData);
        }        

        /**
         * method needed to connect channel after creation by empty constructor
         */
        public void doConnect(string ConnectionString)
        {
            connectionData = ConnectionString;
            Console.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AmazonRootCA1.crt"));
            caCert = X509Certificate.CreateFromCertFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AmazonRootCA1.crt"));
            clientCert = new X509Certificate2(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certificate.cert.pfx"), "");

            mqttClient = new MqttClient(connectionData, brokerPort, true, caCert, clientCert, MqttSslProtocols.TLSv1_2);
            string clientId = Guid.NewGuid().ToString();
            mqttClient.Connect(clientId);
            Console.WriteLine($"Connected to AWS IoT with client id: {clientId}.");
        }

        public bool sendData(List<JObject> toSend)
        {
            bool bRetVal = true;            
            try
            {
                foreach (JObject oneMessage in toSend)
                {
                    var sOriId = oneMessage.GetValue("oriId");
                    topic = oneMessage.GetValue("IOT-Topic").ToString();

                    // remove fields not needed in cloud
                    removeOrgData(oneMessage);
                    // ToDo: remove when timestamp comes from Andre
                    Int32 unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    //oneMessage.Add("TimeStamp", unixTimestamp);
                    mqttClient.Publish(topic, Encoding.UTF8.GetBytes($"{oneMessage}"));
                    Console.WriteLine("Topic:" + topic + " - sent Data:" + oneMessage);

                    SentDataEventArgs sentDataEventArgs = new SentDataEventArgs();
                    var sentDataID = new JObject();
                    sentDataID.Add("oriId", sOriId);
                    sentDataEventArgs.SentData = sentDataID;
                    sentDataEventArgs.IsSuccessful = true;
                    onSendCompleted(sentDataEventArgs);
                }
            }
            catch
            {
                bRetVal = false;
            }
            return bRetVal;
        }

        /**
         * remove Data from JObject not needed in Cloud.
         * IOT-Topic, oriId and Status are removed
         */
        private void removeOrgData(JObject oneMessageToSend)
        {
            oneMessageToSend.Remove("IOT-Topic");
            oneMessageToSend.Remove("Status");
            oneMessageToSend.Remove("oriId");
        }

        /**
         * call sentDataEventHandler 
         */
        public void onSendCompleted( EventArgs e)
        {
            sentDataEventHandler?.Invoke(this, e);
        }
    }
}
