using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sem.Pages
{
	public class Register : PageModel
	{

		public IActionResult OnPostRegister(string login, string pas, string repeat_pas)
		{
			var message = "";
			if (login == "" || pas == "")
			{
				message = "Filed field must not be empty!";
			}
			else if (login == null || pas == null || login.Length < 5 || pas.Length < 5)
			{
				message = "Field must contain more than 4 characters!";
			}
			else if (login.Length > 49 || pas.Length > 49)
			{
				message = "Field must not exceed 50 characters!";
			}
			else if (Regex.IsMatch(pas, @"[А-Яа-я]+"))
			{
				message = "Password must contain only Latin letters and signs!";
			}
			else if (pas != repeat_pas)
			{
				message = "Passwords do not match!";
			}
			else
			{
				var user = DB_Operations.UserOps.GetUser(login);
				if (user == null)
				{
					DB_Operations.UserOps.Add(login, pas);
					Response.Cookies.Delete("login");
					Response.Cookies.Delete("password");
					Response.Cookies.Delete("user_id");
					Response.Cookies.Append("login", login);
					Response.Cookies.Append("password", DataBase.GenerateHash(pas, DataBase.salt));
					Pages.SignIn.userView = true;
					user = DB_Operations.UserOps.GetUser(login);
					Response.Cookies.Append("user_id", user.User_id.ToString());
				}
				else
				{
					message = "A user with this " + DataBase.pars[0] + " already exists!";
				}
			}
			return Content(message);
		}
	}
}