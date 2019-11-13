using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace ProjetoChatProgramDistrubuida.service
{
    public class Mineracao
    {
        public static string valor="";
        public static void Send()
        {
            while (true)
            {
                
                Thread.Sleep(Program.configuracao.BroadCastSenderTimer);
                if (Program.Lider !=null) {

                    IPEndPoint target = new IPEndPoint(IPAddress.Parse(Program.Lider.IP), Program.configuracao.Port);

                    byte[] message = Encoding.ASCII.GetBytes(Program.processRequest);
                    Program.socket.Send(message, message.Length, target);
                    Console.WriteLine("send>[" + Program.Lider.IP + ":" + Program.configuracao.Port.ToString() + "]:\t" + Program.processRequest);
                }
            }
        }

        public static bool Processo()
        {
            //Process;[início];[fim];[timestamp];[hash];[zeros]
            DateTime incio = DateTime.Now;


            DateTime fim = DateTime.Now;

            return false;
        }
        
    }
}
