using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace ClientConsole
{
    class Client
    {
        static void Main(string[] args) {

            ServerClient client = new ServerClient();
            client.SendMessage();

            //----------------------------------------------------------
            // 发送数据并接收
            //----------------------------------------------------------
            //const int BUFFER_SIZE = 8192;

            //Console.WriteLine("client is running ... ");
            //TcpClient client;
            //ConsoleKey key;

            //try {
            //    client = new TcpClient();
            //    client.Connect("localhost", 8500);

            //} catch (Exception e) {
            //    Console.WriteLine(e.Message);
            //    return;
            //}

            //Console.WriteLine("Server Connected！{0} --> {1}",
            //    client.Client.LocalEndPoint,
            //    client.Client.RemoteEndPoint
            //    );

            //NetworkStream streamToServer = client.GetStream();
            //Console.WriteLine("Menu: S - Send, X - Exit");

            ////do {
            //for(int i=0; i<3; ++i) {
            //    //key = Console.ReadKey(true).Key;
            //    //if (key == ConsoleKey.S) {
            //        string msg = "NicoNicoNi..."; //Console.ReadLine();
            //        byte[] buffer = Encoding.Unicode.GetBytes(msg);
                    
            //        try {
            //            lock (streamToServer) {
            //                streamToServer.Write(buffer, 0, buffer.Length);
            //            }
            //            Console.WriteLine("Sent: {0}", msg);

            //            //int bytesRead;
            //            //buffer = new byte[BUFFER_SIZE];

            //            //lock (streamToServer) {
            //            //    bytesRead = streamToServer.Read(buffer, 0, BUFFER_SIZE);
            //            //}

            //            //msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);
            //            //Console.WriteLine("Received: {0}", msg);

            //        } catch(Exception e) {
            //            Console.WriteLine(e.Message);
            //            break;
            //        }
                    
            //    //}
            //} //while (key != ConsoleKey.X);
            //streamToServer.Dispose();
            //client.Close();

            //----------------------------------------------------------
            // 循环:建立连接
            //----------------------------------------------------------
            //for (int i = 0; i < 3; ++i) {
            //    client = new TcpClient();
            //    try {
            //        client.Connect("localhost", 8500);

            //    } catch (Exception e) {
            //        Console.WriteLine(e.Message);
            //        break;
            //    }
            //    Console.WriteLine("Server Connected！{0} --> {1}",
            //        client.Client.LocalEndPoint,
            //        client.Client.RemoteEndPoint
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
