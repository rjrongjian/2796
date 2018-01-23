using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepNaiWorkshop_2796
{
    class Const
    {
        public static String PROJECT_ID = "DeepNai 2796";//项目识别标记

        public static String SOFT_FLAG_FOR_REGISTER = "TMALLL";//软件本身的标记，防止开发的不同软件的注册码都能使用
        
        public static String VALUE_NAME_FOR_VALIDATE_IN_REGISTRY = "VALIDATE";//注册表中使用的识别码
        //在HKEY_LOCAL_MACHINE/SOFTWARE下开始创建
        public static String REGISTRY_LOCATION = PROJECT_ID;//注册表基项位置

        public static String VERSION = "V1.1";//版本号

        

        public static String COUPON_BACK_IMG_NAME = "template_ip61";//优惠券背景图片名

        public static String COUPON_BACK_IMG_NAME_ORDER = "order2";//订单截图背景图片

        public static String COUPON_FONT = "微软雅黑";//优惠券页面使用的字体

    }
}
