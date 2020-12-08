using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sem.Pages
{
	public class Article : PageModel
	{
		public List<object> ArticlePars;
		public int CountArticleToUser;
		public List<List<object>> tags;

		public void OnGet(int index)
		{
			ArticlePars = DataBase.Select("SELECT * FROM articles WHERE article_id = " + index + ";")[0];
			var sel = DataBase.Select("SELECT DISTINCT(tags.*) FROM tags_to_articles, tags WHERE tags.tag_id = tags_to_articles.tag_id AND article_id = " + index + ";");
			if (sel != null)
				tags = sel;
			if (Request.Cookies["user_id"] != null)
				CountArticleToUser = DataBase.SelectCheck<int>("SELECT * FROM articles_to_users WHERE user_id = " + Request.Cookies["user_id"] + "AND article_id = " + index + " LIMIT 1;").Count;
		}

		public void OnPostAdd(int index)
		{
			OnGet(index);
			if (CountArticleToUser == 0)
				DataBase.Add("INSERT INTO articles_to_users VALUES (" + index + ", " + Request.Cookies["user_id"] + "); ");
		}

		public void OnPostRemove(int index)
		{
			OnGet(index);
			if (CountArticleToUser != 0)
				DataBase.Add("DELETE FROM articles_to_users WHERE user_id = " + Request.Cookies["user_id"] + " AND article_id = " + index + "; ");
		}
	}
}