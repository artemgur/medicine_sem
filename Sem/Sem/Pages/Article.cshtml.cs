using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sem.Pages
{
	public class Article : PageModel
	{
		public List<object> ArticlePars;

		public void OnGet(int articleId)
		{
			ArticlePars = DataBase.Select("SELECT * FROM articles WHERE article_id = " + articleId + ";")[0];
		}

		public void OnPostAdd(int articleId)
		{
			OnGet(articleId);
			if (DataBase.SelectCheck<int>("SELECT * FROM articles_to_users WHERE user_id = " + Request.Cookies["user_id"] + "AND article_id = " + articleId + ";").Count == 0)
				DataBase.Add("INSERT INTO articles_to_users VALUES (" + Request.Cookies["user_id"] + ", " + articleId + "); ");
		}

		public void OnPostRemove(int articleId)
		{
			OnGet(articleId);
			if (DataBase.SelectCheck<int>("SELECT * FROM articles_to_users WHERE user_id = " + Request.Cookies["user_id"] + "AND article_id = " + articleId + ";").Count != 0)
				DataBase.Add("DELETE FROM articles_to_users WHERE user_id = " + Request.Cookies["user_id"] + " AND article_id = " + articleId + "; ");
		}
	}
}