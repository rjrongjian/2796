using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepNaiWorkshop_2796
{
    public partial class MainForm : Form
    {
        private Label logLabel;
        private Image originalCouponPic;//原始的优惠券截图背景图片
        private PictureBox waterPictureBox;//水印图片控件
        public MainForm()
        {
            InitializeComponent();
        }
        //TODO 网络读取图片 http://www.jb51.net/article/78993.htm




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
                    alert(validateResult.message);
                }
                else
                {
                    //开始解析数据
                    LogTool.log("开始解析天猫（淘宝）详情页面：" + url, this.logLabel);
                    
                    LogTool.log("正在获取网页信息", this.logLabel);
                    String htmlWebContent = WebTool.getHtmlContent(url);
                    LogTool.log("开始解析网页", this.logLabel);
                    //目前只支持天猫

                    if (TaoBaoTool.GOOD_TYPE_TMALL == int.Parse(validateResult.message))
                    {
                        BaseDataBean dataBean = taoBaoTool.parseShopData(int.Parse(validateResult.message), htmlWebContent);
                        if (dataBean == null)
                        {
                            alert("不能正常解析数据，请手动录入");
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
                        

                    }
                    else
                    {
                        alert("目前不支持淘宝商品数据爬取，请手动录入");
                    }

                    setMainFormBtnStatus(7);//获取商品数据、生成截图按钮可用
                }
            }
            catch(Exception ex)
            {
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

            originalCouponPic = this.pictureBox3.Image;
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
                
            }
            else if (status == 2)//2 按钮全部不可用
            {
                this.button1.Enabled = false;
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                this.button4.Enabled = false;
                this.button5.Enabled = false;
                
            }
            else if (status == 3)//3 获取商品信息按钮可用
            {
                this.button1.Enabled = true;
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                this.button4.Enabled = false;
                this.button5.Enabled = false;
                
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
                
            }
            else if (status == 6)//6 截图订单详情按钮可用
            {
                this.button1.Enabled = false;
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                this.button4.Enabled = true;
            }
            else if (status == 7)//7 获取商品数据、生成截图按钮可用
            {
                this.button1.Enabled = true;
                this.button2.Enabled = true;
                this.button3.Enabled = false;
                this.button4.Enabled = false;
                this.button5.Enabled = false;
                
            }
            else
            {
                LogTool.log("未能识别的按钮状态值："+status,this.logLabel);
                this.button1.Enabled = false;
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                this.button4.Enabled = false;
                this.button5.Enabled = false;
                
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
            bool validateResult = validateData();
            if (!validateResult)//生成截图时用到的数据都正常
            {
                //Console.WriteLine("将数据生成到页面");
                return;
            }

            PictureBox couponPb = this.pictureBox3;
            //开优惠券截图开始插入数据
            couponPb.Image = ResourceTool.getImage(Const.COUPON_BACK_IMG_NAME); ;//初始化背景图片
            Graphics g = Graphics.FromImage(couponPb.Image);
            //商家名称
            String shopName = this.textBox11.Text;
            SolidBrush shopNameBrush = new SolidBrush(Color.White);
            Font shopNameFont = new Font(Const.COUPON_FONT, 11);
            //获取字符串宽度
            SizeF shopNameSize = g.MeasureString(shopName, shopNameFont);
            Point shopNamePoint = new Point((int)(couponPb.Image.Width- shopNameSize.Width)/2, 119);
            
            g.DrawString(shopName, shopNameFont, shopNameBrush, shopNamePoint, StringFormat.GenericDefault);
            //商品缩略图 123x123
            //Image goodPic = (Image)pictureBox1.Image.Clone();//拷贝一个图片
            g.DrawImage(pictureBox1.Image, 14, 291, 114, 114);

            //优惠券价格
            String rmb = "￥";
            SolidBrush rmbBrush = new SolidBrush(ColorTool.getColorFromHtml("#d0021b"));
            Font rmbFont = new Font(Const.COUPON_FONT, 20);
            SizeF rmbSize = g.MeasureString(rmb, rmbFont);
            

            String couponValue = this.textBox3.Text;
            Font couponValueFont = new Font(Const.COUPON_FONT, 38);
            SizeF couponValueSize = g.MeasureString(couponValue, couponValueFont);

            int baseX = 51;
            int baseY = 160;
            Point rmbPoint = new Point(((int)(170-rmbSize.Width- couponValueSize.Width)/2)+baseX, baseY+(int)(couponValueSize.Height-rmbSize.Width));
            Point couponValuePoint = new Point((int)(rmbPoint.X+rmbSize.Width), (int)(rmbPoint.Y- (couponValueSize.Height - rmbSize.Width)));
            g.DrawString(rmb, rmbFont, rmbBrush, rmbPoint, StringFormat.GenericDefault);
            g.DrawString(couponValue, couponValueFont, rmbBrush, couponValuePoint, StringFormat.GenericDefault);

            //优惠券日期
            String couponTime = this.dateTimePicker1.Value.ToString("yyyy.MM.dd")+ "-" + this.dateTimePicker2.Value.ToString("yyyy.MM.dd");
            Font couponTimeFont = new Font(Const.COUPON_FONT, 10);
            SolidBrush couponTimeBrush = new SolidBrush(ColorTool.getColorFromHtml("#9b9b9b"));
            SizeF couponTimeSize = g.MeasureString(couponTime, couponTimeFont);
            Point couponTimePoint = new Point(51+(int)(170- couponTimeSize.Width)/2,230);
            g.DrawString(couponTime, couponTimeFont, couponTimeBrush, couponTimePoint, StringFormat.GenericDefault);

            //商品名称 并且自动换行
            String goodName = this.textBox6.Text;
            Font goodNameFont = new Font(Const.COUPON_FONT, 11);
            SolidBrush goodNameBrush = new SolidBrush(ColorTool.getColorFromHtml("#333"));
            Point goodNamePoint = new Point(140,288);

            Brush fontBrush = SystemBrushes.ControlText;
            //SizeF sizeText = e.Graphics.MeasureString(nodeText, font);
            //e.Graphics.DrawString(nodeText, font, fontBrush, (this.Width - sizeText.Width) / 2, (this.Height - sizeText.Height) / 2);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            g.DrawString(goodName, goodNameFont, fontBrush, new Rectangle(goodNamePoint.X, goodNamePoint.Y,220,37), sf);
            //现价
            String price = this.textBox2.Text;
            Font priceFont = new Font(Const.COUPON_FONT, 11);
            SolidBrush priceBrush = new SolidBrush(ColorTool.getColorFromHtml("#333"));
            Point pricePoint = new Point(181,363);
            g.DrawString(price, priceFont, priceBrush, pricePoint, StringFormat.GenericDefault);

            //成交量
            String volume = this.textBox4.Text+"笔成交";
            Font volumeFont = new Font(Const.COUPON_FONT, 11);
            SolidBrush volumeBrush = new SolidBrush(ColorTool.getColorFromHtml("#9b9b9b"));
            SizeF volumeSize = g.MeasureString(volume, volumeFont);
            Point volumePoint = new Point(375- (int)volumeSize.Width- 15, 336);
            g.DrawString(volume, volumeFont, volumeBrush, volumePoint, StringFormat.GenericDefault);

            //券后价
            double priceAfter = double.Parse(price) -double.Parse(couponValue);
            Font priceAfterFont = new Font(Const.COUPON_FONT, 24);
            
            SolidBrush priceAfterBrush = new SolidBrush(ColorTool.getColorFromHtml("#f40"));
            Point priceAfterPoint = new Point(216, 379);
            g.DrawString(priceAfter.ToString(), priceAfterFont, priceAfterBrush, priceAfterPoint, StringFormat.GenericDefault);

            
            //开始处理水印
            Image watermarker = null;
            if (this.radioButton1.Checked)//图片水印
            {

                watermarker = this.pictureBox2.Image ;
            }
            else {//文字水印
                Font watermarkerFont = null;
                if (checkBox1.Checked&&!checkBox2.Checked)//加粗
                {
                    watermarkerFont = new Font(Const.COUPON_FONT, 12, FontStyle.Bold|FontStyle.Italic);
                }else if(!checkBox1.Checked && checkBox2.Checked)//删除线
                {
                    watermarkerFont = new Font(Const.COUPON_FONT, 12, FontStyle.Strikeout | FontStyle.Italic);
                }
                else if(checkBox1.Checked && checkBox2.Checked)//加粗 删除线
                {
                    watermarkerFont = new Font(Const.COUPON_FONT, 12, FontStyle.Strikeout | FontStyle.Bold | FontStyle.Italic);
                }
                else
                {
                    watermarkerFont = new Font(Const.COUPON_FONT, 12,FontStyle.Italic);
                }
                SolidBrush watermarkerBrush = new SolidBrush(ColorTool.getColorFromHtml("#333333"));
                
                SizeF watermarkerText = g.MeasureString(this.textBox5.Text, watermarkerFont);

                int watermarkerImgWidth = (int)watermarkerText.Width+1;
                int watermarkerImgHeight = (int)watermarkerText.Height +1;
                Bitmap image = new Bitmap(watermarkerImgWidth, watermarkerImgHeight);
                Graphics gi = Graphics.FromImage(image);
                gi.Clear(Color.Transparent);//透明
                
                gi.DrawString(this.textBox5.Text, watermarkerFont, watermarkerBrush, new Rectangle(0, 0, watermarkerImgWidth, watermarkerImgHeight));
                watermarker = image;
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

            //按钮全部可用
            setMainFormBtnStatus(1);


            
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

                    
                    Console.WriteLine(mouseMoveEvent.X + "," + mouseMoveEvent.Y);
                    Point mousePos = new Point(waterPictureBox.Location.X, waterPictureBox.Location.Y);
                    mousePos.Offset(mouseMoveEvent.X, mouseMoveEvent.Y);
                    //mousePos.X -= this.waterPictureBox.Width / 2;
                    //mousePos.Y -= this.waterPictureBox.Height / 2;

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
                            alert("不能获取商品图片，请检查商品图片路径是否正确");
                            return false;
                        }
                        this.pictureBox1.Image = goodPic;
                    }
                    catch(Exception e)
                    {
                        alert("不能获取商品图片，请检查商品图片路径是否正确");
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

            g.DrawImage(waterImg, new Point(waterPictureBox.Width, waterPictureBox.Height));

            g.Dispose();
            Clipboard.SetImage(watermarkerBackImg);
            alert("优惠券截图已拷贝到剪贴板！");

        }
    }
}
