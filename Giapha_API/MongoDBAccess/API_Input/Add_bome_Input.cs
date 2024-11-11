using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.API_Input
{
    /// <summary>
    /// Thông tin khi thêm bố mẹ
    /// </summary>
    public class Add_bome_Input
    {
        /// <summary>
        /// Thông tin con
        /// </summary>
        public Models.Gia_pha Con_Info { get; set; }
        /// <summary>
        /// Thông tin bố
        /// </summary>
        public Models.Gia_pha Bo_Info { get; set; }
        /// <summary>
        /// Thông tin mẹ
        /// </summary>
        public Models.Gia_pha Me_Info { get; set; }
    }
}
