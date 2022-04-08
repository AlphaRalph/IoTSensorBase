using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceGateway.Config
{
    [Serializable]
    public class InboundChannelConfiguration
    {
        public InboundChannelConfiguration()
        { }
        public InboundChannelConfiguration(string Name, string Connection)
        {
            ChannelName = Name;
            ConnectionString = Connection;
        }
        public string ChannelName { get; set; }
        public string ConnectionString { get; set; }
    }
}
