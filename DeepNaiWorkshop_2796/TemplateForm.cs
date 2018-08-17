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
        private Image UploadBackImg;
        private Image UploadOriginalImg;
        public TemplateForm()
        {
            InitializeComponent();
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
    }
}
