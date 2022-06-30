using DeviceGateway.Config;
using DeviceGateway.InboundChannels;
using DeviceGateway.OutboundChannels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceGateway
{
    public static class ChannelProvider
    {        
        public static List<IInboundChannel> getInboundChannels()
        {
            List<IInboundChannel> _inboundChannels = new List<IInboundChannel>();            
            /**
             * Create Inbound Channels defined in config-File
             */
            #region handleInboundChannelConfig
            foreach (InboundChannelConfiguration channelConf in DeviceGWConfiguration.Deserialize("Config.xml").InboundChannelConfigurations)
            {
                Console.WriteLine("adding new Channel:" + channelConf.ChannelName);
                Console.WriteLine("Connectiondata    :" + channelConf.ConnectionString + "\n");
                IInboundChannel newChannel = (IInboundChannel)Activator.CreateInstance(Type.GetType(channelConf.ChannelName));
                newChannel.doConnect(channelConf.ConnectionString);
                _inboundChannels.Add(newChannel);
            }
            #endregion

            return _inboundChannels;
        }

        public static List<IOutboundChannel> getOutboundChannels()
        {
            List<IOutboundChannel> _outboundChannels = new List<IOutboundChannel>();
            /**
             * Create Outbound Channels defined in config-File
             * connect channel after creation
             */
            #region handleOutboundChannelConfig
            foreach (OutboundChannelConfiguration channelConf in DeviceGWConfiguration.Deserialize("Config.xml").OutboundChannelConfigurations)
            {
                Console.WriteLine("adding new Channel:" + channelConf.ChannelName);
                Console.WriteLine("Connectiondata    :" + channelConf.ConnectionString + "\n");
                IOutboundChannel newChannel = (IOutboundChannel)Activator.CreateInstance(Type.GetType(channelConf.ChannelName));
                try
                {                    
                    newChannel.doConnect(channelConf.ConnectionString);
                    _outboundChannels.Add(newChannel);
                }
                catch (Exception e)
                {
                    Console.WriteLine("UIJE:" + e.Message);
                    Console.ReadLine();
                }
                
            }
            #endregion
            return _outboundChannels;
        }
    }
}
