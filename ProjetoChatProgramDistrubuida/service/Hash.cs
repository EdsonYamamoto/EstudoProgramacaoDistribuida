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

namespace ProjetoChatProgramDistrubuida.service
{
    class Hash
    {

        public static void Teste()
        {


            string str = "0000000000000000e067a478024addfecdc93628978aa52d91fabd4292982a501234569876";

            Console.WriteLine(GetHashString(str));


            Bloco b = new Bloco();
            //b.hashAnterior = "00000004fa2ebde0680a0434362269685583b246878644b1e6075b4f69f1d5db";
            //b.qtdZ = 6;

            b.hashAnterior = "00000004fa2ebde0680a0434362269685583b246878644b1e6075b4f69f1d5db";
            b.qtdZ = 6;

            Regex regex = new Regex("^[0]+$");

            for (int timestamp = 1; timestamp < 2000000000; timestamp++)
            {
                for (int nonce = 1; nonce < 2000000000; nonce++)
                {

                    if (regex.IsMatch(GetHashString((b.hashAnterior + nonce.ToString() + timestamp.ToString())).Substring(0, b.qtdZ)))
                    {
                        b.nonce = nonce.ToString();
                        b.timestamp = timestamp.ToString();
                        break;
                    }
                }
            }
            Console.WriteLine("nonce: "+b.nonce+ " timestamp: "+b.timestamp);
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
