using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Extend
{
    /// <summary>
    /// Mối quan hệ gần nhất giữa 2 người
    /// </summary>
    public class LCA_Info
    {
        public Gia_pha Person1 { get; set; }
        public Gia_pha Person2 { get; set; }

    }
}
