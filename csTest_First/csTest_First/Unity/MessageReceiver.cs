using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace csTest_First
{
    //----------------------------------------------------------------
    // 声明委托 : PortNumberReady
    //----------------------------------------------------------------
    public delegate void PortNumberReadyEventHandler(int portNumber);

    class MessageReceiver : IMessageReceiver 
    {
        //----------------------------------------------------------------
        // 定义事件 x 4
        //----------------------------------------------------------------
        public event MessageReceivedEventHandler MessageReceived;
        public event ClientConnectedEventHandler ClientConnected;
        public event ConnectionLostEventHandler ClientLost;

        public event PortNumberReadyEventHandler PortNumberReady;

        //----------------------------------------------------------------
        // 线程相关变量
        //----------------------------------------------------------------
        private Thread workerThread;
        private TcpListener listener;
        
        //----------------------------------------------------------------
        // 构造函数:直接启用监听
        //----------------------------------------------------------------
        public MessageReceiver() {
            ((IMessageReceiver)this).StartListen();
        }


        void IMessageReceiver.StartListen() {
            ThreadStart start = new ThreadStart(ListenThreadMethod);// 将函数传给线程
            workerThread = new Thread(start);
            workerThread.IsBackground = true;
            workerThread.Start();
        }

        //----------------------------------------------------------------
        // 传递给监听线程的函数
        //----------------------------------------------------------------
        private void ListenThreadMethod() {
            // 设置本地IP，并开启对其的监听
            IPAddress localIp = IPAddress.Parse("127.0.0.1");
            //IPAddress localIp = IPAddress.Any;
            listener = new TcpListener(localIp, 8500);
            listener.Start();

            // 从监听器获得本地的 IpEndPoint (包含端口信息等)
            IPEndPoint endPoint = listener.LocalEndpoint as IPEndPoint;
            int portNumber = endPoint.Port;

            if (PortNumberReady != null) { // 调用事件 : PortNumberReady
                PortNumberReady(portNumber);
            }

            while (true) {
                TcpClient remoteClient;
                try {
                    remoteClient = listener.AcceptTcpClient(); // 获取远程客户端信息
                } catch {
                    break;
                }
                if (ClientConnected != null) { // 调用事件 : ClientConnected 处理连接后的工作
                    endPoint = remoteClient.Client.RemoteEndPoint as IPEndPoint;
                    ClientConnected(endPoint);
                }

                Stream streamToClient = remoteClient.GetStream();// 获取客户端流数据
                byte[] buffer = new byte[8192];

                while (true) { // 读取，直到没数据
                    try {
                        int bytesRead = streamToClient.Read(buffer, 0, 8192);
                        if (bytesRead == 0) {
                            throw new Exception("客户端已断开连接");// 通过抛出异常而退出循环
                        }
                        string msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);

                        if (MessageReceived != null) { // 调用事件 : MessageReceived 
                            MessageReceived(msg);
                        }
                    } catch (Exception ex) {
                        if (ClientLost != null) { // 调用事件 : ClientLost 处理客户端断开后的操作
                            ClientLost(ex.Message);
                            break;                 
                        }
                    }
                }
            }
        }

        public void StopListen() {
            try {
                listener.Stop();
                listener = null;
                workerThread.Abort();
            } catch { }
        }
    }
}