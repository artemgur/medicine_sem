using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sem.DB_Operations;

namespace Sem.Pages
{
	public class ArticleConstructor : PageModel
	{
		[BindProperty] public IFormFile Image { get; set; }
		public IActionResult OnPost(string title, string text)
		{
			if (Image != null && title != null && text != null)
            {
				ArticleOps.AddArticle(Image, Request, title, text);
				return Redirect("./");
			}
			else
				return Redirect("./ArticleConstructor");
		}
	}
}