using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convertdata
{
    class CreateJsonFile
    {
        public string FormirateString(PageNode page) // формирование строки запроса, подходящего для сайта telegraph
        {
            string result = "[{\"tag\":\"";
            result += page.tag + "\",\"attrs\":\"" + page.attrs + "\",\"children\":[\"" + page.children + "\"]}]";


            return result;
        }
    }
}
