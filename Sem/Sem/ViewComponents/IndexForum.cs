using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sem.ModelsTables;

namespace Sem
{
	public class IndexForum:ViewComponent
	{
		public IndexForum()
		{
			
		}
		
		public async Task<IViewComponentResult> InvokeAsync(Chat forumIndex)
		{
			return View("IndexForumView", forumIndex);
		}
	}
}