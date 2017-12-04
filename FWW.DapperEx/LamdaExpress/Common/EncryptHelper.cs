using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace FWW.Framework.DapperEx
{
    /// <summary>
    /// 
    /// </summary>
    public class EncryptHelper
    {
        private const string key = "wlkdjsik";
        /// <summary>
        /// 
        /// </summary>
        public static string EncryKey;
        static EncryptHelper()
        {
            string encryKey=string.Empty;
            foreach (var item in key)
            {
                encryKey += (item + 1);
            }
            EncryKey = encryKey.Substring(0, 8);
        }
        public static string DecrypConnectionstring(string connectionstring)
        {
            try
            {
                string password = null;
                var keys = connectionstring.Split(';');
                foreach (var item in keys)
                {
                    if (item.IndexOf("password", StringComparison.CurrentCultureIgnoreCase) >= 0
                        || item.IndexOf("pwd", StringComparison.CurrentCultureIgnoreCase)>=0)
                    {
                        password = item.Split('=')[1];
                        var desword = DecryptDES(password, EncryKey);
                        connectionstring = connectionstring.Replace(item, item.Replace(password, desword));
                        break;
                    }
                }
                return connectionstring;
            }
            catch (Exception ex)
            {
                throw new Exception("解密失败，请使用 http://192.168.134.192:7171 加密数据库密码");
            }
        }
        /// <summary>  
        /// MD5加密不区分大小写的  
        /// </summary>  
        /// <param name="pwd">源字符串</param>  
        /// <param name="type">16位还是32位，16位就是取32位的第8到16位</param>  
        /// <returns></returns>  
        public static string Md5Encode(string pwd, int type)
        {
            byte[] result = Encoding.Default.GetBytes(pwd);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            if (type == 16)
                return BitConverter.ToString(output).Replace("-", "").ToLower().Substring(8, 16);
            else
                return BitConverter.ToString(output).Replace("-", "").ToLower();
        }

        /// <summary>  
        /// 对字符串进行SHA1加密  
        /// </summary>  
        /// <param name="Source_String">需要加密的字符串</param>  
        /// <returns>密文</returns>  
        public static string SHA1Encrypt(string Source_String)
        {
            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }

        /// <summary>  
        /// SHA256加密，不可逆转  
        /// </summary>  
        /// <param name="str">string str:被加密的字符串</param>  
        /// <returns>返回加密后的字符串</returns>  
        public static string SHA256Encrypt(string str)
        {
            System.Security.Cryptography.SHA256 s256 = new System.Security.Cryptography.SHA256Managed();
            byte[] byte1;
            byte1 = s256.ComputeHash(Encoding.Default.GetBytes(str));
            s256.Clear();
            return Convert.ToBase64String(byte1);
        }

        /// <summary>  
        /// SHA384加密，不可逆转  
        /// </summary>  
        /// <param name="str">string str:被加密的字符串</param>  
        /// <returns>返回加密后的字符串</returns>  
        public static string SHA384Encrypt(string str)
        {
            System.Security.Cryptography.SHA384 s384 = new System.Security.Cryptography.SHA384Managed();
            byte[] byte1;
            byte1 = s384.ComputeHash(Encoding.Default.GetBytes(str));
            s384.Clear();
            return Convert.ToBase64String(byte1);
        }

        /// <summary>  
        /// SHA512加密，不可逆转  
        /// </summary>  
        /// <param name="str">string str:被加密的字符串</param>  
        /// <returns>返回加密后的字符串</returns>  
        public static string SHA512Encrypt(string str)
        {
            System.Security.Cryptography.SHA512 s512 = new System.Security.Cryptography.SHA512Managed();
            byte[] byte1;
            byte1 = s512.ComputeHash(Encoding.Default.GetBytes(str));
            s512.Clear();
            return Convert.ToBase64String(byte1);
        }

        /// <summary>  
        /// DES加密  
        /// </summary>  
        /// <param name="encryptString">待加密的字符串</param>  
        /// <param name="encryptKey">加密密匙，要求为8位</param>  
        /// <returns></returns>  
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                des.Key = UTF8Encoding.UTF8.GetBytes(encryptKey);// ASCIIEncoding.ASCII.GetBytes(encryptKey);  
                des.IV = UTF8Encoding.UTF8.GetBytes(encryptKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }

                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:x2}", b);
                }
                ret.ToString();

                return ret.ToString();
            }
        }

        /// <summary>  
        /// DES解密字符串  
        /// </summary>  
        /// <param name="decryptString">待解密的字符串</param>  
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>  
        /// <returns></returns>  
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //put  the  input  string  into  the  byte  array    
            byte[] inputbytearray = new byte[decryptString.Length / 2];
            for (int x = 0; x < decryptString.Length / 2; x++)
            {
                int i = (Convert.ToInt32(decryptString.Substring(x * 2, 2), 16));
                inputbytearray[x] = (byte)i;
            }

            //建立加密对象的密钥和偏移量，此值重要，不能修改    
            des.Key = UTF8Encoding.UTF8.GetBytes(decryptKey);
            des.IV = UTF8Encoding.UTF8.GetBytes(decryptKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            //flush  the  data  through  the  crypto  stream  into  the  memory  stream    
            cs.Write(inputbytearray, 0, inputbytearray.Length);
            cs.FlushFinalBlock();

            //get  the  decrypted  data  back  from  the  memory  stream    
            //建立stringbuild对象，createdecrypt使用的是流对象，必须把解密后的文本变成流对象    
            StringBuilder ret = new StringBuilder();

            return Encoding.UTF8.GetString(ms.ToArray(), 0, ms.ToArray().Length);
        }

        /// <summary>  
        /// BASE64加密字符串  
        /// </summary>  
        /// <param name="str">源字符串</param>  
        /// <returns></returns>   
        public static string Base64Encode(string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>  
        /// BASE64解密字符串  
        /// </summary>  
        /// <param name="base64Str">Base64字串</param>  
        /// <returns></returns>   
        public static string Base64Decode(string base64Str)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64Str));
        }

    }
}
