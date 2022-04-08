using DeviceGateway.InboundChannels;
using DeviceGateway.Config;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DeviceGateway
{
    class Program
    {
        static List<IInboundChannel> inboundChannels = new List<IInboundChannel>{};

        static void Main(string[] args)        
        {
            Console.WriteLine("starting Gateway...");
            // read Config-File
            DeviceGWConfiguration config = DeviceGWConfiguration.Deserialize("config.xml");                        

            foreach ( InboundChannelConfiguration channelConf in config.InboundChannelConfigurations )
            {
                Console.WriteLine("adding new Channel:" + channelConf.ChannelName);
                Console.WriteLine("Connectiondata    :" + channelConf.ConnectionString+"\n");
                IInboundChannel newChannel = (IInboundChannel)Activator.CreateInstance(Type.GetType(channelConf.ChannelName));
                newChannel.doConnect(channelConf.ConnectionString);                
                inboundChannels.Add(newChannel);
            }
            
            // create channels defined in config

            while (true)
            {
                Console.WriteLine("read data from inbound-Channel");
                foreach (IInboundChannel inChannel in inboundChannels)
                {
                    List<string> result = inChannel.getInboundData();
                    foreach (string jo in result)
                    {
                        Console.WriteLine("Data got from Database:" + jo);
                    }
                }
                Console.WriteLine("send data to outbound-Channel");
                
                Thread.Sleep(5000);
            }
            
        }
    }
}
