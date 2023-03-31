using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Web;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Diagnostics;

namespace DarkSib
{
    public partial class Form1 : Form
    {
        static int a = 0;
        string Key_File = "http://t1est111.ru/rts/keys.txt"; // сайт с ключами для проверки на привязку
        MD5 md5 = new MD5CryptoServiceProvider();
        string s;
        string ke;
        string kes;
        string Infa = String.Empty;
        string temp = String.Empty;
        string temp1 = String.Empty;

        public Form1()
        {
            InitializeComponent();
            Thread load = new Thread(ГенерацияКлюча);
            load.IsBackground = true;
            load.Start();
        }
        void ГенерацияКлюча()
        {
            ke = HWIDGrabber.GetUHI;
            kes = RemoveChars(ke);
            byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(kes));
            ke = BitConverter.ToString(checkSum).Replace("-", String.Empty);
            kes = RemoveChars(ke);
            int n = int.Parse("8");
            List<string> lst = new List<string>();
            for (int i = 0; i + n <= kes.Length; i += n)
            {
                lst.Add(kes.Substring(i, n));
            }
            s = string.Join("/", lst.Select((x) => new string(x.Reverse().ToArray())));
            key.Invoke((MethodInvoker)(() => key.Text = s));
            Запрос();
            Проверка();
        }
        private  void Запрос()
        {
                HttpWebRequest rew = (HttpWebRequest)WebRequest.Create(Key_File);
                HttpWebResponse resp = (HttpWebResponse)rew.GetResponse();
                Stream str = resp.GetResponseStream();
                StreamReader readStream = new StreamReader(str, Encoding.UTF8);
                string message = readStream.ReadToEnd();
                richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.AppendText(message)));   
        }
        void Проверка()
        {
            while (a != -1)
            {
                richTextBox1.Invoke((MethodInvoker)(() => a = this.richTextBox1.Find(s, a, RichTextBoxFinds.None))); 
                if (a != -1)
                {
                    this.Invoke((MethodInvoker)(() => this.Hide()));
                    Form2 f2 = new Form2();
                    f2.ShowDialog();
                    this.Invoke((MethodInvoker)(() => this.Text = "Ключ Активирован")); 
                }
                else
                {
                    this.Invoke((MethodInvoker)(() => this.Text = "Ключ не активирован"));
                    copy.Invoke((MethodInvoker)(() => copy.Enabled = true));
                    chck.Invoke((MethodInvoker)(() => chck.Enabled = true));
                    key.Invoke((MethodInvoker)(() => key.Enabled = true));
                    pic.Invoke((MethodInvoker)(() => pic.Enabled = false));
                }
                break;
            }
            a++;
        }     
        public static string RemoveChars(string inpStr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in inpStr)
            {
                if (Char.IsDigit(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(key.Text);
            MessageBox.Show("Ключ '" + s + "' скопирован!", "Ключ скопирован!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void chck_Click(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)(() => this.Text = "Проверка лицензии!"));
            copy.Invoke((MethodInvoker)(() => copy.Enabled = false));
            chck.Invoke((MethodInvoker)(() => chck.Enabled = false));
            key.Invoke((MethodInvoker)(() => key.Enabled = false));
            pic.Invoke((MethodInvoker)(() => pic.Enabled = true));
            Thread load = new Thread(ГенерацияКлюча);
            load.IsBackground = true;
            load.Start();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Process.Start("https://t.me/devx_channel");
        }
    }
}