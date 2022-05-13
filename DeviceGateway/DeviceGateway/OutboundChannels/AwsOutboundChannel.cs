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

        public void sendData(List<JObject> toSend)
        {
            foreach (JObject oneMessage in toSend)
            {
                topic = oneMessage.GetValue("Topic").ToString();
                oneMessage.Remove("Topic");
                oneMessage.Remove("Status");
                mqttClient.Publish(topic, Encoding.UTF8.GetBytes($"{oneMessage}"));
                Console.WriteLine("Topic:"+ topic+ " - sent Data:" + oneMessage);
            }
            
        }
    }
}
