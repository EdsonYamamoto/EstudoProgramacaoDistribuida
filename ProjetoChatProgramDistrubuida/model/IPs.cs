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
        public string IP;

        public int contagemReply;
        public int contagemRequest;
    }
}
