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

            Console.WriteLine("client is running ... ");
            TcpClient client = new TcpClient();

            try {
                client.Connect("localhost", 8500);
            
            } catch (Exception e){
                Console.WriteLine(e.Message);
                return;
            }
            Console.WriteLine("Server Connected！{0} --> {1}",
                client.Client.LocalEndPoint,
                client.Client.RemoteEndPoint
                );



            Console.WriteLine("\n\n输入\"X\"键退出");
            ConsoleKey exitKey;
            do {
                exitKey = Console.ReadKey(true).Key;
            } while (exitKey != ConsoleKey.X);
        }
    }
}
