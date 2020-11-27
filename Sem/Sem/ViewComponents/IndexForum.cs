using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sem
{
	public class IndexForum:ViewComponent
	{
		public IndexForum()
		{
			
		}
		
		public async Task<IViewComponentResult> InvokeAsync(List<object> forumIndex)
		{
			return View("IndexForumView", forumIndex);
		}
	}
}