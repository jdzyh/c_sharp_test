using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace csTest_First
{
    public class MessageSender : IMessageSender
    {
        TcpClient client;
        Stream streamToServer;

        public bool Connect(IPAddress ip, int port){
            try {
                client = new TcpClient();
                client.Connect(ip, port);
                streamToServer = client.GetStream();    // 获取连接至远程的流
                return true;
            } catch {
                return false;
            }
        }

        public bool SendMessage(Message msg)
        {
            try {
                lock (streamToServer) { // 锁
                    byte[] buffer = Encoding.Unicode.GetBytes(msg.ToString());
                    streamToServer.Write(buffer, 0, buffer.Length); // 写入流
                    return true;
                }
            } catch {
                return false;
            }
        }

        public void SignOut()
        {
            if (streamToServer != null)
                streamToServer.Dispose(); // 切断流
            if (client != null)
                client.Close();
        }
    }
}
