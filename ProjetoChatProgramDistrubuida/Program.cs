using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ProjetoChatProgramDistrubuida
{
    class Program
    {
        static void OnUdpData(IAsyncResult result)
        {
            // this is what had been passed into BeginReceive as the second parameter:
            UdpClient socket = result.AsyncState as UdpClient;
            // points towards whoever had sent the message:
            IPEndPoint source = new IPEndPoint(0, 0);
            // get the actual message and fill out the source:
            byte[] message = socket.EndReceive(result, ref source);
            // do what you'd like with `message` here:
            Console.WriteLine("Got " + message.Length + " bytes from " + source);
            Console.WriteLine(System.Text.Encoding.ASCII.GetString(message));

            String json = System.Text.Encoding.ASCII.GetString(message);

            model.Mensagem modeloPro = JsonConvert.DeserializeObject<model.Mensagem>(json);
            Console.WriteLine(modeloPro.Message);
            
                // schedule the next receive operation once reading is done:
            socket.BeginReceive(new AsyncCallback(OnUdpData), socket);
        }


        static void OutUdpData( model.Mensagem mensagem)
        {
            UdpClient socket = new UdpClient(5394); // `new UdpClient()` to auto-pick port
                                                    // schedule the first receive operation:
            socket.BeginReceive(new AsyncCallback(OnUdpData), socket);
            // sending data (for the sake of simplicity, back to ourselves):
            IPEndPoint target = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5394);

            // send a couple of sample messages:
            byte[] message = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(mensagem));
            socket.Send(message, message.Length, target);
        }

        static void EnviarMensagem(){
            model.Mensagem mensagem = new model.Mensagem();
            mensagem.Message = "ola mundo";

            OutUdpData(mensagem);

            Console.ReadKey();
        }
        static readonly string textFile = "/config/config.json";


        static void Main(string[] args){

            Console.WriteLine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory));
            String caminhoArquivo = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + textFile;
            if (File.Exists(caminhoArquivo)){
                // Read entire text file content in one string  
                string text = File.ReadAllText(caminhoArquivo);
                Console.WriteLine(text);
            }

        }
    }
}
