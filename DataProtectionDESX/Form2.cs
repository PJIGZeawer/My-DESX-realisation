using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DataProtectionDESX
{
    public partial class Form2 : Form
    {
        Form1 fm1;
        public Form2(Form1 form1)
        {
            InitializeComponent();
            fm1 = form1;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1251 = Encoding.GetEncoding("windows-1251");
            DESX desX = new DESX(fm1.C0string);
            byte [] tmp = desX.Encrypt(textBox1.Text, fm1.Key, fm1.md);
            

            string result = enc1251.GetString(tmp);
            Debug.WriteLine(result);
            tmp = enc1251.GetBytes(result);
            


            



            if (checkBox1.Checked) {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = @"C:\Users\pavel\source\repos\DataProtectionDESX\DataProtectionDESX\bin\Debug\net7.0-windows";
                sfd.Filter = "Text Files | *.txt";
                sfd.DefaultExt = "txt";
                sfd.ShowDialog();
                
                var filee = sfd.OpenFile();
                

                using (StreamWriter writer = new StreamWriter(filee))
                {
                    writer.Write(result);
                }
                filee.Dispose();
            }
            else
            {
                //tmp = desX.Decrypt(result, fm1.Key, fm1.md);
                //result = enc1251.GetString(tmp);
                textBox2.Text = result;
            }
        }
    }
}
