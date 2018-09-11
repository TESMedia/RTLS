using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Common
{
    public static class CommonHasher
    {
        public static string Encrypt(string toEncrypt, bool useHashing)
        {

            byte[] resultArray = null;
            if (!string.IsNullOrEmpty(toEncrypt))
            {
                byte[] keyArray;
                byte[] toEncryptArray = Encoding.ASCII.GetBytes(toEncrypt);

                System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                // Get the key from config file

                string key = (string)settingsReader.GetValue("SecurityKey",
                                                                 typeof(String));
                //System.Windows.Forms.MessageBox.Show(key);
                //If hashing use get hashcode regards to your key
                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(Encoding.ASCII.GetBytes(key));
                    //Always release the resources and flush data
                    // of the Cryptographic service provide. Best Practice

                    hashmd5.Clear();
                }
                else
                    keyArray = Encoding.ASCII.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes.
                //We choose ECB(Electronic code Book)
                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)

                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                //transform the specified region of bytes array to resultArray
                resultArray =
                  cTransform.TransformFinalBlock(toEncryptArray, 0,
                  toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor
                tdes.Clear();
                //Return the encrypted data into unreadable string format
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            return null;
        }

        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            if (!string.IsNullOrEmpty(cipherString))
            {
                //get the byte code of the string

                byte[] toEncryptArray = Convert.FromBase64String(cipherString);

                System.Configuration.AppSettingsReader settingsReader =
                                                    new AppSettingsReader();
                //Get your key from config file to open the lock!
                string key = (string)settingsReader.GetValue("SecurityKey",
                                                             typeof(String));

                if (useHashing)
                {
                    //if hashing was used get the hash code with regards to your key
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(Encoding.ASCII.GetBytes(key));
                    //release any resource held by the MD5CryptoServiceProvider

                    hashmd5.Clear();
                }
                else
                {
                    //if hashing was not implemented get the byte code of the key
                    keyArray = Encoding.ASCII.GetBytes(key);
                }

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes. 
                //We choose ECB(Electronic code Book)

                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                                     toEncryptArray, 0, toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor                
                tdes.Clear();
                //return the Clear decrypted TEXT
                string res= Encoding.UTF8.GetString(resultArray);
                return res;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static bool IsBase64(this string base64String)
        {
            if (base64String == null || base64String.Length == 0 || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }

            catch (Exception exception)
            {
                // Handle the exception
            }
            return false;
        }
    }
}
