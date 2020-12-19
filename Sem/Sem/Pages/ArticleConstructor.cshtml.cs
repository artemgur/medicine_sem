using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sem.DB_Operations;

namespace Sem.Pages
{
	public class ArticleConstructor : PageModel
	{
		[BindProperty] public IFormFile Image { get; set; }
		public IActionResult OnPost(string title, string text, string[] tags)
		{
			if (Image != null && title != null && text != null)
            {
				int id = ArticleOps.AddArticle(Image, Request, title, text);
				TagsOps.AddTagsToArticleIndex(tags, id);
				return Redirect("./Article/" + id);
			}
			else
				return Redirect("./ArticleConstructor");
		}
	}
}