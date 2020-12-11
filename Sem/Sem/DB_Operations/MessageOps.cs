using Microsoft.AspNetCore.Http;
using Sem.ModelsTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sem.DB_Operations
{
    public class MessageOps
    {
        public static List<Message> GetChatMessages(int index)
        {
            return GetMessages("SELECT * FROM messages WHERE chat_id = " + index + ";");
        }

        public static void AddMessage(int forumId, HttpRequest request, string text)
        {
            try
            {
                DataBase.Add("INSERT INTO messages VALUES (" + forumId + ", " + request.Cookies["user_id"] + ", \'" + DataBase.ReplacingChars(text) + "\');");
            }
            catch
            {
                throw new Exception("Invalid data entered!");
            }
        }

        private static List<Message> GetMessages(string command)
        {
            var messages = new List<Message>();
            var messagesObjs = DataBase.Select(command);

            foreach (var message in messagesObjs)
                messages.Add(new Message(message));

            return messages;
        }
    }
}
