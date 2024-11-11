using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Extend
{
    /// <summary>
    /// Tên gọi các đời
    /// </summary>
    public class LevelName
    {
        /// <summary>
        /// Giới tính
        /// </summary>
        public Enums.Gender? Gender { get; set; }
        /// <summary>
        /// Cấp độ so với người gọi
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// Tên gọi
        /// </summary>
        public string Name { get; set; }
    }
}
