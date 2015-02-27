using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ServerConsole
{
    class Server
    {
        static void Main(string[] args) {
            const int BUFFER_SIZE = 8192;

            Console.WriteLine("Server is running ... ");
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            //IPAddress ip = Dns.GetHostEntry("localhost").AddressList[0];
            TcpListener listener = new TcpListener(ip, 8500);
            
            listener.Start();
            Console.WriteLine("Start Listening ...");

            //----------------------------------------------------------
            // 接收并显示流
            //----------------------------------------------------------
            TcpClient linkedClient = listener.AcceptTcpClient();
            Console.WriteLine("{0}<---{1}",
                linkedClient.Client.LocalEndPoint,
                linkedClient.Client.RemoteEndPoint
                );

            NetworkStream streamToClient = linkedClient.GetStream();
            while (true) {
                byte[] buffer = new byte[BUFFER_SIZE];
                int bytesRead;
                try {
                    lock (streamToClient) {
                        bytesRead = streamToClient.Read(buffer, 0, BUFFER_SIZE);
                    }
                    if (bytesRead == 0) throw new Exception("读取到0字节");

                    Console.WriteLine("Reading data, {0} bytes ...", bytesRead);
                    string msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received : {0}", msg);

                    msg = msg.ToUpper();
                    buffer = Encoding.Unicode.GetBytes(msg);
                    lock (streamToClient) {
                        streamToClient.Write(buffer, 0, buffer.Length);
                    }
                    Console.WriteLine("Sent: {0}", msg);

                } catch(Exception e) {
                    Console.WriteLine( e.Message );
                    break;
                }
            }//while
            streamToClient.Dispose();
            linkedClient.Close();

            // 分次读取并转存
            //byte[] buffer = new byte[BufferSize];
            //int bytesRead;
            //MemoryStream msStream = new MemoryStream();
            //do {
            //    bytesRead = streamToClient.Read(buffer, 0, BufferSize);
            //    msStream.Write(buffer, 0, bytesRead);
            //} while (bytesRead > 0);

            //buffer = msStream.GetBuffer();
            //string msg = Encoding.Unicode.GetString(buffer);


            //----------------------------------------------------------
            // 循环:读取 listener 的 socket 连接
            //----------------------------------------------------------
            //TcpClient linkedClient;
            //while (true) {
            //    linkedClient = listener.AcceptTcpClient();

            //    Console.WriteLine("{0}<---{1}",
            //        linkedClient.Client.LocalEndPoint,
            //        linkedClient.Client.RemoteEndPoint
            //        );


            //}
            //----------------------------------------------------------
            // 按键退出
            //----------------------------------------------------------
            Console.WriteLine("\n\n输入\"X\"键退出");
            ConsoleKey exitKey;
            do {
                exitKey = Console.ReadKey(true).Key;
            } while (exitKey != ConsoleKey.X);

        }
    }
}
