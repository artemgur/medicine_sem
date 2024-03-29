﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sem.Operations;

namespace Sem.Pages
{
	public class Account : PageModel
	{
		[BindProperty] public IFormFile Image { get; set; }

		public IActionResult OnPostExit()
		{
			AccountOps.ExitUser(Response);
			return Content("");
		}

		public IActionResult OnPostChange(string login, string old_pas, string new_pas)
		{
			return Content(AccountOps.Change(login, old_pas, new_pas, Request));
		}
		public void OnPost()
		{
			AccountOps.SetImage(Image, Request);
		}
	}
}