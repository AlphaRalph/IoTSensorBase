using DeviceGateway.InboundChannels;
using DeviceGateway.OutboundChannels;
using DeviceGateway.Config;
using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DeviceGateway
{

    public class DeviceGateway
    {
        private static List<IInboundChannel> _inboundChannels = new List<IInboundChannel> { };
        private static List<IOutboundChannel> _outboundChannels = new List<IOutboundChannel> { };
        private string _sDeviceGatewayName;
        private int _iSleeptime;
        //DeviceGWConfiguration config;

        public DeviceGateway()
        {
            Console.WriteLine("starting Gateway...");
            // read Config-File
            //config = DeviceGWConfiguration.Deserialize("config.xml");
        }

        public DeviceGateway ( List<IInboundChannel> inboundChannels, List<IOutboundChannel> outboundChannels , string sDeviceGatewayName, int iSleeptime)
        {
            _inboundChannels = inboundChannels;
            _outboundChannels = outboundChannels;
            _sDeviceGatewayName = sDeviceGatewayName;
            _iSleeptime = iSleeptime;

            foreach (IOutboundChannel outbound in outboundChannels)
            {
                outbound.sentDataEventHandler += updateReceivedData;
            }
        }


        public Task Synchronize(CancellationToken token)
        {
            return Task.Run(()=>doSyncronisation(token, _iSleeptime), token);

        }

        private void doSyncronisation(CancellationToken token, int iSleeptime)
        {
            /**
             * only when there is minimum one inbound and one outbound channel
             * we start the loop
             */
            while (!token.IsCancellationRequested && _inboundChannels.Count > 0 && _outboundChannels.Count > 0)
            {
                /**
                 * first version of Error Handling
                 * after connection lost we should try to re-initialize
                 */
                try
                {

                    List<JObject> result = getInboundData();

                    if ( result.Count > 0)
                    {                    
                        /**
                         * add TOPIC to Json-String
                         * Topic represents the Raspberry ( maybe a room )
                         */                    
                        addIotTopic(result);


                        /**
                         * send all data to ALL outbound channels
                         */
                        sendAllData(result);
                    }
                }
                catch ( Exception e)
                {
                    Console.WriteLine("Error: {0}", e.ToString());
                }

                Console.WriteLine("waiting {0} seconds for next run", iSleeptime);
                //Thread.Sleep(config.sleepTime * 1000);
                try
                {
                    Task.Delay(iSleeptime * 1000, token).Wait();
                }
                catch (AggregateException ex) //TaskCanceledException
                {
                    Console.WriteLine("abgebrochen");
                }
            }
        }

        private List<JObject> getInboundData()
        {
            Console.WriteLine("read data from inbound-Channel");
            List<JObject> result = new List<JObject>();

            /**
             * collect inbound-data from various channels             
             */
            foreach (IInboundChannel inChannel in _inboundChannels)
            {
                result.AddRange(inChannel.getInboundData());
                foreach (JObject jo in result)
                {
                    Console.WriteLine("Data got from Database:" + jo);
                }
            }
            return result;
        }

        private void addIotTopic( List<JObject> toSend )
        {
            foreach (JObject message in toSend)
            {
                message.Add("IOT-Topic", _sDeviceGatewayName );
            }
        }

        private bool sendAllData(List<JObject> toSend)
        {
            bool sendOk = false;
            foreach (IOutboundChannel outChannel in _outboundChannels)
            {
                sendOk = outChannel.sendData(toSend);
                Console.WriteLine("sent all Data");
            }
            return sendOk;
        }

        private void runCleanup()
        {
            _inboundChannels = new List<IInboundChannel> { };
            _outboundChannels = new List<IOutboundChannel> { };
            Console.WriteLine("Cleanup done ...");
        }

        public static void updateReceivedData(object sender, EventArgs e)
        {
            Console.WriteLine("updateReceivedData" + ((SentDataEventArgs)e).SentData.GetValue("oriId") + " to done:" + ((SentDataEventArgs)e).IsSuccessful);
            foreach (IInboundChannel inChannel in _inboundChannels)
            {
                inChannel.updateDataToDone(((SentDataEventArgs)e).SentData);
            }
        }
    }
}
