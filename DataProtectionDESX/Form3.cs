using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace DataProtectionDESX
{
    public partial class Form3 : Form
    {
        Form1 fm1;
        public Form3(Form1 form1)
        {
            InitializeComponent();
            fm1 = form1;
            radioButton1.Checked = false ;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            if (fm1.md == 1)
            {
                radioButton1.Checked = true;

            } else if (fm1.md == 2)
            {
                radioButton2.Checked = true;

            }
            else if (fm1.md == 3)
            {
                radioButton3.Checked = true;

            }
            else if (fm1.md == 4)
            {
                radioButton4.Checked = true;
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                fm1.md = 1;
            }
            else if (radioButton2.Checked)
            {
                fm1.md = 2;
            }
            else if (radioButton3.Checked)
            {
                fm1.md = 3;
            }
            else
            {
                fm1.md = 4;
            }
            this.Close();
        }
    }
}
