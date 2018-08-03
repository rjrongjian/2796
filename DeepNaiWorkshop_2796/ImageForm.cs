using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    }
}
