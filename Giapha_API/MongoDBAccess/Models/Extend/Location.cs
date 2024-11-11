using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Extend
{
    /// <summary>
    /// Tọa độ
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Vĩ độ
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// Kinh độ
        /// </summary>
        public double Lng { get; set; }
        public string ToVSG()
        {
            return $"{this.Lat},{this.Lng}";
        }
    }
}
