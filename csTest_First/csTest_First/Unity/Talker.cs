﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace csTest_First
{
    public class Talker
    {
        private IMessageReceiver receiver;
        private IMessageSender sender;

        public Talker(IMessageReceiver receiver, IMessageSender sender) {
            this.receiver = receiver;
            this.sender = sender;
        }

        public Talker() {
            this.receiver = new MessageReceiver();
            this.sender = new MessageSender();
        }

        public event MessageReceivedEventHandler MessageReceived {
            add {
                receiver.MessageReceived += value;
            }
            remove {
                receiver.MessageReceived -= value;
            }
        }

        public event ClientConnectedEventHandler ClientConnected {
            add {
                receiver.ClientConnected += value;
            }
            remove {
                receiver.ClientConnected -= value;
            }
        }

        public event ConnectionLostEventHandler ClientLost {
            add {
                receiver.ClientLost += value;
            }
            remove {
                receiver.ClientLost -= value;
            }
        }

        // 注意这个事件
        public event PortNumberReadyEventHandler PortNumberReady {
            add {
                ((MessageReceiver)receiver).PortNumberReady += value;
            }
            remove {
                ((MessageReceiver)receiver).PortNumberReady -= value;
            }
        }


        // 连接远程 - 使用主机名
        public bool ConnectByHost(string hostName, int port) {
            IPAddress[] ips = Dns.GetHostAddresses(hostName);
            return sender.Connect(ips[0], port);
        }

        // 连接远程 - 使用IP
        public bool ConnectByIp(string ip, int port) {
            IPAddress ipAddress;
            try {
                ipAddress = IPAddress.Parse(ip);
            } catch {
                return false;
            }
            return sender.Connect(ipAddress, port);
        }


        // 发送消息
        public bool SendMessage(Message msg) {
            return sender.SendMessage(msg);
        }

        // 释放资源，停止侦听
        public void Dispose() {
            try {
                sender.SignOut();
                receiver.StopListen();
            } catch {
            }
        }

        // 注销
        public void SignOut() {
            try {
                sender.SignOut();
            } catch {
            }
        }
    }
}
