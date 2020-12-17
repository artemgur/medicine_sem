using System;
using System.Collections.Generic;
using System.Linq;

namespace Sem.ModelsTables
{
    public class Article
    {
        public Article(List<object> articleParams)
        {
            Title = articleParams[0].ToString();
            Image = articleParams[1] == null || articleParams[1].ToString() == "" ? "/img/placeholder.jpg" : articleParams[1].ToString();
            Description = articleParams[2].ToString();
            var setDate = articleParams[3].ToString().Split(' ', ':', '.').Select(x => int.Parse(x)).ToArray();
            Date = new DateTime(setDate[2], setDate[1], setDate[0]);
            Article_id = int.Parse(articleParams[4].ToString());
        }

        public string Title { get; private set; }

        public string Image { get; private set; }

        public string Description { get; private set; }

        public DateTime Date { get; private set; }

        public int Article_id { get; private set; }
    }
}
