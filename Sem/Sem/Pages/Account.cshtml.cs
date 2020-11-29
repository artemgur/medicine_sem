using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Threading.Tasks;

namespace Sem.Pages
{
	public class Account : PageModel
	{
		public bool hiddenBool { get; private set; } = true;

		public void OnGet()
		{

		}

		public async Task<IActionResult> OnPostExitAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			Response.Cookies.Delete("login");
			Response.Cookies.Delete("password");
			Response.Cookies.Delete("user_id");
			Pages.SignIn.userView = false;

			return RedirectToPage("./Index");
		}

		public async Task<IActionResult> OnPostChangeAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			if (Request.Form["login"] != Request.Cookies["login"])
			{
				DataBase.Select("UPDATE users SET login = \'" + Request.Form["login"] + "\' WHERE login = \'" + Request.Cookies["login"] + "\';");
				Response.Cookies.Delete("login");
				Response.Cookies.Append("login", Request.Form["login"]);
			}

			var salt = DataBase.Select("SELECT salt FROM users WHERE login = \'" + Request.Cookies["login"] + "\';")[0][0];
			var hash = DataBase.GenerateHash(Request.Form["old_password"], Encoding.Unicode.GetBytes((string)salt));

			if (hash == Request.Cookies["password"])
			{
				var newHash = DataBase.GenerateHash(Request.Form["new_password"], Encoding.Unicode.GetBytes(salt.ToString()));
				DataBase.Select("UPDATE users SET password = \'" + newHash + "\' WHERE password = \'" + hash + "\';");
				Response.Cookies.Delete("password");
				Response.Cookies.Append("password", newHash);
			}
			else
            {
				hiddenBool = false;
			}

			return RedirectToPage("./Index");
		}
	}
}