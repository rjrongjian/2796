using DeepNaiWorkshop_2796.MyModel;
using DeepNaiWorkshop_2796.MyTool;
using MyTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeepNaiWorkshop_2796
{
    public partial class TemplateForm : Form
    {
        private StringFormat sf;
        private StringFormat sf1;
        private Image UploadBackImg;
        private Image UploadOriginalImg;
        public TemplateForm()
        {
            InitializeComponent();

            this.comboBox1.DataSource = CacheData.fontList;//防止多个combobox绑定同一个数据源导致事件联动
            this.comboBox1.ValueMember = "moduleName";
            this.comboBox1.DisplayMember = "moduleName";

            this.comboBox2.DataSource = CacheData.fontList;//防止多个combobox绑定同一个数据源导致事件联动
            this.comboBox2.ValueMember = "moduleName";
            this.comboBox2.DisplayMember = "moduleName";


            this.comboBox3.DataSource = CacheData.fontList;//防止多个combobox绑定同一个数据源导致事件联动
            this.comboBox3.ValueMember = "moduleName";
            this.comboBox3.DisplayMember = "moduleName";

            this.comboBox4.DataSource = CacheData.fontList;//防止多个combobox绑定同一个数据源导致事件联动
            this.comboBox4.ValueMember = "moduleName";
            this.comboBox4.DisplayMember = "moduleName";

            this.comboBox5.DataSource = CacheData.fontList;//防止多个combobox绑定同一个数据源导致事件联动
            this.comboBox5.ValueMember = "moduleName";
            this.comboBox5.DisplayMember = "moduleName";

            this.comboBox6.DataSource = CacheData.fontList;//防止多个combobox绑定同一个数据源导致事件联动
            this.comboBox6.ValueMember = "moduleName";
            this.comboBox6.DisplayMember = "moduleName";

            this.comboBox7.DataSource = CacheData.fontList;//防止多个combobox绑定同一个数据源导致事件联动
            this.comboBox7.ValueMember = "moduleName";
            this.comboBox7.DisplayMember = "moduleName";


            sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            sf1 = new StringFormat();
            sf1.Alignment = StringAlignment.Near;
            sf1.LineAlignment = StringAlignment.Center;
        }
        //上传背景图片
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "图像文件(*.jpg;*.jpg;*.jpeg;*.gif;*.png;*.bmp)|*.jpg;*.jpeg;*.gif;*.png;*.bmp";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string file = dialog.FileName;
                Image watermark = ImageTool.getLocalImageBy(file);
                UploadBackImg = watermark;
                UploadOriginalImg = watermark;

                int watermarkHeight = Convert.ToUInt16(watermark.Height.ToString());
                int watermarkWidth = Convert.ToUInt16(watermark.Width.ToString());
                if (watermarkHeight > 558)
                {

                    watermarkHeight = 558;//水印高
                    MessageBox.Show("超过水印图片的最大高度558px,自动缩减大小至558px");

                }
                if (watermarkWidth > 375)
                {
                    watermarkWidth = 375;//水印宽
                    MessageBox.Show("超过水印图片的最大宽度375px,自动缩减大小至375px");
                }
                
                this.numericUpDown2.Value = watermarkHeight;//水印高
                this.numericUpDown1.Value = watermarkWidth;//水印宽
                this.textBox1.Text = file;

                UploadBackImg = ImageClass.ResizeImage(new Bitmap(UploadBackImg), Convert.ToInt16(this.numericUpDown1.Value), Convert.ToInt16(this.numericUpDown2.Value));
                this.pictureBox1.Image = UploadBackImg;

            }
        }
        //调整背景
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || UploadBackImg == null)
            {
                MessageBox.Show("请先选择背景模板图片！");
                return;
            }
            UploadBackImg = ImageClass.ResizeImage(new Bitmap(UploadOriginalImg), Convert.ToInt16(this.numericUpDown1.Value), Convert.ToInt16(this.numericUpDown2.Value));
            this.pictureBox1.Image = UploadBackImg;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://tool.chinaz.com/tools/use");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://tool.chinaz.com/tools/use");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://tool.chinaz.com/tools/use");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://tool.chinaz.com/tools/use");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://tool.chinaz.com/tools/use");
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://tool.chinaz.com/tools/use");
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://tool.chinaz.com/tools/use");
        }

        //private Image shopNameWatermark;
        //private PictureBox shopNamePictureBox;
        //店铺名称
        private void button4_Click(object sender, EventArgs e)
        {
            if (!checkBox2.Checked)
            {
                MessageBox.Show("勾选模块后才能在模板中调试");
                return;
            }
            if (UploadBackImg == null)
            {
                MessageBox.Show("请先上传背景图片");
                return;
            }
            if (shopNamePictureBox != null)
            {
                this.pictureBox1.Controls.Remove(shopNamePictureBox);

            }
            int fontSizeTemp = Convert.ToInt16(numericUpDown3.Value);
            String fontColorTemp = textBox12.Text;
            String fontType = comboBox1.SelectedText;
            int xPointTemp = Convert.ToInt16(numericUpDown5.Value);
            int yPointTemp = Convert.ToInt16(numericUpDown4.Value);
            int widthTemp = Convert.ToInt16(numericUpDown7.Value);
            int heightTemp = Convert.ToInt16(numericUpDown6.Value);
            String contentTemp = textBox2.Text;
            if (String.IsNullOrWhiteSpace(fontColorTemp))
            {

                MessageBox.Show("请指定字体颜色码");
                return;
            }
            if (String.IsNullOrWhiteSpace(fontColorTemp))
            {

                MessageBox.Show("请指定字体");
                return;
            }
            if (String.IsNullOrWhiteSpace(contentTemp))
            {
                MessageBox.Show("请输入测试内容，用于显示");
                return;
            }

            Font watermarkerFont = null;
            FontStyle result = FontStyle.Regular;
            List<FontStyle> style = new List<FontStyle>();
            /*
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
            */
            watermarkerFont = new Font(fontType, fontSizeTemp, result);
            Color fontColor;
            try
            {
                fontColor = ColorTool.getColorFromHtml(fontColorTemp);
            }
            catch (Exception ex)
            {
                MessageBox.Show("不能识别的颜色代码！" + fontColorTemp);
                return;
            }
            SolidBrush watermarkerBrush = new SolidBrush(fontColor);
            //SolidBrush watermarkerBrush = new SolidBrush(Color.);
            //SizeF watermarkerText = Graphics.FromImage(pictureBox1.Image).MeasureString(contentTemp, watermarkerFont);

            //防止随着图片大小改变而缩放尺寸
            Bitmap image2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics gi2 = Graphics.FromImage(image2);
            SizeF watermarkerText = gi2.MeasureString(contentTemp, watermarkerFont);
            //--------------end--------------

            int watermarkerImgWidth = widthTemp;
            int watermarkerImgHeight = heightTemp;
            Bitmap image = new Bitmap(watermarkerImgWidth, watermarkerImgHeight);
            Graphics gi = Graphics.FromImage(image);
            gi.Clear(Color.Green);//透明
            gi2.Clear(Color.Transparent);//透明




            gi.DrawString(contentTemp, watermarkerFont, watermarkerBrush, new Rectangle(0, 0, watermarkerImgWidth, watermarkerImgHeight),sf);
            shopNameWatermark = image;


            //事件
            shopNamePictureBox = new PictureBox();
            shopNamePictureBox.BackColor = Color.Transparent;
            shopNamePictureBox.Image = shopNameWatermark;
            shopNamePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            shopNamePictureBox.Height = shopNameWatermark.Height;
            shopNamePictureBox.Width = shopNameWatermark.Width;
            shopNamePictureBox.Location = new Point(xPointTemp, yPointTemp);
            //给水印图片添加鼠标按下事件
            //控件首次移动
            bool isDrag = false;
            Point prePoint1 = new Point();//优惠券截图中的水印图片位置（移动前）
            bool isFirstMove = true;
            shopNamePictureBox.MouseDown += new MouseEventHandler(couponImg_watermarker_MouseDown);
            shopNamePictureBox.MouseUp += new MouseEventHandler(couponImg_watermarker_MouseUp);
            shopNamePictureBox.MouseMove += new MouseEventHandler(couponImg_watermarker_MouseMove);
            //this.textBox8.Text = waterPictureBox.Height.ToString();
            //this.textBox9.Text = waterPictureBox.Width.ToString();
            
            
            this.pictureBox1.Controls.Add(shopNamePictureBox);


            gi2.Dispose();
            gi.Dispose();




            //水印控件按下事件
            void couponImg_watermarker_MouseDown(object downObj, MouseEventArgs mouseDownEvent)
            {
                //Console.WriteLine(mouseDownEvent.X + "," + mouseDownEvent.Y);

                if (mouseDownEvent.Button == MouseButtons.Left)
                {
                    isDrag = true;

                    prePoint1 = new Point(mouseDownEvent.X, mouseDownEvent.Y);
                }
            }
            //水印控件鼠标移动事件
            void couponImg_watermarker_MouseMove(object mouseMoveSender, MouseEventArgs mouseMoveEvent)
            {
                if (isDrag)
                {
                    if (isFirstMove)
                    {
                        shopNamePictureBox.Location = new Point(shopNamePictureBox.Location.X + 1, shopNamePictureBox.Location.Y + 1);
                        isFirstMove = false;
                    }
                    else
                    {


                        //Console.WriteLine(mouseMoveEvent.X + "," + mouseMoveEvent.Y);
                        Point mousePos = new Point(shopNamePictureBox.Location.X, shopNamePictureBox.Location.Y);
                        mousePos.Offset(mouseMoveEvent.X, mouseMoveEvent.Y);
                        mousePos.X -= this.shopNamePictureBox.Width / 2;
                        mousePos.Y -= this.shopNamePictureBox.Height / 2;

                        //int offsetX = (mouseMoveEvent.X - prePoint1.X);
                        //int offsetY = mouseMoveEvent.Y - prePoint1.Y;
                        //waterPictureBox.Location = new Point(waterPictureBox.Location.X + offsetX, waterPictureBox.Location.Y + offsetY);

                        shopNamePictureBox.Location = mousePos;
                        this.numericUpDown5.Value = mousePos.X;
                        this.numericUpDown4.Value = mousePos.Y;
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
            MessageBox.Show("托动用以调整位置");
        }
        private PictureBox shopSLTYPictureBox;
        private Image shopSLTYWatermark;
        //缩略图
        private void button5_Click(object sender, EventArgs e)
        {
            if (!checkBox2.Checked)
            {
                MessageBox.Show("勾选模块后才能在模板中调试");
                return;
            }
            if (UploadBackImg == null)
            {
                MessageBox.Show("请先上传背景图片");
                return;
            }
            if (shopSLTYPictureBox != null)
            {
                this.pictureBox1.Controls.Remove(shopSLTYPictureBox);

            }

            int xPointTemp = Convert.ToInt16(numericUpDown11.Value);
            int yPointTemp = Convert.ToInt16(numericUpDown10.Value);
            int widthTemp = Convert.ToInt16(numericUpDown9.Value);
            int heightTemp = Convert.ToInt16(numericUpDown8.Value);



            Font watermarkerFont = null;
            FontStyle result = FontStyle.Regular;
            List<FontStyle> style = new List<FontStyle>();
            /*
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
            */

            //SolidBrush watermarkerBrush = new SolidBrush(Color.);
            //SizeF watermarkerText = Graphics.FromImage(pictureBox1.Image).MeasureString(contentTemp, watermarkerFont);

            //防止随着图片大小改变而缩放尺寸
            Bitmap image2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics gi2 = Graphics.FromImage(image2);
            //--------------end--------------

            int watermarkerImgWidth = widthTemp;
            int watermarkerImgHeight = heightTemp;
            Bitmap image = new Bitmap(widthTemp, heightTemp);
            Graphics gi = Graphics.FromImage(image);
            gi.Clear(Color.Green);//透明
            gi2.Clear(Color.Transparent);//透明




            gi.DrawRectangle(new Pen(Color.Green), new Rectangle(0,0,widthTemp, heightTemp));
            shopSLTYWatermark = image;


            //事件
            shopSLTYPictureBox = new PictureBox();
            shopSLTYPictureBox.BackColor = Color.Transparent;
            shopSLTYPictureBox.Image = shopSLTYWatermark;
            shopSLTYPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            shopSLTYPictureBox.Height = shopSLTYWatermark.Height;
            shopSLTYPictureBox.Width = shopSLTYWatermark.Width;
            shopSLTYPictureBox.Location = new Point(xPointTemp, yPointTemp);
            //给水印图片添加鼠标按下事件
            //控件首次移动
            bool isDrag = false;
            Point prePoint1 = new Point();//优惠券截图中的水印图片位置（移动前）
            bool isFirstMove = true;
            shopSLTYPictureBox.MouseDown += new MouseEventHandler(couponImg_watermarker_MouseDown);
            shopSLTYPictureBox.MouseUp += new MouseEventHandler(couponImg_watermarker_MouseUp);
            shopSLTYPictureBox.MouseMove += new MouseEventHandler(couponImg_watermarker_MouseMove);
            //this.textBox8.Text = waterPictureBox.Height.ToString();
            //this.textBox9.Text = waterPictureBox.Width.ToString();


            this.pictureBox1.Controls.Add(shopSLTYPictureBox);


            gi2.Dispose();
            gi.Dispose();




            //水印控件按下事件
            void couponImg_watermarker_MouseDown(object downObj, MouseEventArgs mouseDownEvent)
            {
                //Console.WriteLine(mouseDownEvent.X + "," + mouseDownEvent.Y);

                if (mouseDownEvent.Button == MouseButtons.Left)
                {
                    isDrag = true;

                    prePoint1 = new Point(mouseDownEvent.X, mouseDownEvent.Y);
                }
            }
            //水印控件鼠标移动事件
            void couponImg_watermarker_MouseMove(object mouseMoveSender, MouseEventArgs mouseMoveEvent)
            {
                if (isDrag)
                {
                    if (isFirstMove)
                    {
                        shopSLTYPictureBox.Location = new Point(shopSLTYPictureBox.Location.X + 1, shopSLTYPictureBox.Location.Y + 1);
                        isFirstMove = false;
                    }
                    else
                    {


                        //Console.WriteLine(mouseMoveEvent.X + "," + mouseMoveEvent.Y);
                        Point mousePos = new Point(shopSLTYPictureBox.Location.X, shopSLTYPictureBox.Location.Y);
                        mousePos.Offset(mouseMoveEvent.X, mouseMoveEvent.Y);
                        mousePos.X -= this.shopSLTYPictureBox.Width / 2;
                        mousePos.Y -= this.shopSLTYPictureBox.Height / 2;

                        //int offsetX = (mouseMoveEvent.X - prePoint1.X);
                        //int offsetY = mouseMoveEvent.Y - prePoint1.Y;
                        //waterPictureBox.Location = new Point(waterPictureBox.Location.X + offsetX, waterPictureBox.Location.Y + offsetY);

                        shopSLTYPictureBox.Location = mousePos;
                        this.numericUpDown5.Value = mousePos.X;
                        this.numericUpDown4.Value = mousePos.Y;
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
            MessageBox.Show("托动用以调整位置");
        }
        private PictureBox couponValuePictureBox;
        private Image couponValueWatermark;
        //优惠券价格
        private void button6_Click(object sender, EventArgs e)
        {
            if (!checkBox4.Checked)
            {
                MessageBox.Show("勾选模块后才能在模板中调试");
                return;
            }
            if (UploadBackImg == null)
            {
                MessageBox.Show("请先上传背景图片");
                return;
            }
            if (couponValuePictureBox != null)
            {
                this.pictureBox1.Controls.Remove(couponValuePictureBox);

            }
            int fontSizeTemp = Convert.ToInt16(numericUpDown16.Value);
            String fontColorTemp = textBox4.Text;
            String fontType = comboBox2.SelectedText;
            int xPointTemp = Convert.ToInt16(numericUpDown15.Value);
            int yPointTemp = Convert.ToInt16(numericUpDown14.Value);
            int widthTemp = Convert.ToInt16(numericUpDown13.Value);
            int heightTemp = Convert.ToInt16(numericUpDown12.Value);
            String contentTemp = textBox3.Text;
            if (String.IsNullOrWhiteSpace(fontColorTemp))
            {

                MessageBox.Show("请指定字体颜色码");
                return;
            }
            if (String.IsNullOrWhiteSpace(fontColorTemp))
            {

                MessageBox.Show("请指定字体");
                return;
            }
            if (String.IsNullOrWhiteSpace(contentTemp))
            {
                MessageBox.Show("请输入测试内容，用于显示");
                return;
            }

            Font watermarkerFont = null;
            FontStyle result = FontStyle.Regular;
            List<FontStyle> style = new List<FontStyle>();
            /*
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
            */
            watermarkerFont = new Font(fontType, fontSizeTemp, result);
            Color fontColor;
            try
            {
                fontColor = ColorTool.getColorFromHtml(fontColorTemp);
            }
            catch (Exception ex)
            {
                MessageBox.Show("不能识别的颜色代码！" + fontColorTemp);
                return;
            }
            SolidBrush watermarkerBrush = new SolidBrush(fontColor);
            //SolidBrush watermarkerBrush = new SolidBrush(Color.);
            //SizeF watermarkerText = Graphics.FromImage(pictureBox1.Image).MeasureString(contentTemp, watermarkerFont);

            //防止随着图片大小改变而缩放尺寸
            Bitmap image2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics gi2 = Graphics.FromImage(image2);
            SizeF watermarkerText = gi2.MeasureString(contentTemp, watermarkerFont);
            //--------------end--------------

            int watermarkerImgWidth = widthTemp;
            int watermarkerImgHeight = heightTemp;
            Bitmap image = new Bitmap(watermarkerImgWidth, watermarkerImgHeight);
            Graphics gi = Graphics.FromImage(image);
            gi.Clear(Color.Green);//透明
            gi2.Clear(Color.Transparent);//透明




            gi.DrawString(contentTemp, watermarkerFont, watermarkerBrush, new Rectangle(0, 0, watermarkerImgWidth, watermarkerImgHeight), sf);
            couponValueWatermark = image;


            //事件
            couponValuePictureBox = new PictureBox();
            couponValuePictureBox.BackColor = Color.Transparent;
            couponValuePictureBox.Image = couponValueWatermark;
            couponValuePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            couponValuePictureBox.Height = couponValueWatermark.Height;
            couponValuePictureBox.Width = couponValueWatermark.Width;
            couponValuePictureBox.Location = new Point(xPointTemp, yPointTemp);
            //给水印图片添加鼠标按下事件
            //控件首次移动
            bool isDrag = false;
            Point prePoint1 = new Point();//优惠券截图中的水印图片位置（移动前）
            bool isFirstMove = true;
            couponValuePictureBox.MouseDown += new MouseEventHandler(couponImg_watermarker_MouseDown);
            couponValuePictureBox.MouseUp += new MouseEventHandler(couponImg_watermarker_MouseUp);
            couponValuePictureBox.MouseMove += new MouseEventHandler(couponImg_watermarker_MouseMove);
            //this.textBox8.Text = waterPictureBox.Height.ToString();
            //this.textBox9.Text = waterPictureBox.Width.ToString();


            this.pictureBox1.Controls.Add(couponValuePictureBox);


            gi2.Dispose();
            gi.Dispose();




            //水印控件按下事件
            void couponImg_watermarker_MouseDown(object downObj, MouseEventArgs mouseDownEvent)
            {
                //Console.WriteLine(mouseDownEvent.X + "," + mouseDownEvent.Y);

                if (mouseDownEvent.Button == MouseButtons.Left)
                {
                    isDrag = true;

                    prePoint1 = new Point(mouseDownEvent.X, mouseDownEvent.Y);
                }
            }
            //水印控件鼠标移动事件
            void couponImg_watermarker_MouseMove(object mouseMoveSender, MouseEventArgs mouseMoveEvent)
            {
                if (isDrag)
                {
                    if (isFirstMove)
                    {
                        couponValuePictureBox.Location = new Point(couponValuePictureBox.Location.X + 1, couponValuePictureBox.Location.Y + 1);
                        isFirstMove = false;
                    }
                    else
                    {


                        //Console.WriteLine(mouseMoveEvent.X + "," + mouseMoveEvent.Y);
                        Point mousePos = new Point(couponValuePictureBox.Location.X, couponValuePictureBox.Location.Y);
                        mousePos.Offset(mouseMoveEvent.X, mouseMoveEvent.Y);
                        mousePos.X -= this.couponValuePictureBox.Width / 2;
                        mousePos.Y -= this.couponValuePictureBox.Height / 2;

                        //int offsetX = (mouseMoveEvent.X - prePoint1.X);
                        //int offsetY = mouseMoveEvent.Y - prePoint1.Y;
                        //waterPictureBox.Location = new Point(waterPictureBox.Location.X + offsetX, waterPictureBox.Location.Y + offsetY);

                        couponValuePictureBox.Location = mousePos;
                        this.numericUpDown15.Value = mousePos.X;
                        this.numericUpDown14.Value = mousePos.Y;
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
            MessageBox.Show("托动用以调整位置");
        }
        private PictureBox couponTimePictureBox;
        private Image couponTimeWatermark;
        //优惠券时间
        private void button7_Click(object sender, EventArgs e)
        {
            if (!checkBox5.Checked)
            {
                MessageBox.Show("勾选模块后才能在模板中调试");
                return;
            }
            if (UploadBackImg == null)
            {
                MessageBox.Show("请先上传背景图片");
                return;
            }
            if (couponTimePictureBox != null)
            {
                this.pictureBox1.Controls.Remove(couponTimePictureBox);

            }
            int fontSizeTemp = Convert.ToInt16(numericUpDown21.Value);
            String fontColorTemp = textBox6.Text;
            String fontType = comboBox3.SelectedText;
            int xPointTemp = Convert.ToInt16(numericUpDown20.Value);
            int yPointTemp = Convert.ToInt16(numericUpDown19.Value);
            int widthTemp = Convert.ToInt16(numericUpDown18.Value);
            int heightTemp = Convert.ToInt16(numericUpDown17.Value);
            String contentTemp = textBox5.Text;
            if (String.IsNullOrWhiteSpace(fontColorTemp))
            {

                MessageBox.Show("请指定字体颜色码");
                return;
            }
            if (String.IsNullOrWhiteSpace(fontColorTemp))
            {

                MessageBox.Show("请指定字体");
                return;
            }
            if (String.IsNullOrWhiteSpace(contentTemp))
            {
                MessageBox.Show("请输入测试内容，用于显示");
                return;
            }

            Font watermarkerFont = null;
            FontStyle result = FontStyle.Regular;
            List<FontStyle> style = new List<FontStyle>();
            /*
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
            */
            watermarkerFont = new Font(fontType, fontSizeTemp, result);
            Color fontColor;
            try
            {
                fontColor = ColorTool.getColorFromHtml(fontColorTemp);
            }
            catch (Exception ex)
            {
                MessageBox.Show("不能识别的颜色代码！" + fontColorTemp);
                return;
            }
            SolidBrush watermarkerBrush = new SolidBrush(fontColor);
            //SolidBrush watermarkerBrush = new SolidBrush(Color.);
            //SizeF watermarkerText = Graphics.FromImage(pictureBox1.Image).MeasureString(contentTemp, watermarkerFont);

            //防止随着图片大小改变而缩放尺寸
            Bitmap image2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics gi2 = Graphics.FromImage(image2);
            SizeF watermarkerText = gi2.MeasureString(contentTemp, watermarkerFont);
            //--------------end--------------

            int watermarkerImgWidth = widthTemp;
            int watermarkerImgHeight = heightTemp;
            Bitmap image = new Bitmap(watermarkerImgWidth, watermarkerImgHeight);
            Graphics gi = Graphics.FromImage(image);
            gi.Clear(Color.Green);//透明
            gi2.Clear(Color.Transparent);//透明




            gi.DrawString(contentTemp, watermarkerFont, watermarkerBrush, new Rectangle(0, 0, watermarkerImgWidth, watermarkerImgHeight), sf);
            couponTimeWatermark = image;


            //事件
            couponTimePictureBox = new PictureBox();
            couponTimePictureBox.BackColor = Color.Transparent;
            couponTimePictureBox.Image = couponTimeWatermark;
            couponTimePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            couponTimePictureBox.Height = couponTimeWatermark.Height;
            couponTimePictureBox.Width = couponTimeWatermark.Width;
            couponTimePictureBox.Location = new Point(xPointTemp, yPointTemp);
            //给水印图片添加鼠标按下事件
            //控件首次移动
            bool isDrag = false;
            Point prePoint1 = new Point();//优惠券截图中的水印图片位置（移动前）
            bool isFirstMove = true;
            couponTimePictureBox.MouseDown += new MouseEventHandler(couponImg_watermarker_MouseDown);
            couponTimePictureBox.MouseUp += new MouseEventHandler(couponImg_watermarker_MouseUp);
            couponTimePictureBox.MouseMove += new MouseEventHandler(couponImg_watermarker_MouseMove);
            //this.textBox8.Text = waterPictureBox.Height.ToString();
            //this.textBox9.Text = waterPictureBox.Width.ToString();


            this.pictureBox1.Controls.Add(couponTimePictureBox);


            gi2.Dispose();
            gi.Dispose();




            //水印控件按下事件
            void couponImg_watermarker_MouseDown(object downObj, MouseEventArgs mouseDownEvent)
            {
                //Console.WriteLine(mouseDownEvent.X + "," + mouseDownEvent.Y);

                if (mouseDownEvent.Button == MouseButtons.Left)
                {
                    isDrag = true;

                    prePoint1 = new Point(mouseDownEvent.X, mouseDownEvent.Y);
                }
            }
            //水印控件鼠标移动事件
            void couponImg_watermarker_MouseMove(object mouseMoveSender, MouseEventArgs mouseMoveEvent)
            {
                if (isDrag)
                {
                    if (isFirstMove)
                    {
                        couponTimePictureBox.Location = new Point(couponTimePictureBox.Location.X + 1, couponTimePictureBox.Location.Y + 1);
                        isFirstMove = false;
                    }
                    else
                    {


                        //Console.WriteLine(mouseMoveEvent.X + "," + mouseMoveEvent.Y);
                        Point mousePos = new Point(couponTimePictureBox.Location.X, couponTimePictureBox.Location.Y);
                        mousePos.Offset(mouseMoveEvent.X, mouseMoveEvent.Y);
                        mousePos.X -= this.couponTimePictureBox.Width / 2;
                        mousePos.Y -= this.couponTimePictureBox.Height / 2;

                        //int offsetX = (mouseMoveEvent.X - prePoint1.X);
                        //int offsetY = mouseMoveEvent.Y - prePoint1.Y;
                        //waterPictureBox.Location = new Point(waterPictureBox.Location.X + offsetX, waterPictureBox.Location.Y + offsetY);

                        couponTimePictureBox.Location = mousePos;
                        this.numericUpDown20.Value = mousePos.X;
                        this.numericUpDown19.Value = mousePos.Y;
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
            MessageBox.Show("托动用以调整位置");
        }
        private PictureBox goodsNamePictureBox;
        private Image goodsNameWatermark;
        //商品名称
        private void button8_Click(object sender, EventArgs e)
        {
            if (!checkBox6.Checked)
            {
                MessageBox.Show("勾选模块后才能在模板中调试");
                return;
            }
            if (UploadBackImg == null)
            {
                MessageBox.Show("请先上传背景图片");
                return;
            }
            if (goodsNamePictureBox != null)
            {
                this.pictureBox1.Controls.Remove(goodsNamePictureBox);

            }
            int fontSizeTemp = Convert.ToInt16(numericUpDown26.Value);
            String fontColorTemp = textBox8.Text;
            String fontType = comboBox4.SelectedText;
            int xPointTemp = Convert.ToInt16(numericUpDown25.Value);
            int yPointTemp = Convert.ToInt16(numericUpDown24.Value);
            int widthTemp = Convert.ToInt16(numericUpDown23.Value);
            int heightTemp = Convert.ToInt16(numericUpDown22.Value);
            String contentTemp = textBox7.Text;
            if (String.IsNullOrWhiteSpace(fontColorTemp))
            {

                MessageBox.Show("请指定字体颜色码");
                return;
            }
            if (String.IsNullOrWhiteSpace(fontColorTemp))
            {

                MessageBox.Show("请指定字体");
                return;
            }
            if (String.IsNullOrWhiteSpace(contentTemp))
            {
                MessageBox.Show("请输入测试内容，用于显示");
                return;
            }

            Font watermarkerFont = null;
            FontStyle result = FontStyle.Regular;
            List<FontStyle> style = new List<FontStyle>();
            /*
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
            */
            watermarkerFont = new Font(fontType, fontSizeTemp, result);
            Color fontColor;
            try
            {
                fontColor = ColorTool.getColorFromHtml(fontColorTemp);
            }
            catch (Exception ex)
            {
                MessageBox.Show("不能识别的颜色代码！" + fontColorTemp);
                return;
            }
            SolidBrush watermarkerBrush = new SolidBrush(fontColor);
            //SolidBrush watermarkerBrush = new SolidBrush(Color.);
            //SizeF watermarkerText = Graphics.FromImage(pictureBox1.Image).MeasureString(contentTemp, watermarkerFont);

            //防止随着图片大小改变而缩放尺寸
            Bitmap image2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics gi2 = Graphics.FromImage(image2);
            SizeF watermarkerText = gi2.MeasureString(contentTemp, watermarkerFont);
            //--------------end--------------

            int watermarkerImgWidth = widthTemp;
            int watermarkerImgHeight = heightTemp;
            Bitmap image = new Bitmap(watermarkerImgWidth, watermarkerImgHeight);
            Graphics gi = Graphics.FromImage(image);
            gi.Clear(Color.Green);//透明
            gi2.Clear(Color.Transparent);//透明




            gi.DrawString(contentTemp, watermarkerFont, watermarkerBrush, new Rectangle(0, 0, watermarkerImgWidth, watermarkerImgHeight), sf1);
            goodsNameWatermark = image;


            //事件
            goodsNamePictureBox = new PictureBox();
            goodsNamePictureBox.BackColor = Color.Transparent;
            goodsNamePictureBox.Image = goodsNameWatermark;
            goodsNamePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            goodsNamePictureBox.Height = goodsNameWatermark.Height;
            goodsNamePictureBox.Width = goodsNameWatermark.Width;
            goodsNamePictureBox.Location = new Point(xPointTemp, yPointTemp);
            //给水印图片添加鼠标按下事件
            //控件首次移动
            bool isDrag = false;
            Point prePoint1 = new Point();//优惠券截图中的水印图片位置（移动前）
            bool isFirstMove = true;
            goodsNamePictureBox.MouseDown += new MouseEventHandler(couponImg_watermarker_MouseDown);
            goodsNamePictureBox.MouseUp += new MouseEventHandler(couponImg_watermarker_MouseUp);
            goodsNamePictureBox.MouseMove += new MouseEventHandler(couponImg_watermarker_MouseMove);
            //this.textBox8.Text = waterPictureBox.Height.ToString();
            //this.textBox9.Text = waterPictureBox.Width.ToString();


            this.pictureBox1.Controls.Add(goodsNamePictureBox);


            gi2.Dispose();
            gi.Dispose();




            //水印控件按下事件
            void couponImg_watermarker_MouseDown(object downObj, MouseEventArgs mouseDownEvent)
            {
                //Console.WriteLine(mouseDownEvent.X + "," + mouseDownEvent.Y);

                if (mouseDownEvent.Button == MouseButtons.Left)
                {
                    isDrag = true;

                    prePoint1 = new Point(mouseDownEvent.X, mouseDownEvent.Y);
                }
            }
            //水印控件鼠标移动事件
            void couponImg_watermarker_MouseMove(object mouseMoveSender, MouseEventArgs mouseMoveEvent)
            {
                if (isDrag)
                {
                    if (isFirstMove)
                    {
                        goodsNamePictureBox.Location = new Point(goodsNamePictureBox.Location.X + 1, goodsNamePictureBox.Location.Y + 1);
                        isFirstMove = false;
                    }
                    else
                    {


                        //Console.WriteLine(mouseMoveEvent.X + "," + mouseMoveEvent.Y);
                        Point mousePos = new Point(goodsNamePictureBox.Location.X, goodsNamePictureBox.Location.Y);
                        mousePos.Offset(mouseMoveEvent.X, mouseMoveEvent.Y);
                        mousePos.X -= this.goodsNamePictureBox.Width / 2;
                        mousePos.Y -= this.goodsNamePictureBox.Height / 2;

                        //int offsetX = (mouseMoveEvent.X - prePoint1.X);
                        //int offsetY = mouseMoveEvent.Y - prePoint1.Y;
                        //waterPictureBox.Location = new Point(waterPictureBox.Location.X + offsetX, waterPictureBox.Location.Y + offsetY);

                        goodsNamePictureBox.Location = mousePos;
                        this.numericUpDown25.Value = mousePos.X;
                        this.numericUpDown24.Value = mousePos.Y;
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
            MessageBox.Show("托动用以调整位置");
        }
        private PictureBox pricePictureBox;
        private Image priceWatermark;
        //商品价格
        private void button9_Click(object sender, EventArgs e)
        {
            if (!checkBox7.Checked)
            {
                MessageBox.Show("勾选模块后才能在模板中调试");
                return;
            }
            if (UploadBackImg == null)
            {
                MessageBox.Show("请先上传背景图片");
                return;
            }
            if (pricePictureBox != null)
            {
                this.pictureBox1.Controls.Remove(pricePictureBox);

            }
            int fontSizeTemp = Convert.ToInt16(numericUpDown31.Value);
            String fontColorTemp = textBox10.Text;
            String fontType = comboBox5.SelectedText;
            int xPointTemp = Convert.ToInt16(numericUpDown30.Value);
            int yPointTemp = Convert.ToInt16(numericUpDown29.Value);
            int widthTemp = Convert.ToInt16(numericUpDown28.Value);
            int heightTemp = Convert.ToInt16(numericUpDown27.Value);
            String contentTemp = textBox9.Text;
            if (String.IsNullOrWhiteSpace(fontColorTemp))
            {

                MessageBox.Show("请指定字体颜色码");
                return;
            }
            if (String.IsNullOrWhiteSpace(fontColorTemp))
            {

                MessageBox.Show("请指定字体");
                return;
            }
            if (String.IsNullOrWhiteSpace(contentTemp))
            {
                MessageBox.Show("请输入测试内容，用于显示");
                return;
            }

            Font watermarkerFont = null;
            FontStyle result = FontStyle.Regular;
            List<FontStyle> style = new List<FontStyle>();
            
            if (checkBox10.Checked)
            {
                result = result | FontStyle.Bold;
            }
            if (checkBox1.Checked)
            {
                result = result | FontStyle.Strikeout;
            }
            if (checkBox11.Checked)
            {
                result = result | FontStyle.Italic;
            }
            
            watermarkerFont = new Font(fontType, fontSizeTemp, result);
            Color fontColor;
            try
            {
                fontColor = ColorTool.getColorFromHtml(fontColorTemp);
            }
            catch (Exception ex)
            {
                MessageBox.Show("不能识别的颜色代码！" + fontColorTemp);
                return;
            }
            SolidBrush watermarkerBrush = new SolidBrush(fontColor);
            //SolidBrush watermarkerBrush = new SolidBrush(Color.);
            //SizeF watermarkerText = Graphics.FromImage(pictureBox1.Image).MeasureString(contentTemp, watermarkerFont);

            //防止随着图片大小改变而缩放尺寸
            Bitmap image2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics gi2 = Graphics.FromImage(image2);
            SizeF watermarkerText = gi2.MeasureString(contentTemp, watermarkerFont);
            //--------------end--------------

            int watermarkerImgWidth = widthTemp;
            int watermarkerImgHeight = heightTemp;
            Bitmap image = new Bitmap(watermarkerImgWidth, watermarkerImgHeight);
            Graphics gi = Graphics.FromImage(image);
            gi.Clear(Color.Green);//透明
            gi2.Clear(Color.Transparent);//透明




            gi.DrawString(contentTemp, watermarkerFont, watermarkerBrush, new Rectangle(0, 0, watermarkerImgWidth, watermarkerImgHeight), sf1);
            priceWatermark = image;


            //事件
            pricePictureBox = new PictureBox();
            pricePictureBox.BackColor = Color.Transparent;
            pricePictureBox.Image = priceWatermark;
            pricePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pricePictureBox.Height = priceWatermark.Height;
            pricePictureBox.Width = priceWatermark.Width;
            pricePictureBox.Location = new Point(xPointTemp, yPointTemp);
            //给水印图片添加鼠标按下事件
            //控件首次移动
            bool isDrag = false;
            Point prePoint1 = new Point();//优惠券截图中的水印图片位置（移动前）
            bool isFirstMove = true;
            pricePictureBox.MouseDown += new MouseEventHandler(couponImg_watermarker_MouseDown);
            pricePictureBox.MouseUp += new MouseEventHandler(couponImg_watermarker_MouseUp);
            pricePictureBox.MouseMove += new MouseEventHandler(couponImg_watermarker_MouseMove);
            //this.textBox8.Text = waterPictureBox.Height.ToString();
            //this.textBox9.Text = waterPictureBox.Width.ToString();


            this.pictureBox1.Controls.Add(pricePictureBox);


            gi2.Dispose();
            gi.Dispose();




            //水印控件按下事件
            void couponImg_watermarker_MouseDown(object downObj, MouseEventArgs mouseDownEvent)
            {
                //Console.WriteLine(mouseDownEvent.X + "," + mouseDownEvent.Y);

                if (mouseDownEvent.Button == MouseButtons.Left)
                {
                    isDrag = true;

                    prePoint1 = new Point(mouseDownEvent.X, mouseDownEvent.Y);
                }
            }
            //水印控件鼠标移动事件
            void couponImg_watermarker_MouseMove(object mouseMoveSender, MouseEventArgs mouseMoveEvent)
            {
                if (isDrag)
                {
                    if (isFirstMove)
                    {
                        pricePictureBox.Location = new Point(pricePictureBox.Location.X + 1, pricePictureBox.Location.Y + 1);
                        isFirstMove = false;
                    }
                    else
                    {


                        //Console.WriteLine(mouseMoveEvent.X + "," + mouseMoveEvent.Y);
                        Point mousePos = new Point(pricePictureBox.Location.X, pricePictureBox.Location.Y);
                        mousePos.Offset(mouseMoveEvent.X, mouseMoveEvent.Y);
                        mousePos.X -= this.pricePictureBox.Width / 2;
                        mousePos.Y -= this.pricePictureBox.Height / 2;

                        //int offsetX = (mouseMoveEvent.X - prePoint1.X);
                        //int offsetY = mouseMoveEvent.Y - prePoint1.Y;
                        //waterPictureBox.Location = new Point(waterPictureBox.Location.X + offsetX, waterPictureBox.Location.Y + offsetY);

                        pricePictureBox.Location = mousePos;
                        this.numericUpDown30.Value = mousePos.X;
                        this.numericUpDown29.Value = mousePos.Y;
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
            MessageBox.Show("托动用以调整位置");
        }
        private PictureBox volumePictureBox;
        private Image volumeWatermark;
        //商品销量
        private void button10_Click(object sender, EventArgs e)
        {
            if (!checkBox8.Checked)
            {
                MessageBox.Show("勾选模块后才能在模板中调试");
                return;
            }
            if (UploadBackImg == null)
            {
                MessageBox.Show("请先上传背景图片");
                return;
            }
            if (volumePictureBox != null)
            {
                this.pictureBox1.Controls.Remove(volumePictureBox);

            }
            int fontSizeTemp = Convert.ToInt16(numericUpDown36.Value);
            String fontColorTemp = textBox13.Text;
            String fontType = comboBox6.SelectedText;
            int xPointTemp = Convert.ToInt16(numericUpDown35.Value);
            int yPointTemp = Convert.ToInt16(numericUpDown34.Value);
            int widthTemp = Convert.ToInt16(numericUpDown33.Value);
            int heightTemp = Convert.ToInt16(numericUpDown32.Value);
            String contentTemp = textBox11.Text;
            if (String.IsNullOrWhiteSpace(fontColorTemp))
            {

                MessageBox.Show("请指定字体颜色码");
                return;
            }
            if (String.IsNullOrWhiteSpace(fontColorTemp))
            {

                MessageBox.Show("请指定字体");
                return;
            }
            if (String.IsNullOrWhiteSpace(contentTemp))
            {
                MessageBox.Show("请输入测试内容，用于显示");
                return;
            }

            Font watermarkerFont = null;
            FontStyle result = FontStyle.Regular;
            List<FontStyle> style = new List<FontStyle>();
            /*
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
            */
            watermarkerFont = new Font(fontType, fontSizeTemp, result);
            Color fontColor;
            try
            {
                fontColor = ColorTool.getColorFromHtml(fontColorTemp);
            }
            catch (Exception ex)
            {
                MessageBox.Show("不能识别的颜色代码！" + fontColorTemp);
                return;
            }
            SolidBrush watermarkerBrush = new SolidBrush(fontColor);
            //SolidBrush watermarkerBrush = new SolidBrush(Color.);
            //SizeF watermarkerText = Graphics.FromImage(pictureBox1.Image).MeasureString(contentTemp, watermarkerFont);

            //防止随着图片大小改变而缩放尺寸
            Bitmap image2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics gi2 = Graphics.FromImage(image2);
            SizeF watermarkerText = gi2.MeasureString(contentTemp, watermarkerFont);
            //--------------end--------------

            int watermarkerImgWidth = widthTemp;
            int watermarkerImgHeight = heightTemp;
            Bitmap image = new Bitmap(watermarkerImgWidth, watermarkerImgHeight);
            Graphics gi = Graphics.FromImage(image);
            gi.Clear(Color.Green);//透明
            gi2.Clear(Color.Transparent);//透明




            gi.DrawString(contentTemp, watermarkerFont, watermarkerBrush, new Rectangle(0, 0, watermarkerImgWidth, watermarkerImgHeight), sf1);
            volumeWatermark = image;


            //事件
            volumePictureBox = new PictureBox();
            volumePictureBox.BackColor = Color.Transparent;
            volumePictureBox.Image = volumeWatermark;
            volumePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            volumePictureBox.Height = volumeWatermark.Height;
            volumePictureBox.Width = volumeWatermark.Width;
            volumePictureBox.Location = new Point(xPointTemp, yPointTemp);
            //给水印图片添加鼠标按下事件
            //控件首次移动
            bool isDrag = false;
            Point prePoint1 = new Point();//优惠券截图中的水印图片位置（移动前）
            bool isFirstMove = true;
            volumePictureBox.MouseDown += new MouseEventHandler(couponImg_watermarker_MouseDown);
            volumePictureBox.MouseUp += new MouseEventHandler(couponImg_watermarker_MouseUp);
            volumePictureBox.MouseMove += new MouseEventHandler(couponImg_watermarker_MouseMove);
            //this.textBox8.Text = waterPictureBox.Height.ToString();
            //this.textBox9.Text = waterPictureBox.Width.ToString();


            this.pictureBox1.Controls.Add(volumePictureBox);


            gi2.Dispose();
            gi.Dispose();




            //水印控件按下事件
            void couponImg_watermarker_MouseDown(object downObj, MouseEventArgs mouseDownEvent)
            {
                //Console.WriteLine(mouseDownEvent.X + "," + mouseDownEvent.Y);

                if (mouseDownEvent.Button == MouseButtons.Left)
                {
                    isDrag = true;

                    prePoint1 = new Point(mouseDownEvent.X, mouseDownEvent.Y);
                }
            }
            //水印控件鼠标移动事件
            void couponImg_watermarker_MouseMove(object mouseMoveSender, MouseEventArgs mouseMoveEvent)
            {
                if (isDrag)
                {
                    if (isFirstMove)
                    {
                        volumePictureBox.Location = new Point(volumePictureBox.Location.X + 1, volumePictureBox.Location.Y + 1);
                        isFirstMove = false;
                    }
                    else
                    {


                        //Console.WriteLine(mouseMoveEvent.X + "," + mouseMoveEvent.Y);
                        Point mousePos = new Point(volumePictureBox.Location.X, volumePictureBox.Location.Y);
                        mousePos.Offset(mouseMoveEvent.X, mouseMoveEvent.Y);
                        mousePos.X -= this.volumePictureBox.Width / 2;
                        mousePos.Y -= this.volumePictureBox.Height / 2;

                        //int offsetX = (mouseMoveEvent.X - prePoint1.X);
                        //int offsetY = mouseMoveEvent.Y - prePoint1.Y;
                        //waterPictureBox.Location = new Point(waterPictureBox.Location.X + offsetX, waterPictureBox.Location.Y + offsetY);

                        volumePictureBox.Location = mousePos;
                        this.numericUpDown35.Value = mousePos.X;
                        this.numericUpDown34.Value = mousePos.Y;
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
            MessageBox.Show("托动用以调整位置");
        }
        private PictureBox priceAfterPictureBox;
        private Image priceAfterWatermark;
        //券后价格
        private void button11_Click(object sender, EventArgs e)
        {
            if (!checkBox9.Checked)
            {
                MessageBox.Show("勾选模块后才能在模板中调试");
                return;
            }
            if (UploadBackImg == null)
            {
                MessageBox.Show("请先上传背景图片");
                return;
            }
            if (priceAfterPictureBox != null)
            {
                this.pictureBox1.Controls.Remove(priceAfterPictureBox);

            }
            int fontSizeTemp = Convert.ToInt16(numericUpDown41.Value);
            String fontColorTemp = textBox15.Text;
            String fontType = comboBox7.SelectedText;
            int xPointTemp = Convert.ToInt16(numericUpDown40.Value);
            int yPointTemp = Convert.ToInt16(numericUpDown39.Value);
            int widthTemp = Convert.ToInt16(numericUpDown38.Value);
            int heightTemp = Convert.ToInt16(numericUpDown37.Value);
            String contentTemp = textBox14.Text;
            if (String.IsNullOrWhiteSpace(fontColorTemp))
            {

                MessageBox.Show("请指定字体颜色码");
                return;
            }
            if (String.IsNullOrWhiteSpace(fontColorTemp))
            {

                MessageBox.Show("请指定字体");
                return;
            }
            if (String.IsNullOrWhiteSpace(contentTemp))
            {
                MessageBox.Show("请输入测试内容，用于显示");
                return;
            }

            Font watermarkerFont = null;
            FontStyle result = FontStyle.Regular;
            List<FontStyle> style = new List<FontStyle>();
            /*
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
            */
            watermarkerFont = new Font(fontType, fontSizeTemp, result);
            Color fontColor;
            try
            {
                fontColor = ColorTool.getColorFromHtml(fontColorTemp);
            }
            catch (Exception ex)
            {
                MessageBox.Show("不能识别的颜色代码！" + fontColorTemp);
                return;
            }
            SolidBrush watermarkerBrush = new SolidBrush(fontColor);
            //SolidBrush watermarkerBrush = new SolidBrush(Color.);
            //SizeF watermarkerText = Graphics.FromImage(pictureBox1.Image).MeasureString(contentTemp, watermarkerFont);

            //防止随着图片大小改变而缩放尺寸
            Bitmap image2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics gi2 = Graphics.FromImage(image2);
            SizeF watermarkerText = gi2.MeasureString(contentTemp, watermarkerFont);
            //--------------end--------------

            int watermarkerImgWidth = widthTemp;
            int watermarkerImgHeight = heightTemp;
            Bitmap image = new Bitmap(watermarkerImgWidth, watermarkerImgHeight);
            Graphics gi = Graphics.FromImage(image);
            gi.Clear(Color.Green);//透明
            gi2.Clear(Color.Transparent);//透明




            gi.DrawString(contentTemp, watermarkerFont, watermarkerBrush, new Rectangle(0, 0, watermarkerImgWidth, watermarkerImgHeight), sf);
            priceAfterWatermark = image;


            //事件
            priceAfterPictureBox = new PictureBox();
            priceAfterPictureBox.BackColor = Color.Transparent;
            priceAfterPictureBox.Image = priceAfterWatermark;
            priceAfterPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            priceAfterPictureBox.Height = priceAfterWatermark.Height;
            priceAfterPictureBox.Width = priceAfterWatermark.Width;
            priceAfterPictureBox.Location = new Point(xPointTemp, yPointTemp);
            //给水印图片添加鼠标按下事件
            //控件首次移动
            bool isDrag = false;
            Point prePoint1 = new Point();//优惠券截图中的水印图片位置（移动前）
            bool isFirstMove = true;
            priceAfterPictureBox.MouseDown += new MouseEventHandler(couponImg_watermarker_MouseDown);
            priceAfterPictureBox.MouseUp += new MouseEventHandler(couponImg_watermarker_MouseUp);
            priceAfterPictureBox.MouseMove += new MouseEventHandler(couponImg_watermarker_MouseMove);
            //this.textBox8.Text = waterPictureBox.Height.ToString();
            //this.textBox9.Text = waterPictureBox.Width.ToString();


            this.pictureBox1.Controls.Add(priceAfterPictureBox);


            gi2.Dispose();
            gi.Dispose();




            //水印控件按下事件
            void couponImg_watermarker_MouseDown(object downObj, MouseEventArgs mouseDownEvent)
            {
                //Console.WriteLine(mouseDownEvent.X + "," + mouseDownEvent.Y);

                if (mouseDownEvent.Button == MouseButtons.Left)
                {
                    isDrag = true;

                    prePoint1 = new Point(mouseDownEvent.X, mouseDownEvent.Y);
                }
            }
            //水印控件鼠标移动事件
            void couponImg_watermarker_MouseMove(object mouseMoveSender, MouseEventArgs mouseMoveEvent)
            {
                if (isDrag)
                {
                    if (isFirstMove)
                    {
                        priceAfterPictureBox.Location = new Point(priceAfterPictureBox.Location.X + 1, priceAfterPictureBox.Location.Y + 1);
                        isFirstMove = false;
                    }
                    else
                    {


                        //Console.WriteLine(mouseMoveEvent.X + "," + mouseMoveEvent.Y);
                        Point mousePos = new Point(priceAfterPictureBox.Location.X, priceAfterPictureBox.Location.Y);
                        mousePos.Offset(mouseMoveEvent.X, mouseMoveEvent.Y);
                        mousePos.X -= this.priceAfterPictureBox.Width / 2;
                        mousePos.Y -= this.priceAfterPictureBox.Height / 2;

                        //int offsetX = (mouseMoveEvent.X - prePoint1.X);
                        //int offsetY = mouseMoveEvent.Y - prePoint1.Y;
                        //waterPictureBox.Location = new Point(waterPictureBox.Location.X + offsetX, waterPictureBox.Location.Y + offsetY);

                        priceAfterPictureBox.Location = mousePos;
                        this.numericUpDown40.Value = mousePos.X;
                        this.numericUpDown39.Value = mousePos.Y;
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
            MessageBox.Show("托动用以调整位置");
        }


    }
}
