using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHedge
{
    class Program
    {
        static void Main(string[] args)
        {
            Work work = new Work();
            string startTime = DateTime.Now.ToString();
            work.work3();
            string endTime = DateTime.Now.ToString();
            Log.i(startTime);
            Log.i(endTime);
        }
    }
}
