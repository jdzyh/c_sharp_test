using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace csTest_First
{
    public interface IMessageSender
    {
        bool Connect(IPAddress ip, int port);
        bool SendMessage(Message msg);
        void SignOut();
    }
}
