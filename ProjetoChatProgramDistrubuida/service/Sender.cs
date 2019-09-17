using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ProjetoChatProgramDistrubuida.service
{
    public static class Sender
    {
        public static void Send()
        {
            foreach (model.Ip ip in Program.IPs.Ips)
            {
                IPEndPoint target = new IPEndPoint(IPAddress.Parse(ip.IP), Program.configuracao.Port);

                byte[] message = Encoding.ASCII.GetBytes(Program.heartbeatReq);
                Program.socket.Send(message, message.Length, target);
                Console.WriteLine("send>[" + ip.IP + ":" + Program.configuracao.Port.ToString() + "]:" + Program.heartbeatReq);
            }
        }
    }
}
