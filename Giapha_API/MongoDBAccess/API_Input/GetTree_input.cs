using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.API_Input
{
    public class GetTree_input
    {
        public uint iDongho_id { get; set; }
        public Models.Extend.Tree_box_Config iConfig { get; set; }
    }
}
