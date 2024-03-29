﻿using Microsoft.AspNetCore.Http;
using Sem.ModelsTables;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sem.DB_Operations
{
    public static class ArticleOps
    {
        public static List<Article> GetAccountArticles(HttpRequest request)
        {
            return GetArticles("SELECT articles.* FROM articles_to_users, articles WHERE articles_to_users.user_id = " +
                request.Cookies["user_id"] +
                " AND articles_to_users.article_id = articles.article_id;");
        }
        public static int AddArticle(IFormFile image, string name, string description, string user_id)
        {
            var all = GetAllArticles();
            int article_id;
            if (all.Count != 0)
                article_id = all.Count + 1;
            else
                article_id = 1;
            var files = Directory.GetFiles(@"wwwroot\img\article\", article_id + ".*");
            foreach (var e in files)
                File.Delete(e);
            var file = image;
            var extension = Path.GetExtension(file.FileName);

            using (var fileStream = File.Open(@$"wwwroot\img\article\{article_id}{extension}", FileMode.Create))
            {
                file.CopyTo(fileStream);
                fileStream.Close();
            }

            DataBase.Add("INSERT INTO articles (article_id, image, title, description, user_id) VALUES (" + article_id + ", \'" + @$"/img/article/{article_id}{extension}" + "\', \'" + name + "\', \'" + description + "\', " + user_id + ");");
            return article_id;
        }

        public static Article GetArticle(int index)
        {
            return GetArticles("SELECT * FROM articles WHERE article_id = " + index + ";").FirstOrDefault();
        }

        public static List<Article> GetAllArticles()
        {
            return GetArticles("SELECT * FROM articles;");
        }

        public static List<Article> GetSearchArticles(string title, string tags, int countTags)
        {
            return GetArticles("SELECT DISTINCT(articles.*) FROM articles WHERE " + title + "(SELECT COUNT(tag) FROM tags_to_articles WHERE tags_to_articles.article_id = articles.article_id" + tags + ") > " + (countTags - 1) + ";");
        }

        private static List<Article> GetArticles(string command)
        {
            var articles = new List<Article>();
            var articlesObjs = DataBase.Select(command);

            foreach (var article in articlesObjs)
                articles.Add(new Article(article));

            return articles;
        }
    }
}
