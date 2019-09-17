using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ProjetoChatProgramDistrubuida
{
    class Program {
        static model.Config configuracao;
        static model.IPs IPs;

        static UdpClient socket;

        //public static List<string> listaIP  = new List<string>();

        static readonly string configFile = @"config/config.json";
        static readonly string ipsFile = @"config/configIPs.json";
        static readonly string logPath = @"log.txt";
        static readonly string heartbeatReq = "Heartbeat Request";
        static readonly string heartbeatRep = "Heartbeat Reply";

        static void Main(string[] args) {

            if (File.Exists(ipsFile)) {
                IPs = JsonConvert.DeserializeObject<model.IPs>(File.ReadAllText(ipsFile));
            }

            if (!File.Exists(logPath)) {
                using (StreamWriter sw = File.CreateText(logPath))
                    sw.WriteLine(DateTime.Now);
            }

            if (File.Exists(configFile)){
                configuracao = JsonConvert.DeserializeObject<model.Config>(File.ReadAllText(configFile));

                socket = new UdpClient(configuracao.Port);
                while (true)
                {
                    Thread receiver = new Thread(new ThreadStart(Receiver));
                    receiver.Start();

                    Thread sender = new Thread(new ThreadStart(OutUdpData));
                    sender.Start();

                    Thread.Sleep(5000);
                    Console.WriteLine("");
                }
            }
        }

        static void Receiver()
        {
            socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            socket.BeginReceive(new AsyncCallback(OnUdpDataV2), socket);
        }

        static void OutUdpData()
        {
            foreach(model.Ip ip in IPs.Ips)
            {
                IPEndPoint target = new IPEndPoint(IPAddress.Parse(ip.IP), configuracao.Port);

                byte[] message = Encoding.ASCII.GetBytes(heartbeatReq);
                socket.Send(message, message.Length, target);
                Console.WriteLine("send>[" + ip.IP + ":" + configuracao.Port.ToString() + "]:"+ heartbeatReq );
            }
        }

        static void OnUdpDataV2(IAsyncResult result){

            UdpClient socket = result.AsyncState as UdpClient;
            IPEndPoint source = new IPEndPoint(IPAddress.Any, configuracao.PortReceiver);
            byte[] message = socket.EndReceive(result, ref source);

            String response = Encoding.ASCII.GetString(message);
            Console.WriteLine("rece<[" + source.Address.ToString() + ":" + configuracao.PortReceiver.ToString()+ "]:" +response);

            socket.BeginReceive(new AsyncCallback(OnUdpDataV2), socket);

            IPEndPoint target = new IPEndPoint(IPAddress.Parse(source.Address.ToString()), configuracao.Port);
            byte[] messageReply = Encoding.ASCII.GetBytes(heartbeatRep);
            socket.Send(messageReply, messageReply.Length, target);

            Console.WriteLine("send>[" + source.Address.ToString() +":"+ configuracao.Port.ToString() + "]:" + heartbeatRep);
        }
    }
}
