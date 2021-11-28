using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AndreiFile
{
    public partial class Form1 : Form
    {
        struct MySt
        {
            public string Name;
            public string Sername;
            public int Osenk;
            public double ave;
        }
        public Form1()
        {
            InitializeComponent();
        }

        static bool IsNum(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsDigit(c)) return false;
            }
            return true;
        }

        static bool IsString(string s)
        {
            foreach (char c in s)
            {
                if (Char.IsDigit(c)) return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.TextLength < 4 || textBox1.TextLength <= 0 || textBox2.TextLength <= 0)
            {
                MessageBox.Show("Не корректное заполнение формы!");
            }
            else
            {
                MySt Val = new MySt();
                Val.Name = textBox3.Text;
                Val.Sername = textBox1.Text;
                Val.Osenk = Convert.ToInt32(textBox2.Text);


                string path = @"All.txt";
                if (!File.Exists(path))
                {
                    MessageBox.Show("Файл создан");
                }
                using (StreamWriter sw = File.AppendText(path))
                {
                    string log = $"{Val.Name} {Val.Sername}  {Val.Osenk}" + Environment.NewLine;
                    sw.Write(log);
                    sw.Close();
                    MessageBox.Show("Данные записаны в файл!");
                }
            }
            //Очистка всех текс боксов
            foreach (Control c in Controls)
            {
                if (c.GetType() == typeof(GroupBox))
                    foreach (Control d in c.Controls)
                        if (d.GetType() == typeof(TextBox))
                            d.Text = string.Empty;

                if (c.GetType() == typeof(TextBox))
                    c.Text = string.Empty;
            }
        }
     

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!IsString(textBox3.Text))
            {
                MessageBox.Show("Имя строка!");
                textBox3.Clear();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!IsString(textBox1.Text))
            {
                MessageBox.Show("Фамилия строка!");
                textBox1.Clear();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe", "All.txt");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = @"All.txt", pathout = @"Out.txt";
            string[][] a = File.ReadLines(path).Select(v => v.Split(' ').ToArray()).ToArray();
            MySt[] b = new MySt[a.GetLength(0)];
            for (int i = 0; i < b.Length; i++)
            {
                b[i].Sername = a[i][0] + ' ' + a[i][1];
                if (int.TryParse(a[i][2], out b[i].Osenk))
                    if (b[i].Osenk > 5)
                b[i].ave = (double)(b[i].Osenk) / 3;
            }

            File.AppendAllText(pathout, "Не имеющие троек:" + Environment.NewLine);
            for (int i = 0; i < b.Length; i++)
            {
                int[] ave1 = { b[i].Osenk };
                if (ave1.Average() > 3)
                    File.AppendAllText(pathout, b[i].Sername + Environment.NewLine);
            }

            File.AppendAllText(pathout, Environment.NewLine + $"Ср. бал: {b.Select(v => v.Osenk).Average()}; " + Environment.NewLine);

            File.AppendAllText(pathout, Environment.NewLine + "Максимальный средний балл имеет(ют):");
            for (int i = 0; i < b.Length; i++)
                if (b[i].ave == b.Select(v => v.ave).Max())
                    File.AppendAllText(pathout, Environment.NewLine + b[i].Sername);

            File.AppendAllText(pathout, Environment.NewLine + Environment.NewLine + "В порядке убывания среднего балла:" +
                Environment.NewLine + String.Join(Environment.NewLine, b.OrderByDescending(v => v.ave).Select(v => v.Sername)));

           
            if (!File.Exists(pathout))
            {
                MessageBox.Show("Домов нет!");
            }
            using (var sr = new StreamReader(pathout))
            {
                var str = sr.ReadToEnd();
                textBox4.Text = str.ToString();
            }
        }
    }
}
