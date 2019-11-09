using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoChatProgramDistrubuida{

    enum Stuff
    {
        Thing1,
        Thing2
    }

    static class StuffMethods
    {

        public static String GetString(this Stuff s1)
        {
            switch (s1)
            {
                case Stuff.Thing1:
                    return "Yeah!";
                case Stuff.Thing2:
                    return "Okay!";
                default:
                    return "What?!";
            }
        }
    }

    public class Program1
    {


        static void MainT()
        {
            Stuff thing = Stuff.Thing1;
            String str = thing.GetString();
        }
    }

}
