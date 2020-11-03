using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sem
{
	public class IndexForum:ViewComponent
	{
		public IndexForum()
		{
			
		}
		
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View("IndexForumView");
		}
	}
}