using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace API.Helpers
{
    public class ExampleRSA
    {

        static string publicKey = @"-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCZZlGU+e6wtjJok9bXzJ3SjWcy
T2R22Zo3HkznrjtW0HPceEEIKz4JgcZVMGRIwWDRhnZUskgBdIp6Vwe11js9ZCgN
p7T/pg9o0M2x3KbboNakOQWiB7Gz+0M9bTZzPdsURv6D+X1zAhx8O4Wek+mXELZh
obf6tk4EtrOlZFkDWQIDAQAB
-----END PUBLIC KEY-----";

        static string privateKey = @"-----BEGIN RSA PRIVATE KEY-----
MIICXAIBAAKBgQCZZlGU+e6wtjJok9bXzJ3SjWcyT2R22Zo3HkznrjtW0HPceEEI
Kz4JgcZVMGRIwWDRhnZUskgBdIp6Vwe11js9ZCgNp7T/pg9o0M2x3KbboNakOQWi
B7Gz+0M9bTZzPdsURv6D+X1zAhx8O4Wek+mXELZhobf6tk4EtrOlZFkDWQIDAQAB
AoGBAILlh4n7N418+0i3lqWwiZ+oX73Td2PfTTPpXDB6QVJUL/mad8uzso1EOuxo
jbpzoN1JwuGE++KaAqxgZUjp1MO6dAhn3uobO/ic3VZCpqT32KU3jWTtM1r1H8Y/
9SMiQ9i/5qUnWzbL+mNJTxDWzxlchzdEMc2Lxg5Y2peXvJEBAkEA34Bmd1kMZVKx
t1fQGWmChJQTUjAma8X/RmJSaPgnpnEyVJkJTyRwEZf8q/CuEjGezzeIU0zq0uQp
4X0KDoLWuQJBAK+0c/p1qKdUAnocNCcOrOTqyId+1o1NghoqoHcKrSxFp4lJFn7k
7vdONI07V/a9P0DiT1rRpmfirb4h+RJjQaECQGqZeUXPSSRCjtTdozmbo07MuXJn
2Mtqglj1qEy8n1y5fMHwxAnlXTZnAXKYts2isRYwbGsespmnjXopA1TsfrECQAIx
bGkp4ssD32wKwhPiuQG6dHpMeP0WhJMzCQGzXmn5DJhIE0HONbVcgodowDZSMmHq
8Tae+0a8q5J99g/msUECQH9ZHIrupS5j44Iw6jM4y06naS8ofC+kR/AS1dTAUQNI
8PSPqVJly7oU6rb7F4EoJdXQQ7BPQxcLb8InMOK94wU=
-----END RSA PRIVATE KEY-----";

        public static void test(string plainTextData)
        {
            try
            {
                byte[] DataToSign = Encoding.UTF8.GetBytes(plainTextData);
                string v = Encoding.UTF8.GetString(Convert.FromBase64String(privateKey.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "")));
                 



                var vPublic = RSAKeys.ImportPublicKey(publicKey);
                 
                var Signed = vPublic.SignData(DataToSign, new SHA1CryptoServiceProvider());


                var vPrivate = RSAKeys.ImportPrivateKey(privateKey);
                var vCheck = vPrivate.VerifyData(DataToSign, SHA256.Create(), Signed);
                Console.WriteLine("");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("err : " + e.StackTrace);
            }

        }
    }
}