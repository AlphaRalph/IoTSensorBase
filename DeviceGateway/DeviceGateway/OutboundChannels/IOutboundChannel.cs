using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace DeviceGateway.OutboundChannels
{
    public interface IOutboundChannel
    {
        bool sendData(List<JObject> toSend);
        void doConnect(string ConnectionString);
        void onSendCompleted(EventArgs e);
        event EventHandler sentDataEventHandler;
    }
}
