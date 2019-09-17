using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ProjetoChatProgramDistrubuida.service
{
    public static class Receiver
    {
        public static void Receive()
        {
            Program.socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            Program.socket.BeginReceive(new AsyncCallback(OnUdpDataV2), Program.socket);
        }

        static void OnUdpDataV2(IAsyncResult result)
        {

            UdpClient socket = result.AsyncState as UdpClient;
            IPEndPoint source = new IPEndPoint(IPAddress.Any, Program.configuracao.PortReceiver);
            byte[] message = socket.EndReceive(result, ref source);

            String response = Encoding.ASCII.GetString(message);
            Console.WriteLine("rece<[" + source.Address.ToString() + ":" + Program.configuracao.PortReceiver.ToString() + "]:" + response);

            socket.BeginReceive(new AsyncCallback(OnUdpDataV2), socket);

            IPEndPoint target = new IPEndPoint(IPAddress.Parse(source.Address.ToString()), Program.configuracao.Port);
            byte[] messageReply = Encoding.ASCII.GetBytes(Program.heartbeatRep);
            socket.Send(messageReply, messageReply.Length, target);

            Console.WriteLine("send>[" + source.Address.ToString() + ":" + Program.configuracao.Port.ToString() + "]:" + Program.heartbeatRep);
        }
    }
}
