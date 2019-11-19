using ProjetoChatProgramDistrubuida.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ProjetoChatProgramDistrubuida.service
{
    public class Mineracao
    {
        //Pasta para salvar o log de registro.
        private static string path = @"c:\temp\Resultado.txt";

        public static string valor = "";

        public static int incio = 0;
        public static int fim = 40000;
        public static HashFacens hashFacens = new HashFacens();
        public static int nonce;

        public static bool finalizado = false;

        public static string Processo(long inicio, long fim, long timestamp, string hash, int zeros)
        {
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Hash:" + hash + " Zeros:" + zeros);
                }
            }

            return MineraHash(hash, timestamp.ToString(), inicio, fim, Convert.ToInt32(zeros));
        }


        public static string MineraHash(string hashAnterior, string timestamp,long inicio, long fim, int qtdZeros)
        {

            Regex regex = new Regex("^[0]+$");

            for (long nonce = inicio; nonce < fim; nonce++)
            {
                string hash = GetHashString((hashAnterior + nonce.ToString() + timestamp.ToString()));
                if (regex.IsMatch(hash.Substring(0, qtdZeros)))
                {

                    return nonce.ToString();
                }
            }
            return null;
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
