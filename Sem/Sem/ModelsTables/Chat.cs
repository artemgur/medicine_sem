using System.Collections.Generic;

namespace Sem.ModelsTables
{
    public class Chat
    {
        public Chat(List<object> chatParams)
        {
            if (chatParams[0] != null)
                Name = chatParams[0].ToString();
            if (chatParams[1] != null)
                Short_description = chatParams[1].ToString();
            Chat_id = int.Parse(chatParams[2].ToString());
        }

        public string Name { get; private set; }

        public string Short_description { get; private set; }

        public int Chat_id { get; private set; }
    }
}
