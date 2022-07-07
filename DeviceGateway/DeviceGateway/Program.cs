using System;
using System.Threading;

namespace DeviceGateway
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = new CancellationTokenSource();
            var token = source.Token;
            string _exitMessage = string.Empty;

                DeviceGateway deviceGateway = new DeviceGateway(ChannelProvider.getInboundChannels(), 
                                                                ChannelProvider.getOutboundChannels(), 
                                                                DeviceGWConfiguration.Deserialize("Config.xml").DWGName ,
                                                                30);
                // deviceGateway.doSyncronisation();
                var task = deviceGateway.Synchronize(token);//.Wait();            
                while (_exitMessage != "EXIT")
                {
                    Console.WriteLine("Enter EXIT to stop the program");
                    _exitMessage = Console.ReadLine();
                }
                source.Cancel();            
        }
    }
}
