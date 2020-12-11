using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sem
{
	public class AccountSidebar:ViewComponent
	{
		public AccountSidebar()
		{
			
		}
		
		public async Task<IViewComponentResult> InvokeAsync(object img)
		{
			return View("AccountSidebarView", img);
		}
	}
}