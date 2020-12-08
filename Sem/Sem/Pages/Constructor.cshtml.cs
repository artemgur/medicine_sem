using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sem.Pages
{
	public class Constructor : PageModel
	{
		public IActionResult OnPostCreate(string name, string description)
		{
			var message = "";
			var select = DataBase.Select("SELECT * FROM chats WHERE name = \'" + name + "\';");
			if (name != null && description != null && select.Count == 0)
            {
				DataBase.Add("INSERT INTO chats (name, short_description) VALUES (\'" + name + "\', \'" + description +"\');");
            }
			else
            {
				message = "This chat already exists!";
			}

			return Content(message);
		}
	}
}