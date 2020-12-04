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
			var select = DataBase.Select("SELECT password, salt, user_id FROM users WHERE login = \'" + login + "\';");
			if (select.Count != 0)
			{
				var hash = DataBase.GenerateHash(pas, Encoding.Unicode.GetBytes(select[0][1].ToString()));
				if (select[0][0].ToString() == hash)
				{
					Response.Cookies.Delete("login");
					Response.Cookies.Delete("password");
					Response.Cookies.Delete("user_id");
					Response.Cookies.Append("login", login);
					Response.Cookies.Append("password", hash);
					Response.Cookies.Append("user_id", select[0][2].ToString());
					userView = true;
					message = "";
				}
			}

			return Content(message);
		}
	}
}