using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sem.ModelsTables
{
    public class Tag
    {
        public Tag(List<object> tagParams)
        {
            if (tagParams[0] != null)
                Name = tagParams[0].ToString();
        }

        public string Name { get; private set; }
    }
}
