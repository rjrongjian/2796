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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SoftRegister sr = new DefaultSoftRegister();
            Console.WriteLine("获取的机器码：" + SoftRegister.getMNum());
            Console.WriteLine("生成的注册码：" + sr.generateRegistCode(SoftRegister.getMNum()));
            Console.WriteLine("生成的注册码2：" + sr.generateRegistCode(SoftRegister.getMNum()));
            RespMessage respMessage = sr.checkReg(sr.generateRegistCode(SoftRegister.getMNum()));
            Console.WriteLine("获取的注册信息："+respMessage.message);
        }
    }
}
