using RegeditActivity;
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
    public partial class TemplateDisplayForm : Form
    {
        private MainForm MainForm;
        public TemplateDisplayForm(MainForm mainForm)
        {
            MainForm = mainForm;
            InitializeComponent();
            this.Hide();
        }


        public void DisplayImg(Image image)
        {
            this.Show();
            pictureBox1.Image = image;
        }

        private void TemplateDisplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.Hide();
        }

        private void TemplateDisplayForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.ResetTemplateForm();
        }
    }
}
