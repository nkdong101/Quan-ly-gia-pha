using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.DataAccess.MongoDB
{
    public class MongoDBContext
    {
        private MongoClient vClient;
        private IMongoDatabase DB;
        public MongoDBContext(MongoClient iClient = null)
        {
            if (vClient == null)
            {
                if (iClient != null)
                    vClient = iClient;
                else

                    vClient = new MongoClient(string.Format("{0}{1}", Global.MongoConnectionString, Global.DatabaseName));
            }
            if (DB == null)
                DB = vClient.GetDatabase(Global.DatabaseName);
        }

        public MongoDBContext(string MongoConnectionString, string DatabaseName)
        {
            if (vClient == null)
                vClient = new MongoClient(string.Format("{0}{1}", MongoConnectionString, DatabaseName));
            if (DB == null)
                DB = vClient.GetDatabase(DatabaseName);
        }
        public MongoClient GetClient()
        {
            return vClient;
        } 
        
        public IMongoDatabase GetDB()
        {
            return DB;
        }
        public IMongoCollection<T> DbSet<T>() where T : Objects.ObjectBase
        {
            var table = typeof(T).Name;
            try
            {
                if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
                    BsonClassMap.RegisterClassMap<T>(cm =>
                    {
                        cm.AutoMap();
                        cm.SetIgnoreExtraElements(true);
                        //cm.GetMemberMap(p => p.DateCreate).SetDefaultValue(DateTime.Now);
                    });
            }
            catch (Exception)
            {

            }



            return DB.GetCollection<T>(table);
        }
    }
}
