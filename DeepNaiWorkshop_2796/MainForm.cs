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
            
            String url = this.textBox1.Text;
            TaoBaoTool taoBaoTool = new TaoBaoTool();
            //判断是否是天猫 淘宝链接
            RespMessage validateResult = taoBaoTool.isTmallOrTaoBaoItemPage(url);
            if (validateResult.code!=1)
            {
                alert(validateResult.message);
            }
            else
            {
                //开始解析数据
                LogTool.log("开始解析天猫（淘宝）详情页面：" + url, this.logLabel);
                //按钮不可用
                setMainFormBtnStatus(2);
                LogTool.log("正在获取网页信息",this.logLabel);
                String htmlWebContent = WebTool.getHtmlContent(url);
                LogTool.log("开始解析网页", this.logLabel);
                //目前只支持天猫
                
                if(TaoBaoTool.GOOD_TYPE_TMALL == int.Parse(validateResult.message))
                {
                    BaseDataBean dataBean = taoBaoTool.parseShopData(int.Parse(validateResult.message), htmlWebContent);
                    if (dataBean == null)
                    {
                        alert("不能正常解析数据，请手动录入");
                    }
                }
                else
                {
                    alert("目前不支持淘宝商品数据爬取，请手动录入");
                }
                



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
                this.button1.Enabled = true;
                this.button1.Enabled = true;
                this.button1.Enabled = true;
            }
            else if (status == 2)//2 按钮全部不可用
            {
                this.button1.Enabled = false;
                this.button1.Enabled = false;
                this.button1.Enabled = false;
                this.button1.Enabled = false;
            }
            else if (status == 3)//3 获取商品信息按钮可用
            {
                this.button1.Enabled = true;
                this.button1.Enabled = false;
                this.button1.Enabled = false;
                this.button1.Enabled = false;
            }
            else if (status == 4)//4 生成截图按钮可用
            {
                this.button1.Enabled = false;
                this.button1.Enabled = true;
                this.button1.Enabled = false;
                this.button1.Enabled = false;
            }
            else if (status == 5)//5 截图优惠券按钮可用
            {
                this.button1.Enabled = false;
                this.button1.Enabled = false;
                this.button1.Enabled = true;
                this.button1.Enabled = false;
            }
            else if (status == 6)//6 截图订单详情按钮可用
            {
                this.button1.Enabled = false;
                this.button1.Enabled = false;
                this.button1.Enabled = false;
                this.button1.Enabled = true;
            }
            else
            {
                LogTool.log("未能识别的按钮状态值："+status,this.logLabel);
                this.button1.Enabled = false;
                this.button1.Enabled = false;
                this.button1.Enabled = false;
                this.button1.Enabled = false;
            }
        }
    }
}
