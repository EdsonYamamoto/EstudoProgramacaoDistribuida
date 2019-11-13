﻿using Newtonsoft.Json;
using ProjetoChatProgramDistrubuida.service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace ProjetoChatProgramDistrubuida
{
    class Program {
        public static model.Config configuracao;
        public static model.IPs IPs;

        public static model.Ip Lider = new model.Ip();

        public static UdpClient socket;

        //public static List<string> listaIP  = new List<string>();

        static readonly string configFile = @"config/config.json";
        static readonly string ipsFile = @"config/configIPs.json";
        static readonly string logPath = @"log.txt";
        public static readonly string heartbeatReq = "Heartbeat Request";
        public static readonly string heartbeatRep = "Heartbeat Reply";
        public static readonly string processRequest = "ProcessRequest";
        public static readonly string processAnswerNo = "ProcessAnswerNo";
        public static readonly string processAnswerYes = "ProcessAnswerYes;";        
        public static readonly string processInterrupt = "ProcessInterrupt";


        public static model.Ip header;

        static void Main(string[] args) {
            Hash.Teste();

            Console.WriteLine(html);



            }
                html = reader.ReadToEnd();
            {
            using (StreamReader reader = new StreamReader(stream))
            using (Stream stream = response.GetResponseStream())
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())

            request.AutomaticDecompression = DecompressionMethods.GZip;
            request = (HttpWebRequest)WebRequest.Create(url);
            html = string.Empty;
            url = @"https://mineracao-facens.000webhostapp.com/submit.php?timestamp=&nonce=&poolname=";
            if (intervalo.Millisecond>1 && intervalo.Millisecond < 2000000000) ;

            DateTime intervalo = DateTime.Now;
            Console.WriteLine(facens.hash);
            }
                facens = JsonConvert.DeserializeObject<model.HashFacens>(reader.ReadToEnd());
            {
            using (StreamReader reader = new StreamReader(stream))
            using (Stream stream = response.GetResponseStream())
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            request.AutomaticDecompression = DecompressionMethods.GZip;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            string url = @"https://mineracao-facens.000webhostapp.com/request.php";
            string html = string.Empty;
            model.HashFacens facens;
            /*
            // Carrega IP do arquivo de IPs iniciais
            if (File.Exists(ipsFile)) {
                IPs = JsonConvert.DeserializeObject<model.IPs>(File.ReadAllText(ipsFile));
            }

            // Carrega configurações
            if (!File.Exists(logPath)) {
                using (StreamWriter sw = File.CreateText(logPath))
                    sw.WriteLine(DateTime.Now);
            }

            if (File.Exists(configFile)){
                configuracao = JsonConvert.DeserializeObject<model.Config>(File.ReadAllText(configFile));

                socket = new UdpClient(configuracao.Port);
                Thread receiver = new Thread(new ThreadStart(service.Receiver.Receive));
                receiver.Start();

                Thread sender = new Thread(new ThreadStart(service.Sender.Send));
                sender.Start();

                Thread lider = new Thread(new ThreadStart(service.Geral.ElegeLider));
                lider.Start();

                Thread mineracao = new Thread(new ThreadStart(service.Mineracao.Send));
                mineracao.Start();
            }
            */
        }
    }
}
