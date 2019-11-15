using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using ProjetoChatProgramDistrubuida.model;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using RestSharp;
using System.Threading;
using System.Threading.Tasks;

namespace ProjetoChatProgramDistrubuida.service
{
    class Hash
    {

        private static string path = @"c:\temp\MyTest.txt";
        public static void Teste()
        {
            Bloco b = new Bloco();

            b.hashAnterior = "0000000000000000e067a478024addfecdc93628978aa52d91fabd4292982a50";
            b.qtdZ = 6;
            b.timestamp = "9876";
            b.nonce = "123456";

            // This text is added only once to the file.

            Regex regex = new Regex("^[0]+$");

            HashFacens facensHash = facade.FacensWebService.ReqBitcoinsWebService();
            Console.WriteLine("Hash facens: "+facensHash.hash+", "+facensHash.zeros);
            Console.WriteLine(GetHashString(facensHash.hash));
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Hash:"+facensHash.hash+" Zeros:"+facensHash.zeros);
                }
            }


            for (int timestamp = 0; timestamp < 20000000; timestamp++)
            {
                for (int nonce = 0; nonce < 20000000; nonce++)
                {
                    Task.Run(() => MineraHash(facensHash.hash, nonce.ToString(), timestamp.ToString(), Convert.ToInt32(facensHash.zeros)));

                }
            }

            //Console.WriteLine(facade.FacensWebService.ReqBitcoinsResultWebService(timestamp.ToString(), nonce.ToString()));

            //Console.WriteLine("nonce: "+b.nonce+ " timestamp: "+b.timestamp);
        }


        public static void MineraHash(string hashAnterior, string nonce, string timestamp, int qtdZeros)
        {

            Regex regex = new Regex("^[0]+$");
            string hash = GetHashString((hashAnterior + nonce.ToString() + timestamp.ToString()));
            if (regex.IsMatch(hash.Substring(0, qtdZeros)))
            {
                Console.WriteLine(timestamp + " " + nonce);
                string str = facade.FacensWebService.ReqBitcoinsResultWebService(timestamp, nonce);
                if (!str.Contains("\r\n"))
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(" Nonce:" + nonce + " Timestamp:" + timestamp + " Hash256:"+hash+ " -> "+str);
                    }

                }

            }

            return;
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }


        /// <summary>
        /// Decrypt a string.
        /// </summary>
        /// <param name="encryptedText">String to be decrypted</param>
        /// <param name="password">Password used during encryption</param>
        /// <exception cref="FormatException"></exception>
        public static string Decrypt(string encryptedText, string password)
        {
            if (encryptedText == null)
            {
                return null;
            }

            if (password == null)
            {
                password = String.Empty;
            }

            // Get the bytes of the string
            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
    }
}
