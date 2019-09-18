using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoChatProgramDistrubuida.model
{
    public class IPs
    {
        public Ip[] Ips;
    }

    public class Ip
    {
        public int Prioridade;
        public string IP;

        public int contagemReceived;
        public int contagemSent;
    }

}
