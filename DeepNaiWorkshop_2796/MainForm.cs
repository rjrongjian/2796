
using MyTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeepNaiWorkshop_2796;
using DeepNaiWorkshop_2796.MyTool;
using DeepNaiWorkshop_2796.MyModel;
using www_52bang_site_enjoy.MyTool;
using System.Drawing.Text;
using FileCreator.Model;
using System.IO;
using DeepNaiWorkshop_6001.MyTool;
using FileCreator.MyTool;

namespace RegeditActivity
{
    public partial class MainForm : Form
    {
        private BaseDataBean dataBean;//生成的有关商品的数据（注意：如果没有通过代码调用网址获取商品数据，就不能获取评论）
        private Label logLabel;
        private Image originalCouponPic;//原始的优惠券截图背景图片
        private PictureBox waterPictureBox;//水印图片控件
        private PictureBox waterPictureBox2;//水印图片控件(订单)
        private TemplateDisplayForm templateDisplayForm; 

        public MainForm()
        {
            InitializeComponent();

            CacheData.ExpiredTime = YiYunUtil.GetExpired(CacheData.UserName);
            this.Text = this.Text + "（当前登录的账号：" + CacheData.UserName + ",过期时间：" + CacheData.ExpiredTime + "）";
            this.timer1.Enabled = true;
            this.label2.Text = CacheData.ExpiredTime;


            //字体集合
            CacheData.fontList = new List<ResourceInfoForCombox>();
            InstalledFontCollection MyFont = new InstalledFontCollection();
            FontFamily[] MyFontFamilies = MyFont.Families;
            int Count = MyFontFamilies.Length;
            for (int i = 0; i < Count; i++)
            {
                CacheData.fontList.Add(new ResourceInfoForCombox { index = CacheData.fontList.Count, moduleName = MyFontFamilies[i].Name });
                //Console.WriteLine(MyFontFamilies[i].Name);
                //MyFontFamilies[i]

            }

            label30.Text = CacheData.NotifyInfo;

            //模板列表
            comboBox1.DataSource = MySystemUtil.GetAllTemplateNames();

            this.webBrowser1.ScriptErrorsSuppressed = true;


            templateDisplayForm = new TemplateDisplayForm(this);


            

        }

        public void ResetTemplateForm()
        {
            templateDisplayForm = null;
        }

        public void ResetTemplates()
        {
            comboBox1.DataSource = MySystemUtil.GetAllTemplateNames();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            System.Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)  //获取商品信息
        {
            //按钮不可用
            setMainFormBtnStatus(2);
            try
            {
                String url = this.textBox1.Text;
                TaoBaoTool taoBaoTool = new TaoBaoTool();
                //判断是否是天猫 淘宝链接
                RespMessage validateResult = taoBaoTool.isTmallOrTaoBaoItemPage(url);
                if (validateResult.code != 1)
                {
                    setMainFormBtnStatus(7);//获取商品数据、生成截图按钮可用
                    alert(validateResult.message);
                }
                else
                {
                    //开始解析数据
                    LogTool.log("开始解析天猫（淘宝）详情页面：" + url, this.logLabel);
                    
                    LogTool.log("正在获取网页信息", this.logLabel);
                    Console.WriteLine("正在获取网页信息...");
                    String htmlWebContent = "";
                    /*
                    if (validateResult.message=="3"&&url.Contains("chaoshi.detail.tmall.com"))//天猫超市的链接，需要转换成手机端访问
                    {
                        url = url.Replace("chaoshi.detail.tmall.com", "detail.m.tmall.com");
                        htmlWebContent = WebTool.getHtmlContent(url, "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.117 Mobile Safari/537.36");
                    }
                    else
                    {
                        htmlWebContent = WebTool.getHtmlContent(url);
                    }
                    */

                    if (validateResult.message == "3")//天猫超市的链接需要通过浏览器爬取
                    {
                        //url = url.Replace("chaoshi.detail.tmall.com", "detail.m.tmall.com");
                        //htmlWebContent = WebTool.getHtmlContent(url, "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.117 Mobile Safari/537.36");
                        WebbroswerForm webbroswerForm = new WebbroswerForm();
                        webbroswerForm.LoadUrl(url);
                        webbroswerForm.ShowDialog();
                        if (CacheData.BaseDataBean==null)
                        {
                            setMainFormBtnStatus(7);//获取商品数据、生成截图按钮可用
                            MessageBox.Show("未能获取天猫超市数据，请手动更改数据");
                            return;
                        }
                        else
                        {
                            dataBean = CacheData.BaseDataBean;
                        }
                    }
                    else
                    {
                        htmlWebContent = WebTool.getHtmlContent(url);
                        dataBean = taoBaoTool.parseShopData(int.Parse(validateResult.message), htmlWebContent);
                        
                    }

                    Console.WriteLine("开始解析网页...");
                    //Console.WriteLine("网页内容："+ htmlWebContent);
                    //LogTool.log("开始解析网页", this.logLabel);
                    //目前只支持天猫

                    //if (TaoBaoTool.GOOD_TYPE_TMALL == int.Parse(validateResult.message))
                    //{
                    if (dataBean == null)
                    {
                        alert("不能正常解析数据，请手动录入");
                        return;
                    }

                    //赋值
                    this.textBox3.Text = dataBean.CouponValue.ToString();//自定义券价格
                    this.textBox2.Text = dataBean.Price.ToString();//商品价格 
                    this.textBox6.Text = dataBean.Name;//商品名称
                    this.textBox4.Text = dataBean.Volume.ToString();//商品销量
                    this.textBox7.Text = dataBean.MainPicStr;// 商品图片地址
                    this.textBox11.Text = dataBean.ShopName;//店铺名称
                    this.pictureBox1.Image = dataBean.MainPic;// 商品图片
                    LogTool.log("数据解析完成，数据可手动更改...",this.logLabel);
                        

                    //}
                    //else
                    //{
                        //alert("目前不支持淘宝商品数据爬取，请手动录入");
                    //}

                    setMainFormBtnStatus(7);//获取商品数据、生成截图按钮可用
                }
            }
            catch(Exception ex)
            {
                setMainFormBtnStatus(7);//获取商品数据、生成截图按钮可用
                ExceptionTool.log(ex);
                alert("解析数据失败，请手动录入");
            }

        }

        private void alert(String content)
        {

            //Button messBoxInfoBtn = new Button();
            //messBoxInfoBtn.Text = content;
            //messBoxInfoBtn.Location = new Point(50, 70);

            //int mainFormWith = this.Width;
            //int mainFormHeight = this.Height;
            //Form messageForm = new Form();
            //messageForm.Width = 200;
            //messageForm.Height = 100;

            //messageForm.ControlBox = false;

            //messageForm.Controls.Add(messBoxInfoBtn);
            //messageForm.Show();

            MessageBox.Show(content);


        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Location = new Point((Screen.PrimaryScreen.Bounds.Width - this.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);
            this.StartPosition = FormStartPosition.Manual;
            this.logLabel = this.label4;
            //this.pictureBox3.BackgroundImage = ResourceTool.getImage(Const.COUPON_BACK_IMG_NAME);
            //originalCouponPic = this.pictureBox3.Image;
            //测试在pictureBox添加另一个
            //PictureBox p = new PictureBox();
            //p.Image = ResourceTool.getImage("test2");
            ////Console.WriteLine("显示了吗"+p.Image.ToString());
            //p.Show();
            //this.pictureBox3.Controls.Add(p);
        }
        /*
         * 设置mainForm按钮状态
         * 
         */ 
        private void setMainFormBtnStatus(int status)
        {
            //1按钮全部可用
            if (status == 1)
            {
                this.button1.Enabled = true;
                this.button2.Enabled = true;
                this.button3.Enabled = true;
                this.button4.Enabled = true;
                this.button5.Enabled = true;
                this.button6.Enabled = true;

            }
            else if (status == 2)//2 按钮全部不可用
            {
                this.button1.Enabled = false;
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                this.button4.Enabled = false;
                this.button5.Enabled = false;
                this.button6.Enabled = false;

            }
            else if (status == 3)//3 获取商品信息按钮可用
            {
                this.button1.Enabled = true;
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                this.button4.Enabled = false;
                this.button5.Enabled = false;
                this.button6.Enabled = false;

            }
            else if (status == 4)//4 生成截图按钮可用
            {
                this.button1.Enabled = false;
                this.button2.Enabled = true;
                this.button3.Enabled = false;
                this.button4.Enabled = false;
                this.button5.Enabled = false;
                
            }
            else if (status == 5)//5 截图优惠券按钮可用
            {
                this.button1.Enabled = false;
                this.button2.Enabled = false;
                this.button3.Enabled = true;
                this.button4.Enabled = false;
                this.button5.Enabled = false;
                this.button6.Enabled = false;

            }
            else if (status == 6)//6 截图订单详情按钮可用
            {
                this.button1.Enabled = false;
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                this.button4.Enabled = true;
                this.button5.Enabled = false;
                this.button6.Enabled = false;
            }
            else if (status == 7)//7 获取商品数据、生成截图按钮可用
            {
                this.button1.Enabled = true;
                this.button2.Enabled = true;
                this.button3.Enabled = false;
                this.button4.Enabled = false;
                this.button5.Enabled = false;
                this.button6.Enabled = false;

            }
            else if (status == 8)//8 评论页面按钮全部失效
            {
                this.button7.Enabled = false;
                this.button8.Enabled = false;
                this.button9.Enabled = false;
                this.button10.Enabled = false;

            }
            else if (status == 9)//9 评论页面按钮全部有效
            {
                this.button7.Enabled = true;
                this.button8.Enabled = true;
                this.button9.Enabled = true;
                this.button10.Enabled = true;

            }
            else
            {
                LogTool.log("未能识别的按钮状态值："+status,this.logLabel);
                this.button1.Enabled = false;
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                this.button4.Enabled = false;
                this.button5.Enabled = false;
                this.button6.Enabled = false;

            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)//输入销量（不以0开头）
        {
            if (Char.IsNumber(e.KeyChar))
            {
                if (String.IsNullOrEmpty(this.textBox3.Text))//输入的金额首个字符为. 取消事件
                {
                    if (e.KeyChar == (char)48)
                    {
                        e.Handled = true;//取消事件
                    }
                }
            }
            else
            {
                if (e.KeyChar != (char)8)
                {
                    e.Handled = true;//取消事件
                }

            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)//这种校验不能解决4444. 这种数据 只能输入商品价格
        {
            //如果输入的不是数字键，也不是.则取消该输入
            if ((Char.IsNumber(e.KeyChar)) || e.KeyChar == (char)46 || e.KeyChar == (char)8)
            {
                if (e.KeyChar == (char)46)//输入的是.
                {
                    if (String.IsNullOrEmpty(this.textBox2.Text))//输入的金额首个字符为. 取消事件
                    {
                        e.Handled = true;//取消事件
                    }
                    else
                    {
                        if (this.textBox2.Text.Contains("."))//文字中已经输入过.
                        {
                            e.Handled = true;//取消事件
                        }
                    }


                }
                else
                {
                    if(String.IsNullOrWhiteSpace(this.textBox2.Text)&& e.KeyChar == (char)48)//不能以0开头
                    {
                        e.Handled = true;//取消事件
                    }

                    if (e.KeyChar != (char)8)
                    {
                        if (this.textBox2.Text.IndexOf('.') != -1)//输入数字时已经包含. 判断当前.后面的位数
                        {
                            String[] splitArr = this.textBox2.Text.Split('.');
                            if (splitArr[1].Length >= 2)
                            {
                                e.Handled = true;//取消事件
                            }
                        }
                    }

                }
            }
            else
            {
                e.Handled = true;//取消事件
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                if (String.IsNullOrEmpty(this.textBox4.Text))//输入的金额首个字符为. 取消事件
                {
                    if(e.KeyChar == (char)48)
                    {
                        e.Handled = true;//取消事件
                    }
                }
            }
            else
            {
                if(e.KeyChar != (char)8)
                {
                    e.Handled = true;//取消事件
                }
                
            }
        }

        

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是数字键，也不是.则取消该输入
            if ((Char.IsNumber(e.KeyChar)) || e.KeyChar == (char)46 || e.KeyChar == (char)8)
            {
                if (e.KeyChar == (char)46)//输入的是.
                {
                    if (String.IsNullOrEmpty(this.textBox8.Text))//输入的金额首个字符为. 取消事件
                    {
                        e.Handled = true;//取消事件
                    }
                    else
                    {
                        if (this.textBox8.Text.Contains("."))//文字中已经输入过.
                        {
                            e.Handled = true;//取消事件
                        }
                    }


                }
                else
                {
                    if (e.KeyChar != (char)8)
                    {
                        if (this.textBox8.Text.IndexOf('.') != -1)//输入数字时已经包含. 判断当前.后面的位数
                        {
                            String[] splitArr = this.textBox8.Text.Split('.');
                            if (splitArr[1].Length >= 2)
                            {
                                e.Handled = true;//取消事件
                            }
                        }
                    }

                }
            }
            else
            {
                e.Handled = true;//取消事件
            }
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是数字键，也不是.则取消该输入
            if ((Char.IsNumber(e.KeyChar)) || e.KeyChar == (char)46 || e.KeyChar == (char)8)
            {
                if (e.KeyChar == (char)46)//输入的是.
                {
                    if (String.IsNullOrEmpty(this.textBox9.Text))//输入的金额首个字符为. 取消事件
                    {
                        e.Handled = true;//取消事件
                    }
                    else
                    {
                        if (this.textBox9.Text.Contains("."))//文字中已经输入过.
                        {
                            e.Handled = true;//取消事件
                        }
                    }


                }
                else
                {
                    if (e.KeyChar != (char)8)
                    {
                        if (this.textBox9.Text.IndexOf('.') != -1)//输入数字时已经包含. 判断当前.后面的位数
                        {
                            String[] splitArr = this.textBox9.Text.Split('.');
                            if (splitArr[1].Length >= 2)
                            {
                                e.Handled = true;//取消事件
                            }
                        }
                    }

                }
            }
            else
            {
                e.Handled = true;//取消事件
            }
        }

        private void radioButton1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;//该值确定是否可以选择多个文件
                dialog.Title = "请选择文件夹";
                dialog.Filter = "图像文件(*.jpg;*.jpg;*.jpeg;*.gif;*.png;*.bmp)|*.jpg;*.jpeg;*.gif;*.png;*.bmp";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string file = dialog.FileName;
                    Image watermark = ImageTool.getLocalImageBy(file);
                    this.pictureBox2.Image = watermark;
                    this.label14.Text = watermark.Height.ToString();//水印高
                    this.label15.Text = watermark.Width.ToString();//水印图片宽

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)//生成截图
        {
            CacheData.GoodsUrl = this.textBox1.Text;
            

            if (string.IsNullOrWhiteSpace((string)comboBox1.SelectedValue))
            {
                MessageBox.Show("请先选中模板");
                return;
            }
           
            //加载选中的模板
            String templateConfigPath = MySystemUtil.GetTemplateImgRoot() + comboBox1.SelectedValue + "\\template.json";
            if (!File.Exists(templateConfigPath)){
                MessageBox.Show("当前选中的模板信息不完整");
                return;
            }
            string content = MyFileUtil.readFileAll(templateConfigPath);
            MyJsonUtil<TemplateConfig> myJsonUtil = new MyJsonUtil<TemplateConfig>();
            TemplateConfig templateConfig = myJsonUtil.parseJsonStr(content);

            
            //获取图片模板
            String templateImgPath = MySystemUtil.GetTemplateImgRoot() + comboBox1.SelectedValue + "\\" + templateConfig.BackImg;
            if (!File.Exists(templateImgPath)){
                MessageBox.Show("当前选中的模板中不存在背景图片");
                return;
            }
            Image img = Image.FromFile(templateImgPath);
            

            bool validateResult = validateData();
            if (!validateResult)//生成截图时用到的数据都正常
            {
                //Console.WriteLine("将数据生成到页面");
                return;
            }

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("请先加载图片");
                return;
            }

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            StringFormat sf1 = new StringFormat();
            sf1.Alignment = StringAlignment.Near;
            sf1.LineAlignment = StringAlignment.Center;

            PictureBox couponPb = this.pictureBox3;
            //开优惠券截图开始插入数据
            //couponPb.Image = ResourceTool.getImage(Const.COUPON_BACK_IMG_NAME); ;//初始化背景图片
            couponPb.Image = img;
            Graphics g = Graphics.FromImage(couponPb.Image);
            if (templateConfig.IsUseShopName)
            {
                //商家名称
                String shopName = this.textBox11.Text;
                //shopName = "啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊";
                SolidBrush shopNameBrush = new SolidBrush(ColorTool.getColorFromHtml(templateConfig.ShopNameFontColor));
                Font shopNameFont = new Font(templateConfig.ShopNameFontType, templateConfig.ShopNameSize);
                //获取字符串宽度
                SizeF shopNameSize = g.MeasureString(shopName, shopNameFont);
                //Point shopNamePoint = new Point((int)(couponPb.Image.Width- templateConfig.ShopNameFontWidth)/2, 117);//templateConfig
                Point shopNamePoint = new Point(templateConfig.ShopNameFontX, templateConfig.ShopNameFontY);

                //g.DrawString(shopName, shopNameFont, shopNameBrush, shopNamePoint, StringFormat.GenericDefault);

                g.DrawString(shopName, shopNameFont, shopNameBrush, new Rectangle(shopNamePoint.X, shopNamePoint.Y, templateConfig.ShopNameFontWidth, templateConfig.ShopNameFontHeight), sf);
            }
            if (templateConfig.IsUseShopSLT)
            {
                //商品缩略图 123x123
                //Image goodPic = (Image)pictureBox1.Image.Clone();//拷贝一个图片
                g.DrawImage(pictureBox1.Image, templateConfig.ShopSLTX, templateConfig.ShopSLTY, templateConfig.ShopSLTWidth, templateConfig.ShopSLTHeight);
            }
            if (templateConfig.IsUseCouponValue)
            {
                /*
                //优惠券价格
                String rmb = "￥";
                SolidBrush rmbBrush = new SolidBrush(ColorTool.getColorFromHtml("#d0021b"));
                //Font rmbFont = new Font("Arial", 16);
                Font rmbFont = new Font(Const.COUPON_FONT, 16);
                SizeF rmbSize = g.MeasureString(rmb, rmbFont);
                */

                String couponValue = this.textBox3.Text;
                Font couponValueFont = new Font(templateConfig.CouponValueFontType, templateConfig.CouponValueSize);
                SizeF couponValueSize = g.MeasureString(couponValue, couponValueFont);
                SolidBrush rmbBrush = new SolidBrush(ColorTool.getColorFromHtml(templateConfig.CouponValueFontColor));
                //int baseX = 51;
                //int baseY = 160;
               // Point rmbPoint = new Point(((int)(170 - rmbSize.Width - couponValueSize.Width) / 2) + baseX + 10, baseY + (int)(couponValueSize.Height - rmbSize.Width) - 15);
                //Point couponValuePoint = new Point((int)(rmbPoint.X + rmbSize.Width) - 10, (int)(rmbPoint.Y - (couponValueSize.Height - rmbSize.Height) + 8));
                //g.DrawString(rmb, rmbFont, rmbBrush, rmbPoint, StringFormat.GenericDefault);
                //g.DrawString(couponValue, couponValueFont, rmbBrush, couponValuePoint, StringFormat.GenericDefault);
                g.DrawString(couponValue, couponValueFont, rmbBrush, new Rectangle(templateConfig.CouponValueFontX, templateConfig.CouponValueFontY, templateConfig.CouponValueFontWidth, templateConfig.CouponValueFontHeight), sf);
            }

            if (templateConfig.IsUseCouponTime)
            {
                //优惠券日期
                String couponTime = this.dateTimePicker1.Value.ToString("yyyy.MM.dd") + "-" + this.dateTimePicker2.Value.ToString("yyyy.MM.dd");
                Font couponTimeFont = new Font(templateConfig.CouponTimeFontType, templateConfig.CouponTimeSize);
                SolidBrush couponTimeBrush = new SolidBrush(ColorTool.getColorFromHtml(templateConfig.CouponTimeFontColor));
                SizeF couponTimeSize = g.MeasureString(couponTime, couponTimeFont);
                //Point couponTimePoint = new Point(51 + (int)(170 - couponTimeSize.Width) / 2, 230);
                //g.DrawString(couponTime, couponTimeFont, couponTimeBrush, couponTimePoint, StringFormat.GenericDefault);
                g.DrawString(couponTime, couponTimeFont, couponTimeBrush, new Rectangle(templateConfig.CouponTimeFontX, templateConfig.CouponTimeFontY, templateConfig.CouponTimeFontWidth, templateConfig.CouponTimeFontHeight), sf);
            }
            if (templateConfig.IsUseGoodsName)
            {
                //商品名称 并且自动换行
                String goodName = this.textBox6.Text;
                Font goodNameFont = new Font(templateConfig.GoodsNameFontType, templateConfig.GoodsNameSize);
                SolidBrush goodNameBrush = new SolidBrush(ColorTool.getColorFromHtml(templateConfig.GoodsNameFontColor));
                //Point goodNamePoint = new Point(140, 288);

                //Brush fontBrush = SystemBrushes.ControlText;
                //SizeF sizeText = e.Graphics.MeasureString(nodeText, font);
                //e.Graphics.DrawString(nodeText, font, fontBrush, (this.Width - sizeText.Width) / 2, (this.Height - sizeText.Height) / 2);
                /*
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                */
                g.DrawString(goodName, goodNameFont, goodNameBrush, new Rectangle(templateConfig.GoodsNameFontX, templateConfig.GoodsNameFontY, templateConfig.GoodsNameFontWidth, templateConfig.GoodsNameFontHeight), sf1);
            }
            if (templateConfig.IsUsePrice)
            {
                //现价
                String price = this.textBox2.Text;

                Font priceFont = null;
                FontStyle result = FontStyle.Regular;
                List<FontStyle> style = new List<FontStyle>();
                
                if (templateConfig.PriceFontBold)
                {
                    result = result | FontStyle.Bold;
                }
                if (templateConfig.PriceFontDelLine)
                {
                    result = result | FontStyle.Strikeout;
                }
                if (templateConfig.PriceFontItalic)
                {
                    result = result | FontStyle.Italic;
                }
                try
                {
                    if (result == 0)
                    {
                        priceFont = new Font(templateConfig.PriceFontType, templateConfig.PriceSize);
                    }
                    else
                    {
                        priceFont = new Font(templateConfig.PriceFontType, templateConfig.PriceSize, result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("当前电脑不支持此字体，请更换");

                    return;
                }
                
                
               
                SolidBrush priceBrush = new SolidBrush(ColorTool.getColorFromHtml(templateConfig.PriceFontColor));
                //Point pricePoint = new Point(183, 360);
                //g.DrawString(price, priceFont, priceBrush, pricePoint, StringFormat.GenericDefault);
                g.DrawString(price, priceFont, priceBrush, new Rectangle(templateConfig.PriceFontX, templateConfig.PriceFontY, templateConfig.PriceFontWidth, templateConfig.PriceFontHeight), sf1);
            }

            if (templateConfig.IsUseVolume)
            {
                //成交量
                String volume = this.textBox4.Text + "笔成交";
                Font volumeFont = new Font(templateConfig.VolumeFontType,templateConfig.VolumeSize);
                SolidBrush volumeBrush = new SolidBrush(ColorTool.getColorFromHtml(templateConfig.VolumeFontColor));
                //SizeF volumeSize = g.MeasureString(volume, volumeFont);
               // Point volumePoint = new Point(375 - (int)volumeSize.Width - 15, 332);
                //g.DrawString(volume, volumeFont, volumeBrush, volumePoint, StringFormat.GenericDefault);
                g.DrawString(volume, volumeFont, volumeBrush, new Rectangle(templateConfig.VolumeFontX, templateConfig.VolumeFontY, templateConfig.VolumeFontWidth, templateConfig.VolumeFontHeight), sf1);
            }

            if (templateConfig.IsUsePriceAfter)
            {
                //券后价
                double priceAfter = double.Parse(this.textBox2.Text) - double.Parse(this.textBox3.Text);
                Font priceAfterFont = new Font(templateConfig.PriceAfterFontType, templateConfig.PriceAfterSize);

                SolidBrush priceAfterBrush = new SolidBrush(ColorTool.getColorFromHtml(templateConfig.PriceAfterFontColor));
                //Point priceAfterPoint = new Point(216, 375);
                //g.DrawString(priceAfter.ToString(), priceAfterFont, priceAfterBrush, priceAfterPoint, StringFormat.GenericDefault);
                g.DrawString(priceAfter.ToString(), priceAfterFont, priceAfterBrush, new Rectangle(templateConfig.PriceAfterFontX, templateConfig.PriceAfterFontY, templateConfig.PriceAfterFontWidth, templateConfig.PriceAfterFontHeight), sf1);


            }


            //开始处理水印
            int watermarkerFontSize = Convert.ToInt32(numericUpDown1.Value);//水印字体的大小
            Image watermarker = null;
            if (this.radioButton1.Checked)//图片水印
            {

                watermarker = this.pictureBox2.Image ;
            }
            else {//文字水印
                Font watermarkerFont = null;
                if (checkBox1.Checked&&!checkBox2.Checked)//加粗
                {
                    watermarkerFont = new Font(Const.COUPON_FONT, watermarkerFontSize, FontStyle.Bold|FontStyle.Italic);
                }else if(!checkBox1.Checked && checkBox2.Checked)//删除线
                {
                    watermarkerFont = new Font(Const.COUPON_FONT, watermarkerFontSize, FontStyle.Strikeout | FontStyle.Italic);
                }
                else if(checkBox1.Checked && checkBox2.Checked)//加粗 删除线
                {
                    watermarkerFont = new Font(Const.COUPON_FONT, watermarkerFontSize, FontStyle.Strikeout | FontStyle.Bold | FontStyle.Italic);
                }
                else
                {
                    watermarkerFont = new Font(Const.COUPON_FONT, watermarkerFontSize, FontStyle.Italic);
                }
                //设置文字水印的颜色
                if (String.IsNullOrWhiteSpace(textBox12.Text))
                {
                    MessageBox.Show("颜色代码不能为空！");
                    return;
                }
                Color fontColor ;
                try
                {
                    fontColor = ColorTool.getColorFromHtml(textBox12.Text);
                }catch(Exception ex)
                {
                    MessageBox.Show("不能识别的颜色代码！"+ textBox12.Text);
                    return;
                }
                SolidBrush watermarkerBrush = new SolidBrush(fontColor);
                //SolidBrush watermarkerBrush = new SolidBrush(Color.);

                SizeF watermarkerText = g.MeasureString(this.textBox5.Text, watermarkerFont);

                //防止随着图片大小改变而缩放尺寸
                Bitmap image2 = new Bitmap(pictureBox3.Width, pictureBox3.Height);
                Graphics gi2 = Graphics.FromImage(image2);
                watermarkerText = gi2.MeasureString(this.textBox5.Text, watermarkerFont);
                //--------------end--------------
                
                int watermarkerImgWidth = (int)watermarkerText.Width+1;
                int watermarkerImgHeight = (int)watermarkerText.Height +1;
                Bitmap image = new Bitmap(watermarkerImgWidth, watermarkerImgHeight);
                Graphics gi = Graphics.FromImage(image);
                gi.Clear(Color.Transparent);//透明
                
                


                gi.DrawString(this.textBox5.Text, watermarkerFont, watermarkerBrush, new Rectangle(0, 0, watermarkerImgWidth, watermarkerImgHeight));
                watermarker = image;
                gi2.Dispose();
                gi.Dispose();

            }

            waterPictureBox = new PictureBox();
            waterPictureBox.Image = watermarker;
            waterPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            waterPictureBox.Height = watermarker.Height;
            waterPictureBox.Width = watermarker.Width;
            waterPictureBox.Location = new Point(10, 410);
            //给水印图片添加鼠标按下事件
            //控件首次移动
            bool isDrag = false;
            Point prePoint1 = new Point();//优惠券截图中的水印图片位置（移动前）
            bool isFirstMove = true;
            waterPictureBox.MouseDown += new MouseEventHandler(couponImg_watermarker_MouseDown);
            waterPictureBox.MouseUp += new MouseEventHandler(couponImg_watermarker_MouseUp);
            waterPictureBox.MouseMove += new MouseEventHandler(couponImg_watermarker_MouseMove);
            this.textBox8.Text = waterPictureBox.Height.ToString();
            this.textBox9.Text = waterPictureBox.Width.ToString();

            this.pictureBox3.Controls.Clear();
            this.pictureBox3.Controls.Add(waterPictureBox);

            g.Dispose();

            


            
            //水印控件按下事件
            void couponImg_watermarker_MouseDown(object downObj, MouseEventArgs mouseDownEvent)
            {
                //Console.WriteLine(mouseDownEvent.X + "," + mouseDownEvent.Y);

                if (mouseDownEvent.Button == MouseButtons.Left)
                {
                    isDrag = true;

                    prePoint1 = new Point(mouseDownEvent.X, mouseDownEvent.Y) ;
                }
            }
            //水印控件鼠标移动事件
            void couponImg_watermarker_MouseMove(object mouseMoveSender, MouseEventArgs mouseMoveEvent)
            {
                if (isDrag)
                {
                    if (isFirstMove)
                    {
                        waterPictureBox.Location = new Point(waterPictureBox.Location.X + 1, waterPictureBox.Location.Y + 1);
                        isFirstMove = false;
                    }
                    else
                    {

                    
                    //Console.WriteLine(mouseMoveEvent.X + "," + mouseMoveEvent.Y);
                    Point mousePos = new Point(waterPictureBox.Location.X, waterPictureBox.Location.Y);
                    mousePos.Offset(mouseMoveEvent.X, mouseMoveEvent.Y);
                    mousePos.X -= this.waterPictureBox.Width / 2;
                    mousePos.Y -= this.waterPictureBox.Height / 2;

                    //int offsetX = (mouseMoveEvent.X - prePoint1.X);
                    //int offsetY = mouseMoveEvent.Y - prePoint1.Y;
                    //waterPictureBox.Location = new Point(waterPictureBox.Location.X + offsetX, waterPictureBox.Location.Y + offsetY);

                    waterPictureBox.Location = mousePos;
                    }




                    //Console.WriteLine(mouseMoveEvent.X+","+ mouseMoveEvent.Y);


                }
            }
            //水印控件鼠标抬起事件
            void couponImg_watermarker_MouseUp(object mouseUpSender, MouseEventArgs mouseMoveEvent)
            {
                if (isDrag)
                {
                    isDrag = false;
                }
            }



            //开始订单截图
            PictureBox orderPb = this.pictureBox4;
            //开优惠券截图开始插入数据
            orderPb.Image = ResourceTool.getImage(Const.COUPON_BACK_IMG_NAME_ORDER);//初始化背景图片
            Graphics g2 = Graphics.FromImage(orderPb.Image);
            //商家名称
            String shopName2 = this.textBox11.Text;
            SolidBrush shopName2Brush = new SolidBrush(Color.Black);
            Font shopName2Font = new Font(Const.COUPON_FONT, 11);
            //获取字符串宽度
           // SizeF shopNameSize = g.MeasureString(shopName, shopNameFont);
            Point shopName2Point = new Point(34,56);
            g2.DrawString(shopName2, shopName2Font, shopName2Brush, shopName2Point, StringFormat.GenericDefault);
            //商品缩略图 123x123
            //Image goodPic = (Image)pictureBox1.Image.Clone();//拷贝一个图片
            g2.DrawImage(pictureBox1.Image, 11, 93, 94, 94);

            //商品名称 并且自动换行
            String goodName2 = this.textBox6.Text;
            Font goodName2Font = new Font(Const.COUPON_FONT, 11);
            SolidBrush goodName2Brush = new SolidBrush(ColorTool.getColorFromHtml("#707375"));
            Point goodName2Point = new Point(112, 91);

            Brush font2Brush = SystemBrushes.ControlText;
            //SizeF sizeText = e.Graphics.MeasureString(nodeText, font);
            //e.Graphics.DrawString(nodeText, font, fontBrush, (this.Width - sizeText.Width) / 2, (this.Height - sizeText.Height) / 2);

            StringFormat sf2 = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            g2.DrawString(goodName2, goodName2Font, font2Brush, new Rectangle(goodName2Point.X, goodName2Point.Y, 270, 36), sf2);

            //优惠券价格
            String couponValue2 = "省" + this.textBox3.Text+"元";
            Font couponValue2Font = new Font(Const.COUPON_FONT, 11);
            SolidBrush couponValue2Brush = new SolidBrush(ColorTool.getColorFromHtml("#707375"));
            SizeF couponValue2Size = g2.MeasureString(couponValue2, couponValue2Font);
            Point couponValue2Point = new Point((int)(82+260- couponValue2Size.Width),312);
            g2.DrawString(couponValue2, couponValue2Font, couponValue2Brush, couponValue2Point, StringFormat.GenericDefault);

            //现价
            String price2 = this.textBox2.Text;
            Font price2Font = new Font(Const.COUPON_FONT, 14);
            SolidBrush price2Brush = new SolidBrush(ColorTool.getColorFromHtml("#ff5001"));
            Point price2Point = new Point(127, 169);
            g2.DrawString(price2, price2Font, price2Brush, price2Point, StringFormat.GenericDefault);

            //券后价
            double priceAfter2 = double.Parse(this.textBox2.Text) - double.Parse(this.textBox3.Text);
            Font priceAfter2Font = new Font(Const.COUPON_FONT, 12);
            Font priceAfter3Font = new Font(Const.COUPON_FONT, 12);//小计

            SolidBrush priceAfter2Brush = new SolidBrush(ColorTool.getColorFromHtml("#ff5001"));
            Point priceAfter2Point = new Point(210, 612);
            Point priceAfter3Point = new Point(313, 490);
            g2.DrawString(priceAfter2 < 9999 ? (priceAfter2.ToString().Contains(".") ? priceAfter2.ToString() : priceAfter2.ToString() + ".00") : int.Parse(priceAfter2.ToString()).ToString(), priceAfter2Font, priceAfter2Brush, priceAfter2Point, StringFormat.GenericDefault);
            g2.DrawString(priceAfter2<9999?(priceAfter2.ToString().Contains(".")? priceAfter2.ToString(): priceAfter2.ToString()+".00"): int.Parse(priceAfter2.ToString()).ToString(), priceAfter3Font, priceAfter2Brush, priceAfter3Point, StringFormat.GenericDefault);


            //开始处理水印 订单截图
            Image watermarker2 = null;
            if (this.radioButton1.Checked)//图片水印
            {

                watermarker2 = this.pictureBox2.Image;
            }
            else
            {//文字水印
                Font watermarker2Font = null;
                if (checkBox1.Checked && !checkBox2.Checked)//加粗
                {
                    watermarker2Font = new Font(Const.COUPON_FONT, watermarkerFontSize, FontStyle.Bold | FontStyle.Italic);
                }
                else if (!checkBox1.Checked && checkBox2.Checked)//删除线
                {
                    watermarker2Font = new Font(Const.COUPON_FONT, watermarkerFontSize, FontStyle.Strikeout | FontStyle.Italic);
                }
                else if (checkBox1.Checked && checkBox2.Checked)//加粗 删除线
                {
                    watermarker2Font = new Font(Const.COUPON_FONT, watermarkerFontSize, FontStyle.Strikeout | FontStyle.Bold | FontStyle.Italic);
                }
                else
                {
                    watermarker2Font = new Font(Const.COUPON_FONT, watermarkerFontSize, FontStyle.Italic);
                }

                //设置文字水印的颜色
                if (String.IsNullOrWhiteSpace(textBox12.Text))
                {
                    MessageBox.Show("颜色代码不能为空！");
                    return;
                }
                Color fontColor;
                try
                {
                    fontColor = ColorTool.getColorFromHtml(textBox12.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("不能识别的颜色代码！" + textBox12.Text);
                    return;
                }
                SolidBrush watermarkerBrush = new SolidBrush(fontColor);

                SizeF watermarkerText = g2.MeasureString(this.textBox5.Text, watermarker2Font);

                int watermarkerImgWidth = (int)watermarkerText.Width + 1;
                int watermarkerImgHeight = (int)watermarkerText.Height + 1;
                Bitmap image = new Bitmap(watermarkerImgWidth, watermarkerImgHeight);
                Graphics gi = Graphics.FromImage(image);
                gi.Clear(Color.Transparent);//透明

                gi.DrawString(this.textBox5.Text, watermarker2Font, watermarkerBrush, new Rectangle(0, 0, watermarkerImgWidth, watermarkerImgHeight));
                watermarker2 = image;
                gi.Dispose();
            }

            waterPictureBox2 = new PictureBox();
            waterPictureBox2.Image = watermarker2;
            waterPictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            waterPictureBox2.Height = watermarker2.Height;
            waterPictureBox2.Width = watermarker2.Width;
            waterPictureBox2.Location = new Point(10, 410);
            //给水印图片添加鼠标按下事件
            //控件首次移动
            bool isDrag2 = false;
            Point prePoint2 = new Point();//优惠券截图中的水印图片位置（移动前）
            bool isFirstMove2 = true;
            waterPictureBox2.MouseDown += new MouseEventHandler(orderImg_watermarker_MouseDown);
            waterPictureBox2.MouseUp += new MouseEventHandler(orderImg_watermarker_MouseUp);
            waterPictureBox2.MouseMove += new MouseEventHandler(orderImg_watermarker_MouseMove);
            //this.textBox8.Text = waterPictureBox2.Height.ToString();
            //this.textBox9.Text = waterPictureBox2.Width.ToString();

            this.pictureBox4.Controls.Clear();
            this.pictureBox4.Controls.Add(waterPictureBox2);

            g2.Dispose();





            //水印控件按下事件
            void orderImg_watermarker_MouseDown(object downObj, MouseEventArgs mouseDownEvent)
            {
                //Console.WriteLine(mouseDownEvent.X + "," + mouseDownEvent.Y);

                if (mouseDownEvent.Button == MouseButtons.Left)
                {
                    isDrag2 = true;

                    prePoint2 = new Point(mouseDownEvent.X, mouseDownEvent.Y);
                }
            }
            //水印控件鼠标移动事件
            void orderImg_watermarker_MouseMove(object mouseMoveSender, MouseEventArgs mouseMoveEvent)
            {
                if (isDrag2)
                {
                    if (isFirstMove2)
                    {
                        waterPictureBox2.Location = new Point(waterPictureBox2.Location.X + 1, waterPictureBox2.Location.Y + 1);
                        isFirstMove2 = false;
                    }
                    else
                    {


                        //Console.WriteLine(mouseMoveEvent.X + "," + mouseMoveEvent.Y);
                        Point mousePos = new Point(waterPictureBox2.Location.X, waterPictureBox2.Location.Y);
                        mousePos.Offset(mouseMoveEvent.X, mouseMoveEvent.Y);
                        mousePos.X -= this.waterPictureBox.Width / 2;
                        mousePos.Y -= this.waterPictureBox.Height / 2;

                        //int offsetX = (mouseMoveEvent.X - prePoint1.X);
                        //int offsetY = mouseMoveEvent.Y - prePoint1.Y;
                        //waterPictureBox.Location = new Point(waterPictureBox.Location.X + offsetX, waterPictureBox.Location.Y + offsetY);

                        waterPictureBox2.Location = mousePos;
                    }




                    //Console.WriteLine(mouseMoveEvent.X+","+ mouseMoveEvent.Y);


                }
            }
            //水印控件鼠标抬起事件
            void orderImg_watermarker_MouseUp(object mouseUpSender, MouseEventArgs mouseMoveEvent)
            {
                if (isDrag2)
                {
                    isDrag2 = false;
                }
            }

            //按钮全部可用
            setMainFormBtnStatus(1);
            alert("截图已经生成!");
        }
        
        

        private bool validateData()//校验生成截图时用到的数据
        {
            //自定义券价格校验
            if (String.IsNullOrWhiteSpace(this.textBox3.Text))
            {
                alert("请输入自定义券价格");
                return false;
            }

            //如果选择的是水印图片,校验图片是否获取到
            if (this.radioButton1.Checked)
            {
                if (this.pictureBox2.Image == null)
                {
                    alert("请重新选择水印图片");
                    return false;
                }
            }

            //如果选择的是文字图片，允许为空

            //校验商品图片
            if (this.pictureBox1.Image == null)
            {
                if (String.IsNullOrWhiteSpace(this.textBox7.Text))//商品图片空，商品地址也为空
                {
                    alert("请输入正确的商品图片链接");
                    return false;
                }
                else
                {
                    //根据图片地址生成图片
                    //判断图片路径是否合法（本地图片或者网络图片）
                    int isLegal = ImageTool.isLegalOfImgPath(this.textBox7.Text);//0 不合法 1 网络图片 2 本地图片
                    if (isLegal==0)//不合法
                    {
                        alert("请输入合法的图片路径（本地或网络图片完整地址）");
                        return false;
                    }

                    try
                    {
                        Image goodPic = null;
                        if (isLegal == 1)//网络图片
                        {
                            goodPic = ImageTool.getImageBy(this.textBox7.Text);
                        }
                        else //本地图片
                        {
                            goodPic = ImageTool.getLocalImageBy(this.textBox7.Text);
                        }
                        if (goodPic == null)
                        {
                            alert("不能获取商品图片，请检查商品图片路径是否正确1");
                            return false;
                        }
                        this.pictureBox1.Image = goodPic;
                    }
                    catch(Exception e)
                    {
                        alert("不能获取商品图片，请检查商品图片路径是否正确");
                        MyLogUtil.ErrToLog("不能正确加载图片："+e);
                        return false;
                    }

                }
            }

            //检验商品价格
            if(String.IsNullOrWhiteSpace(this.textBox2.Text))
            {
                alert("商品价格不能为空");
                return false;
            }
            if (this.textBox2.Text.EndsWith("."))
            {
                alert("商品价格不能以.结尾");
                return false;
            }

            //检验商品销量
            if (String.IsNullOrWhiteSpace(this.textBox4.Text))
            {
                alert("商品销量不能为空");
                return false;
            }
            //检验名称
            if (String.IsNullOrWhiteSpace(this.textBox6.Text))
            {
                alert("商品名称不能为空");
                return false;
            }

            //检验店铺名称
            if (String.IsNullOrWhiteSpace(this.textBox11.Text))
            {
                alert("商品名称不能为空");
                return false;
            }

            return true;
        }

        private void button5_Click(object sender, EventArgs e)//更改水印尺寸
        {
            if (String.IsNullOrWhiteSpace(this.textBox9.Text))
            {
                alert("水印宽度不能为空");
                return;
            }
            else
            {
                waterPictureBox.Width = int.Parse(this.textBox9.Text);
            }
            if (String.IsNullOrWhiteSpace(this.textBox8.Text))
            {
                alert("水印高度不能为空");
                return;
            }
            else
            {
                waterPictureBox.Height = int.Parse(this.textBox8.Text);
            }
            
            
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)//复制优惠券截图
        {
            //合并图片
            //背景图片 this.pictureBox3.Image
            Image watermarkerBackImg = (Image)this.pictureBox3.Image.Clone();
            Graphics g = Graphics.FromImage(watermarkerBackImg);
            Image waterImg = ImageTool.resetImgSize(waterPictureBox.Image, waterPictureBox.Width, waterPictureBox.Height);

            g.DrawImage(waterImg, new Point(waterPictureBox.Location.X, waterPictureBox.Location.Y));

            g.Dispose();
            //Clipboard.SetImage(watermarkerBackImg);
            //alert("优惠券截图已拷贝到剪贴板！");
            new ImageForm(watermarkerBackImg).Show();

        }

        private void button4_Click(object sender, EventArgs e)//估值订单详情截图
        {
            //合并图片
            //背景图片 this.pictureBox3.Image
            Image watermarkerBackImg = (Image)this.pictureBox4.Image.Clone();
            Graphics g = Graphics.FromImage(watermarkerBackImg);
            Image waterImg = ImageTool.resetImgSize(waterPictureBox2.Image, waterPictureBox2.Width, waterPictureBox2.Height);

            g.DrawImage(waterImg, new Point(waterPictureBox2.Location.X, waterPictureBox2.Location.Y));

            g.Dispose();
            //Clipboard.SetImage(watermarkerBackImg);
            //alert("订单详情截图已拷贝到剪贴板！");
            new ImageForm(watermarkerBackImg).Show();
        }

        private void button6_Click_1(object sender, EventArgs e)//更改订单中水印图片尺寸
        {
            if (String.IsNullOrWhiteSpace(this.textBox9.Text))
            {
                alert("水印宽度不能为空");
                return;
            }
            else
            {
                waterPictureBox2.Width = int.Parse(this.textBox9.Text);
            }
            if (String.IsNullOrWhiteSpace(this.textBox8.Text))
            {
                alert("水印高度不能为空");
                return;
            }
            else
            {
                waterPictureBox2.Height = int.Parse(this.textBox8.Text);
            }

        }

        private void button9_Click(object sender, EventArgs e)//抓取更多评论

        {
            setMainFormBtnStatus(8);
            if (dataBean == null)
            {
                setMainFormBtnStatus(9);
                alert("请通过本软件获取商品信息才能抓取评论");
            }
            else//单线程版本 目前
            {
                if (string.IsNullOrWhiteSpace(dataBean.RateUrl)){

                    MessageBox.Show("暂不支持淘宝抓取评论");
                    setMainFormBtnStatus(9);
                    return;
                }
                /*
                 * 疑问：第一次抓取不到评论
                 * 
                 * 
                 */
                //  LogTool.log("开始抓取评论...", logLabel);
                String rateContent = WebTool.getHtmlContent(dataBean.RateUrl + (dataBean.CurrentPage)+"&order=1");
                //Console.WriteLine("抓取的网页内容："+ dataBean.RateUrl + (dataBean.CurrentPage) + "&order=1");
                //Console.WriteLine(rateContent);
                //Regex regPid = new Regex("^\"rateContent\": \"([\\s\\S]*)\"$");
                // 搜索匹配的字符串
                MatchCollection matches = Regex.Matches(rateContent, ",\"rateContent\":\"([\\s\\S]*?)\",");
                if (matches.Count <= 0)//到达最后一页
                {
                    setMainFormBtnStatus(9);
                    //Console.WriteLine("不能获取更多评论");
                    alert("没有更多或淘宝拦截请再次尝试");
                }
                else
                {
                    this.textBox10.Text = "";//每次翻页，清空当前页面内容
                    foreach (Match match in matches)
                    {
                        //Console.WriteLine("匹配的结果："+ match.Groups[1].Value);
                        this.textBox10.Text = this.textBox10.Text  + "  "+match.Groups[1].Value + "\r\n\r\n";

                    }
                }
                setMainFormBtnStatus(9);

                dataBean.CurrentPage += 1;
            }
        }

        private void button10_Click(object sender, EventArgs e)//抓取更多图片
        {
            setMainFormBtnStatus(8);
            if (dataBean == null)
            {
                setMainFormBtnStatus(9);
                alert("请通过本软件获取商品信息才能抓取图片");
            }
            else//单线程版本 目前
            {
                if (string.IsNullOrWhiteSpace(dataBean.RateUrl))
                {
                    MessageBox.Show("暂不支持淘宝抓取评论");
                    setMainFormBtnStatus(9);
                    return;
                }
                LogTool.log("开始抓取评论...", logLabel);
                String rateContent = WebTool.getHtmlContent(dataBean.RateUrl + dataBean.CurrentPage + "&order=3&picture=1");
                //Console.WriteLine("抓取的网页内容：");
                //Console.WriteLine(rateContent);
                MatchCollection matches = Regex.Matches(rateContent, "\"pics\":\\[\"([\\s\\S]*?)\"\\],\"");
                if (matches.Count <= 0)//到达最后一页
                {
                    setMainFormBtnStatus(9);
                    Console.WriteLine("不能获取更多图片");
                    alert("没有更多或淘宝拦截请再次尝试");
                }
                else
                {
                    
                    dataBean.RateImg = new List<string>();
                    foreach (Match match in matches)
                    {
                        //Console.WriteLine("匹配的结果："+ match.Groups[1].Value);
                        //this.textBox10.Text = this.textBox10.Text + "  " + match.Groups[1].Value + "\r\n\r\n";
                        
                        if (!String.IsNullOrWhiteSpace(match.Groups[1].Value))
                        {
                            String[] picUrls = match.Groups[1].Value.Split("\",\"".ToCharArray());
                            //Console.WriteLine("抓取的图片："+ match.Groups[1].Value);
                            foreach(String picUrl in picUrls)
                            {
                                if (!String.IsNullOrWhiteSpace(picUrl))
                                {
                                    dataBean.RateImg.Add(StringTool.replaceStartWith(picUrl, "//", "https://"));
                                }
                                
                            }

                            this.pictureBox5.Image = ImageTool.getImageBy(dataBean.RateImg[0]);
                            dataBean.CurrentRateImgIndex = 0;


                        }
                    }
                }


                dataBean.CurrentPage += 1;
                setMainFormBtnStatus(9);

            }

        }

        private void button7_Click(object sender, EventArgs e)//换一张图片
        {
            
            if (dataBean == null)
            {
                alert("请通过本软件获取商品信息才能抓取图片");
            }
            else//单线程版本 目前
            {
                if (dataBean.CurrentRateImgIndex != null)
                {
                    if (dataBean.RateImg != null)
                    {
                        if (dataBean.RateImg.Count - 1 <= dataBean.CurrentRateImgIndex)
                        {
                            alert("已到最后，重新抓取");
                        }
                        else
                        {
                            Console.WriteLine("获取的地址：" + dataBean.RateImg[dataBean.CurrentRateImgIndex + 1]);
                            this.pictureBox5.Image = ImageTool.getImageBy(dataBean.RateImg[dataBean.CurrentRateImgIndex + 1]);
                        }
                        dataBean.CurrentRateImgIndex++;
                    }
                    else
                    {
                        alert("请重新获取更多数据");
                    }
                    
                }
                
            }
        }

        private void button8_Click(object sender, EventArgs e)//复制图片
        {
            if (this.pictureBox5.Image == null)
            {
                alert("请先抓取图片！");
            }
            else
            {
                Clipboard.SetImage(this.pictureBox5.Image);
                alert("订单详情截图已拷贝到剪贴板！");

            }
            
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://tool.chinaz.com/tools/use");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否关闭窗口", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                e.Cancel = true;

            }
            else
            {
                //_qrWebWeChat.jieshu = false;
                //程序完全退出
                YiYunUtil.Logout(CacheData.UserName, CacheData.Ret);
                System.Environment.Exit(0);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            YiYunUtil.CheckUserStatus();
            //YiYunUtil.Val();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            YiYunUtil.CheckUserStatus();
            //YiYunUtil.Val();
            
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {

                WebBrowser webBrowser = (WebBrowser)sender;
                if (webBrowser1.ReadyState < WebBrowserReadyState.Complete) return;
                //天猫评论 在url上加
                if (webBrowser.Url.ToString().Contains("tmall"))
                {
                    webBrowser1.Document.Window.ScrollTo(200, 980);
                    HtmlDocument htmlDoc = webBrowser1.Document;

                    HtmlElementCollection bodyList = webBrowser1.Document.GetElementsByTagName("body");
                    Console.WriteLine(bodyList.Count);
                    //滚动条解决 https://www.cnblogs.com/fj99/p/4218801.html
                    if (bodyList != null && bodyList.Count > 0)
                    {
                        HtmlElement body = bodyList[0];
                        //body.SetAttribute("scroll", "no");
                        //body.SetAttribute("style", "overflow-x:hidden");
                        if (body.Style == null)
                        {
                            body.Style = " overflow-x: hidden; ";
                        }
                        else
                        {
                            body.Style += " overflow-x: hidden; ";
                        }
                    }
                }else if (webBrowser.Url.ToString().Contains("taobao"))
                {
                    webBrowser1.Document.Window.ScrollTo(200, 1280);
                    HtmlDocument htmlDoc = webBrowser1.Document;

                    HtmlElementCollection bodyList = webBrowser1.Document.GetElementsByTagName("body");
                    Console.WriteLine(bodyList.Count);
                    //滚动条解决 https://www.cnblogs.com/fj99/p/4218801.html
                    if (bodyList != null && bodyList.Count > 0)
                    {
                        HtmlElement body = bodyList[0];
                        //body.SetAttribute("scroll", "no");
                        //body.SetAttribute("style", "overflow-x:hidden");
                        if (body.Style == null)
                        {
                            body.Style = " overflow-x: hidden; ";
                        }
                        else
                        {
                            body.Style += " overflow-x: hidden; ";
                        }
                    }
                }

            }catch(Exception ex)
            {
                MyLogUtil.ErrToLog("加载网页失败，原因："+ex);
            }


        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            new TemplateForm(this).Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {

            this.ResetTemplates();
            MessageBox.Show("重新加载模板成功！");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox7.Text))
            {
                MessageBox.Show("请输入正确的图片地址");
                return;
            }
            else
            {
                try
                {
                    this.pictureBox1.Image = ImageTool.getImageBy(textBox7.Text);
                }
                catch(Exception ex)
                {
                    MyLogUtil.ErrToLog("加载图片失败，原因："+ex);
                    MessageBox.Show("加载图片失败，请检查图片地址");
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(textBox1.Text))
                {
                    //在webbroswer加载评论
                    this.webBrowser1.Navigate(textBox1.Text);
                }
                else
                {
                    MessageBox.Show("请在商品详情链接中添加正确地址");
                }
            }catch(Exception ex)
            {
                MyLogUtil.ErrToLog("加载评论失败，原因："+ex);
                MessageBox.Show("请检查商品详情链接是否正确");
            }
            
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (templateDisplayForm == null)
            {
                templateDisplayForm = new TemplateDisplayForm(this);
            }
            if (checkBox3.Checked)
            {
                if (string.IsNullOrWhiteSpace((string)comboBox1.SelectedValue))
                {
                    MessageBox.Show("请先选中模板");
                    return;
                }

                //加载选中的模板
                String templateConfigPath = MySystemUtil.GetTemplateImgRoot() + comboBox1.SelectedValue + "\\template.json";
                if (!File.Exists(templateConfigPath))
                {
                    MessageBox.Show("当前选中的模板信息不完整");
                    return;
                }
                string content = MyFileUtil.readFileAll(templateConfigPath);
                MyJsonUtil<TemplateConfig> myJsonUtil = new MyJsonUtil<TemplateConfig>();
                TemplateConfig templateConfig = myJsonUtil.parseJsonStr(content);


                //获取图片模板
                String templateImgPath = MySystemUtil.GetTemplateImgRoot() + comboBox1.SelectedValue + "\\" + templateConfig.BackImg;
                if (!File.Exists(templateImgPath))
                {
                    MessageBox.Show("当前选中的模板中不存在背景图片");
                    return;
                }
                Image img = Image.FromFile(templateImgPath);
                templateDisplayForm.DisplayImg(img);
            }
            else
            {
                templateDisplayForm.Hide();
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox3.Checked&& templateDisplayForm!=null)
            {
                templateDisplayForm.Hide();
            }
        }
    }
}
