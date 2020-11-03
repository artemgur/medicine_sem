using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sem
{
	public class IndexArticleViewComponent:ViewComponent
	{
		public IndexArticleViewComponent()
		{
			
		}
		
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View("IndexArticleView");
		}
	}
}