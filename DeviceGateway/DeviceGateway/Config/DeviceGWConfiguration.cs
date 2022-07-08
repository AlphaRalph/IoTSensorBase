using DeviceGateway.InboundChannels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceGateway.Config
{
    [Serializable]
    public class DeviceGWConfiguration
    {
        public int sleepTime { get; set; }
        public string DWGName { get; set; }
        public List<InboundChannelConfiguration> InboundChannelConfigurations { get; set; }
        public List<OutboundChannelConfiguration> OutboundChannelConfigurations { get; set; }

        public DeviceGWConfiguration()
        {
            InboundChannelConfigurations = new List<InboundChannelConfiguration>();
            OutboundChannelConfigurations = new List<OutboundChannelConfiguration>();
        }

        public static void Serialize(string file, DeviceGWConfiguration c)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(c.GetType());
            StreamWriter writer = File.CreateText(file);
            xs.Serialize(writer, c);
            writer.Flush();
            writer.Close();
        }
        public static DeviceGWConfiguration Deserialize(string file)
        {
            DeviceGWConfiguration config;
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(
                  typeof(DeviceGWConfiguration));
            try
            {
                StreamReader reader = File.OpenText(file);
                config = (DeviceGWConfiguration)xs.Deserialize(reader);
                reader.Close();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Konfiguration nicht gefunden - schreibe Standard");
                config = new DeviceGWConfiguration();
                config.InboundChannelConfigurations.Add(new InboundChannelConfiguration("DeviceGateway.InboundChannels.FileInboundChannel", "yasdasd"));
                config.OutboundChannelConfigurations.Add(new OutboundChannelConfiguration("DeviceGateway.OutboundChannels.AwsOutboundChannel", "enter_your_connection_data_here.amazonaws.com"));
                Serialize(file, config);
            }

            return config;
        }
    }
}
