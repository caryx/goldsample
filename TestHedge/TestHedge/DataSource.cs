using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
        static Stream getStockStream(string codeStr)
        {
            //设定服务器IP地址
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, 8885)); //配置服务器IP与端口
                //Console.WriteLine("连接服务器成功");
            }
            catch
            {
                Console.WriteLine("连接服务器失败，请按回车键退出！");
                return null;
            }

            byte[] result = new byte[1024];
            ////通过clientSocket接收数据
            //int receiveLength = clientSocket.Receive(result);
            //Console.WriteLine("接收服务器消息：{0}", Encoding.ASCII.GetString(result, 0, receiveLength));

            //FileStream fs = new FileStream(@"D:\test.txt", FileMode.Create);
            NetworkStream networkStream = new NetworkStream(clientSocket);
            int bufferSize = 1024 * 10;
            int readSize = 0;
            byte[] buffer = new byte[bufferSize];
            byte[] stockCodeBuf = Encoding.ASCII.GetBytes(codeStr);
            networkStream.Write(stockCodeBuf, 0, stockCodeBuf.Length);

            return networkStream;
        }

        static public List<StockDayData> getData(string path)
        {
            Stream networkStream = getStockStream(path);
            if (networkStream == null)
            {
                return null;
            }

            int readSize = 0;
            byte[] buffer = new byte[1024 * 10];


            readSize = networkStream.Read(buffer, 0, buffer.Length);
            string result2 = Encoding.ASCII.GetString(buffer, 0, readSize);
            int fileSize = 19072;
            if (result2.Equals("NO"))
            {
                networkStream.Close();
                return null;
            }
            else
            {

                string length = Encoding.ASCII.GetString(buffer, 0, readSize);
                fileSize = int.Parse(length);
            }

            byte[] stockCodeBuf = Encoding.ASCII.GetBytes("READY");
            networkStream.Write(stockCodeBuf, 0, stockCodeBuf.Length);

            //int readCount = 0;
            //byte[] fileBuffer = new byte[fileSize];
            //while (readCount < fileSize)
            //{
            //    readSize = networkStream.Read(fileBuffer, 0, fileBuffer.Length);
            //    readCount += readSize;
            //    //Console.WriteLine("Read part. size = " + readSize + "/ readCount = " + readCount);
            //}
            //networkStream.Close();

            ////////////////////////////////////////////////////////////////////////////////////////////

            //using (MemoryStream ms = new MemoryStream(fileBuffer))
            {
                List<StockDayData> list = new List<StockDayData>();
                BinaryReader br = new BinaryReader(networkStream);
                int days = fileSize / 32;
                for (int i = 0; i < days; i++)
                {
                    StockDayData item = new StockDayData();
                    //item.DataDate =DateTime.Parse(new string(br.ReadChars(8)));
                    int date = br.ReadInt32();
                    if (date == 0)
                    {
                        break;
                    }
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
                return list;
            }
        }
    }


}
