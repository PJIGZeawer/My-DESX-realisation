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
    public partial class Form4 : Form
    {
        Form1 fm1;
        public Form4(Form1 form1)
        {
            InitializeComponent();
            fm1 = form1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1251 = Encoding.GetEncoding("windows-1251");
            DESX desX = new DESX(fm1.C0string);
            byte[] tmp = desX.EncryptFile(fm1.Key, fm1.md);
            string result = enc1251.GetString(tmp);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @"C:\Usersы\pavel\source\repos\DataProtectionDESX\DataProtectionDESX\bin\Debug\net7.0-windows";
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

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
