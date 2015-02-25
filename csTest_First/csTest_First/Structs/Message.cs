using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csTest_First
{
    public struct Message
    {
        private readonly string userName;
        private readonly string content;
        private readonly string postDate;

        public Message(string userName, string content)
        {
            this.userName = userName;
            this.content = content;
            this.postDate = DateTime.Now.ToString();
        }

        public Message(string content) : this("System", content) { }

        public string UserName
        {
            get { return userName; }
        }
        public string Content
        {
            get { return content; }
        }
        public string PostDate
        {
            get { return postDate; }
        }

        public string ToString()
        {
            return String.Format("{0}[{1}]:\r\n{2}\r\n",userName, postDate, content);
        }
    }
}
