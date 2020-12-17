using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;

namespace Sem.Operations
{
    public static class AccountOps
    {
		public static void ExitUser(HttpResponse response)
        {
			response.Cookies.Delete("login");
			response.Cookies.Delete("password");
			response.Cookies.Delete("user_id");
			Pages.SignIn.userView = false;
		}

        public static void SetImage(IFormFile image, HttpRequest request)
        {
			if (image != null)
			{
				var user_id = request.Cookies["user_id"];
				var files = Directory.GetFiles(@"wwwroot\img\", user_id + ".*");
				foreach (var e in files)
					File.Delete(e);
				var file = image;
				var extension = Path.GetExtension(file.FileName);

				using (var fileStream = File.Open(@$"wwwroot\img\{user_id}{extension}", FileMode.Create))
				{
					file.CopyTo(fileStream);
					fileStream.Close();
				}

				request.HttpContext.Response.Cookies.Delete("img_user");
				request.HttpContext.Response.Cookies.Append("img_user", @$"/img/{user_id}{extension}");

				DataBase.Add("UPDATE users SET img = \'" + @$"/img/{user_id}{extension}" + "\' WHERE user_id = \'" + user_id + "\';");
			}
		}

		public static string Change(string login, string old_pas, string new_pas, HttpRequest request)
        {
			var currentLogin = request.Cookies["login"];
			string message = "";

			message += ChangeLogin(ref currentLogin, login, request.HttpContext.Response);

			message += ChangePassword(old_pas, new_pas, currentLogin, request);

			return message;
		}


		private static string ChangeLogin(ref string currentLogin, string login, HttpResponse response)
        {
			if (login != null && login != currentLogin)
			{
				DataBase.Select("UPDATE users SET login = \'" + login + "\' WHERE login = \'" + currentLogin + "\';");
				response.Cookies.Delete("login");
				response.Cookies.Append("login", login);
				currentLogin = login;
				return "";
			}
			else
				return "login is not changed. ";
		}

		private static string ChangePassword(string old_pas, string new_pas, string currentLogin, HttpRequest request)
		{
			string result = "";
			if (old_pas != null && new_pas != null)
			{
				var select = DataBase.Select("SELECT salt FROM users WHERE password = \'" + currentLogin + "\';");
				var salt = select[0][0];
				var hash = DataBase.GenerateHash(old_pas, Encoding.Unicode.GetBytes((string)salt));

				if (DataBase.GenerateHash(new_pas, Encoding.Unicode.GetBytes(salt.ToString())) == request.Cookies["password"])
				{
					if (hash != request.Cookies["password"])
					{
						var newHash = DataBase.GenerateHash(request.Form["new_password"], Encoding.Unicode.GetBytes(salt.ToString()));
						DataBase.Select("UPDATE users SET password = \'" + newHash + "\' WHERE password = \'" + hash + "\';");
						request.HttpContext.Response.Cookies.Delete("password");
						request.HttpContext.Response.Cookies.Append("password", newHash);
					}
					else
						result += "The passwords match. ";
				}
				else
					result += "The password was entered incorrectly. ";
			}

			return result;
		}
	}
}
