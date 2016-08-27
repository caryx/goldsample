using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHedge
{
    public class Log
    {
        public static void e(string str)
        {
            e(str, false);
        }

        public static void e(string str, bool time)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (time)
            { 
                Console.Write(DateTime.Now.Date.ToString() + " - ");
            }
            Console.WriteLine(str);
            Console.ResetColor();
        }

        public static void w(string str)
        {
            w(str, false);
        }

        public static void w(string str, bool time)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (time)
            {
                Console.Write(DateTime.Now.Date.ToString() + " - ");
            }
            Console.WriteLine(str);
            Console.ResetColor();
        }

        public static void i(string str)
        {
            i(str, false);
        }
        public static void i(string str, bool time)
        {
            if (time)
            {
                Console.Write(DateTime.Now.Date.ToString() + " - ");
            }

            Console.WriteLine(str);
        }
    }
}
