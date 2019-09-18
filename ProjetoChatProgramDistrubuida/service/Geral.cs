using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoChatProgramDistrubuida.service
{
    public static class Geral
    {
        public static void ElegeLider()
        {
            model.Ip lider = new model.Ip();
            foreach(model.Ip ip in Program.IPs.Ips) {
                if (lider.contagemReceived < ip.contagemReceived)
                    lider = ip;

                ip.contagemReceived = 0;
                ip.contagemSent = 0;
            }
            Program.Lider = lider;
        }
    }
}
