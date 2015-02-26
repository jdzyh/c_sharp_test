using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace csTest_First
{
    public partial class PrimaryForm : Form
    {

        private Talker talker;
        private string userName;

        public PrimaryForm(string name) {

            InitializeComponent();

            userName = lbName.Text = name;
            this.talker = new Talker();
            this.Text = userName + " Talking ...";

            //----------------------------------------------------------------
            // 注册各种事件所调用的函数
            //----------------------------------------------------------------
            talker.ClientLost +=
                new ConnectionLostEventHandler(talker_ClientLost);
            talker.ClientConnected +=
                new ClientConnectedEventHandler(talker_ClientConnected);
            talker.MessageReceived +=
                new MessageReceivedEventHandler(talker_MessageReceived);
            talker.PortNumberReady +=
                new PortNumberReadyEventHandler(PrimaryForm_PortNumberReady);
        }

        void ConnectStatus() { }
        void DisconnectStatus() { }

        //----------------------------------------------------------------
        // 待注册的四个函数
        //----------------------------------------------------------------
        // 端口号OK
        void PrimaryForm_PortNumberReady(int portNumber) {
            PortNumberReadyEventHandler del = delegate(int port) {
                lbPort.Text = port.ToString();
            };
            lbPort.Invoke(del, portNumber); /** inoke : 确保在主线程中修改控件的属性*/
        }

        // 接收到消息
        void talker_MessageReceived(string msg) {
            MessageReceivedEventHandler del = delegate(string m) {
                txtContent.Text += m;
            };
            txtContent.Invoke(del, msg);
        }

        // 有客户端连接到本机
        void talker_ClientConnected(IPEndPoint endPoint) {
            ClientConnectedEventHandler del = delegate(IPEndPoint end) {
                IPHostEntry host = Dns.GetHostEntry(end.Address);
                txtContent.Text +=
                    String.Format("System[{0}]：\r\n远程主机{1}连接至本地。\r\n", DateTime.Now, end);
            };
            txtContent.Invoke(del, endPoint);
        }

        // 客户端连接断开
        void talker_ClientLost(string info) {
            ConnectionLostEventHandler del = delegate(string information) {
                txtContent.Text +=
                    String.Format("System[{0}]：\r\n{1}\r\n", DateTime.Now, information);
            };
            txtContent.Invoke(del, info);
        }

        //----------------------------------------------------------------
        // 控件的事件函数
        //----------------------------------------------------------------
        // 发送消息
        private void btnSend_Click(object sender, EventArgs e) {
            if (String.IsNullOrEmpty(txtMessage.Text)) {
                MessageBox.Show("请输入内容！");
                txtMessage.Clear();
                txtMessage.Focus();
                return;
            }

            Message msg = new Message(userName, txtMessage.Text);
            if (talker.SendMessage(msg)) {
                txtContent.Text += msg.ToString();
                //txtMessage.Clear();
            } else {
                txtContent.Text +=
                    String.Format("System[{0}]：\r\n远程主机已断开连接\r\n", DateTime.Now);
                DisconnectStatus();
            }
        }

        // 点击连接
        private void btnConnect_Click(object sender, EventArgs e) {
            string host = txtHost.Text;
            string ip = txtHost.Text;
            int port;

            if (String.IsNullOrEmpty(txtHost.Text)) {
                MessageBox.Show("主机名称或地址不能为空");
            }

            try {
                port = Convert.ToInt32(txtPort.Text);
            } catch {
                MessageBox.Show("端口号不能为空，且必须为数字");
                return;
            }

            if (talker.ConnectByHost(host, port)) {
                ConnectStatus();
                txtContent.Text +=
                    String.Format("System[{0}]：\r\n已成功连接至远程\r\n", DateTime.Now);
                return;
            } else {
                MessageBox.Show("无法按照 host 连接");
            }
            //try {
            //    talker.ConnectByHost(host, port);
            //    ConnectStatus();
            //    txtContent.Text +=
            //        String.Format("System[{0}]：\r\n已成功连接至远程\r\n", DateTime.Now);
            //} catch {
            //    MessageBox.Show("无法按照 host 连接");
            //}

            if (talker.ConnectByIp(ip, port)) {
                ConnectStatus();
                txtContent.Text +=
                    String.Format("System[{0}]：\r\n已成功连接至远程\r\n", DateTime.Now);
            } else {
                MessageBox.Show("远程主机不存在，或者拒绝连接！");
            }

            txtMessage.Focus();
        }

        // 关闭按钮点按
        private void btnClose_Click(object sender, EventArgs e) {
            try {
                talker.Dispose();
                Application.Exit();
            } catch {
            }
        }
        // 直接点击右上角的叉
        private void PrimaryForm_FormClosing(object sender, FormClosingEventArgs e) {
            try {
                talker.Dispose();
                Application.Exit();
            } catch {
            }
        }
        // 点击注销
        private void btnSignout_Click(object sender, EventArgs e) {
            talker.SignOut();
            DisconnectStatus();
            txtContent.Text +=
                String.Format("System[{0}]：\r\n已经注销\r\n", DateTime.Now);
        }

        private void btnClear_Click(object sender, EventArgs e) {
            txtContent.Clear();
        }
    }
}
