using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ProjetoChatProgramDistrubuida.service
{
    public static class Receiver
    {
        public static void Receive()
        {
            while (true)
            {
                Thread.Sleep(Program.configuracao.ReceiverTimer);

                Program.socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                Program.socket.BeginReceive(new AsyncCallback(OnUdpDataV2), Program.socket);
            }
        }

        static void OnUdpDataV2(IAsyncResult result)
        {
            UdpClient socket = result.AsyncState as UdpClient;
            IPEndPoint source = new IPEndPoint(IPAddress.Any, Program.configuracao.PortReceiver);
            byte[] message = socket.EndReceive(result, ref source);

            String response = Encoding.ASCII.GetString(message);

            if (response.ToUpper().CompareTo(Program.heartbeatRep.ToUpper()) == 0)
                foreach (model.Ip ip in Program.IPs.Ips)
                {
                    if (ip.IP.CompareTo(source.Address.ToString()) == 0)
                    {
                        ip.contagemReply += 1;

                        Console.WriteLine("\trece<<<[" + source.Address.ToString() + ":" + Program.configuracao.PortReceiver.ToString() + "]:\t" + response);
                    }
                }

            if (response.ToUpper().CompareTo(Program.heartbeatReq.ToUpper()) == 0) {

                IPEndPoint target = new IPEndPoint(IPAddress.Parse(source.Address.ToString()), Program.configuracao.Port);
                byte[] messageReply = Encoding.ASCII.GetBytes(Program.heartbeatRep);
                socket.Send(messageReply, messageReply.Length, target);

                foreach (model.Ip ip in Program.IPs.Ips)
                    if (ip.IP.CompareTo(source.Address.ToString()) == 0) {
                        ip.contagemRequest += 1;
                        Console.WriteLine("rece<<<[" + source.Address.ToString() + ":" + Program.configuracao.PortReceiver.ToString() + "]:\t" + response + " Response> "+ Program.heartbeatRep);
                    }
            }

            if (response.ToUpper().CompareTo(Program.processRequest.ToUpper()) == 0)
            {

                IPEndPoint target = new IPEndPoint(IPAddress.Parse(source.Address.ToString()), Program.configuracao.Port);
                byte[] messageReply = null;

                if(Mineracao.Processo())
                {
                    messageReply = Encoding.ASCII.GetBytes(Program.processAnswerYes+"["+Mineracao.valor+"]");

                }
                else
                {
                    messageReply = Encoding.ASCII.GetBytes(Program.processAnswerNo);

                }


                socket.Send(messageReply, messageReply.Length, target);

            }

            //Console.WriteLine("send>[" + source.Address.ToString() + ":" + Program.configuracao.Port.ToString() + "]:\t" + Program.heartbeatRep);
        }
    }
}
