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
                        this.pictureBox1.Image = dataBean.MainPic;// 商品图片
                        LogTool.log("数据解析完成，数据可手动更改...",this.logLabel);
                        setMainFormBtnStatus(7);//获取商品数据、生成截图按钮可用

                    }
                    else
                    {
                        alert("目前不支持淘宝商品数据爬取，请手动录入");
                    }

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
            //测试在pictureBox添加另一个
            PictureBox p = new PictureBox();
            p.Image = ResourceTool.getImage("test2");
            //Console.WriteLine("显示了吗"+p.Image.ToString());
            p.Show();
            this.pictureBox3.Controls.Add(p);
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
            }
            else if (status == 2)//2 按钮全部不可用
            {
                this.button1.Enabled = false;
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                this.button4.Enabled = false;
            }
            else if (status == 3)//3 获取商品信息按钮可用
            {
                this.button1.Enabled = true;
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                this.button4.Enabled = false;
            }
            else if (status == 4)//4 生成截图按钮可用
            {
                this.button1.Enabled = false;
                this.button2.Enabled = true;
                this.button3.Enabled = false;
                this.button4.Enabled = false;
            }
            else if (status == 5)//5 截图优惠券按钮可用
            {
                this.button1.Enabled = false;
                this.button2.Enabled = false;
                this.button3.Enabled = true;
                this.button4.Enabled = false;
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
            }
            else
            {
                LogTool.log("未能识别的按钮状态值："+status,this.logLabel);
                this.button1.Enabled = false;
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                this.button4.Enabled = false;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是数字键，也不是.则取消该输入
            if ((Char.IsNumber(e.KeyChar)) || e.KeyChar == (char)46 || e.KeyChar == (char)8)
            {
                if (e.KeyChar == (char)46)//输入的是.
                {
                    if (String.IsNullOrEmpty(this.textBox3.Text))//输入的金额首个字符为. 取消事件
                    {
                        e.Handled = true;//取消事件
                    }
                    else
                    {
                        if (this.textBox3.Text.Contains("."))//文字中已经输入过.
                        {
                            e.Handled = true;//取消事件
                        }
                    }
                    

                }
                else
                {
                    if(e.KeyChar != (char)8)
                    {
                        if (this.textBox3.Text.IndexOf('.') != -1)//输入数字时已经包含. 判断当前.后面的位数
                        {
                            String[] splitArr = this.textBox3.Text.Split('.');
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)//这种校验不能解决4444. 这种数据
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
                dialog.Filter = "图像文件(*.jpg;*.jpg;*.jpeg;*.gif;*.png)|*.jpg;*.jpeg;*.gif;*.png";
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
            if (validateResult)//生成截图时用到的数据都正常
            {
                Console.WriteLine("将数据生成到页面");
            }
        }

        private bool validateData()//校验生成截图时用到的数据
        {
            return true;
        }
    }
}
