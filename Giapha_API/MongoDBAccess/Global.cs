using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDBAccess.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess
{
    public class Global
    {
        public static string MongoConnectionString { get; internal set; }
        public static string DatabaseName { get; internal set; }
        /// <summary>
        /// Danh sách tên gọi theo đời đối với người gọi
        /// </summary>
        public static List<Models.Extend.LevelName> vList_LevelName = new List<Models.Extend.LevelName>()
        {
            new Models.Extend.LevelName(){Level= 5, Name="Tổ tiên" },
            new Models.Extend.LevelName(){Level= 4, Name="Kỵ" },
            new Models.Extend.LevelName(){Level= 3, Name="Cụ" },
            new Models.Extend.LevelName(){Level= 2, Name="Bà",  Gender= Models.Enums.Gender.male },
            new Models.Extend.LevelName(){Level= 2, Name="Ông", Gender= Models.Enums.Gender.female },
            new Models.Extend.LevelName(){Level= 1, Name="Bác",  Gender= Models.Enums.Gender.male},
            new Models.Extend.LevelName(){Level= 1, Name="Bá",  Gender= Models.Enums.Gender.female},
            new Models.Extend.LevelName(){Level= 1, Name="Chú",  Gender= Models.Enums.Gender.male},
            new Models.Extend.LevelName(){Level= 1, Name="Cô",  Gender= Models.Enums.Gender.female},
            new Models.Extend.LevelName(){Level= 1, Name="Dì",  Gender= Models.Enums.Gender.female},
            new Models.Extend.LevelName(){Level= 1, Name="Bố",  Gender= Models.Enums.Gender.male},
            new Models.Extend.LevelName(){Level= 1, Name="Mẹ",  Gender= Models.Enums.Gender.female},
            new Models.Extend.LevelName(){Level= 0, Name="Tôi" },
            new Models.Extend.LevelName(){Level=-1, Name="Con" },
            new Models.Extend.LevelName(){Level=-1, Name="Cháu" },
            new Models.Extend.LevelName(){Level=-2, Name="Cháu nội" },
            new Models.Extend.LevelName(){Level=-2, Name="Cháu ngoại" },
            new Models.Extend.LevelName(){Level=-3, Name="Chắt" },
            new Models.Extend.LevelName(){Level=-4, Name="Chút" },
            new Models.Extend.LevelName(){Level=-5, Name="Chít" },
        };
        private static int Count { get; set; }

        public static void Init(string iMongoConn, string iDatabase)
        {
            MongoConnectionString = iMongoConn;
            DatabaseName = iDatabase;
            BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeSerializer(DateTimeKind.Local));
        }
        /// <summary>
        /// Lấy ID theo số tự tăng của từng bảng
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static uint GetSequance<T>()
        {
            var vResult = new DataAccess.MongoDB.SequenceHelper(1).GetNextSequence<T>();
            return (uint)vResult;
        }
    }
}
