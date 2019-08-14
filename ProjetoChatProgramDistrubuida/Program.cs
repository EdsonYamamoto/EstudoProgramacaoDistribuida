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
            // this is what had been passed into BeginReceive as the second parameter:
            UdpClient socket = result.AsyncState as UdpClient;
            // points towards whoever had sent the message:
            IPEndPoint source = new IPEndPoint(0, 0);
            // get the actual message and fill out the source:
            byte[] message = socket.EndReceive(result, ref source);
            // do what you'd like with `message` here:
            Console.WriteLine("Got " + message.Length + " bytes from " + source);

            String json = System.Text.Encoding.ASCII.GetString(message);

            model.Mensagem MensagemRecebida = JsonConvert.DeserializeObject<model.Mensagem>(json);
            Console.WriteLine(MensagemRecebida);
            
            socket.BeginReceive(new AsyncCallback(OnUdpData), socket);
        }

        static void OutUdpData( model.Mensagem mensagem) {
            socket.Client.SetSocketOption( SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            socket.BeginReceive(new AsyncCallback(OnUdpData), socket);
            // sending data (for the sake of simplicity, back to ourselves):
            IPEndPoint target = new IPEndPoint(IPAddress.Parse(configuracao.IP), configuracao.Port);

            // send a couple of sample messages:
            byte[] message = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(mensagem));
            socket.Send(message, message.Length, target);
        }

        static readonly string textFile = @"config/config.json";
        static void Main(string[] args){

            String caminhoArquivo = textFile;
            if (File.Exists(caminhoArquivo)){

                // Read entire text file content in one string  

                configuracao = JsonConvert.DeserializeObject<model.Config>(File.ReadAllText(caminhoArquivo));

                socket = new UdpClient(configuracao.Port);
                while (true){
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
