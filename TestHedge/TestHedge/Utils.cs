using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHedge
{
    class Utils
    {
        public static string Int2Str(long dec, int width)
        {
            string format = "{0:" + new string('0', width) + "}";
            string result = string.Format(format, dec);
            StringBuilder sb = new StringBuilder(result);
            for(int i=0;i<sb.Length-1;++i)
            {
                if (sb[i] == '0')
                {
                    sb[i] = ' ';
                }
                else
                {
                    break;
                }                
            }

            return sb.ToString();
        }
    }
}
