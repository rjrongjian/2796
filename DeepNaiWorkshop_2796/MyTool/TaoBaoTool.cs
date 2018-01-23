using System;
using MyTools;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeepNaiWorkshop_2796.MyTool
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
            //Console.WriteLine(htmlContent);
            if (String.IsNullOrEmpty(htmlContent))
            {
                return null;
            }
            if(tmallOrTaoBao== TaoBaoTool.GOOD_TYPE_TMALL)
            {
                //Console.WriteLine(htmlContent);
                Regex reg = new Regex("([\\s\\S]*)<title>([\\s\\S]*)-tmall.com天猫</title>[\\s\\S]*<img id=\"J_ImgBooth\"[\\s\\S]*?src=\"([\\s\\S]*?)\"[\\s\\S]*<input type=\"hidden\" name=\"seller_nickname\" value=\"([\\s\\S]*?)\" />[\\s\\S]*defaultItemPrice\":\"([\\s\\S]*?)\"([\\s\\S]*)");

                Match match = reg.Match(htmlContent);

                String mainPic = match.Groups[3].Value;
                mainPic = StringTool.replaceStartWith(mainPic, "//", "http://");

                String name = match.Groups[2].Value;
                String shopName = match.Groups[4].Value;
                String price = match.Groups[5].Value;

                String subHtmlContent = match.Groups[1].Value;
                //Console.WriteLine(subHtmlContent);
                Regex reg2 = new Regex("[\\s\\S]*w.g_config={[\\s\\S]*?itemId:\"([0-9]+)\"[\\s\\S]+sellerId:\"([0-9]+)\"[\\s\\S]*");
                Match match2 = reg2.Match(subHtmlContent);
                String sellerId = match2.Groups[2].Value;
                String itemId = match2.Groups[1].Value;
                String subHtmlContent2 = match.Groups[6].Value;
                Regex reg3 = new Regex("[\\s\\S]*\"spuId\":([0-9]+)[\\s\\S]*");
                Match match3 = reg3.Match(subHtmlContent2);
                String spuid = match3.Groups[1].Value;
                shopData.RateUrl = "https://rate.tmall.com/list_detail_rate.htm?itemId="+ itemId + "&spuId="+ spuid + "&sellerId="+ sellerId + "&currentPage=";
                //TODO 抓取的数据
                //Console.WriteLine("获取的数据");
                //Console.WriteLine("mainPic:"+mainPic);
                //Console.WriteLine("name:"+name);
                //Console.WriteLine("price:"+price);
                //Console.WriteLine("shopName:" + shopName);
                //Console.WriteLine("sellerId:" + sellerId);
                //Console.WriteLine("itemId:" + itemId);
                //Console.WriteLine("spuid:" + spuid);


                shopData.CurrentPage = 1;
                shopData.Name = name;
                shopData.MainPicStr = mainPic;
                shopData.MainPic = ImageTool.getImageBy(mainPic);
                String[] priceArr = price.Split('-');//说明有多个价格
                shopData.Price = double.Parse(priceArr[0].Trim());
                shopData.Volume = DataTool.randomVal(2500,1000);//销量
                shopData.ShopName = shopName;
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
