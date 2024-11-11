using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class BieuDoCotResult
    {
        /// <summary>
        /// Danh sách đối tượng
        /// </summary>
        public List<string> Head { get; set; }
        /// <summary>
        /// Dữ liệu biểu đồ
        /// </summary>
        public List<BieuDoCot> Data { get; set; }
    }
    public class BieuDoCot
    {
        /// <summary>
        /// labek
        /// </summary>
        public string valueX { get; set; }
        /// <summary>
        /// values
        /// </summary>
        public List<double> Details { get; set; }
    }


    public class BieuDoTronResult
    {
        public List<BieuDoTron> Data { get; set; }
    }

    public class BieuDoTron
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public double Value { get; set; }
    }
}