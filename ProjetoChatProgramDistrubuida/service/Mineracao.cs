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
        public static int fim = 100000;
        public static int timestamp = 1;
        public static string hash = "0000000000000000e067a478024addfecdc93628978aa52d91fabd4292982a50";
        public static int zeros = 6;

        public static int contador = 0;

        public static bool finalizado = false;
        public static void Send()
        {
            while (true) {

                Thread.Sleep(Program.configuracao.ProcessRequestTimer);
                if (Program.Lider.IP != null) {

                    if (contador < 1)
                    {
                        IPEndPoint target = new IPEndPoint(IPAddress.Parse(Program.Lider.IP), Program.configuracao.Port);

                        byte[] message = Encoding.ASCII.GetBytes(Program.processRequest);
                        Program.socket.Send(message, message.Length, target);
                        Console.WriteLine("send>[" + Program.Lider.IP + ":" + Program.configuracao.Port.ToString() + "]:\t" + Program.processRequest);
                        contador++;
                    }
                }
            }
        }

        public static string Processo(int inicio, int fim, int timestamp, string hash, int zeros)
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


        public static string MineraHash(string hashAnterior, string timestamp,int inicio, int fim, int qtdZeros)
        {

            Regex regex = new Regex("^[0]+$");

            for (int nonce = inicio; nonce < fim; nonce++)
            {
                string hash = GetHashString((hashAnterior + nonce.ToString() + timestamp.ToString()));
                if (regex.IsMatch(hash.Substring(0, qtdZeros)))
                {
                    Console.WriteLine(timestamp + " " + nonce);
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(" Nonce:" + nonce + " Timestamp:" + timestamp + " Hash256:" + hash + " -> " + hash);
                    }
                    return nonce.ToString();
                }
            }


            return null;
        }
        /*
        public static bool Processo()
        {
            Bloco b = new Bloco();

            b.hashAnterior = "0000000000000000e067a478024addfecdc93628978aa52d91fabd4292982a50";
            b.qtdZ = 6;
            b.timestamp = "9876";
            b.nonce = "123456";

            Regex regex = new Regex("^[0]+$");

            HashFacens facensHash = facade.FacensWebService.ReqBitcoinsWebService();
            Console.WriteLine("Hash facens: " + facensHash.hash + ", " + facensHash.zeros);
            Console.WriteLine(GetHashString(facensHash.hash));
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Hash:" + facensHash.hash + " Zeros:" + facensHash.zeros);
                }
            }

            for (int timestamp = 1001; timestamp < 20000000; timestamp++)
            {
                Task.Run(() => MineraHash(facensHash.hash, timestamp.ToString(), Convert.ToInt32(facensHash.zeros)));

            }

            return false;
        }

        public static void MineraHash(string hashAnterior, string timestamp, int qtdZeros)
        {

            Regex regex = new Regex("^[0]+$");

            for (int nonce = 0; nonce < 20000000; nonce++)
            {
                string hash = GetHashString((hashAnterior + nonce.ToString() + timestamp.ToString()));
                if (regex.IsMatch(hash.Substring(0, qtdZeros)))
                {
                    Console.WriteLine(timestamp + " " + nonce);
                    string str = facade.FacensWebService.ReqBitcoinsResultWebService(timestamp, nonce.ToString());
                    if (!str.Contains("<html>"))
                    {
                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.WriteLine(" Nonce:" + nonce + " Timestamp:" + timestamp + " Hash256:" + hash + " -> " + str);
                        }
                    }
                }
            }


            return;
        }
        */
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
