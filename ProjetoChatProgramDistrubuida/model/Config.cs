using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoChatProgramDistrubuida.model
{
    class Config
    {
        public int Port;
        public int PortReceiver;

        public int ReceiverTimer;
        public int BroadCastSenderTimer;
        public int ProcessRequestTimer;
        public int LeaderElection;
    }
}
