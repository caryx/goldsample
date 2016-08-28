using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using TestHedge;

namespace DataServer
{
    class TestData
    {
        public string str;
        public int val;
        public DateTime dataTime;

        public TestData()
        {
            str = "NULL";
            val = 0;
            dataTime = DateTime.Now;
        }
        public TestData(int value)
        {
            str = "Hello";
            val = value;
            dataTime = DateTime.Now;
        }
    }

    class Program
    {
        static string shBasePath = @"C:\new_gtja_v6\vipdoc\sh\lday\";
        static string szBasePath = @"C:\new_gtja_v6\vipdoc\sz\lday\";

        private static byte[] result = new byte[1024];
        private static int myProt = 8885;   //端口
        static Socket serverSocket;
        static void Main(string[] args)
        {
            preLoadFile();

            //服务器IP地址
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, myProt));  //绑定IP地址：端口
            serverSocket.Listen(10);    //设定最多10个排队连接请求
            Console.WriteLine("启动监听{0}成功", serverSocket.LocalEndPoint.ToString());
            //通过Clientsoket发送数据
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
            Console.ReadLine();
        }


        static Dictionary<string, byte[]> dataDict = new Dictionary<string, byte[]>();
        private static void preLoadFile()
        {
            var items = Directory.EnumerateFiles(shBasePath);
            foreach(var item in items)
            {
                //Console.WriteLine(item);
                using (FileStream fs = new FileStream(item, FileMode.Open))
                { 
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    dataDict.Add(item, buffer);
                }
            }

            items = Directory.EnumerateFiles(szBasePath);
            foreach (var item in items)
            {
                //Console.WriteLine(item);
                using (FileStream fs = new FileStream(item, FileMode.Open))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    dataDict.Add(item, buffer);
                }
            }

            Console.WriteLine("");
        }

        /// <summary>
        /// 监听客户端连接
        /// </summary>
        private static void ListenClientConnect()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                byte[] codeBuffer = new byte[1024];
                NetworkStream networkStream = new NetworkStream(clientSocket);
                int receiveNumber = networkStream.Read(codeBuffer, 0, codeBuffer.Length);
                string path = Encoding.ASCII.GetString(codeBuffer, 0, receiveNumber);
                //Console.WriteLine("接收客户端{0}消息{1}", "Code", path);
                
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
                        Log.w("unknown code: " + path);
                        goto FAILURE;
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
                        goto FAILURE;
                    }
                }

                if (!File.Exists(filename))
                {
                    //Console.ForegroundColor = ConsoleColor.Red; //设置前景色，即字体颜色
                    //Console.WriteLine("File \"" + filename + "\" does not exist.");
                    //Console.ResetColor();

                    Log.i("File \"" + filename + "\" does not exist.");
                    goto FAILURE;
                }
                else
                {
                    goto SUCCESS;
                }

            FAILURE:
                clientSocket.Send(Encoding.ASCII.GetBytes("NO"));
                continue;
            SUCCESS:
                //filename = @"D:\sz002725.day";
                if (dataDict.ContainsKey(filename))
                {
                    byte[] buffer = dataDict[filename];
                    {
                        //clientSocket.Send(Encoding.ASCII.GetBytes("" + fs.Length));
                        byte[] fileLength = Encoding.ASCII.GetBytes("" + buffer.Length);
                        networkStream.Write(fileLength, 0, fileLength.Length);
                        receiveNumber = networkStream.Read(codeBuffer, 0, codeBuffer.Length);
                        string cmd = Encoding.ASCII.GetString(codeBuffer, 0, receiveNumber);
                        if (cmd.Equals("READY"))
                        {
                            int readCount = 0;
                            int readSize = 0;
                            readSize = buffer.Length;
                            while (readCount < buffer.Length)
                            {
                                networkStream.Write(buffer, 0, readSize);
                                readCount += readSize;
                            }
                        }

                        networkStream.Flush();
                    }
                }
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="clientSocket"></param>
        private static void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    //通过clientSocket接收数据
                    int receiveNumber = myClientSocket.Receive(result);
                    Console.WriteLine("接收客户端{0}消息{1}", myClientSocket.RemoteEndPoint.ToString(), Encoding.ASCII.GetString(result, 0, receiveNumber));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }
    }
}
