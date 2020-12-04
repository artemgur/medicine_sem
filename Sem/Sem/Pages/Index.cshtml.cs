using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Sem.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		public List<List<object>> Articles = DataBase.Select("SELECT * FROM articles;");

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}
	}
}