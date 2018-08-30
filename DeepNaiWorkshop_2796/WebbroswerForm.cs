using DeepNaiWorkshop_2796.MyModel;
using MyTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using www_52bang_site_enjoy.MyTool;

namespace DeepNaiWorkshop_2796
{
    public partial class WebbroswerForm : Form
    {

        private DateTime starttime = DateTime.Now;
        public WebbroswerForm()
        {
            InitializeComponent();
            this.webBrowser1.ScriptErrorsSuppressed = true;
            //webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted);
            this.timer1.Start();
        }
        public void LoadUrl(String url)
        {
            this.webBrowser1.Navigate(url);
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)

        {

            WebBrowser webBrowser = (WebBrowser)sender;

            if (webBrowser.ReadyState == WebBrowserReadyState.Complete && webBrowser.IsBusy == false)
            {
                Console.WriteLine("进来了几次：" + webBrowser.Url.ToString());
                //获取文档编码
                /*
                StreamReader sr = new StreamReader(webBrowser1.DocumentStream, Encoding.GetEncoding(("gbk")));
                string strhtml = sr.ReadToEnd();
                CacheData.UrlContent = strhtml;
                */
                //Console.WriteLine("进来没"+ webBrowser.Document.All.ToString());
                //base.Close();

                HtmlDocument hd = webBrowser.Document;
                HtmlElementCollection listSpan = hd.GetElementsByTagName("span");
                int temp = 0;
                //抓取价格

                double price = 0.0;

                for (int i = 0; i < listSpan.Count; i++)
                {
                    HtmlElement he = listSpan[i];
                    if (he.GetAttribute("className").Equals("tm-price"))
                    {
                        Double priceTemp = Convert.ToDouble(he.InnerHtml);
                        Console.WriteLine("获取的价格：" + priceTemp);
                        if (price == 0.0)
                        {
                            price = priceTemp;
                        }
                        else
                        {
                            if (price > priceTemp)
                            {
                                price = priceTemp;
                            }
                        }

                    }
                    /*
                    else if (he.GetAttribute("className").Equals("tm-count") && temp == 0)
                    //else if (he.GetAttribute("className").Equals("tm-count"))
                    {
                        Console.WriteLine("看看结果啊：" + he.InnerHtml);
                        volume = Convert.ToInt32(he.InnerHtml.Trim());
                        temp++;//第一个才是销量
                    }
                    */
                }

                Console.WriteLine("获取的价格：" + price);
                //抓取销量
                int volume = 0;
                HtmlElementCollection listUl = hd.GetElementsByTagName("ul");
                for (int i = 0; i < listUl.Count; i++)
                {
                    HtmlElement he = listUl[i];
                    Console.WriteLine("查看销量：" + he.InnerHtml);

                }
                Console.WriteLine("销量：" + volume);
                //加载图片
                string mainPic = "";
                /*
                HtmlElementCollection listLi = hd.GetElementsByTagName("li");
                for (int i = 0; i < listLi.Count; i++)
                {
                    HtmlElement he = listLi[i];
                    if (he.GetAttribute("className").Equals("tb-selected"))
                    {

                        Console.WriteLine("获取的主图："+ he.InnerHtml);
                        Regex reg = new Regex("([\\s\\S]*)<title>([\\s\\S]*)-tmall.com天猫</title>[\\s\\S]*<img id=\"J_ImgBooth\"[\\s\\S]*?src=\"([\\s\\S]*?)\"[\\s\\S]*<input type=\"hidden\" name=\"seller_nickname\" value=\"([\\s\\S]*?)\" />[\\s\\S]*defaultItemPrice\":\"([\\s\\S]*?)\"([\\s\\S]*)");
                        reg = new Regex("<a href=\"#\"><img src=\"([\\s\\S]*)\"></a>");
                        Match match = reg.Match(he.InnerHtml);
                        //mainPic = (string.IsNullOrWhiteSpace(match.Groups[1].Value) ? mainPic : match.Groups[1].Value);
                        mainPic = match.Groups[1].Value;
                        mainPic = StringTool.replaceStartWith(mainPic, "//", "http://");
                        //Console.WriteLine(he.InnerHtml+"获取的图片路径：" + mainPic);
                    }
                    else
                    {
                        Console.WriteLine("获取的主图2："+ he.InnerHtml);
                    }
                    
                }*/
                HtmlElement main = hd.GetElementById("J_UlThumb");
                Console.WriteLine("看下结果："+main.InnerHtml);
                Console.WriteLine("获取的图片路径：" + mainPic);
                String shopName = "";
                //加载店铺名称
                shopName = "天猫超市";

                String goodsName = "";
                //加载商品名称
                HtmlElementCollection listH1 = hd.GetElementsByTagName("h1");
                for (int i = 0; i < listH1.Count; i++)
                {
                    HtmlElement he = listH1[i];
                    if (!he.InnerHtml.Contains("span"))
                    {
                        goodsName = he.InnerHtml.Trim();
                    }

                }

                Console.WriteLine("商品名称：" + goodsName);

                BaseDataBean shopData = new BaseDataBean();
                shopData.CurrentPage = 1;
                shopData.Name = goodsName;
                shopData.MainPicStr = mainPic;
                //shopData.MainPic = ImageTool.getImageBy(mainPic);

                shopData.Price = price;
                shopData.Volume = volume;//销量
                shopData.ShopName = shopName;
                int couponValue = (int)(shopData.Price * 0.4);
                shopData.CouponValue = couponValue < 20 ? couponValue : (couponValue + (10 - (couponValue % 10)));//自定义券价值
                shopData.CouponValue = shopData.CouponValue > 100 ? 100 : shopData.CouponValue;

                CacheData.BaseDataBean = shopData;
                //panelEx1.Text = "页面加载完成";
                //timer1.Enabled = false;
                //this.Close();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {


                if (webBrowser1.ReadyState == WebBrowserReadyState.Complete && webBrowser1.IsBusy == false)
                {
                    Console.WriteLine("进来了几次：" + webBrowser1.Url.ToString());
                    //获取文档编码
                    /*
                    StreamReader sr = new StreamReader(webBrowser1.DocumentStream, Encoding.GetEncoding(("gbk")));
                    string strhtml = sr.ReadToEnd();
                    CacheData.UrlContent = strhtml;
                    */
                    //Console.WriteLine("进来没"+ webBrowser.Document.All.ToString());
                    //base.Close();

                    HtmlDocument hd = webBrowser1.Document;
                    HtmlElementCollection listSpan = hd.GetElementsByTagName("span");
                    int temp = 0;
                    //抓取价格

                    double price = 0.0;

                    for (int i = 0; i < listSpan.Count; i++)
                    {
                        HtmlElement he = listSpan[i];
                        if (he.GetAttribute("className").Equals("tm-price"))
                        {
                            Double priceTemp = Convert.ToDouble(he.InnerHtml);
                            Console.WriteLine("获取的价格：" + priceTemp);
                            if (price == 0.0)
                            {
                                price = priceTemp;
                            }
                            else
                            {
                                if (price > priceTemp)
                                {
                                    price = priceTemp;
                                }
                            }

                        }
                        /*
                        else if (he.GetAttribute("className").Equals("tm-count") && temp == 0)
                        //else if (he.GetAttribute("className").Equals("tm-count"))
                        {
                            Console.WriteLine("看看结果啊：" + he.InnerHtml);
                            volume = Convert.ToInt32(he.InnerHtml.Trim());
                            temp++;//第一个才是销量
                        }
                        */
                    }

                    Console.WriteLine("获取的价格：" + price);
                    //抓取销量
                    int volume = 0;
                    HtmlElementCollection listUl = hd.GetElementsByTagName("ul");
                    for (int i = 0; i < listUl.Count; i++)
                    {
                        HtmlElement he = listUl[i];
                        if (he.InnerHtml != null && he.InnerHtml.Contains("tm-ind-item tm-ind-sellCount"))
                        {
                            //Console.WriteLine("解析前：" + he.InnerHtml);
                            Regex reg = new Regex("<a href=\"#\"><img src=\"([\\s\\S]*)\"></a>");
                            reg = new Regex("[\\s\\S]*月销量</span><span class=\"tm-count\">([\\s\\S]*?)</span></div></li>[\\s\\S]*");
                            Match match = reg.Match(he.InnerHtml);
                            //mainPic = (string.IsNullOrWhiteSpace(match.Groups[1].Value) ? mainPic : match.Groups[1].Value);
                            //Console.WriteLine("最后的串："+ match.Groups[1].Value);
                            volume = Convert.ToInt32(match.Groups[1].Value);
                        }


                    }
                    Console.WriteLine("销量：" + volume);
                    //加载图片
                    string mainPic = "";
                    HtmlElementCollection listLi = hd.GetElementsByTagName("li");
                    for (int i = 0; i < listLi.Count; i++)
                    {
                        HtmlElement he = listLi[i];
                        if (he.GetAttribute("className").Equals("tb-selected"))
                        {

                            Console.WriteLine("获取的主图：" + he.InnerHtml);
                            Regex reg = new Regex("[\\s\\S]*src=\"([\\s\\S]*)\"></a>");
                            Match match = reg.Match(he.InnerHtml);
                            //mainPic = (string.IsNullOrWhiteSpace(match.Groups[1].Value) ? mainPic : match.Groups[1].Value);
                            mainPic = match.Groups[1].Value;
                            mainPic = StringTool.replaceStartWith(mainPic, "//", "http://");

                            string[] mainPicArr = Regex.Split(mainPic, ".jpg", RegexOptions.None);
                            
                            if (mainPicArr[0].Contains(".jpg"))
                            {
                                mainPic = mainPicArr[0];
                            }
                            else
                            {
                                mainPic = mainPicArr[0] + ".jpg";
                            }
                            //Console.WriteLine(he.InnerHtml+"获取的图片路径：" + mainPic);
                        }/*
                    else if(he.GetAttribute("className").Equals("tm-ind-item tm-ind-sellCount"))
                    {
                        Console.WriteLine("获取的主图2：" + he.InnerHtml);
                    }*/

                    }
                    Console.WriteLine("获取的图片路径：" + mainPic);
                    String shopName = "";
                    //加载店铺名称
                    shopName = "天猫超市";

                    String goodsName = "";
                    //加载商品名称
                    HtmlElementCollection listH1 = hd.GetElementsByTagName("h1");
                    for (int i = 0; i < listH1.Count; i++)
                    {
                        HtmlElement he = listH1[i];
                        if (!he.InnerHtml.Contains("span"))
                        {
                            goodsName = he.InnerHtml.Trim();
                        }

                    }

                    Console.WriteLine("商品名称：" + goodsName);

                    BaseDataBean shopData = new BaseDataBean();
                    shopData.CurrentPage = 1;
                    shopData.Name = goodsName;
                    shopData.MainPicStr = mainPic;
                    shopData.MainPic = ImageTool.getImageBy(mainPic);

                    shopData.Price = price;
                    shopData.Volume = volume;//销量
                    shopData.ShopName = shopName;
                    int couponValue = (int)(shopData.Price * 0.4);
                    shopData.CouponValue = couponValue < 20 ? couponValue : (couponValue + (10 - (couponValue % 10)));//自定义券价值
                    shopData.CouponValue = shopData.CouponValue > 100 ? 100 : shopData.CouponValue;

                    CacheData.BaseDataBean = shopData;
                    //panelEx1.Text = "页面加载完成";
                    timer1.Enabled = false;
                    this.Close();
                }
            }catch(Exception ex)
            {
                MyLogUtil.ErrToLog("获取天猫超市商品数据失败，原因："+ex);
                CacheData.BaseDataBean = null;
                this.Close();

            }
        }
            
    }
    
}
