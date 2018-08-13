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
            Console.WriteLine("正则解析数据...");
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
                //淘宝评论页面
                //https://rate.taobao.com/feedRateList.htm?auctionNumId=573876368555&userNumId=2965298499&currentPageNum=1&pageSize=20&rateType=&orderType=sort_weight&attribute=&sku=&hasSku=false&folded=0&ua=098%23E1hvM9vUvbpvUpCkvvvvvjiPPsM9gjnjnL5hAjEUPmPpsjYEPLLOtjEbRFMZAjDPR4wCvvpvvUmmRphvCvvvvvvPvpvhMMGvvvhCvvOvCvvvphvEvpCWm2fXvvw6aNoxfXk4jLkxfwLyd3ODN%2BLyaNoAdcHVafknIfvtv0ABDpcBHbUf8%2B1libmAdcHVaNoxfXkwjLFp%2BExreC9aUExr1nAKHdyCvm9vvvvvphvvvvvvvDxvpvs7vvm2phCvhRvvvUnvphvppvvv96CvpCCvkphvC99vvOC0p8yCvv9vvUmAOIbqXv%3D%3D&_ksTS=1533888495287_1089&callback=jsonp_tbcrate_reviews_list

                // String goodName = 
                //Regex reg = new Regex(".+?item:.+?pic\\s+?:\\s+?'(.+?)',.+<h3 class=\"tb - main - title\" data-title=\"(.+?)\" >.+<strong id=\"J_StrPrice\"><em class=\"tb - rmb\">&yen;</em><em class=\"tb - rmb - num\">(.+?)</em></strong>.+");
                Regex reg = new Regex("[\\s\\S]*<link rel=\"canonical\" href=\"(\\s\\S]*)\" /><title>([\\s\\S]*)-淘宝网</title>[\\s\\S]*sellerId         : '([\\s\\S]*)',[\\s\\S]*sellerNick       : '([\\s\\S]*)',[\\s\\S]*pic              : '([\\s\\S]*?)',[\\s\\S]*<input type=\"hidden\" name=\"current_price\" value= \"([\\s\\S]*)\"/>[\\s\\S]*");
                reg = new Regex("([\\s\\S]*)<title>([\\s\\S]*)-淘宝网</title>([\\s\\S]*)");
                Match match = reg.Match(htmlContent);


                String itemId = match.Groups[1].Value;
                Regex reg1 = new Regex("[\\s\\S]*<link rel=\"canonical\" href=\"https://item.taobao.com/item.htm\\?id=([\\s\\S]*?)\" />[\\s\\S]*");
                Match match1 = reg1.Match(itemId);
                itemId = match1.Groups[1].Value;
                String name = match.Groups[2].Value;
                String temp = match.Groups[3].Value;
                Regex reg2 = new Regex("[\\s\\S]*sellerId         : '([\\s\\S]*?)',([\\s\\S]*)");
                Match match2 = reg2.Match(temp);
                String sellerId = match2.Groups[1].Value;
                String temp2 = match2.Groups[2].Value;
                Regex reg3 = new Regex("[\\s\\S]*pic              : '([\\s\\S]*?)',([\\s\\S]*)");
                Match match3 = reg3.Match(temp2);
                String mainPic = match3.Groups[1].Value;
                mainPic = StringTool.replaceStartWith(mainPic, "//", "http://");

                String temp4 = match3.Groups[2].Value;
                //Console.WriteLine("要解析的内容："+temp4);
                Regex reg4 = new Regex("[\\s\\S]*sellerNick       : '([\\s\\S]*?)',([\\s\\S]*)");
                Match match4 = reg4.Match(temp4);
                String shopName = match4.Groups[1].Value;
                Console.WriteLine(shopName);
                String temp5 = match4.Groups[2].Value;

                //Console.WriteLine("内容啊："+ temp5);
                
                Regex reg5 = new Regex("[\\s\\S]*<input type=\"hidden\" name=\"current_price\" value= \"([\\s\\S]*?)\"/>([\\s\\S]*)");
                //<img id="J_ImgBooth" src="//gd2.alicdn.com/imgextra/i4/2965298499/TB2cH4jGhWYBuNjy1zkXXXGGpXa_!!2965298499.jpg_400x400.jpg" data-hasZoom="700" data-size="400x400"/>
                Match match5 = reg5.Match(temp5);
                String price = match5.Groups[1].Value;

                Console.WriteLine("获取的内容");
                Console.WriteLine(itemId);
                Console.WriteLine(name);
                Console.WriteLine(sellerId);
                Console.WriteLine(shopName);
                Console.WriteLine(mainPic);
                Console.WriteLine(price);
                Console.WriteLine("获取结束");

                shopData.CurrentPage = 1;
                shopData.Name = name;
                shopData.MainPicStr = mainPic;
                shopData.MainPic = ImageTool.getImageBy(mainPic);
                String[] priceArr = price.Split('-');//说明有多个价格
                shopData.Price = double.Parse(priceArr[0].Trim());
                shopData.Volume = DataTool.randomVal(2500, 1000);//销量
                shopData.ShopName = shopName;
                int couponValue = (int)(shopData.Price * 0.4);
                shopData.CouponValue = couponValue < 20 ? couponValue : (couponValue + (10 - (couponValue % 10)));//自定义券价值
                shopData.CouponValue = shopData.CouponValue > 100 ? 100 : shopData.CouponValue;
                //暂不提供评论抓取
                shopData.RateUrl = "";//https://rate.taobao.com/feedRateList.htm?auctionNumId=573876368555&userNumId=2965298499&currentPageNum=1&pageSize=20&rateType=&orderType=sort_weight&attribute=&sku=&hasSku=false&folded=0&ua=098%23E1hvM9vUvbpvUpCkvvvvvjiPPsM9gjnjnL5hAjEUPmPpsjYEPLLOtjEbRFMZAjDPR4wCvvpvvUmmRphvCvvvvvvPvpvhMMGvvvhCvvOvCvvvphvEvpCWm2fXvvw6aNoxfXk4jLkxfwLyd3ODN%2BLyaNoAdcHVafknIfvtv0ABDpcBHbUf8%2B1libmAdcHVaNoxfXkwjLFp%2BExreC9aUExr1nAKHdyCvm9vvvvvphvvvvvvvDxvpvs7vvm2phCvhRvvvUnvphvppvvv96CvpCCvkphvC99vvOC0p8yCvv9vvUmAOIbqXv%3D%3D&_ksTS=1533888495287_1089&callback=jsonp_tbcrate_reviews_list

                
                
                
                return shopData;
            }

        }

    }
}
