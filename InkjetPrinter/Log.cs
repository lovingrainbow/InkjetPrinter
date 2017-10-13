using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InkjetPrinter
{
    class Log
    {
        public static void ErrorLog(string message, int errorCode)
        {
            Console.WriteLine("Error : " + message);
        }

        public static void log(string message)
        {
            Console.WriteLine(message);  
        }

        public static void warningLog(string message)
        {

        }
    }
}
