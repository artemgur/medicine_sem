﻿using Sem.ModelsTables;
using System.Collections.Generic;

namespace Sem.DB_Operations
{
    public class TagsOps
    {
        public static List<Tag> GetArticleTags(int index)
        {
            return GetTags("SELECT DISTINCT(tags.*) FROM tags_to_articles, tags WHERE tags.tag = tags_to_articles.tag AND article_id = " + 
                index + ";");
        }

        private static List<Tag> GetTags(string command)
        {
            var tags = new List<Tag>();
            var tagsObjs = DataBase.Select(command);

            foreach (var tag in tagsObjs)
                tags.Add(new Tag(tag));

            return tags;
        }
    }
}
