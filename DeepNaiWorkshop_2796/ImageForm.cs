using DeepNaiWorkshop_2796.MyTool;
using MyTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
/*
 * https://www.cnblogs.com/zkwarrior/p/5668137.html
 * https://www.cnblogs.com/lonelyxmas/p/8564650.html
 * https://www.cnblogs.com/gc2013/p/3678212.html
 * https://www.cnblogs.com/xuekai-to-sharp/p/4014998.html
 * https://blog.csdn.net/real_myth/article/details/49821009
 * http://www.makaidong.com/%E5%8D%9A%E5%AE%A2%E5%9B%AD%E6%90%9C/29590.shtml
 * https://www.cnblogs.com/cinlap/p/3688204.html (实现高斯模糊)
 */
namespace DeepNaiWorkshop_2796
{
    public partial class ImageForm : Form
    {
        private Image InitImage;
        private List<Image> ImageHistory = new List<Image>();
        private int CurrentImage;
        private bool IsUseWatermark = false;//是否使用水印图章
        private Bitmap WaterMarkSeal = null;//水印印章图片
        public ImageForm(Image initImage)
        {
            InitializeComponent();
            if (initImage == null)
            {
                initImage = pictureBox1.Image;
            }
            InitImage = initImage;
            ImageHistory.Add(initImage);
            CurrentImage = ImageHistory.Count-1;
            pictureBox1.Image = InitImage;


            if (IsUseWatermark)
            {
                label9.Text = "水印印章已启用";
                label9.ForeColor = Color.Red;
            }
            else
            {
                label9.Text = "水印印章未启用";
                label9.ForeColor = Color.Green;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //回退至上一步
            if (CurrentImage == 0)
            {
                MessageBox.Show("已经是最原始图片");
                return;
            }
            Console.WriteLine("即将移出索引："+ CurrentImage+",元素数："+ImageHistory.Count);
            ImageHistory.RemoveAt(CurrentImage);
            CurrentImage -= 1;
            pictureBox1.Image = (Image)ImageHistory[CurrentImage];

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(ImageHistory[CurrentImage]);
            MessageBox.Show("截图已拷贝到剪贴板！");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = InitImage;
            ImageHistory = new List<Image>();
            ImageHistory.Add(InitImage);
            CurrentImage = ImageHistory.Count - 1;
            pictureBox1.Image = InitImage;
            MessageBox.Show("初始化完成！");
        }

        /// <summary>
        /// 色调取反
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Image tempImage = ImageHistory[CurrentImage];
            // 读入欲转换的图片并转成为 WritableBitmap www.it165.net
            Bitmap bitmap = new Bitmap(tempImage);
            for (int y = 0; y < bitmap.Height; y++)
                {
                for (int x = 0; x < bitmap.Width; x++)
                    {
                    // 取得每一个 pixel
                    var pixel = bitmap.GetPixel(x, y);

                    // 负片效果 将其反转
                    Color newColor = Color.FromArgb(pixel.A, 255 - pixel.R, 255 - pixel.G, 255 - pixel.B);

                    bitmap.SetPixel(x, y, newColor);
                        
                }
                }
            // 显示结果
            SetImage(bitmap);
            AddLog("使用【滤镜-负片】成功");
        }

        private void SetImage(Image image)
        {
            
            CurrentImage = ImageHistory.Count;
            ImageHistory.Add(image);
            Console.WriteLine("设置水印成功，当前图片索引：" + CurrentImage + ",图片数：" + ImageHistory.Count);
            pictureBox1.Image = image;
        }

        private void AddLog(String content)
        {
            if (listBox1.Items.Count > 100)
            {
                listBox1.Items.Clear();
            }
            listBox1.Items.Add(DateTime.Now.ToLongTimeString() + "=>" + content);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        /// <summary>
        /// 增加噪点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            int noiseCount = Convert.ToInt32(numericUpDown1.Value);
            if (noiseCount <= 0)
            {
                MessageBox.Show("噪点数必须大于0");
                return;
            }
            Image tempImage = ImageHistory[CurrentImage];
            // 显示结果
            SetImage(AddNoise(new Bitmap(tempImage), noiseCount));
            AddLog("给图片增加【噪点】成功");
        }
        
        public static Bitmap AddNoise(Bitmap OriginalImage, int Amount)
        {
            
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            //BitmapData NewData = Image.LockImage(NewBitmap);
            //BitmapData OldData = Image.LockImage(OriginalImage);
            //int NewPixelSize = Image.GetPixelSize(NewData);
            //int OldPixelSize = Image.GetPixelSize(OldData);
            Random TempRandom = new Random();
            for (int x = 0; x<NewBitmap.Width; ++x)
            {
                for (int y = 0; y<NewBitmap.Height; ++y)
                {
                    Color CurrentPixel = OriginalImage.GetPixel( x, y);
                    int R = CurrentPixel.R + TempRandom.Next(-Amount, Amount + 1);
                    int G = CurrentPixel.G + TempRandom.Next(-Amount, Amount + 1);
                    int B = CurrentPixel.B + TempRandom.Next(-Amount, Amount + 1);
                    R = R > 255 ? 255 : R;
                    R = R< 0 ? 0 : R;
                    G = G > 255 ? 255 : G;
                    G = G< 0 ? 0 : G;
                    B = B > 255 ? 255 : B;
                    B = B< 0 ? 0 : B;
                    Color TempValue = Color.FromArgb(R, G, B);
                    //Image.SetPixel(NewData, x, y, TempValue, NewPixelSize);
                    NewBitmap.SetPixel(x, y, TempValue);
                }
            }
            //Image.UnlockImage(NewBitmap, NewData);
            //Image.UnlockImage(OriginalImage, OldData);
            return NewBitmap;
        }
        
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Image tempImage = ImageHistory[CurrentImage];
            
            // 显示结果
            SetImage(ImageClass.FD(new Bitmap(tempImage), tempImage.Width, tempImage.Height));
            AddLog("使用【滤镜-浮雕】成功");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Image tempImage = ImageHistory[CurrentImage];

            // 显示结果
            SetImage(ImageClass.FilPic(new Bitmap(tempImage), tempImage.Width, tempImage.Height));
            AddLog("使用【滤镜-滤色】成功");
            
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Image tempImage = ImageHistory[CurrentImage];

            // 显示结果
            SetImage(ImageClass.RevPicLR(new Bitmap(tempImage), tempImage.Width, tempImage.Height));
            AddLog("图片【左右翻转】成功");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Image tempImage = ImageHistory[CurrentImage];

            // 显示结果
            SetImage(ImageClass.RevPicUD(new Bitmap(tempImage), tempImage.Width, tempImage.Height));
            AddLog("图片【上下翻转】成功");
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            Image tempImage = ImageHistory[CurrentImage];
            // 显示结果
            SetImage(ImageClass.BWPic(new Bitmap(tempImage), tempImage.Width, tempImage.Height));
            AddLog("使用【滤镜-滤色】成功");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ImageClass.Gray(Color.Red);
            AddLog("使用【滤镜-滤色】成功");
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
                    int watermarkHeight = Convert.ToUInt16(watermark.Height.ToString());
                    int watermarkWidth = Convert.ToUInt16(watermark.Width.ToString());
                    if(watermarkHeight>558|| watermarkWidth > 375)
                    {
                        MessageBox.Show("超过水印图片的最大尺寸（375x558）,自动缩减大小至100x20");
                        this.numericUpDown4.Value = 20;//水印高
                        this.numericUpDown5.Value = 100;//水印宽
                    }
                    else
                    {
                        this.numericUpDown4.Value = watermarkHeight;//水印高
                        this.numericUpDown5.Value = watermarkWidth;//水印宽
                    }
                    
                    
                }
            }
        }
        /// <summary>
        /// 当启用水印图章时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (IsUseWatermark)
            {

                Image tempImage = ImageHistory[CurrentImage];
                Bitmap newImage = null;
                Image watermarker = null;
                if (this.radioButton1.Checked)//图片水印
                {

                    watermarker = new Bitmap((Image)this.pictureBox2.Image.Clone());
                    watermarker = ImageTool.resetImgSize(watermarker, Convert.ToInt32(numericUpDown5.Value), Convert.ToInt32(numericUpDown4.Value));
                    MouseEventArgs m = (MouseEventArgs)e;
                    // 显示结果
                    SetImage(ImageTool.AddImg(tempImage, m.X + "," + m.Y, watermarker, Convert.ToInt32(numericUpDown3.Value)));
                    AddLog("使用【图片印章】成功");

                }
                else
                {
                    //文字水印
                    int watermarkerFontSize = Convert.ToInt32(numericUpDown2.Value);//水印字体的大小
                    Font watermarkerFont = null;
                    if (checkBox1.Checked && !checkBox2.Checked)//加粗
                    {
                        watermarkerFont = new Font(Const.COUPON_FONT, watermarkerFontSize, FontStyle.Bold | FontStyle.Italic);
                    }
                    else if (!checkBox1.Checked && checkBox2.Checked)//删除线
                    {
                        watermarkerFont = new Font(Const.COUPON_FONT, watermarkerFontSize, FontStyle.Strikeout | FontStyle.Italic);
                    }
                    else if (checkBox1.Checked && checkBox2.Checked)//加粗 删除线
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
                    //Point p = e.Location;
                    MouseEventArgs m = (MouseEventArgs)e;
                    //string X = p.X.ToString();
                    //string Y = p.Y.ToString();
                    // 显示结果
                    SetImage(ImageTool.AddText(tempImage, m.X + "," + m.Y, watermarkerFont, watermarkerBrush, textBox5.Text, Convert.ToInt32(numericUpDown3.Value)));
                    AddLog("使用【文字印章】成功");
                }


            }
            
        }
        //使用或重置印章
        private void button5_Click_1(object sender, EventArgs e)
        {
            IsUseWatermark = true;


            if (IsUseWatermark)
            {
                label9.Text = "水印印章已启用";
                label9.ForeColor = Color.Red;
            }
            else
            {
                label9.Text = "水印印章未启用";
                label9.ForeColor = Color.Green;
            }
            MessageBox.Show("启用水印印章，鼠标单击图片即可添加印章");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            IsUseWatermark = false;
            if (IsUseWatermark)
            {
                label9.Text = "水印印章已启用";
                label9.ForeColor = Color.Red;
            }
            else
            {
                label9.Text = "水印印章未启用";
                label9.ForeColor = Color.Green;
            }
            MessageBox.Show("停止使用水印印章");
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://tool.chinaz.com/tools/use");
        }


    }
}
