using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace YACUF.Utilities
{
    /// <summary>
    /// class with functions for saving / loading encyrpted binary-files
    /// </summary>
    public static class CryptoUtil
    {
        //  Call this function to remove the key from memory after use for security
        [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        private static extern bool ZeroMemory(IntPtr Destination, int Length);


        #region loading and saving-functions
        /// <summary>
        /// tries to load data from an encrypted binary file
        /// </summary>
        /// <typeparam name="T">expected type (must be serializable)</typeparam>
        /// <param name="filepath">absolute filepath of teh binary file</param>
        /// <param name="password">needed password</param>
        /// <param name="value">decrypted value</param>
        /// <returns>true if successfull</returns>
        public static bool TryLoad<T>(string filepath, string password, out T value)
        {
            bool success = false;

            value = default;

            GCHandle gch = GCHandle.Alloc(password, GCHandleType.Pinned);

            try
            {
                byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] salt = new byte[32];

                FileStream fsCrypt = new FileStream(filepath, FileMode.Open);
                fsCrypt.Read(salt, 0, salt.Length);

                RijndaelManaged AES = new RijndaelManaged();
                AES.KeySize = 256;
                AES.BlockSize = 128;
                var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);
                AES.Padding = PaddingMode.PKCS7;
                AES.Mode = CipherMode.CFB;

                CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);


                int read;
                byte[] buffer = new byte[1048576];
                byte[] resultArray = new byte[0];

                try
                {
                    //read byte-stream
                    while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        resultArray = resultArray.Concat(buffer).ToArray();
                    }
                }
                catch (CryptographicException ex_CryptographicException)
                {
                    Console.WriteLine("CryptographicException error: " + ex_CryptographicException.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                //close the stream
                try
                {
                    cs.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while closing CryptoStream: " + ex.Message);
                }
                finally
                {
                    fsCrypt.Close();
                }

                //convert to wanted object with specific type
                try
                {
                    value = FromByteArray<T>(resultArray);

                    success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error creating data baseed on byte-array: " + ex.Message + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading data from encrypted binary-file: " + ex.Message + ex.StackTrace);
            }


            //remove password from memory
            ZeroMemory(gch.AddrOfPinnedObject(), password.Length * 2);
            gch.Free();

            return success;
        }

        
        /// <summary>
        /// tries to save a serializable object as an encrypted binary-file
        /// </summary>
        /// <param name="filePath">absolute filepath</param>
        /// <param name="password">password for encryption</param>
        /// <param name="value">object to be saved (must be serializable)</param>
        /// <returns>true if successfull</returns>
        public static bool TrySaveToFile(string filePath, string password, object value)
        {
            bool success = false;

            if (value != null)
            {
                GCHandle gch = GCHandle.Alloc(password, GCHandleType.Pinned);

                try
                {
                    byte[] byteStream = ObjectToByteArray(value);

                    //generate random salt
                    byte[] salt = RandomNumberGenerator.GetBytes(32);

                    //create output file name
                    FileStream fsCrypt = new FileStream(filePath, FileMode.Create);

                    //convert password string to byte arrray
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                    //Set Rijndael symmetric encryption algorithm
                    RijndaelManaged AES = new RijndaelManaged();
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Padding = PaddingMode.PKCS7;

                    //repeatedly hash the user password along with the salt
                    var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    //set the Cipher mode
                    AES.Mode = CipherMode.CFB;

                    // write salt to the output file
                    fsCrypt.Write(salt, 0, salt.Length);

                    CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);

                    try
                    {
                        //write to file
                        cs.Write(byteStream, 0, byteStream.Length);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                    finally
                    {
                        cs.Close();
                        fsCrypt.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                //remove password from memory
                ZeroMemory(gch.AddrOfPinnedObject(), password.Length * 2);
                gch.Free();
            }

            return success;
        }
        #endregion

        #region convert-functions
        // <summary>
        /// converts an object into a byte-array
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// tries to convert a byte-array to the desired object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T FromByteArray<T>(byte[] data)
        {
            if (data == null)
            {
                return default(T);
            }
            else
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream(data))
                {
                    object obj = bf.Deserialize(ms);
                    return (T)obj;
                }
            }
            
        }

        #endregion
    }
}
