using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoChatProgramDistrubuida.model
{
    class Mensagem
    {
        public String Message;
        public DateTime Data;
        public Mensagem()
        {
            this.Data = DateTime.Now;
        }
    }
}
