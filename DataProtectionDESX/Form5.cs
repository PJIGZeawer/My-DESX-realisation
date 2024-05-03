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
    public partial class Form5 : Form
    {
        Form1 form1;
        public int minSize = 0;
        public bool lim1 = false;
        public bool lim2 = false;
        public bool lim3 = false;
        public string lim3list = "";
        public Form5(Form1 fm1)
        {
            InitializeComponent();
            form1 = fm1;
        }

        private void задатьОграниченияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6(this);
            form6.ShowDialog();
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }
        public bool IsInString(string str, char ch)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ch)
                {
                    return true;
                }
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text == textBox2.Text) && (textBox1.Text != ""))
            {
                if (textBox2.Text.Length >= minSize)
                {
                    int correct = 0;
                    if (!lim1)
                    {
                        
                        correct++;
                    } else
                    {
                        int k = 0;
                        while (k < textBox2.Text.Length)
                        {
                            if ( ((textBox2.Text[k] >= 'A') && (textBox2.Text[k] <= 'Z')) || ((textBox2.Text[k] >= 'А') && (textBox2.Text[k] <= 'Я')))
                            {
                                break;
                            }
                            k++;
                        }
                        if (k < textBox2.Text.Length)
                        {
                            correct++;
                        }
                        else
                        {
                            label3.Text = "В парольной фразе нет прописных букв";
                        }
                    }
                    if (!lim2)
                    {
                        correct++;
                    } else
                    {
                        int k = 0;
                        while (k < textBox2.Text.Length)
                        {
                            if ((textBox2.Text[k] >= '0') && (textBox2.Text[k] <= '9'))
                            {
                                break;
                            }
                            k++;
                        }
                        if (k < textBox2.Text.Length)
                        {
                            correct++;
                        } else
                        {
                            label3.Text = "В парольной фразе нет цифр";
                        }
                    }
                    if (!lim3)
                    {
                        correct++;
                    } else
                    {
                        int tmp = 0;
                        foreach (char c in lim3list)
                        {
                            if (IsInString(textBox2.Text, c))
                            {
                                tmp++;
                            }
                        }
                        if (tmp >= lim3list.Length)
                        {
                            correct++;
                        }
                     else
                    {
                        label3.Text = "В парольной фразе нет всех символов из списка";
                    }
                }
                    if (correct == 3)
                    {
                        form1.Key = textBox2.Text;
                        this.Close();
                    }
                } else
                {
                    label3.Text = "Парольная фраза слишком короткая";
                }
            } else if (textBox1.Text != textBox2.Text)
            {
                label3.Text = "Парольные фразы не совпадают";
            } else
            {
                label3.Text = "Парольная фраза пустая";
            }
            
        }
    }
}
