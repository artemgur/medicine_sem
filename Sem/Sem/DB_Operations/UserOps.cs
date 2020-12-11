using Sem.ModelsTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sem.DB_Operations
{
    public class UserOps
    {
        public static User GetUser(int userId)
        {
            return GetUsers("SELECT * FROM users WHERE user_id = " + userId + ";").FirstOrDefault();
        }

        public static User GetUser(string login)
        {
            return GetUsers("SELECT * FROM users WHERE login = " + login + ";").FirstOrDefault();
        }

        public static void Add(string login, string pas)
        {
            try
            {
                DataBase.Add("INSERT INTO users (login, salt, password) VALUES" +
                                    "(\'" + login + "\', \'" + Encoding.Unicode.GetString(DataBase.RandomSalt()) + "\', \'" + DataBase.GenerateHash(pas, DataBase.salt) + "\');");
            }
            catch
            {
                throw new Exception("Invalid data entered!");
            }
        }

        private static List<User> GetUsers(string command)
        {
            var users = new List<User>();
            var usersObjs = DataBase.Select(command);

            foreach (var user in usersObjs)
                users.Add(new User(user));

            return users;
        }
    }
}
