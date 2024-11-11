using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class Security
    {
        /// <summary>
        /// Mã hóa dữ liệu
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <param name="useHashing"></param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            if (toEncrypt == null)
                toEncrypt = "";
            byte[] keyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(ConfigInfo.SecretCode));
            }
            else keyArray = Encoding.UTF8.GetBytes(ConfigInfo.SecretCode);
            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }




        /// <summary>
        /// Giải mã dữ liệu đã mã hóa
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <param name="useHashing"></param>
        /// <returns></returns>
        public static string Decrypt(string toDecrypt, bool useHashing)
        {
            if (toDecrypt == null)
                toDecrypt = "";
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(ConfigInfo.SecretCode));
            }
            else keyArray = Encoding.UTF8.GetBytes(ConfigInfo.SecretCode);
            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }


        /// <summary>
        /// Mã hóa password với MD5
        /// </summary>
        /// <param name="iPassword"></param>
        /// <returns></returns>
        public static string EncryptPassword(string iPassword)
        {
            if (string.IsNullOrEmpty(iPassword))
                throw new Exception("Thông tin đầu vào không đúng");

            var passwordLength = iPassword.Length;
            var result = $"QazPlm@123{iPassword}QazPlm&123";

            for (int i = 0; i < passwordLength; i++)
            {
                result = MD5Hash(result);
            }


            return result;
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }


    }
}
