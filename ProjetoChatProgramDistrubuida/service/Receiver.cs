﻿using System;
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
            Console.WriteLine("rece<[" + source.Address.ToString() + ":" + Program.configuracao.PortReceiver.ToString() + "]:\t" + response);

            if (response.CompareTo(Program.heartbeatRep)==0)
                foreach (model.Ip ip in Program.IPs.Ips)
                    if (ip.IP.CompareTo(source.Address.ToString()) == 0)
                        ip.contagemSent += 1;

            if (response.CompareTo(Program.heartbeatReq) == 0)
            {
                socket.BeginReceive(new AsyncCallback(OnUdpDataV2), socket);

                IPEndPoint target = new IPEndPoint(IPAddress.Parse(source.Address.ToString()), Program.configuracao.Port);
                byte[] messageReply = Encoding.ASCII.GetBytes(Program.heartbeatRep);
                socket.Send(messageReply, messageReply.Length, target);

                foreach (model.Ip ip in Program.IPs.Ips)
                    if (ip.IP.CompareTo(source.Address.ToString()) == 0)
                        ip.contagemReceived += 1;
            }

            Console.WriteLine("send>[" + source.Address.ToString() + ":" + Program.configuracao.Port.ToString() + "]:\t" + Program.heartbeatRep);
        }
    }
}
