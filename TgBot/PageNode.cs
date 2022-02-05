using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagenode
{
    class PageNode // информация для статьи
    {
        public string tag { get; set; }
        public object attrs { get; set; }
        public string children { get; set; }
    }
}
