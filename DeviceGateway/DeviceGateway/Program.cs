using DeviceGateway.InboundChannels;
using DeviceGateway.OutboundChannels;
using DeviceGateway.Config;
using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace DeviceGateway
{
    class Program
    {
        static List<IInboundChannel> inboundChannels = new List<IInboundChannel> { };
        static List<IOutboundChannel> outboundChannels = new List<IOutboundChannel> { };

        static void Main(string[] args)
        {
            Console.WriteLine("starting Gateway...");
            // read Config-File
            DeviceGWConfiguration config = DeviceGWConfiguration.Deserialize("config.xml");

            /**
             * Create Inbound Channels defined in config-File
             * connect channel after creation
             */
            #region handleInboundChannelConfig
            foreach (InboundChannelConfiguration channelConf in config.InboundChannelConfigurations)
            {
                Console.WriteLine("adding new Channel:" + channelConf.ChannelName);
                Console.WriteLine("Connectiondata    :" + channelConf.ConnectionString + "\n");
                IInboundChannel newChannel = (IInboundChannel)Activator.CreateInstance(Type.GetType(channelConf.ChannelName));
                newChannel.doConnect(channelConf.ConnectionString);
                inboundChannels.Add(newChannel);
            }
            #endregion

            /**
             * Create Outbound Channels defined in config-File
             * connect channel after creation
             */
            #region handleOutboundChannelConfig
            foreach (OutboundChannelConfiguration channelConf in config.OutboundChannelConfigurations)
            {
                Console.WriteLine("adding new Channel:" + channelConf.ChannelName);
                Console.WriteLine("Connectiondata    :" + channelConf.ConnectionString + "\n");
                IOutboundChannel newChannel = (IOutboundChannel)Activator.CreateInstance(Type.GetType(channelConf.ChannelName));
                try
                {
                    newChannel.doConnect(channelConf.ConnectionString);
                    outboundChannels.Add(newChannel);
                }
                catch (Exception e)
                {
                    Console.WriteLine("UIJE:" + e.Message);
                    Console.ReadLine();
                }

            }
            #endregion

            /**
             * only when there is minimum one inbound and one outbound channel
             * we start the loop
             */
            while (inboundChannels.Count > 0 && outboundChannels.Count > 0)
            {
                Console.WriteLine("read data from inbound-Channel");
                List<JObject> result = new List<JObject>();

                /**
                 * collect inbound-data from various channels             
                 */
                foreach (IInboundChannel inChannel in inboundChannels)
                {
                    result.AddRange(inChannel.getInboundData());
                    foreach (JObject jo in result)
                    {
                        Console.WriteLine("Data got from Database:" + jo);
                    }
                }


                foreach ( JObject message in result)
                {
                    message.Add("Topic", config.DWGName );
                }

                foreach (IOutboundChannel outChannel in outboundChannels)
                {
                    outChannel.sendData(result);
                    Console.WriteLine("got all Data");
                }

                Thread.Sleep(5000);
            }

        }
    }
}
