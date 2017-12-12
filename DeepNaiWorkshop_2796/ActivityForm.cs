using Microsoft.Win32;
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
    public partial class ActivityForm : Form
    {
        private SoftRegister sr ;
        private MainForm mainForm;

        public ActivityForm()
        {
            InitializeComponent();
            sr = new DefaultSoftRegister();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Text = this.Text + "-版本" + Const.VERSION;
            this.textBox1.Text = SoftRegister.getMNum();
            RegistryKey fatherKey =  SoftRegister.getFatherKey();
            this.textBox2.Text = fatherKey.GetValue(Const.VALUE_NAME_FOR_VALIDATE_IN_REGISTRY, "").ToString();

            this.Location = new Point((Screen.PrimaryScreen.Bounds.Width-this.Width)/2, (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);
            //Console.WriteLine("获取的机器码：" + SoftRegister.getMNum());
            //Console.WriteLine("生成的注册码：" + sr.generateRegistCode(SoftRegister.getMNum()));
            //Console.WriteLine("生成的注册码2：" + sr.generateRegistCode(SoftRegister.getMNum()));
            //RespMessage respMessage = sr.checkReg(sr.generateRegistCode(SoftRegister.getMNum()));
            //Console.WriteLine("获取的注册信息："+respMessage.message);
        }
        //复制机器码按钮
        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.textBox1.Text);
            this.label3.Text = "复制成功，操作时间："+DateUtil.getCurrentTimeStr();
        }
        //激活软件按钮
        private void button1_Click(object sender, EventArgs e)
        {
            this.label3.Text = "正在校验注册码...";
            this.button1.Enabled = false;
            RespMessage respMessage = sr.checkReg(this.textBox2.Text);
            if (respMessage.code == 1)//激活成功，跳转到mainForm
            {
                //隐藏激活页面
                this.Hide();

                Point activityFormLocation = this.Location;
                //显示功能页面
                mainForm = new MainForm();
                //调整页面位置
                mainForm.Location = activityFormLocation;
                mainForm.Text = mainForm.Text + "-版本" + Const.VERSION;
                mainForm.Show();
            }
            else
            {
                MessageBox.Show(respMessage.message);
            }
        }

        private void ActivityForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
