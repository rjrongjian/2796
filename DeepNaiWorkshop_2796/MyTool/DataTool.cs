using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepNaiWorkshop_2796.MyTool
{
    class DataTool
    {
        /*
         * 随机生成一个值，基值+随机值
         * 
         */
        public static int randomVal(int baseVal,int maxVal)
        {
            Random random = new Random();
            int result = random.Next(maxVal) +baseVal+1;
            
            return result;
        }
    }
}
