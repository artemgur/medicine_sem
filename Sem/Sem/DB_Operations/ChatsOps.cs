using Sem.ModelsTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sem.DB_Operations
{
    public class ChatsOps
    {
        public static int CountForName(string name)
        {
            return GetChats("SELECT * FROM chats WHERE name = \'" + name + "\';").Count;
        }

        public static int AddChat(string name, string description)
        {
            var id = GetAllChats().Count + 1;
            try
            {
                DataBase.Add("INSERT INTO chats (name, short_description) VALUES (\'" + name + "\', \'" + description + "\');");
            }
            catch
            {
                throw new Exception("Invalid data entered!");
            }
            return id;
        }

        public static Chat GetChat(int index)
        {
            return GetChats("SELECT * FROM chats WHERE chat_id = " + index + ";").FirstOrDefault();
        }

        public static List<Chat> GetAllChats()
        {
            return GetChats("SELECT * FROM chats;");
        }

        private static List<Chat> GetChats(string command)
        {
            var chats = new List<Chat>();
            var chatsObjs = DataBase.Select(command);

            foreach (var chat in chatsObjs)
                chats.Add(new Chat(chat));

            return chats;
        }
    }
}
