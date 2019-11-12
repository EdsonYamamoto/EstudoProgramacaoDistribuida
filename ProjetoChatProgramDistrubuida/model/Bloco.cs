using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoChatProgramDistrubuida.model
{
    public class Bloco
    {
        public string timestamp { get; set; }
        public string nonce { get; set; }
        public string hashAnterior { get; set; }
        public int qtdZ { get; set; }
        public string hash { get; set; }

    }
}
