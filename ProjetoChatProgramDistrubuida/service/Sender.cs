using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace ProjetoChatProgramDistrubuida.service
{
    public static class Sender
    {
        public static void Send()
        {
            while (true)
            {

                Thread.Sleep(Program.configuracao.RequestsTimer);

                foreach (model.Ip ip in Program.IPs.Ips)
                {
                    IPEndPoint target = new IPEndPoint(IPAddress.Parse(ip.IP), Program.configuracao.Port);

                    byte[] message = Encoding.ASCII.GetBytes(Program.heartbeatReq);
                    Program.socket.Send(message, message.Length, target);
                    Console.WriteLine("send>[" + ip.IP + ":" + Program.configuracao.Port.ToString() + "]:\t" + Program.heartbeatReq);
                }
            }
        }
    }
}
