using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHedge
{
    public class StockDayData
    {
        override
        public string ToString()
        {
            return Date.ToString() + "\t" + Open.ToString("0.00") + "\t" + Close.ToString("0.00") + " " + Vol.ToString() + "\t" + (Open > Close ? "+" : "-" );
        }

        private int mDate;

        public int Date
        {
            get { return mDate; }
            set { mDate = value; }
        }

        private decimal mOpen;

        public decimal Open
        {
            get { return mOpen; }
            set { mOpen = value; }
        }

        private decimal mHigh;

        public decimal High
        {
            get { return mHigh; }
            set { mHigh = value; }
        }

        private decimal mLow;

        public decimal Low
        {
            get { return mLow; }
            set { mLow = value; }
        }

        private decimal mClose;

        public decimal Close
        {
            get { return mClose; }
            set { mClose = value; }
        }

        private long mAmount;

        public long Amount
        {
            get { return mAmount; }
            set { mAmount = value; }
        }

        private int mVol;

        public int Vol
        {
            get { return mVol; }
            set { mVol = value; }
        }

        private decimal mPreClose;

        public decimal PreClose
        {
            get { return mPreClose; }
            set { mPreClose = value; }
        }

    }

    public class DataSource
    {
        static string shBasePath = @"C:\new_gtja_v6\vipdoc\sh\lday\";
        static string szBasePath = @"C:\new_gtja_v6\vipdoc\sz\lday\";
        static public List<StockDayData> getData(string path)
        {
            string filename = path;
            if (path.Length == 6)
            {
                if (path.StartsWith("6"))
                { 
                    filename = shBasePath + "sh" + path + ".day";
                }
                else if (path.StartsWith("0"))
                {
                    filename = szBasePath + "sz" + path + ".day";
                }
                else
                {
                    Log.w("unknown code: " + path );
                    return null;
                }
            }
            else if (path.Length == 8)
            {
                if (path.StartsWith("6"))
                {
                    filename = shBasePath + path + ".day";
                }
                else if (path.StartsWith("0"))
                {
                    filename = szBasePath + path + ".day";
                }
                else
                {
                    Log.w("unknown code: " + path);
                    return null;
                }

            }

            if (!File.Exists(filename))
            {
                //Console.ForegroundColor = ConsoleColor.Red; //设置前景色，即字体颜色
                //Console.WriteLine("File \"" + filename + "\" does not exist.");
                //Console.ResetColor();

                Log.i("File \"" + filename + "\" does not exist.");
                return null;
            }

            FileStream fs = File.OpenRead(filename);
            BinaryReader br = new BinaryReader(fs);
            int days = (int)fs.Length / 32;
            List<StockDayData> list = new List<StockDayData>();
            for (int i = 0; i < days; i++)
            {
                StockDayData item = new StockDayData();
                //item.DataDate =DateTime.Parse(new string(br.ReadChars(8)));
                int date = br.ReadInt32();
                int year = date / 10000;
                int month = int.Parse(date.ToString().Substring(4, 2));
                int day = int.Parse(date.ToString().Substring(6, 2));
                //item.DataDate = new DateTime(year, month, day);

                int open = br.ReadInt32();
                int high = br.ReadInt32();
                int low = br.ReadInt32();
                int close = br.ReadInt32();
                Single amount = br.ReadSingle();
                //Int32 amount = br.ReadInt32();
                decimal am = Convert.ToDecimal(amount);
                long amstr = Convert.ToInt64(amount);
                int vol = br.ReadInt32();
                int preclose = br.ReadInt32();

                //Decimal open = Convert.ToDecimal(br.ReadSingle());
                item.Date = date;
                item.Open = Convert.ToDecimal(open / 100m);
                item.High = Convert.ToDecimal(high / 100m);
                item.Low = Convert.ToDecimal(low / 100m);
                item.Close = Convert.ToDecimal(close / 100m);
                item.Amount = amstr;
                item.Vol = vol;
                item.PreClose = Convert.ToDecimal(preclose / 100m);

                list.Add(item);
                //list.Insert(0, item);
            }
            br.Close();
            fs.Close();

            return list;
        }
    }


}
