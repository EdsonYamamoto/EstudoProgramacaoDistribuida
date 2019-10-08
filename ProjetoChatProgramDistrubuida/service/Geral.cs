using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProjetoChatProgramDistrubuida.service
{
    public static class Geral
    {
        public static void ElegeLider()
        {
            while (true)
            {
                Thread.Sleep(Program.configuracao.LeaderElection);

                model.Ip lider = new model.Ip();

                bool eleicaoFeita = false;

                Console.WriteLine("*****************************************************");
                foreach (model.Ip ip in Program.IPs.Ips)
                {
                    //if (lider.contagemSent < ip.contagemSent)
                    //    lider = ip;
                    if ( ip.contagemSent > 0 && !eleicaoFeita ) {
                        lider = ip;
                        eleicaoFeita = true;
                        //break;
                    }
                    Console.WriteLine(ip.IP + ">Request:" + ip.contagemReceived + " Reply:" + ip.contagemSent);

                    ip.contagemReceived = 0;
                    ip.contagemSent = 0;
                }
                Console.WriteLine("*****************************************************");

                Program.Lider = lider;

                Console.WriteLine("Lider:" + lider.IP);
                Console.WriteLine("");
            }
        }
    }
}
