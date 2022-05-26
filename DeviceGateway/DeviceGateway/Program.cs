using System;
using System.Threading;

namespace DeviceGateway
{
    class Program
    {
        static void Main(string[] args)
        {
            DeviceGateway deviceGateway = new DeviceGateway();
            deviceGateway.doSyncronisation();
        }
    }
}
