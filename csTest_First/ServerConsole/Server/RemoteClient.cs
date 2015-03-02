using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using ServerConsole.Tools;

namespace ServerConsole
{
    //---------------------------------------------------------------------
    // 封装 Tcpclient 类
    //---------------------------------------------------------------------
    public class RemoteClient
    {
        private TcpClient client;
        private NetworkStream streamToClient;
        private const int BufferSize = 8192;
        private byte[] buffer;
        private RequestHandler handler; // 获取异步传输的完整字符串

        public RemoteClient(TcpClient client) {
            this.client = client;

            // 打印连接到的客户端信息
            Console.WriteLine("\nClient Connected！{0} <-- {1}",
                client.Client.LocalEndPoint, client.Client.RemoteEndPoint);

            // 获得流
            streamToClient = client.GetStream();
            buffer = new byte[BufferSize];

            // 设置RequestHandler
            handler = new RequestHandler();

            // 在构造函数中就开始准备读取
            // "An optional asynchronous callback, to be called when the read is complete."
            AsyncCallback callBack = new AsyncCallback(ReadComplete);
            /** "https://msdn.microsoft.com/en-us/library/system.io.stream.beginread(v=vs.110).aspx"
             * "the new async methods, such as ReadAsync, WriteAsync, CopyToAsync, and FlushAsync, 
             * help you implement asynchronous I/O operations more easily."
            */
            streamToClient.BeginRead(buffer, 0, BufferSize, callBack, null); // 启用一个异步读取操作线程
        }

        // 再读取完成时进行回调
        private void ReadComplete(IAsyncResult ar) {
            int bytesRead = 0;
            try {
                lock (streamToClient) {
                    bytesRead = streamToClient.EndRead(ar);
                    Console.WriteLine("Reading data, {0} bytes ...", bytesRead);
                }
                if (bytesRead == 0) throw new Exception("读取到0字节");

                string msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                Array.Clear(buffer, 0, buffer.Length);        // 清空缓存，避免脏读

                string[] msgArray = handler.GetActualString(msg);   // 获取实际的字符串

                // 遍历获得到的字符串
                foreach (string m in msgArray) {
                    Console.WriteLine("Received: {0}", m);
                    string back = m.ToUpper();

                    // 将得到的字符串改为大写并重新发送
                    byte[] temp = Encoding.Unicode.GetBytes(back);
                    streamToClient.Write(temp, 0, temp.Length);
                    streamToClient.Flush();
                    Console.WriteLine("Sent: {0}", back);
                }

                // 再次调用BeginRead()，完成时调用自身，形成无限循环
                // 起一个新线程
                lock (streamToClient) {
                    AsyncCallback callBack = new AsyncCallback(ReadComplete);
                    streamToClient.BeginRead(buffer, 0, BufferSize, callBack, null);
                }
            } catch (Exception ex) {
                if (streamToClient != null)
                    streamToClient.Dispose();
                client.Close();
                Console.WriteLine(ex.Message);      // 捕获异常时退出程序              
            }
        }
    }
}
