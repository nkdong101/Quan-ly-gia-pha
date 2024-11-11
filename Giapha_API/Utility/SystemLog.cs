using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    /// <summary>
    /// Ghi log hệ thống
    /// </summary>
    public static class SystemLog
    {
        /// <summary>
        /// Lấy đường dẫn tới thư mục log
        /// </summary>
        /// <returns></returns>
        public static string GetPath()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string Folder = uri.Uri.LocalPath.ToUpper().Replace("BIN\\UTILITY.DLL", "LOG");
            return Folder;
        }
        /// <summary>
        /// Ghi log tới thư mục
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="iFunction"></param>
        /// <param name="iData"></param>
        public static void WriteErrorFile(string folder, MethodBase iFunction, string iData)
        {
            StreamWriter st = null;
            System.IO.FileStream fs = null;
            try
            {
                string Info = string.Format("{0}|{1}|{2}|{3}\n", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"), iFunction.DeclaringType, iFunction.Name, iData);
                string FolderLog = GetPath() + "\\" + folder;
                if (!System.IO.Directory.Exists(FolderLog))
                    System.IO.Directory.CreateDirectory(FolderLog);
                string CurentFile = string.Format("{0}\\{1}.txt", FolderLog, DateTime.Now.ToString("yyyy_MM_dd"));
                if (!System.IO.File.Exists(CurentFile))
                {
                    fs = System.IO.File.Create(CurentFile);
                    fs.Close();
                }
                st = new StreamWriter(CurentFile, true);
                st.WriteLine(Info);
                st.Flush();
                st.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (fs != null)
                    fs.Close();
                if (st != null)
                    st.Close();
            }
        }
        /// <summary>
        /// Ghi log lỗi 
        /// </summary>
        /// <param name="iFunction"></param>
        /// <param name="iData"></param>
        public static void WriteError(MethodBase iFunction, string iData)
        {
            WriteErrorFile("FUNCTION_ERR", iFunction, iData);
        }
        /// <summary>
        /// Lỗi call API
        /// </summary>
        /// <param name="iFunction"></param>
        /// <param name="iData"></param>
        public static void WriteAPI(MethodBase iFunction, string iData)
        {
            WriteErrorFile("API", iFunction, iData);
        }

    }
}
