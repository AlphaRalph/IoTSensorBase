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
using MongoDB.Driver.Core.Clusters;
using System.Threading;

namespace DeviceGateway.InboundChannels
{
    class MongoDBInboundChannel : MongoClient, IInboundChannel
    {
        IMongoDatabase db;
        public string connectionData { get; set; }                

        /**
         * empty constructor needed to create instance by typename
         */
        public MongoDBInboundChannel() { }
        public MongoDBInboundChannel(string connectionString) :base(new MongoUrl(connectionString))
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
            db =  GetDatabase(dbname);
            Console.WriteLine("DB : {0} connected", dbname);
        }

        public List<JObject> getInboundData()
        {
            int iHelper = 0;            
            var allDoc = db.GetCollection<BsonDocument>("Values");
            var filter = Builders<BsonDocument>.Filter.Eq("Status", 1);
            var documents = allDoc.Find(filter).ToList();
            List<JObject> lResults = new List<JObject>();
            foreach (BsonDocument doc in documents)
            {
                // save the document ID before removing from BSON
                string docId = doc.GetValue("_id").ToString();
                doc.Remove("_id");
                // add original ID to JSON                
                JObject newJsonDoc = JObject.Parse(doc.ToJson());
                newJsonDoc.Add("oriId", docId);
                lResults.Add(newJsonDoc);
                if (iHelper > 10 )
                {
                    iHelper = 0;
                    break;
                }
                else iHelper++;             
            }
            return lResults;
        }

        public void deleteData(object oToDelete)
        {
            Console.WriteLine("not implemented now");
        }
        
        public void updateDataToDone(JObject oToUpdate)
        {
            var allDoc = db.GetCollection<BsonDocument>("Values");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse((string)oToUpdate.GetValue("oriId")));
            var update = Builders<BsonDocument>.Update.Set("Status", 2);
            allDoc.UpdateMany(filter, update);
        }        
    }
}
