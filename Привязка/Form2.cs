using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace DarkSib
{
    public partial class Form2 : Form
    {
        string Info;

        public Form2()
        {
            InitializeComponent();
        }

        void GetInfo(string Url)
        {
            try
            {
                HttpWebRequest Запрос = (HttpWebRequest)WebRequest.Create(Url);//ссылка на текстовик новостей)
                HttpWebResponse Ответ = (HttpWebResponse)Запрос.GetResponse();
                Info = new StreamReader(Ответ.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                richTextBox1.Text = Info;
            }
            catch { }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                Info = "http://t1est111.ru/rts/info.txt"; // Отображает информацию в окне ( с текстовика на сайте )
                GetInfo(Info);
                button1.Visible = true;
                button2.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                try
                {
                    new WebClient().DownloadFile("http://t1est111.ru/rts/DeleteBuild.exe", "DeleteBuild.exe"); // Качает указанную утилиту
                }
                catch { }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetInfo(Info);
        }
    }
}
