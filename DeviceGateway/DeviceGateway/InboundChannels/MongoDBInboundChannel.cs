using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace DeviceGateway.InboundChannels
{
    class MongoDBInboundChannel : IInboundChannel
    {        
        IMongoDatabase db;
        public string connectionData { get; set; }

        /**
         * empty constructor needed to create i9nstance by typename
         */
        public MongoDBInboundChannel() { }
        public MongoDBInboundChannel(string connectionString)
        {
            connectionData = connectionString;
            doConnect(connectionString);
        }        

        /**
         * method needed to connect channel after creation by empty constructor
         */
        public void doConnect(string ConnectionString)
        {
            var mongoUrl = new MongoUrl(ConnectionString);
            var dbname = mongoUrl.DatabaseName;
            db = new MongoClient(mongoUrl).GetDatabase(dbname);
        }
    
        public List<string> getInboundData()
        {
            var allDoc = db.GetCollection<BsonDocument>("Values");
            var documents = allDoc.Find(new BsonDocument()).ToList();
            List<string> lResults = new List<string>();
            foreach (BsonDocument doc in documents)
            {
                Console.WriteLine(doc.ToJson() );
                lResults.Add(doc.ToJson()) ;
            }
            return lResults;
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
