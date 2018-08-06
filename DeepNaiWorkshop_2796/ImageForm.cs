using DeepNaiWorkshop_2796.MyTool;
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
        public ImageForm(Image initImage)
        {
            InitializeComponent();
            InitImage = initImage;
            ImageHistory.Add(initImage);
            CurrentImage = ImageHistory.Count-1;
            pictureBox1.Image = InitImage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //回退至上一步
            if (CurrentImage == 0)
            {
                MessageBox.Show("已经是最原始图片");
                return;
            }
            ImageHistory.RemoveAt(CurrentImage);
            CurrentImage -= 1;
            pictureBox1.Image = ImageHistory[CurrentImage];

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
    }
}
