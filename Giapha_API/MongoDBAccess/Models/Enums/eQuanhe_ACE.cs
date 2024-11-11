using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Enums
{
    /// <summary>
    /// Quan hệ của anh chị em trong gia đình
    /// </summary>
    public enum eQuanhe_ACE : byte
    {
        /// <summary>
        /// Anh chị em cùng cha cùng mẹ
        /// </summary>
        ACE_Ruot = 1,
        /// <summary>
        /// Anh chị em cùng mẹ khác cha
        /// </summary>
        Cung_me = 2,
        /// <summary>
        /// Anh chị em cùng cha khác mẹ
        /// </summary>
        Cung_Cha = 3
    }
}
