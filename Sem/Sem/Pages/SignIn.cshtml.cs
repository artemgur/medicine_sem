using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sem.Pages
{
	public class SignIn : PageModel
	{
		public static bool userView { get; set; } = false;
		public IActionResult OnPostSign(string login, string pas)
		{
			var message = "You entered an incorrect password or username!";
			var user = DB_Operations.UserOps.GetUser(login);
			if (user.User_id != -1)
			{
				var hash = DataBase.GenerateHash(pas, Encoding.Unicode.GetBytes(user.Salt.ToString()));
				if (user.Password == hash)
				{
					Response.Cookies.Delete("login");
					Response.Cookies.Delete("password");
					Response.Cookies.Delete("user_id");
					Response.Cookies.Append("login", login);
					Response.Cookies.Append("password", hash);
					Response.Cookies.Append("user_id", user.User_id.ToString());
					userView = true;
					message = "";
				}
			}

			return Content(message);
		}
	}
}