using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Threading.Tasks;

namespace Sem.Pages
{
	public class Account : PageModel
	{

		public IActionResult OnPostExit()
		{
			Response.Cookies.Delete("login");
			Response.Cookies.Delete("password");
			Response.Cookies.Delete("user_id");
			Pages.SignIn.userView = false;

			return Content("");
		}

		public IActionResult OnPostChange(string login, string old_pas, string new_pas)
		{
			var message = "";
			var currentLogin = Request.Cookies["login"];

			if (login != null && login != currentLogin)
			{
				DataBase.Select("UPDATE users SET login = \'" + login + "\' WHERE login = \'" + currentLogin + "\';");
				Response.Cookies.Delete("login");
				Response.Cookies.Append("login", login);
				currentLogin = login;
			}
			else
				message += "login is not changed";


			if (old_pas != null && new_pas != null)
			{
				var select = DataBase.Select("SELECT salt FROM users WHERE password = \'" + currentLogin + "\';");
				var salt = select[0][0];
				var hash = DataBase.GenerateHash(old_pas, Encoding.Unicode.GetBytes((string)salt));

				if (DataBase.GenerateHash(new_pas, Encoding.Unicode.GetBytes(salt.ToString())) == Request.Cookies["password"])
				{
					if (hash != Request.Cookies["password"])
					{
						var newHash = DataBase.GenerateHash(Request.Form["new_password"], Encoding.Unicode.GetBytes(salt.ToString()));
						DataBase.Select("UPDATE users SET password = \'" + newHash + "\' WHERE password = \'" + hash + "\';");
						Response.Cookies.Delete("password");
						Response.Cookies.Append("password", newHash);
					}
					else
						message += "The passwords match. ";
				}
				else
					message += "The password was entered incorrectly. ";
			}

			return Content(message);
		}
	}
}