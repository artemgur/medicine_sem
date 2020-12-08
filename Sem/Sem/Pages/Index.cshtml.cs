using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		public List<List<object>> Articles;

		public void OnGet()
        {
			if (Articles == null)
				Articles = DataBase.Select("SELECT * FROM articles;");
		}

		public void OnGetSearch(string array, string name)
        {
			var answer = new StringBuilder();
			if (name != null)
			{
				answer.Append(" AND ");
				answer.Append("title LIKE \'%");
				answer.Append(name);
				answer.Append("%\'");
			}
			if (array != "Теги" && array != null)
			{
				var arr = array.Split(", ");
				for (int i = 0; i < arr.Length; i++)
				{
					answer.Append(" AND ");
					answer.Append("tag = \'");
					answer.Append(arr[i].ToString());
					answer.Append("\'");
				}

			}
			Articles = DataBase.Select("SELECT DISTINCT(articles.*) FROM articles, tags_to_articles, tags WHERE tags_to_articles.tag_id = tags.tag_id AND tags_to_articles.article_id = articles.article_id" + answer + ";");
			OnGet();
		}

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}
	}
}