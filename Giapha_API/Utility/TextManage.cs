using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class TextManage
    {
        public static string ChuHoaDau(string iInput)
        {
            if (string.IsNullOrEmpty(iInput))
                return iInput;
            string vResult = "";
            string[] vStr = iInput.Trim().Split(new char[] { ' ' });
            foreach (var Item in vStr)
                vResult += Item.Substring(0, 1).ToUpper() + Item.ToLower().Substring(1, Item.Length - 1) + " ";
            return vResult.Trim();
        }
    }
}
