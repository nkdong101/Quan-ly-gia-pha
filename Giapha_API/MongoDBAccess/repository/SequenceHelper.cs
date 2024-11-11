using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBAccess.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.DataAccess.MongoDB
{
    /// <summary>
    /// Lấy số thứ tự tự tăng
    /// </summary>
    public class SequenceHelper : ModelGenericRepository<Sequence>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUserId"></param>
        /// <param name="iClient"></param>
        public SequenceHelper(uint iUserId, MongoClient iClient = null) : base(iUserId, iClient)
        {

        }
        /// <summary>
        /// Lấy ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Int64 GetNextSequence<T>()
        {
            string vTableName = typeof(T).Name;
            bool vCheck = CollectionExists(this.DB, "Sequence");
            if (!vCheck)
            {
                this.Create(new Sequence() { Id = 1, TableName = vTableName, Value = 2 },null);
                return 1;
            }
            var ret = this.Collection.FindOneAndUpdate(p => p.TableName == vTableName, Builders<Sequence>.Update.Inc(a => a.Value, 1));
            if (ret == null)
            {
                var vMax = this.Find().SortByDescending(p => p.Id).Project(a => a.Id).FirstOrDefault();
                this.Collection.InsertOne(new Sequence() { Id = (uint)(vMax + 1), TableName = vTableName, Value = 2 });
                return 1;
            }
            return ret == null ? 0 : ret.Value;
        }
        private bool CollectionExists(IMongoDatabase database, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var options = new ListCollectionNamesOptions { Filter = filter };

            return database.ListCollectionNames(options).Any();
        }
    }
}