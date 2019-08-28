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

        static void Main(string[] args) {
            listaIP.Add("172.17.115.26");

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
                while (true) {
                    OutUdpData();
                    Console.WriteLine("");
                    Thread.Sleep(10000);
                }
            }

        }


        static void OutUdpData()
        {
            socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            socket.BeginReceive(new AsyncCallback(OnUdpDataV2), socket);
            foreach(string IP in listaIP){
                IPEndPoint target = new IPEndPoint(IPAddress.Parse(IP), configuracao.Port);

                byte[] message = Encoding.ASCII.GetBytes("Heartbeat Request");
                socket.Send(message, message.Length, target);

            }
        }

        static void OnUdpDataV2(IAsyncResult result){

            UdpClient socket = result.AsyncState as UdpClient;
            IPEndPoint source = new IPEndPoint(IPAddress.Any, configuracao.PortReceiver);
            byte[] message = socket.EndReceive(result, ref source);

            Console.WriteLine("Got " + message.Length + " bytes from " + source);

            String json = Encoding.ASCII.GetString(message);
            Console.WriteLine(json);


            socket.BeginReceive(new AsyncCallback(OnUdpDataV2), socket);
            Console.WriteLine(source.Address.ToString());
            IPEndPoint target = new IPEndPoint(IPAddress.Parse(source.Address.ToString()), configuracao.Port);
            byte[] messageReply = Encoding.ASCII.GetBytes("Heartbeat Reply");
            socket.Send(messageReply, messageReply.Length, target);
        }
    }
}
