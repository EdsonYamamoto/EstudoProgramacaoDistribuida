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

            Console.WriteLine("\treceived<<<[" + source.Address.ToString() + ":" + Program.configuracao.PortReceiver.ToString() + "]:\t" + response);

            if (response.ToUpper().CompareTo(Program.heartbeatRep.ToUpper()) == 0)
                HeartbeatReply(source, response);

            if (response.ToUpper().CompareTo(Program.heartbeatReq.ToUpper()) == 0)
                HeartbeatRequest(socket, source, response);

            if (response.ToUpper().CompareTo(Program.processRequest.ToUpper()) == 0)
                ProcessRequest(socket, source, response);

            if (response.Split(";").Length == 6) 
                MineraRequest(socket, source, response);

            if (response.Contains(Program.processAnswerNo) || response.Contains(Program.processAnswerYes))
                RespostaPositivaNegativa(socket, source, response);

            Console.WriteLine("");
        }


        private static void HeartbeatReply(IPEndPoint source, string response)
        {
            foreach (model.Ip ip in Program.IPs.Ips)
            {
                if (ip.IP.CompareTo(source.Address.ToString()) == 0)
                {
                    ip.contagemReply += 1;
                }
            }
        }



        private static void HeartbeatRequest(UdpClient socket, IPEndPoint source, string response)
        {
            IPEndPoint target = new IPEndPoint(IPAddress.Parse(source.Address.ToString()), Program.configuracao.Port);
            byte[] messageReply = Encoding.ASCII.GetBytes(Program.heartbeatRep);
            socket.Send(messageReply, messageReply.Length, target);

            foreach (model.Ip ip in Program.IPs.Ips)
            {
                if (ip.IP.CompareTo(source.Address.ToString()) == 0)
                {
                    ip.contagemRequest += 1;
                    Console.WriteLine("\t\tResponse> " + Program.heartbeatRep);
                }
            }
        }



        private static void ProcessRequest(UdpClient socket, IPEndPoint source, string response)
        {

            if (Program.Lider != null)
            {
                IPEndPoint target = new IPEndPoint(IPAddress.Parse(source.Address.ToString()), Program.configuracao.Port);
                string resp = $"Process;{Mineracao.incio};{Mineracao.fim};{Mineracao.timestamp};{Mineracao.hash};{Mineracao.zeros}";

                byte[] messageReply = Encoding.ASCII.GetBytes(resp);

                socket.Send(messageReply, messageReply.Length, target);
                Console.WriteLine("\t\tResponse> " + resp);
            }
        }



        private static void MineraRequest(UdpClient socket, IPEndPoint source, string response)
        {


            IPEndPoint target = new IPEndPoint(IPAddress.Parse(source.Address.ToString()), Program.configuracao.Port);
            string resp;
            if (Mineracao.Processo())
                resp = Program.processAnswerYes + "[" + Mineracao.valor + "]";
            else
                resp = Program.processAnswerNo;


            Console.WriteLine("\t\tResponse> " + resp);

            byte[] messageReply = Encoding.ASCII.GetBytes(resp);

            socket.Send(messageReply, messageReply.Length, target);

        }



        private static void RespostaPositivaNegativa(UdpClient socket, IPEndPoint source, string response)
        {
        }
    }
}
