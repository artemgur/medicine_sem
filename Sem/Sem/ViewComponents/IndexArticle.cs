using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sem
{
	public class IndexArticle:ViewComponent
	{
		public IndexArticle()
		{
			
		}
		
		public async Task<IViewComponentResult> InvokeAsync(List<object> article)
		{
			return View("IndexArticleView", article);
		}
	}
}