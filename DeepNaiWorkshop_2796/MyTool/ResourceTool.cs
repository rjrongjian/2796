using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepNaiWorkshop_2796.MyTool
{
    class ResourceTool
    {
        /// <summary>
        /// 读取项目中resources中的资源
        /// </summary>
        /// <param name="imageNameWithoutSuffix">文件名不带后缀</param>
        /// <returns></returns>
        public static Image getImage(String imageNameWithoutSuffix)
        {
           return  (Image)Properties.Resources.ResourceManager.GetObject(imageNameWithoutSuffix);
        }
    }
}
