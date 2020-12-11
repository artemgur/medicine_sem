using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sem.ModelsTables;

namespace Sem
{
	public class IndexArticle:ViewComponent
	{
		public IndexArticle()
		{
			
		}
		
		public async Task<IViewComponentResult> InvokeAsync(Article article)
		{
			return View("IndexArticleView", article);
		}
	}
}