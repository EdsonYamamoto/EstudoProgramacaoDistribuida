using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ProjetoChatProgramDistrubuida
{
    class Program {
        static model.Config configuracao;
        static UdpClient socket;

        static void OnUdpData(IAsyncResult result) {

            UdpClient socket = result.AsyncState as UdpClient;
            IPEndPoint source = new IPEndPoint(0, 0);
            byte[] message = socket.EndReceive(result, ref source);

            Console.WriteLine("Got " + message.Length + " bytes from " + source);

            String json = Encoding.ASCII.GetString(message);
            Console.WriteLine(json);

            //model.Mensagem MensagemRecebida = JsonConvert.DeserializeObject<model.Mensagem>(json);
            //Console.WriteLine(MensagemRecebida);

            using (StreamWriter sw = File.AppendText(logPath))
                sw.WriteLine("RECEIVED>>> "+ json);

            socket.BeginReceive(new AsyncCallback(OnUdpData), socket);
        }

        static void OutUdpData( model.Mensagem mensagem) {
            socket.Client.SetSocketOption( SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            socket.BeginReceive(new AsyncCallback(OnUdpData), socket);

            IPEndPoint target = new IPEndPoint(IPAddress.Parse(configuracao.IP), configuracao.Port);

            byte[] message = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(mensagem));

            using (StreamWriter sw = File.AppendText(logPath))
                sw.WriteLine("SENT>>> "+JsonConvert.SerializeObject(mensagem));

            socket.Send(message, message.Length, target);

        }

        static readonly string textFile = @"config/config.json";
        static readonly string logPath = @"log.txt";

        static void Main(string[] args) {

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
                    model.Mensagem mensagem = new model.Mensagem();
                    Console.WriteLine("Escreva uma mensagem");
                    mensagem.Message = Console.ReadLine();
                    OutUdpData(mensagem);
                    Console.WriteLine("");

                }
            }

        }
    }
}
