using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Extend
{
    /// <summary>
    /// Thông tin phả hệ
    /// </summary>
    public class Pha_he
    {
        /// <summary>
        /// ID mẹ
        /// </summary>
        public uint? Mother_id { get; set; }
        /// <summary>
        /// ID bố
        /// </summary>
        public uint? Father_id { get; set; }
    }
}
