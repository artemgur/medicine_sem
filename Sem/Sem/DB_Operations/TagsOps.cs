using Sem.ModelsTables;
using System.Collections.Generic;
using System.Linq;

namespace Sem.DB_Operations
{
    public class TagsOps
    {
        public static void AddTagsToArticleIndex(string[] tags, int id)
        {
            if (tags != null && tags.Length != 0)
            {
                for (int i = 0; i < tags.Length; i++)
                {
                    AddTag(tags[i], id);
                }
            }
        }

        public static void AddTag(string tag, int id)
        {
            DataBase.Add("INSERT INTO tags_to_articles (tag, article_id) VALUES (\'" + tag + "\', " + id + ")");
        }

        public static List<Tag> GetArticleTags(int index)
        {
            return GetTags("SELECT tag FROM tags_to_articles WHERE article_id = " + index + ";");
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
