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
    
        public List<string> getInboundData()
        {            
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)+ "\\InboundChannels\\TestFile\\JSonTestData.txt";            
            string[] lines = File.ReadAllLines(path);
            return new List<string>(lines) ;
        }

        public void deleteData(object oToDelete)
        {
            Console.WriteLine("not implemented now");
        }

        public void updateDataToDone(object oToUpdate)
        {
            Console.WriteLine("not implemented now");
        }
    }
}
