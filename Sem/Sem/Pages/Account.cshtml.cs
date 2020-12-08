using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sem.Pages
{
	public class Account : PageModel
	{
		[BindProperty] public IFormFile Image { get; set; }

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
		public void OnPost()
		{
			if (Image != null)
			{
				var user_id = Request.Cookies["user_id"];
				var files = Directory.GetFiles(@"wwwroot\img\", user_id + ".*");
				foreach (var e in files)
					System.IO.File.Delete(e);
				var file = Image;
				var extension = Path.GetExtension(file.FileName);

				using (var fileStream = System.IO.File.Open(@$"wwwroot\img\{user_id}{extension}", FileMode.Create))
				{
					file.CopyTo(fileStream);
					fileStream.Close();
				}

				Response.Cookies.Delete("img_user");
				Response.Cookies.Append("img_user", @$"/img/{user_id}{extension}");

				DataBase.Add("UPDATE users SET img = \'" + @$"/img/{user_id}{extension}" + "\' WHERE user_id = \'" + user_id + "\';");
			}
		}
	}
}