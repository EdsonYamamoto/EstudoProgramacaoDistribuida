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
        static UdpClient socket;

        public static List<string> listaIP  = new List<string>();

        static readonly string textFile = @"config/config.json";
        static readonly string logPath = @"log.txt";
        static readonly string heartbeatReq = "Heartbeat Request";
        static readonly string heartbeatRep = "Heartbeat Reply";

        static void Main(string[] args) {
            listaIP.Add("172.18.108.71");
            listaIP.Add("172.17.104.247");
            listaIP.Add("172.18.0.99");
            listaIP.Add("172.18.3.68");
            listaIP.Add("172.18.3.72");
            listaIP.Add("172.18.3.69");
            listaIP.Add("172.18.3.129");
            listaIP.Add("172.18.3.124");
            listaIP.Add("172.18.3.97");
            //listaIP.Add("172.18.3.137");
            listaIP.Add("172.18.3.115");
            listaIP.Add("172.18.0.108");

            //listaIP.Add("172.17.112.71");

            if (!File.Exists(logPath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(logPath))
                    sw.WriteLine(DateTime.Now);
            }
            String caminhoArquivo = textFile;
            if (File.Exists(caminhoArquivo)){

                // Read entire text file content in one string  

                configuracao = JsonConvert.DeserializeObject<model.Config>(File.ReadAllText(caminhoArquivo));

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
            foreach(string IP in listaIP){
                IPEndPoint target = new IPEndPoint(IPAddress.Parse(IP), configuracao.Port);

                byte[] message = Encoding.ASCII.GetBytes(heartbeatReq);
                socket.Send(message, message.Length, target);
                Console.WriteLine("send>[" + IP + ":" + configuracao.Port.ToString() + "]:"+ heartbeatReq );
            }
        }

        static void OnUdpDataV2(IAsyncResult result){

            UdpClient socket = result.AsyncState as UdpClient;
            IPEndPoint source = new IPEndPoint(IPAddress.Any, configuracao.PortReceiver);
            byte[] message = socket.EndReceive(result, ref source);

            String response = Encoding.ASCII.GetString(message);
            Console.WriteLine("rece<[" + source.Address.ToString() + ":" + configuracao.PortReceiver.ToString()+ "]:" +response);

            socket.BeginReceive(new AsyncCallback(OnUdpDataV2), socket);

            if(response.CompareTo(heartbeatReq) == 0) {
                IPEndPoint target = new IPEndPoint(IPAddress.Parse(source.Address.ToString()), configuracao.Port);
                byte[] messageReply = Encoding.ASCII.GetBytes(heartbeatRep);
                socket.Send(messageReply, messageReply.Length, target);

                Console.WriteLine("send>[" + source.Address.ToString() +":"+ configuracao.Port.ToString() + "]:" + heartbeatRep);
            }
        }
    }
}
