using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace ProjetoChatProgramDistrubuida.service
{
    public static class Geral
    {
        public static void ElegeLider()
        {
            while (true)
            {
                Thread.Sleep(Program.configuracao.LeaderElection);

                model.Ip lider = new model.Ip();

                bool eleicaoFeita = false;

                Console.WriteLine("*****************************************************");
                foreach (model.Ip ip in Program.IPs.Ips)
                {
                    if ( ip.contagemReply > 0 && !eleicaoFeita ) {
                        lider = ip;
                        eleicaoFeita = true;
                    }
                    Console.WriteLine(ip.IP + ">Request:" + ip.contagemRequest + " Reply:" + ip.contagemReply);

                    ip.contagemReply = 0;
                    ip.contagemRequest = 0;
                }
                Console.WriteLine("*****************************************************");

                Program.Lider = lider;

                Console.WriteLine("Lider:" + lider.IP);
                Console.WriteLine("");
            }
        }

        public static string GeradorSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
