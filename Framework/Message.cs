using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public class Message
    {
        public enum MessageType
        {
            changeState
        }

        public MessageType type;
        public string content;

        public Message(MessageType type, string content)
        {
            this.type = type;
            this.content = content;
        }
    }
}
