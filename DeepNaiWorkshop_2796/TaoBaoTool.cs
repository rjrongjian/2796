using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeepNaiWorkshop_2796
{
    class TaoBaoTool
    {
        public static int GOOD_TYPE_TMALL = 1;
        public static int GOOD_TYPE_TAOBAO = 2;

        public RespMessage isTmallOrTaoBaoItemPage(String url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return new RespMessage(2,"地址不能为空");
            }

            String taoBaoItemPattern = "item.taobao.com/item.htm";
            String tmallTemPattern = "detail.tmall.com/item.htm";

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
            if(tmallOrTaoBao== TaoBaoTool.GOOD_TYPE_TMALL)
            {
               // Console.WriteLine(htmlContent);
                Regex reg = new Regex("[\\s\\S]*<title>([\\s\\S]*)-tmall.com天猫</title>[\\s\\S]*<img id=\"J_ImgBooth\"[\\s\\S]*?src=\"([\\s\\S]*?)\"[\\s\\S]*defaultItemPrice\":\"([\\s\\S]*?)\"[\\s\\S]*");
                Match match = reg.Match(htmlContent);
                String mainPic = match.Groups[2].Value;
                mainPic = StringTool.replaceStartWith(mainPic, "//", "http://");
                String name = match.Groups[1].Value;
                String price = match.Groups[3].Value;

                //TODO 抓取的数据
                //Console.WriteLine("获取的数据");
                //Console.WriteLine(mainPic);
                //Console.WriteLine(name);
                //Console.WriteLine(price);


                shopData.Name = name;
                shopData.MainPicStr = mainPic;
                shopData.MainPic = ImageTool.getImageBy(mainPic);
                String[] priceArr = price.Split('-');//说明有多个价格
                shopData.Price = double.Parse(priceArr[0].Trim());
                shopData.Volume = DataTool.randomVal(2500,1000);//销量
                int couponValue = (int)(shopData.Price * 0.4);
                shopData.CouponValue = couponValue < 20 ? couponValue : (couponValue+(10-(couponValue % 10)));//自定义券价值
                shopData.CouponValue = shopData.CouponValue > 100 ? 100 : shopData.CouponValue;


                return shopData;
            }
            else
            {
                // String goodName = 
                Regex reg = new Regex(".+?item:.+?pic\\s+?:\\s+?'(.+?)',.+<h3 class=\"tb - main - title\" data-title=\"(.+?)\" >.+<strong id=\"J_StrPrice\"><em class=\"tb - rmb\">&yen;</em><em class=\"tb - rmb - num\">(.+?)</em></strong>.+");
                Match match = reg.Match(htmlContent);
                String mainPic = match.Groups[1].Value;
                String name = match.Groups[2].Value;
                String price = match.Groups[3].Value;

                Console.WriteLine("获取的内容");
                Console.WriteLine(mainPic);
                Console.WriteLine(name);
                Console.WriteLine(price);
                return null;
            }

        }

    }
}
