using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace DeviceGateway.OutboundChannels
{
    public interface IOutboundChannel
    {
        void sendData(List<JObject> toSend);
        void doConnect(string ConnectionString);
    }
}
