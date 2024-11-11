using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Extend
{
    public class DLQG_Huyen_Result
    {
        public string message { get; set; }
        public string Code { get; set; }
        public DLQG_Huyen_data data { get; set; }
    }
    public class DLQG_Huyen_data
    { 
        public List<DLQG_Huyen> listdataView { get; set; }

    }
    public class DLQG_Huyen
    {
        public uint id { get; set; }
        public string categorycode { get; set; }
        public string dataDetail { get; set; }
    }

    public class DLQG_Huyen_Detail
    {
        public int Masocaptinh { get; set; }
        public int MaSoCapQuanHuyenThiXa { get; set; }
        public string TenDonViHanhChinh { get; set; }
    }
}
