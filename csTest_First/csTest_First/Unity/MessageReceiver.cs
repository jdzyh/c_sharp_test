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
    public delegate void PortNumberReadyEventHandler(int portNumber);

    class MessageReceiver : IMessageReceiver 
    {
        public event MessageReceivedEventHandler MessageReceived;
        public event ClientConnectedEventHandler ClientConnected;
        public event ConnectionLostEventHandler ClientLost;

        public event PortNumberReadyEventHandler PortNumberReady;

        private Thread workerThread;
        private TcpListener listener;

        public MessageReceiver() {
            ((IMessageReceiver)this).StartListen();
        }

        void IMessageReceiver.StartListen() {
            ThreadStart start = new ThreadStart(ListenThreadMethod);
            workerThread = new Thread(start);
            workerThread.IsBackground = true;
            workerThread.Start();
        }

        private void ListenThreadMethod() {
            IPAddress localIp = IPAddress.Parse("127.0.0.1");
            listener = new TcpListener(localIp, 0);
            listener.Start();

            IPEndPoint endPoint = listener.LocalEndpoint as IPEndPoint;
            int portNumber = endPoint.Port;
            if (PortNumberReady != null) {
                PortNumberReady(portNumber);
            }

            while (true) {
                TcpClient remoteClient;
                try {
                    remoteClient = listener.AcceptTcpClient();
                } catch {
                    break;
                }
                if (ClientConnected != null) {
                    endPoint = remoteClient.Client.RemoteEndPoint as IPEndPoint;
                    ClientConnected(endPoint);
                }

                Stream streamToClient = remoteClient.GetStream();
                byte[] buffer = new byte[8192];

                while (true) {
                    try {
                        int bytesRead = streamToClient.Read(buffer, 0, 8192);
                        if (bytesRead == 0) {
                            throw new Exception("客户端已断开连接");
                        }
                        string msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);

                        if (MessageReceived != null) {
                            MessageReceived(msg);
                        }
                    } catch (Exception ex) {
                        if (ClientLost != null) {
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