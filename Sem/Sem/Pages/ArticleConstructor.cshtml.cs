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
				return Redirect("./Article/" + ArticleOps.AddArticle(Image, Request, title, text));
			}
			else
				return Redirect("./ArticleConstructor");
		}
	}
}