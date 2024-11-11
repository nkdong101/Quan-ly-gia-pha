using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{

    /// <summary>
    /// Đối tượng khi phân trang
    /// </summary>
    public class PagingData<T>
    {
        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public uint CurentPage { get; set; }
        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public List<T> Data { get; set; }
        /// <summary>
        /// Số bản ghi trên trang
        /// </summary>
        public uint RecordOfPage { get; set; }
        /// <summary>
        /// Tổng số trang
        /// </summary>
        public uint TotalPage { get; set; }
        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        public uint TotalRecord { get; set; }
    }
    /// <summary>
    /// Params tượng phân trang
    /// </summary>
    public class PagingDataFilter
    {
        /// <summary>
        /// Số bản ghi trên trang
        /// </summary>
        public uint iRecordOfPage { get; set; }
        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public uint iPage { get; set; }
        /// <summary>
        /// Tìm kiếm
        /// </summary>
        public string iSearchInfo { get; set; }
    }
}