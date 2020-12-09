using Microsoft.AspNetCore.Http;
using Sem.ModelsTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sem.DB_Operations
{
    public class ForumOps
    {
        public static List<Chat> GetAccountChats(HttpRequest request)
        {
            return GetArticles("SELECT chats.* FROM chats_to_users, chats WHERE user_id = " +
                request.Cookies["user_id"] +
                " AND chats_to_users.chat_id = chats.chat_id;");
        }

        private static List<Chat> GetArticles(string command)
        {
            var chats = new List<Chat>();
            var chatsObjs = DataBase.Select(command);

            foreach (var chat in chatsObjs)
                chats.Add(new Chat(chat));

            return chats;
        }
    }
}
