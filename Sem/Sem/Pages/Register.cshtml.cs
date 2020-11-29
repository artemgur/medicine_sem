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
		public bool hiddenBool { get; private set; } = true;
		public string message { get; private set; } = "";
		public string typeMessage { get; private set; } = "alert alert-danger";

		public async Task<IActionResult> OnPostAsync()
		{
			string[] paramsReq = new[] { Request.Form[DataBase.pars[0]].First(),
				Request.Form[DataBase.pars[1]].First(),
				Request.Form["repeat_password"].First() };

			for (int i = 0; i < 3; i++)
			{
				if (paramsReq[i] == "")
				{
					message = DataBase.pars[i] + " field must not be empty!";
					break;
				}
				else if (paramsReq[i].Length < 9)
				{
					message = DataBase.pars[i] + " must contain more than 8 characters!";
					break;
				}
				if (paramsReq[i].Length > 50)
				{
					message = DataBase.pars[i] + " must not exceed 50 characters!";
					break;
				}
			}
			if (message == "")
			{
				if (Regex.IsMatch(paramsReq[1], @"[А-Яа-я]+"))
				{
					message = "Password must contain only Latin letters and signs!";
				}
				else if (paramsReq[1] != paramsReq[2])
				{
					message = "Passwords do not match!";
				}
				else
				{
					var selectLogin = DataBase.SelectCheck<string>("SELECT login FROM users WHERE login = \'" + paramsReq[0] + "\';");
					if (selectLogin.Count == 0)
					{
						DataBase.Add("INSERT INTO users (login, salt, password) VALUES" +
						"(\'" + paramsReq[0] + "\', \'" + Encoding.Unicode.GetString(DataBase.RandomSalt()) + "\', \'" + DataBase.GenerateHash(paramsReq[1], DataBase.salt) + "\');");
						Response.Cookies.Delete("login");
						Response.Cookies.Delete("password");
						Response.Cookies.Append("login", paramsReq[0]);
						Response.Cookies.Append("password", DataBase.GenerateHash(paramsReq[1], DataBase.salt));
						Pages.SignIn.userView = true;
						Response.Cookies.Append("user_id", DataBase.Select("SELECT user_id FROM users WHERE login = \'" + paramsReq[0] + "\';")[0][0].ToString());

						return RedirectToPage("./Index");
					}
					else
					{
						message = "A user with this " + DataBase.pars[0] + " already exists!";
					}
				}
			}
			hiddenBool = false;
			return Page();
		}
	}
}