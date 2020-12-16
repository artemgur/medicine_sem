﻿using Microsoft.AspNetCore.Http;
using Sem.ModelsTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sem.DB_Operations
{
    public static class ArticleOps
    {
        public static List<Article> GetAccountArticles(HttpRequest request)
        {
            return GetArticles("SELECT articles.* FROM articles_to_users, articles WHERE user_id = " +
                request.Cookies["user_id"] +
                " AND articles_to_users.article_id = articles.article_id;");
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
            return GetArticles("SELECT DISTINCT(articles.*) FROM articles WHERE " + title + "(SELECT COUNT(DISTINCT(tags.*)) FROM tags_to_articles, tags WHERE tags_to_articles.tag = tags.tag AND tags_to_articles.article_id = articles.article_id" + tags + ") > " + (countTags - 1) + ";");
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
