using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sem.ModelsTables
{
    public class Message
    {
        public Message(List<object> MessageParams)
        {
            Chat_id = int.Parse(MessageParams[0].ToString());
            User_id = int.Parse(MessageParams[1].ToString());
            Text = MessageParams[2].ToString();
        }

        public int Chat_id { get; private set; }

        public int User_id { get; private set; }

        public string Text { get; private set; }
    }
}
