using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
namespace DataProtectionDESX
{
public partial class Form1 : Form
    {
        public string Key = "password";
        public int md = 1;
        public string C0string = "";
        public Form1()
        {
            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1251 = Encoding.GetEncoding("windows-1251");
            byte[] C0byte = new byte[8];
            Random rnd = new Random();
            for (int i = 0; i < C0byte.Length; i++)
            {
                C0byte[i] = Convert.ToByte(rnd.Next(0, 255));
            }

            foreach (byte c in C0byte)
            {
                C0string += (Convert.ToString(c, 2).PadLeft(8, '0'));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);
            form2.ShowDialog();
        }

        private void ðåæèìûØèôðîâàíèÿToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(this);
            form3.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4(this);
            form4.ShowDialog();
        }

        private void ïàðîëüíàÿÔðàçàToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5(this);
            form5.ShowDialog(); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7(this);
            form7.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form8 form8 = new Form8(this);
            form8.ShowDialog();
        }

        private void îÏðîãðàììåToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9();
            form9.Show();
        }
    }
    public class DESX {
        private const int blockSize = 64;
        public string C0String = "";
        
        public DESX(string C0string)
        {
            C0String = C0string;
        }
        public byte [] Encrypt(string inputS, string key, int mode)
        {
            var enc1251 = Encoding.GetEncoding("windows-1251");

            byte[] KeyBytes = enc1251.GetBytes(key);
            byte[] hashBytes;
            using (SHA256 sha256 = SHA256.Create())
            {
                hashBytes = sha256.ComputeHash(KeyBytes);
            }
            Debug.WriteLine("hash");
            Debug.WriteLine(hashBytes.Length);
            byte []workkeyb = hashBytes[0..8];
            byte [] dexxorb1 = hashBytes[8..16];
            byte [] dexxorb2 = hashBytes[16..24];
            string workkey = "";
            string dexxor1 = "";
            string dexxor2 = "";
            foreach (byte b in workkeyb)
            {
                workkey += (Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            foreach (byte b in dexxorb1)
            {
                dexxor1 += (Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            foreach (byte b in dexxorb2)
            {
                dexxor2 += (Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            string outputS = InputToCorrectFormatSize(inputS);
           
            Debug.Write("Input In Correct Form: ");
            Debug.WriteLine(outputS);
            string[] Blocks = CutBinaryInputToBlocks(outputS);
            for (int i = 0; i < Blocks.Length; i++)
            {
                Debug.WriteLine(Blocks[i].Length);
                Blocks[i] = XOR(Blocks[i], dexxor1);
            }


            if (mode == 1)
            {
                for (int j = 0; j < Blocks.Length; j++)
                {
                    Blocks[j] = ForwardStartShuffle(Blocks[j]);
                    string CD = FormateStartingKey(workkey);
                    for (int i = 1; i <= 16; i++)
                    {
                        string CDK = "";
                        if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                        {
                            CD = CycleShiftLeft(CD, 2);
                        }
                        else
                        {
                            CD = CycleShiftLeft(CD, 1);
                        }

                        CDK = GetKeyK(CD);

                        //Debug.Write("Block");
                        //Debug.WriteLine(Blocks[j]);
                        Blocks[j] = ForwardFeistelCycle(Blocks[j], CDK);

                    }
                    Blocks[j] = ForwardEndShuffle(Blocks[j]);
                }


            } else if (mode == 2)
            {
                //
                string CD = FormateStartingKey(workkey);
                Blocks[0] = XOR(Blocks[0], C0String);
                Blocks[0] = ForwardStartShuffle(Blocks[0]);
                for (int i = 1; i <= 16; i++)
                {
                    string CDK = "";
                    if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                    {
                        CD = CycleShiftLeft(CD, 2);
                    }
                    else
                    {
                        CD = CycleShiftLeft(CD, 1);
                    }
                    CDK = GetKeyK(CD);
                    Blocks[0] = ForwardFeistelCycle(Blocks[0], CDK);
                }
                Blocks[0] = ForwardEndShuffle(Blocks[0]);


                for (int j = 1; j < Blocks.Length; j++)
                {
                    Debug.WriteLine(Blocks[j]);
                    Blocks[j] = XOR(Blocks[j], Blocks[j - 1]);
                    Blocks[j] = ForwardStartShuffle(Blocks[j]);
                    //Debug.WriteLine(C0String);
                    CD = FormateStartingKey(workkey);
                    for (int i = 1; i <= 16; i++)
                    {
                        string CDK = "";
                        if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                        {
                            CD = CycleShiftLeft(CD, 2);
                        }
                        else
                        {
                            CD = CycleShiftLeft(CD, 1);
                        }

                        CDK = GetKeyK(CD);

                        //Debug.Write("Block");
                        //Debug.WriteLine(Blocks[j]);
                        Blocks[j] = ForwardFeistelCycle(Blocks[j], CDK);

                    }
                    Blocks[j] = ForwardEndShuffle(Blocks[j]);
                }
                //
            }
            else if (mode == 3)
            {
                //
                string CD = "";

                string Ctmp = "";
                Ctmp = C0String;
                CD = FormateStartingKey(workkey);
                Ctmp = ForwardStartShuffle(Ctmp);
                Debug.WriteLine(Blocks[0]);
                for (int i = 1; i <= 16; i++)
                {
                    string CDK = "";
                    if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                    {
                        CD = CycleShiftLeft(CD, 2);
                    }
                    else
                    {
                        CD = CycleShiftLeft(CD, 1);
                    }

                    CDK = GetKeyK(CD);

                    //Debug.Write("Block");
                    //Debug.WriteLine(Blocks[j]);
                    Ctmp = ForwardFeistelCycle(Ctmp, CDK);

                }
                Ctmp = ForwardEndShuffle(Ctmp);
                Blocks[0] = XOR(Ctmp, Blocks[0]);
                
                for (int j = 1; j < Blocks.Length; j++)
                {
                    Debug.WriteLine(Blocks[j]);
                    Ctmp = Blocks[j - 1];
                    CD = FormateStartingKey(workkey);
                    Ctmp = ForwardStartShuffle(Ctmp);
                    for (int i = 1; i <= 16; i++)
                    {
                        string CDK = "";
                        if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                        {
                            CD = CycleShiftLeft(CD, 2);
                        }
                        else
                        {
                            CD = CycleShiftLeft(CD, 1);
                        }

                        CDK = GetKeyK(CD);

                        //Debug.Write("Block");
                        //Debug.WriteLine(Blocks[j]);
                        Ctmp = ForwardFeistelCycle(Ctmp, CDK);
                    }
                    Ctmp = ForwardEndShuffle(Ctmp);
                    
                    Blocks[j] = XOR(Ctmp, Blocks[j]);
                }
                //
            }
            else
            {
                //
                    string[] K = new string[Blocks.Length];
                    K[0] = ForwardStartShuffle(C0String);
                    string CD = FormateStartingKey(workkey);
                    for (int i = 1; i <= 16; i++)
                    {
                        string CDK = "";
                        if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                        {
                            CD = CycleShiftLeft(CD, 2);
                        }
                        else
                        {
                            CD = CycleShiftLeft(CD, 1);
                        }

                        CDK = GetKeyK(CD);

                    //Debug.Write("Block");
                    //Debug.WriteLine(Blocks[j]);
                    K[0] = ForwardFeistelCycle(K[0], CDK);

                    }
                    K[0] = ForwardEndShuffle(K[0]);

                    for (int j = 1; j < Blocks.Length; j++)
                    {
                        K[j] = ForwardStartShuffle(K[j-1]);
                        CD = FormateStartingKey(workkey);
                        for (int i = 1; i <= 16; i++)
                        {
                            string CDK = "";
                            if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                            {
                                CD = CycleShiftLeft(CD, 2);
                            }
                            else
                            {
                                CD = CycleShiftLeft(CD, 1);
                            }

                            CDK = GetKeyK(CD);

                            //Debug.Write("Block");
                            //Debug.WriteLine(Blocks[j]);
                            K[j] = ForwardFeistelCycle(K[j], CDK);
                        

                        }
                        K[j] = ForwardEndShuffle(K[j]);
                    }
                    for (int i = 0; i < Blocks.Length; i++)
                    {
                    Blocks[i] = XOR(K[i], Blocks[i]);
                    }

                //
            }




            outputS = "";
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = XOR(Blocks[i], dexxor2);
                outputS += Blocks[i];
            }
            Debug.WriteLine("outputS");
            Debug.WriteLine(outputS);
            Debug.Write("Result of encode: ");
            Debug.WriteLine(outputS);
            byte[] buffer = new byte[outputS.Length / 8];
            for (int i = 0; i < Convert.ToInt32(outputS.Length / 8); i++)
            {
                buffer[i] = Convert.ToByte(outputS.Substring(8 * i, 8), 2);
            }
            
            
            return buffer;
        }

        public byte[] EncryptFile(string key, int mode)
        {
            string inputText = "";
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Stream fs = ofd.OpenFile();
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        inputText = sr.ReadToEnd();
                    }
                }
            }
            var enc1251 = Encoding.GetEncoding("windows-1251");

            byte[] KeyBytes = enc1251.GetBytes(key);
            byte[] hashBytes;
            using (SHA256 sha256 = SHA256.Create())
            {
                hashBytes = sha256.ComputeHash(KeyBytes);
            }
            Debug.WriteLine("hash");
            Debug.WriteLine(hashBytes.Length);
            byte[] workkeyb = hashBytes[0..8];
            byte[] dexxorb1 = hashBytes[8..16];
            byte[] dexxorb2 = hashBytes[16..24];
            string workkey = "";
            string dexxor1 = "";
            string dexxor2 = "";
            foreach (byte b in workkeyb)
            {
                workkey += (Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            foreach (byte b in dexxorb1)
            {
                dexxor1 += (Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            foreach (byte b in dexxorb2)
            {
                dexxor2 += (Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            string outputS = InputToCorrectFormatSize(inputText);

            Debug.Write("Input In Correct Form: ");
            Debug.WriteLine(outputS);
            string[] Blocks = CutBinaryInputToBlocks(outputS);
            for (int i = 0; i < Blocks.Length; i++)
            {
                Debug.WriteLine(Blocks[i].Length);
                Blocks[i] = XOR(Blocks[i], dexxor1);
                
            }


            if (mode == 1)
            {
                for (int j = 0; j < Blocks.Length; j++)
                {
                    Blocks[j] = ForwardStartShuffle(Blocks[j]);
                    string CD = FormateStartingKey(workkey);
                    for (int i = 1; i <= 16; i++)
                    {
                        string CDK = "";
                        if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                        {
                            CD = CycleShiftLeft(CD, 2);
                        }
                        else
                        {
                            CD = CycleShiftLeft(CD, 1);
                        }

                        CDK = GetKeyK(CD);

                        //Debug.Write("Block");
                        //Debug.WriteLine(Blocks[j]);
                        Blocks[j] = ForwardFeistelCycle(Blocks[j], CDK);

                    }
                    Blocks[j] = ForwardEndShuffle(Blocks[j]);
                }


            }
            else if (mode == 2)
            {
                //
                string CD = FormateStartingKey(workkey);
                Blocks[0] = XOR(Blocks[0], C0String);
                Blocks[0] = ForwardStartShuffle(Blocks[0]);
                for (int i = 1; i <= 16; i++)
                {
                    string CDK = "";
                    if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                    {
                        CD = CycleShiftLeft(CD, 2);
                    }
                    else
                    {
                        CD = CycleShiftLeft(CD, 1);
                    }
                    CDK = GetKeyK(CD);
                    Blocks[0] = ForwardFeistelCycle(Blocks[0], CDK);
                }
                Blocks[0] = ForwardEndShuffle(Blocks[0]);


                for (int j = 1; j < Blocks.Length; j++)
                {
                    Debug.WriteLine(Blocks[j]);
                    Blocks[j] = XOR(Blocks[j], Blocks[j - 1]);
                    Blocks[j] = ForwardStartShuffle(Blocks[j]);
                    //Debug.WriteLine(C0String);
                    CD = FormateStartingKey(workkey);
                    for (int i = 1; i <= 16; i++)
                    {
                        string CDK = "";
                        if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                        {
                            CD = CycleShiftLeft(CD, 2);
                        }
                        else
                        {
                            CD = CycleShiftLeft(CD, 1);
                        }

                        CDK = GetKeyK(CD);

                        //Debug.Write("Block");
                        //Debug.WriteLine(Blocks[j]);
                        Blocks[j] = ForwardFeistelCycle(Blocks[j], CDK);

                    }
                    Blocks[j] = ForwardEndShuffle(Blocks[j]);
                }
                //
            }
            else if (mode == 3)
            {
                //
                string CD = "";

                string Ctmp = "";
                Ctmp = C0String;
                CD = FormateStartingKey(workkey);
                Ctmp = ForwardStartShuffle(Ctmp);
                Debug.WriteLine(Blocks[0]);
                for (int i = 1; i <= 16; i++)
                {
                    string CDK = "";
                    if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                    {
                        CD = CycleShiftLeft(CD, 2);
                    }
                    else
                    {
                        CD = CycleShiftLeft(CD, 1);
                    }

                    CDK = GetKeyK(CD);

                    //Debug.Write("Block");
                    //Debug.WriteLine(Blocks[j]);
                    Ctmp = ForwardFeistelCycle(Ctmp, CDK);

                }
                Ctmp = ForwardEndShuffle(Ctmp);
                Blocks[0] = XOR(Ctmp, Blocks[0]);

                for (int j = 1; j < Blocks.Length; j++)
                {
                    Debug.WriteLine(Blocks[j]);
                    Ctmp = Blocks[j - 1];
                    CD = FormateStartingKey(workkey);
                    Ctmp = ForwardStartShuffle(Ctmp);
                    for (int i = 1; i <= 16; i++)
                    {
                        string CDK = "";
                        if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                        {
                            CD = CycleShiftLeft(CD, 2);
                        }
                        else
                        {
                            CD = CycleShiftLeft(CD, 1);
                        }

                        CDK = GetKeyK(CD);

                        //Debug.Write("Block");
                        //Debug.WriteLine(Blocks[j]);
                        Ctmp = ForwardFeistelCycle(Ctmp, CDK);
                    }
                    Ctmp = ForwardEndShuffle(Ctmp);

                    Blocks[j] = XOR(Ctmp, Blocks[j]);
                }
                //
            }
            else
            {
                //
                string[] K = new string[Blocks.Length];
                K[0] = ForwardStartShuffle(C0String);
                string CD = FormateStartingKey(workkey);
                for (int i = 1; i <= 16; i++)
                {
                    string CDK = "";
                    if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                    {
                        CD = CycleShiftLeft(CD, 2);
                    }
                    else
                    {
                        CD = CycleShiftLeft(CD, 1);
                    }

                    CDK = GetKeyK(CD);

                    //Debug.Write("Block");
                    //Debug.WriteLine(Blocks[j]);
                    K[0] = ForwardFeistelCycle(K[0], CDK);

                }
                K[0] = ForwardEndShuffle(K[0]);

                for (int j = 1; j < Blocks.Length; j++)
                {
                    K[j] = ForwardStartShuffle(K[j - 1]);
                    CD = FormateStartingKey(workkey);
                    for (int i = 1; i <= 16; i++)
                    {
                        string CDK = "";
                        if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                        {
                            CD = CycleShiftLeft(CD, 2);
                        }
                        else
                        {
                            CD = CycleShiftLeft(CD, 1);
                        }

                        CDK = GetKeyK(CD);

                        //Debug.Write("Block");
                        //Debug.WriteLine(Blocks[j]);
                        K[j] = ForwardFeistelCycle(K[j], CDK);


                    }
                    K[j] = ForwardEndShuffle(K[j]);
                }
                for (int i = 0; i < Blocks.Length; i++)
                {
                    Blocks[i] = XOR(K[i], Blocks[i]);
                }

                //
            }





            outputS = "";
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = XOR(Blocks[i], dexxor2);
                outputS += Blocks[i];
            }
            Debug.WriteLine("outputS");
            Debug.WriteLine(outputS);
            Debug.Write("Result of encode: ");
            Debug.WriteLine(outputS);
            byte[] buffer = new byte[outputS.Length / 8];
            for (int i = 0; i < Convert.ToInt32(outputS.Length / 8); i++)
            {
                buffer[i] = Convert.ToByte(outputS.Substring(8 * i, 8), 2);
            }


            return buffer;
        }

        public byte[] Decrypt(string inputS, string key, int mode)
        {
            var enc1251 = Encoding.GetEncoding("windows-1251");

            byte[] KeyBytes = enc1251.GetBytes(key);
            byte[] hashBytes;
            using (SHA256 sha256 = SHA256.Create())
            {
                hashBytes = sha256.ComputeHash(KeyBytes);
            }
            byte[] workkeyb = hashBytes[0..8];
            byte[] dexxorb1 = hashBytes[8..16];
            byte[] dexxorb2 = hashBytes[16..24];
            string workkey = "";
            string dexxor1 = "";
            string dexxor2 = "";
            foreach (byte b in workkeyb)
            {
                workkey += (Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            foreach (byte b in dexxorb1)
            {
                dexxor1 += (Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            foreach (byte b in dexxorb2)
            {
                dexxor2 += (Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            string outputS = InputToCorrectFormatSize(inputS);
            Debug.WriteLine("inputS");
            Debug.WriteLine(inputS);
            Debug.Write("Input In Correct Form: ");
            Debug.WriteLine(outputS);
            string[] Blocks = CutBinaryInputToBlocks(outputS);
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = XOR(Blocks[i], dexxor2);

                //Debug.WriteLine("blocks");
                //Debug.WriteLine(Blocks[i].Length);
            }
            if (mode == 1)
            {
                for (int j = 0; j < Blocks.Length; j++)
                {
                    string CD = FormateStartingKey(workkey);
                    Blocks[j] = ForwardStartShuffle(Blocks[j]);
                    string[] CDs = new string[16];
                    CDs[0] = CycleShiftLeft(CD, 1);
                    for (int i = 2; i <= 16; i++)
                    {
                        if ((i != 2) && (i != 9) && (i != 16))
                        {
                            CDs[i - 1] = CycleShiftLeft(CDs[i - 2], 2);
                        }
                        else
                        {
                            CDs[i - 1] = CycleShiftLeft(CDs[i - 2], 1);
                        }

                    }
                    for (int i = 16; i > 0; i--)
                    {

                        string CDK = GetKeyK(CDs[i - 1]);

                        //Debug.Write("Block");
                        //Debug.WriteLine(Blocks[j]);
                        Blocks[j] = BackwardFeistelCycle(Blocks[j], CDK);

                    }
                    Blocks[j] = ForwardEndShuffle(Blocks[j]);
                }
            }
            else if (mode == 2)
            {
                //
                for (int j = Blocks.Length - 1; j > -1; j--)
                {
                    string CD = FormateStartingKey(workkey);
                    Blocks[j] = ForwardStartShuffle(Blocks[j]);
                    string[] CDs = new string[16];
                    CDs[0] = CycleShiftLeft(CD, 1);
                    for (int i = 2; i <= 16; i++)
                    {
                        if ((i != 2) && (i != 9) && (i != 16))
                        {
                            CDs[i - 1] = CycleShiftLeft(CDs[i - 2], 2);
                        }
                        else
                        {
                            CDs[i - 1] = CycleShiftLeft(CDs[i - 2], 1);
                        }

                    }
                    for (int i = 16; i > 0; i--)
                    {

                        string CDK = GetKeyK(CDs[i - 1]);

                        //Debug.Write("Block");
                        //Debug.WriteLine(Blocks[j]);
                        Blocks[j] = BackwardFeistelCycle(Blocks[j], CDK);

                    }
                    Blocks[j] = ForwardEndShuffle(Blocks[j]);
                    
                    if (j == 0)
                    {
                        Blocks[j] = XOR(Blocks[j], C0String);
                    } else
                    {
                        Blocks[j] = XOR(Blocks[j], Blocks[j - 1]);
                    }
                    Debug.WriteLine(Blocks[j]);
                }
                //
            }
            else if (mode == 3)
            {
                string CD = "";

                string Ctmp = "";
                
                for (int j = Blocks.Length - 1; j > -1; j--)
                {
                    if (j != 0)
                    {
                        Ctmp = Blocks[j - 1];
                    } else
                    {
                        Ctmp = C0String;
                    }
                    CD = FormateStartingKey(workkey);
                    Ctmp = ForwardStartShuffle(Ctmp);
                    for (int i = 1; i <= 16; i++)
                    {
                        string CDK = "";
                        if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                        {
                            CD = CycleShiftLeft(CD, 2);
                        }
                        else
                        {
                            CD = CycleShiftLeft(CD, 1);
                        }

                        CDK = GetKeyK(CD);

                        //Debug.Write("Block");
                        //Debug.WriteLine(Blocks[j]);
                        Ctmp = ForwardFeistelCycle(Ctmp, CDK);
                    }
                    Ctmp = ForwardEndShuffle(Ctmp);
                    Blocks[j] = XOR(Ctmp, Blocks[j]);
                    Debug.WriteLine(Blocks[j]);
                }
            }
            else
            {
                string[] K = new string[Blocks.Length];
                K[0] = ForwardStartShuffle(C0String);
                string CD = FormateStartingKey(workkey);
                for (int i = 1; i <= 16; i++)
                {
                    string CDK = "";
                    if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                    {
                        CD = CycleShiftLeft(CD, 2);
                    }
                    else
                    {
                        CD = CycleShiftLeft(CD, 1);
                    }

                    CDK = GetKeyK(CD);

                    //Debug.Write("Block");
                    //Debug.WriteLine(Blocks[j]);
                    K[0] = ForwardFeistelCycle(K[0], CDK);

                }
                K[0] = ForwardEndShuffle(K[0]);

                for (int j = 1; j < Blocks.Length; j++)
                {
                    K[j] = ForwardStartShuffle(K[j - 1]);
                    CD = FormateStartingKey(workkey);
                    for (int i = 1; i <= 16; i++)
                    {
                        string CDK = "";
                        if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                        {
                            CD = CycleShiftLeft(CD, 2);
                        }
                        else
                        {
                            CD = CycleShiftLeft(CD, 1);
                        }

                        CDK = GetKeyK(CD);

                        //Debug.Write("Block");
                        //Debug.WriteLine(Blocks[j]);
                        K[j] = ForwardFeistelCycle(K[j], CDK);
                    }
                    K[j] = ForwardEndShuffle(K[j]);
                }
                for (int i = 0; i < Blocks.Length; i++)
                {
                    Blocks[i] = XOR(K[i], Blocks[i]);
                }
            }


            outputS = "";
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = XOR(Blocks[i], dexxor1);
                outputS += Blocks[i];
            }

            Debug.Write("Result of decode: ");
            Debug.WriteLine(outputS);

            byte[] buffer = new byte[outputS.Length / 8];
            for (int i = 0; i < Convert.ToInt32(outputS.Length / 8); i++)
            {
                buffer[i] = Convert.ToByte(outputS.Substring(8 * i, 8), 2);
            }

            byte symbol = enc1251.GetBytes("#")[0];
            int k = buffer.Length - 1;
            while (buffer[k] == symbol)
            {
                k--;
            }
            byte[] bufferR = new byte[k + 1];
            while (k > -1)
            {
                bufferR[k] = buffer[k];
                k--;
            }
            return bufferR;
        }
        public byte[] DecryptFile(string key, int mode)
        {
            string inputText = "";
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Stream fs = ofd.OpenFile();
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        inputText = sr.ReadToEnd();
                    }
                }
            }
            var enc1251 = Encoding.GetEncoding("windows-1251");

            byte[] KeyBytes = enc1251.GetBytes(key);
            byte[] hashBytes;
            using (SHA256 sha256 = SHA256.Create())
            {
                hashBytes = sha256.ComputeHash(KeyBytes);
            }
            byte[] workkeyb = hashBytes[0..8];
            byte[] dexxorb1 = hashBytes[8..16];
            byte[] dexxorb2 = hashBytes[16..24];
            string workkey = "";
            string dexxor1 = "";
            string dexxor2 = "";
            foreach (byte b in workkeyb)
            {
                workkey += (Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            foreach (byte b in dexxorb1)
            {
                dexxor1 += (Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            foreach (byte b in dexxorb2)
            {
                dexxor2 += (Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            string outputS = InputToCorrectFormatSize(inputText);
            Debug.WriteLine("inputS");
            Debug.WriteLine(inputText);
            Debug.Write("Input In Correct Form: ");
            Debug.WriteLine(outputS);
            string[] Blocks = CutBinaryInputToBlocks(outputS);
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = XOR(Blocks[i], dexxor2);
                

                //Debug.WriteLine("blocks");
                //Debug.WriteLine(Blocks[i].Length);
            }
            if (mode == 1)
            {
                for (int j = 0; j < Blocks.Length; j++)
                {
                    string CD = FormateStartingKey(workkey);
                    Blocks[j] = ForwardStartShuffle(Blocks[j]);
                    string[] CDs = new string[16];
                    CDs[0] = CycleShiftLeft(CD, 1);
                    for (int i = 2; i <= 16; i++)
                    {
                        if ((i != 2) && (i != 9) && (i != 16))
                        {
                            CDs[i - 1] = CycleShiftLeft(CDs[i - 2], 2);
                        }
                        else
                        {
                            CDs[i - 1] = CycleShiftLeft(CDs[i - 2], 1);
                        }

                    }
                    for (int i = 16; i > 0; i--)
                    {

                        string CDK = GetKeyK(CDs[i - 1]);

                        //Debug.Write("Block");
                        //Debug.WriteLine(Blocks[j]);
                        Blocks[j] = BackwardFeistelCycle(Blocks[j], CDK);

                    }
                    Blocks[j] = ForwardEndShuffle(Blocks[j]);
                }
            }
            else if (mode == 2)
            {
                //
                for (int j = Blocks.Length - 1; j > -1; j--)
                {
                    string CD = FormateStartingKey(workkey);
                    Blocks[j] = ForwardStartShuffle(Blocks[j]);
                    string[] CDs = new string[16];
                    CDs[0] = CycleShiftLeft(CD, 1);
                    for (int i = 2; i <= 16; i++)
                    {
                        if ((i != 2) && (i != 9) && (i != 16))
                        {
                            CDs[i - 1] = CycleShiftLeft(CDs[i - 2], 2);
                        }
                        else
                        {
                            CDs[i - 1] = CycleShiftLeft(CDs[i - 2], 1);
                        }

                    }
                    for (int i = 16; i > 0; i--)
                    {

                        string CDK = GetKeyK(CDs[i - 1]);

                        //Debug.Write("Block");
                        //Debug.WriteLine(Blocks[j]);
                        Blocks[j] = BackwardFeistelCycle(Blocks[j], CDK);

                    }
                    Blocks[j] = ForwardEndShuffle(Blocks[j]);

                    if (j == 0)
                    {
                        Blocks[j] = XOR(Blocks[j], C0String);
                    }
                    else
                    {
                        Blocks[j] = XOR(Blocks[j], Blocks[j - 1]);
                    }
                    Debug.WriteLine(Blocks[j]);
                }
                //
            }
            else if (mode == 3)
            {
                string CD = "";

                string Ctmp = "";

                for (int j = Blocks.Length - 1; j > -1; j--)
                {
                    if (j != 0)
                    {
                        Ctmp = Blocks[j - 1];
                    }
                    else
                    {
                        Ctmp = C0String;
                    }
                    CD = FormateStartingKey(workkey);
                    Ctmp = ForwardStartShuffle(Ctmp);
                    for (int i = 1; i <= 16; i++)
                    {
                        string CDK = "";
                        if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                        {
                            CD = CycleShiftLeft(CD, 2);
                        }
                        else
                        {
                            CD = CycleShiftLeft(CD, 1);
                        }

                        CDK = GetKeyK(CD);

                        //Debug.Write("Block");
                        //Debug.WriteLine(Blocks[j]);
                        Ctmp = ForwardFeistelCycle(Ctmp, CDK);
                    }
                    Ctmp = ForwardEndShuffle(Ctmp);
                    Blocks[j] = XOR(Ctmp, Blocks[j]);
                    Debug.WriteLine(Blocks[j]);
                }
            }
            else
            {
                string[] K = new string[Blocks.Length];
                K[0] = ForwardStartShuffle(C0String);
                string CD = FormateStartingKey(workkey);
                for (int i = 1; i <= 16; i++)
                {
                    string CDK = "";
                    if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                    {
                        CD = CycleShiftLeft(CD, 2);
                    }
                    else
                    {
                        CD = CycleShiftLeft(CD, 1);
                    }

                    CDK = GetKeyK(CD);

                    //Debug.Write("Block");
                    //Debug.WriteLine(Blocks[j]);
                    K[0] = ForwardFeistelCycle(K[0], CDK);

                }
                K[0] = ForwardEndShuffle(K[0]);

                for (int j = 1; j < Blocks.Length; j++)
                {
                    K[j] = ForwardStartShuffle(K[j - 1]);
                    CD = FormateStartingKey(workkey);
                    for (int i = 1; i <= 16; i++)
                    {
                        string CDK = "";
                        if ((i != 1) && (i != 2) && (i != 9) && (i != 16))
                        {
                            CD = CycleShiftLeft(CD, 2);
                        }
                        else
                        {
                            CD = CycleShiftLeft(CD, 1);
                        }

                        CDK = GetKeyK(CD);

                        //Debug.Write("Block");
                        //Debug.WriteLine(Blocks[j]);
                        K[j] = ForwardFeistelCycle(K[j], CDK);
                    }
                    K[j] = ForwardEndShuffle(K[j]);
                }
                for (int i = 0; i < Blocks.Length; i++)
                {
                    Blocks[i] = XOR(K[i], Blocks[i]);
                }
            }

            outputS = "";
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = XOR(Blocks[i], dexxor1);
                outputS += Blocks[i];
            }

            Debug.Write("Result of decode: ");
            Debug.WriteLine(outputS);

            byte[] buffer = new byte[outputS.Length / 8];
            for (int i = 0; i < Convert.ToInt32(outputS.Length / 8); i++)
            {
                buffer[i] = Convert.ToByte(outputS.Substring(8 * i, 8), 2);
            }

            byte symbol = enc1251.GetBytes("#")[0];
            int k = buffer.Length - 1;
            while (buffer[k] == symbol)
            {
                k--;
            }
            byte[] bufferR = new byte[k + 1];
            while (k > -1)
            {
                bufferR[k] = buffer[k];
                k--;
            }
            return bufferR;
        }
        /*private string EditKey(string inpkey)
        {
            string otptkey = "";
            foreach (char c in inpkey)
            {
                otptkey += Convert.ToString(c, 2).PadLeft(8, '0');
            }
            if (otptkey.Length > 56)
            {
                return otptkey.Substring(0, 56);
            }
            return otptkey.PadLeft(56, '0');
        }*/
        private string InputToCorrectFormatSize(string inp)
        {
            int rem = inp.Length % 8;
            string outp1 = inp;
            string outp2 = "";
            if (rem != 0)
            {
                for (int i = 0; i < 8 - rem; i++) outp1 = outp1 + '#';
            }
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1251 = Encoding.GetEncoding(1251);
            byte[] bytes = enc1251.GetBytes(outp1, 0, outp1.Length);
            /*for (int i = 0; i < bytes.Length; i++)
            {
                Debug.Write(bytes[i]);
            }*/
            foreach (byte c in bytes)
            {
                
                outp2 += (Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            
            return outp2;
        }
        private string[] CutBinaryInputToBlocks(string bint)
        {
            int count = bint.Length / blockSize;
            string[] blcks = new string[count];
            for (int i = 0; i < count; i++)
            {
                blcks[i] = bint.Substring(i * blockSize, blockSize);
            }
            
            return blcks;
        }
        private string ForwardStartShuffle(string blck)
        {
            string outblck = "";
            /*int tmprem = 58;
            int tmp = tmprem;
            while (tmprem < 65)
            {
                outblck += blck[tmp - 1];
                tmp -= 8;
                if (tmp < 1)
                {
                    tmprem = tmprem + 2;
                    tmp = tmprem;
                }
            }
            
            tmprem = 57;
            tmp = tmprem;
            while (tmprem <= 63)
            {
                
                outblck += blck[tmp - 1];
                tmp -= 8;
                if (tmp < 1)
                {
                    tmprem = tmprem + 2;
                    tmp = tmprem;
                }
            }*/
            int[] arr = new int[]
        {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
        };
            for (int i =0; i < arr.Length;i++)
            {
                outblck += blck[arr[i] - 1];
            }
            return outblck;
        }
        private string ForwardEndShuffle(string inp)
        {
            string outp = "";
            int[] PP = new int[]
            {
                40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31,
                38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29,
                36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27,
                34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25
            };
            for (int i = 0; i < PP.Length; i++)
            {
                outp += inp[PP[i] - 1];
            }
            return outp;
        }
        private string ForwardFeistelCycle(string blck, string key)
        {
            string L = blck.Substring(0, blck.Length / 2);
            string R = blck.Substring(blck.Length / 2, blck.Length / 2);
            //Debug.WriteLine(blck);
            //Debug.WriteLine(L);
            //Debug.WriteLine(R);
            //Debug.WriteLine(R + XOR(L, f(R, key)));
            return R + XOR(L, f(R, key));
        }
        private string BackwardFeistelCycle(string blck, string key)
        {
            string L = blck.Substring(0, blck.Length / 2);
            string R = blck.Substring(blck.Length / 2, blck.Length / 2);
            //Debug.WriteLine(L);
            return XOR(R, f(L, key)) + L;
        }
        private string XOR(string L, string R)
        {
            string res = "";
            /*Debug.WriteLine("L");
            Debug.WriteLine(L);
            Debug.WriteLine("R");
            Debug.WriteLine(R);*/
            for (int i = 0; i < L.Length;i++)
            {
                int k1 = Convert.ToInt32(L[i]);
                int k2 = Convert.ToInt32(R[i]);
                res += Convert.ToString(k1 ^ k2);
            }
            
            return res;
        }
        private string f(string R, string k)
        {
            int[] arrE = { 32, 1, 2, 3, 4, 5, 4, 5, 6, 7, 8, 9, 8, 9, 10, 11, 12, 13, 12, 13, 14, 15, 16, 17, 16, 17, 18, 19, 20, 21, 20, 21, 22, 23, 24, 25, 24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1 };
            string ER = "";
            
            for (int i = 0; i < arrE.Length; i++)
            {
                ER += R[arrE[i] - 1];
            }
            ER = XOR(ER, k);
            
                int[,,] S = {{{14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 }, 
                            { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 }, 
                            { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 }, 
                            { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 }}

                            ,{ {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10},
                             {3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5},
                             {0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15},
                             {13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9} }

                            ,{ {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8},
                             {13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1},
                             {13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7},
                             {1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12} }

                            ,{ { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15},
                             { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9},
                             { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4},
                             { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14} }

                            ,{ { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9},
                             { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6},
                             { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14},
                             { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3} }

                            ,{ { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11},
                             { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8},
                             { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6},
                             { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13} }

                            ,{ { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1},
                             { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6},
                             { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2},
                             { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12} }

                            ,{ { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7},
                             { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2},
                             { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8},
                             { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11} } };
            
            string[] temp = new string[8];
            for (int i = 0; i < 8; i++)
            {
                temp[i] = ER.Substring(i * 6, 6);
            }
            string result = "";
            for (int i = 0; i < 8; i++) {
                
                string n1 = (Convert.ToString(temp[i][0]) + Convert.ToString(temp[i][5]));
                string n2 = temp[i].Substring(1, 4);
                
                int n11 = Convert.ToInt32(n1[0] - '0') * 2 + Convert.ToInt32(n1[1] - '0');
                int n22 = Convert.ToInt32(n2[0] - '0') * 8 + Convert.ToInt32(n2[1] - '0') * 4 + Convert.ToInt32(n2[2] - '0') * 2 + Convert.ToInt32(n2[3] - '0');
                //Debug.WriteLine(n2);
                //Debug.WriteLine(Convert.ToInt32(n2[0]));
                int num = S[i, n11, n22];
                string n3 = "";
                while (num > 0)
                {
                    n3 += Convert.ToString(num % 2);
                    num = num / 2;
                }
                
                result += (n3.PadLeft(4, '0'));
            }
            
            int[] arrP = {
    16, 7, 20, 21, 29, 12, 28, 17,
    1, 15, 23, 26, 5, 18, 31, 10,
    2, 8, 24, 14, 32, 27, 3, 9,
    19, 13, 30, 6, 22, 11, 4, 25
};
            string resultP = "";
            for (int i = 0; i < arrP.Length;i++)
            {
                resultP += result[arrP[i] - 1];
            }
            //Debug.Write("resultP");
            //Debug.WriteLine(resultP);
            return resultP;
        }
        private string FormateStartingKey(string inpkey)
        {
            string outpkey = "";
            for (int i = 0; i < 64; i++)
            {
                if ((i + 1) % 8 == 0)
                {
                    string tmps = outpkey.Substring(i - 7, 7);
                    int count = 0;
                    foreach (char c in tmps)
                    {
                        if (c == '1') count++;
                    }
                    if (count % 2 != 0)
                    {
                        outpkey += "0";
                    }
                    else
                    {
                        outpkey += "1";
                    }

                }
                else
                {
                    
                    outpkey += inpkey[i - Convert.ToInt32(i / 8)];
                }
            }



            string outpkey2 = "";
            int tmprem = 57;
            int tmp = tmprem;
            while (tmprem < 61)
            {
                outpkey2 += outpkey[tmp - 1];
                tmp -= 8;
                if (tmp < 1)
                {
                    tmprem = tmprem + 1;
                    tmp = tmprem;
                }
            }

            tmprem = 63;
            tmp = tmprem;
            while (tmprem >= 61)
            {
                outpkey2 += outpkey[tmp - 1];
                tmp -= 8;
                if (tmp < 1)
                {
                    tmprem = tmprem - 1;
                    tmp = tmprem;
                }
            }
            outpkey2 = outpkey2 + outpkey[27] + outpkey[19] + outpkey[11] + outpkey[3];
            return outpkey2;
        }
        private string CycleShiftLeft(string inp, int count)
        {
            string outp = "";
            for (int i = 0; i < inp.Length - count; i++)
            {
                outp += inp[i + count];
            }
            for (int i = 0; i < count; i++)
            {
                outp += inp[i];
            }
            return outp;
        }
      
        private string GetKeyK(string inpkey)
        {
            int[] arr = { 14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32 };
            string temp = "";
            for (int i = 0; i < arr.Length; i++)
            {
                temp += inpkey[arr[i] - 1];
            }
            return temp;
        }
    }
}