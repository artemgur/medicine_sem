using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sem
{
	public class IndexArticle:ViewComponent
	{
		public IndexArticle()
		{
			
		}
		
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View("IndexArticleView");
		}
	}
}