using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataProtectionDESX
{
    public partial class Form6 : Form
    {
        Form5 fm5;
        public Form6(Form5 form5)
        {
            InitializeComponent();

            fm5 = form5;
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(textBox1.Text) > 0)
            fm5.minSize = Convert.ToInt32(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                fm5.lim1 = true;
            } 
            if (checkBox2.Checked)
            {
                fm5.lim2 = true;
            } 
            if (checkBox3.Checked)
            {
                fm5.lim3 = true;
                fm5.lim3list = textBox2.Text;
            }
            this.Close();
        }

        private void Form6_Shown(object sender, EventArgs e)
        {
            if (fm5.lim1)
            {
                checkBox1.Checked = true;
            }
            if (fm5.lim2)
            {
                checkBox2.Checked = true;
            }
            if (fm5.lim3)
            {
                checkBox3.Checked = true;
            }
        }
    }
}
