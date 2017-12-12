using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepNaiWorkshop_2796
{
    class TaoBaoTool
    {

        public RespMessage isTmallOrTaoBaoItemPage(String url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return new RespMessage(2,"地址不能为空");
            }

            String taoBaoItemPattern = "detail.tmall.com/item.htm";
            String tmallTemPattern = "item.taobao.com/item.htm";

            if (url.Contains(taoBaoItemPattern) )
            {
                if (url.Contains("id=")){
                    return new RespMessage(1, "2");//淘宝
                }
            }

            if (url.Contains(tmallTemPattern))
            {
                if (url.Contains("id="))
                {
                    return new RespMessage(1,"1");//天猫
                }
            }


            return new RespMessage(2, "不是天猫或淘宝商品详情页地址");

        }

        public BaseDataBean parseShopData(int tmallOrTaoBao,String htmlContent)
        {

            BaseDataBean shopData = new BaseDataBean();
            if (String.IsNullOrEmpty(htmlContent))
            {
                return null;
            }

            return null;






        }

    }
}
