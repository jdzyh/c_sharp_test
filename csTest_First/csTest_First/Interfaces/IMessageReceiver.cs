using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace csTest_First
{
    public delegate void MessageReceivedEventHandler(string msg);
    public delegate void ClientConnectedEventHandler(IPEndPoint endPoint);
    public delegate void ConnectionLostEventHandler(string info);

    public interface IMessageReceiver
    {
        event MessageReceivedEventHandler MessageReceived;
        event ClientConnectedEventHandler ClientConnected;
        event ConnectionLostEventHandler ClientLost;

        void StartListen();
        void StopListen();
    }
}
