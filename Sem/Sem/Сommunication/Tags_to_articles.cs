using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sem.Сommunication
{
    public class Tags_to_articles : Communication
    {
        public Tags_to_articles(int tag_id, int article_id)
        {
            SetId("tags_to_articles", tag_id, article_id, "tag_id", "article_id");
        }
    }
}
