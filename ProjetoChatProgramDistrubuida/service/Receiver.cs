﻿using System;
using System.Collections.Generic;
using System.IO;
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

            if (!Mineracao.finalizado)
            {
                if (response.ToUpper().CompareTo(Program.processRequest.ToUpper()) == 0)
                    ProcessRequest(socket, source, response);

                if (response.Split(";").Length == 6)
                    MineraRequest(socket, source, response);

                if (response.Contains(Program.processAnswerYes))
                    RespostaPositiva(socket, source, response);

                if (response.Contains(Program.processAnswerNo))
                    RespostaNegativa(socket, source, response);

                if (response.Contains(Program.processInterrupt))
                    RespostaInterrompeProcesso(socket, source, response);
                

            }
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

                string resp = $"Process;{Mineracao.incio};{Mineracao.fim};{DateTime.Now.ToString("yyyyMMddHHmmss")};{Mineracao.hashFacens.hash};{Mineracao.hashFacens.zeros}";

                byte[] messageReply = Encoding.ASCII.GetBytes(resp);

                socket.Send(messageReply, messageReply.Length, target);
                Console.WriteLine("\t\tResponse> " + resp);

                Mineracao.incio = Mineracao.fim;
                Mineracao.fim += 50000;
                if (Mineracao.fim >= 20000000)
                {
                    Mineracao.incio = 0;
                    Mineracao.fim = 50000;
                }
            }
        }

        private static void MineraRequest(UdpClient socket, IPEndPoint source, string response)
        {
            IPEndPoint target = new IPEndPoint(IPAddress.Parse(source.Address.ToString()), Program.configuracao.Port);
            string resp;
            string[] vetStr = response.Split(";");

            string resposta = Mineracao.Processo(Convert.ToInt32(vetStr[1]), Convert.ToInt32(vetStr[2]), Convert.ToInt64(vetStr[3]), vetStr[4], Convert.ToInt32(vetStr[5]));
            if (resposta != null)
            {
                resp = resposta;
                Console.WriteLine("****************************************************************");
                Console.WriteLine("****************************************************************");
                Console.WriteLine($"\t\tResponse> {Convert.ToInt64(vetStr[3])} nounce:{resp} hash:{vetStr[4]}" );
                Console.WriteLine("****************************************************************");
                Console.WriteLine("****************************************************************");

                string respostaWEB = facade.FacensWebService.ReqBitcoinsResultWebService(vetStr[3], resposta);

                using (StreamWriter sw = File.CreateText(@"c:\Temp\"+ DateTime.Now.ToString("yyyyMMddHHmmss") + "Resultado.txt"))
                {
                    sw.WriteLine($"\t\tResponse> {Convert.ToInt64(vetStr[3])} nounce:{resp} hash:{vetStr[4]}\n{respostaWEB}");
                }

                resp = Program.processAnswerYes + resposta;
            }
            else
                resp = Program.processAnswerNo;


            Console.WriteLine("\t\tResponse> " + resp);

            byte[] messageReply = Encoding.ASCII.GetBytes(resp);

            socket.Send(messageReply, messageReply.Length, target);

        }



        private static void RespostaPositiva(UdpClient socket, IPEndPoint source, string response)
        {
            foreach (model.Ip ip in Program.IPs.Ips)
            {

                IPEndPoint target = new IPEndPoint(IPAddress.Parse(ip.IP), Program.configuracao.Port);
                byte[] messageReply = Encoding.ASCII.GetBytes(Program.processInterrupt);
                socket.Send(messageReply, messageReply.Length, target);

                Console.WriteLine("\t\tResponse> " + Program.processInterrupt);
            }
        }

        private static void RespostaNegativa(UdpClient socket, IPEndPoint source, string response)
        {

        }

        private static void RespostaInterrompeProcesso(UdpClient socket, IPEndPoint source, string response)
        {
            Mineracao.finalizado = true;
        }
        
    }
}
