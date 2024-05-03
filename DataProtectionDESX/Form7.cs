using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataProtectionDESX
{
    public partial class Form7 : Form
    {
        Form1 fm1;
        public Form7(Form1 form1)
        {
            InitializeComponent();
            fm1 = form1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1251 = Encoding.GetEncoding("windows-1251");
            DESX desX = new DESX(fm1.C0string);
            byte[] tmp = desX.Decrypt(textBox1.Text, fm1.Key, fm1.md);


            string result = enc1251.GetString(tmp);
            tmp = enc1251.GetBytes(result);







            if (checkBox1.Checked)
            {
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
