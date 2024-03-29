﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DeviceGateway.InboundChannels
{
    public interface IInboundChannel
    {
        List<JObject> getInboundData();
        void updateDataToDone(JObject oToUpdate);
        void deleteData(object oToDelete );
        void doConnect(string ConnectionString);
    }
}
