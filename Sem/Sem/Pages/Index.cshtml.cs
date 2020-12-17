using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Sem.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		public List<ModelsTables.Article> Articles;

		public void OnGet()
        {
			if (Articles == null)
			Articles = DB_Operations.ArticleOps.GetAllArticles();

		}

		public void OnGetSearch(string array, string name)
		{
			Articles = DataBase.SearchFromTags(name, array);
		}

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}
	}
}