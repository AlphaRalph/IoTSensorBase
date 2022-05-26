using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceGateway.OutboundChannels
{
    public class SentDataEventArgs : EventArgs
    {
        public bool IsSuccessful { get; set; }
        public JObject SentData { get; set; }
    }
}
