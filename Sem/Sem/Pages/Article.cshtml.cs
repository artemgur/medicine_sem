using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Sem.DB_Operations;
using Sem.ModelsTables;
using System.Linq;

namespace Sem.Pages
{
	public class Article : PageModel
	{
		public ModelsTables.Article ArticlePars;
		public int CountArticleToUser = -1;
		public List<string> tags;

		public void OnGet(int index)
		{
			ArticlePars = ArticleOps.GetArticle(index);
			var articleTags = TagsOps.GetArticleTags(index).Select(x => x.Name).ToList();
			if (articleTags != null)
				tags = articleTags;
			if (Request.Cookies["user_id"] != null)
				CountArticleToUser = new Сommunication.Articles_to_users(index, int.Parse(Request.Cookies["user_id"])).CountComs();
		}

		public void OnPostAdd(int index)
		{
			OnGet(index);
			if (CountArticleToUser == 0)
				new Сommunication.Articles_to_users(index, int.Parse(Request.Cookies["user_id"])).Add();
		}

		public void OnPostRemove(int index)
		{
			OnGet(index);
			if (CountArticleToUser != 0)
				new Сommunication.Articles_to_users(index, int.Parse(Request.Cookies["user_id"])).Remove();
		}
	}
}