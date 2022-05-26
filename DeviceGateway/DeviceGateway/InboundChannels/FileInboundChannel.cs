using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DeviceGateway.InboundChannels
{
    class FileInboundChannel : IInboundChannel
    {        
        public string filePathName { get; set; }

        /**
         * empty constructor needed to create i9nstance by typename
         */
        public FileInboundChannel() { }
        public FileInboundChannel(string fileName)
        {
            filePathName = fileName;
        }        

        /**
         * implementation needed for Interface
         * no connection needed for File
         */
        public void doConnect(string ConnectionString)
        {
        }
    
        public List<JObject> getInboundData()
        {            
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)+ "\\InboundChannels\\TestFile\\JSonTestData.txt";            
            string[] lines = File.ReadAllLines(path);
            List<JObject> lResults = new List<JObject>();
            foreach (string oneLine in lines)
            {
                Console.WriteLine(oneLine);
                lResults.Add(JObject.Parse(oneLine));
            }
            return lResults;
        }

        public void deleteData(object oToDelete)
        {
            Console.WriteLine("not implemented now");
        }

        public void updateDataToDone(JObject oToUpdate)
        {
            Console.WriteLine("not implemented now");
        }
    }
}
