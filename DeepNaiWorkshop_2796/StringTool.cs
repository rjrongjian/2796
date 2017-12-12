using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepNaiWorkshop_2796
{
    class StringTool
    {
        public static String replaceStartWith(String str,String startWith,String replaceWithStr)
        {
            if (str.StartsWith(startWith))
            {
                return replaceWithStr + str.Substring(str.IndexOf(startWith));
            }
            else
            {
                return str;
            }
        }
    }
}
