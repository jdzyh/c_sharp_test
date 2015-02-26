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

            Console.WriteLine("Server is running ... ");
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            //IPAddress ip = Dns.GetHostEntry("localhost").AddressList[0];
            TcpListener listener = new TcpListener(ip, 8500);
            
            listener.Start();
            Console.WriteLine("Start Listening ...");




            Console.WriteLine("\n\n输入\"X\"键退出");
            ConsoleKey exitKey;
            do {
                exitKey = Console.ReadKey(true).Key;
            } while (exitKey != ConsoleKey.X);
        }
    }
}
