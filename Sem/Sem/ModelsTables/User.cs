using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sem.ModelsTables
{
    public class User
    {
        public User(List<object> userParams)
        {
            Login = userParams[0].ToString();
            Password = userParams[1].ToString();
            Salt = userParams[2].ToString();
            Img = userParams[3] != null ? userParams[3].ToString() : "https://img.icons8.com/color/36/000000/administrator-male.png";
            User_id = int.Parse(userParams[4].ToString());
        }

        public string Login { get; private set; }

        public string Password { get; private set; }

        public string Salt { get; private set; }

        public string Img { get; private set; }

        public int User_id { get; private set; }
    }
}
