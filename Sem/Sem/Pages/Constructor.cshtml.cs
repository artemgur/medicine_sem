using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sem.Pages
{
	public class Constructor : PageModel
	{
		public IActionResult OnPostCreate(string name, string description)
		{
			var message = "";
			if (name != null && description != null && DB_Operations.ChatsOps.CountForName(name) == 0)
            {
				DB_Operations.ChatsOps.AddChat(name, description);

			}
			else
            {
				message = "This chat already exists!";
			}

			return Content(message);
		}
	}
}