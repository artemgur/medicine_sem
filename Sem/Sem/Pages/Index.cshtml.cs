using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Sem.Сommunication;

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