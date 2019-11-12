using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using ProjetoChatProgramDistrubuida.model;
using System.Text.RegularExpressions;

namespace ProjetoChatProgramDistrubuida.service
{
    class Hash
    {

        public static void Teste()
        {
            Bloco b = new Bloco();
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
    }
}
