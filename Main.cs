using System;
using System.Windows.Forms;
using System.IO;
namespace Word_Search
{
    public partial class Main : Form
    {
        string fileContent;
        bool checker = false;
        string filePath = null;
        public Main()
        {
            InitializeComponent();
        }
        private void openfile_btn_Click(object sender, EventArgs e)
        {
            get_file();
        }

        private void findbutton_click(object sender, EventArgs e)
        {
            if (filePath == null)
            {
                toolStripStatusLabel1.Text = "No file found!\n";
                return;    
            }
            switch(algorithm_menu.SelectedIndex)
            {
                case 0:
                    show_bruteforce_results();
                    break;
                case 1:
                    show_rabinkarp_results();
                    break;
                case 2:
                    show_kmp_results();
                    break;
                default:
                    toolStripStatusLabel1.Text = "Option out of bound!";
                    break;
            }
        }

        private void get_file()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    toolStripStatusLabel1.Text = "Current file path: " + filePath;
                }
            }
        }

        private void show_bruteforce_results()
        {
            String pattern = textBox1.Text;
            int ptnLength = pattern.Length;
            int readLength;
            int row = 1;
            textBox2.Text = "---RESULT BY BRUTE FORCE---\r\n\r\n";
            using (StreamReader file = new StreamReader(filePath))
            {
                while ((fileContent = file.ReadLine()) != null)
                {
                    readLength = fileContent.Length;
                    if (BruteForce(fileContent, pattern, readLength, ptnLength) > 0)
                    {
                        textBox2.Text += "\r\nRow Number: " + row.ToString() + "\r\n\r\n";
                    }
                    row++;
                }
                file.Close();
            }
        }

        private void show_rabinkarp_results()
        {
            String pattern = textBox1.Text;
            int ptnLength = pattern.Length;
            int readLength;
            int row = 1;
            textBox2.Text = "---RESULT BY RABINKARP FORCE---\r\n\r\n";
            using (StreamReader file = new StreamReader(filePath))
            {
                while ((fileContent = file.ReadLine()) != null)
                {
                    readLength = fileContent.Length;
                    if (RabinKarp(fileContent, pattern) > 0)
                    {
                        textBox2.Text += "\r\nRow Number: " + row.ToString() + "\r\n\r\n";
                    }
                    row++;
                }
                file.Close();
            }
        }

        private void show_kmp_results()
        {
            String pattern = textBox1.Text;
            int ptnLength = pattern.Length;
            int readLength;
            int row = 1;
            textBox2.Text = "---RESULT BY KMP---\r\n\r\n";
            using (StreamReader file = new StreamReader(filePath))
            {
                while ((fileContent = file.ReadLine()) != null)
                {
                    readLength = fileContent.Length;
                    if (KMP(fileContent, pattern, readLength, ptnLength) > 0)
                    {
                        textBox2.Text += "\r\nRow Number: " + row.ToString() + "\r\n\r\n";
                    }
                    row++;
                }
                file.Close();
            }
        }

        public int RabinKarp(string txt, string pat)
        {
            if (pat.Length == 0 || txt.Length == 0)
            {
                toolStripStatusLabel1.Text = "No Content Entered!";
                return -1;
            }
            String tmp;
            int d = 256;
            int prime = 101;
            int M = pat.Length;
            int N = txt.Length;
            int i, j;
            int p = 0;
            int t = 0;
            int h = 1;
            for (i = 0; i < M - 1; i++)
            {
                h = (h * d) % prime;
            }
            for (i = 0; i < M; i++)
            {
                p = (d * p + pat[i]) % prime;
                t = (d * t + txt[i]) % prime;
            }
            for (i = 0; i <= N - M; i++)
            {
                tmp = "";
                if (p == t)
                {
                    for (j = 0; j < M; j++)
                    {
                        if (txt[i + j] != pat[j])
                            break;
                    }
                    if (j == M)
                    {
                        int x = i;
                        while (txt[x] != ' ')
                        {
                            x--;
                            if (x < 0)
                            {
                                break;
                            }
                        }
                        x++;
                        while (txt[x] != ' ')
                        {
                            tmp += txt[x];
                            x++;
                            if (x == N)
                            {
                                break;
                            }
                        }
                        if (checkBox1.Checked)
                        {
                            if (tmp != pat && tmp.ToLower() != pat.ToLower())
                            {
                                return -1;
                            }
                        }
                        textBox2.Text += "Word Found : " + tmp;
                        textBox2.Text += "\r\nColumn Number : " + i.ToString() + "\r\n";
                        checker = true;
                    }
                }
                if (i < N - M)
                {
                    t = (d * (t - txt[i] * h) + txt[i + M]) % prime;
                    if (t < 0)
                        t = (t + prime);
                }
            }
            if (checker)
            {
                checker = false;
                return 1;
            }
            toolStripStatusLabel1.Text = "Command executed successfully!";
            return -1;
        }

        public int KMP(string Text, string Pattern, int N, int M)
        {
            if (Pattern.Length == 0)
            {
                textBox2.Text = "No content entered!";
                return -1;
            }
            String tmp = "";
            int[] prefixArray = new int[M];
            int matches = 0;
            prefixArray[0] = matches;
            for (int i = 1; i < M; i++)
            {
                if (Pattern[i] == Pattern[matches])
                {
                    prefixArray[i] = ++matches;
                }
                else
                {
                    matches = 0;
                    prefixArray[i] = matches;
                }
            }
            int j = 0;
            for (int i = 0; i <= N - M; i++)
            {
                tmp = "";
                if (Char.ToLower(Text[i]) == Char.ToLower(Pattern[0]))
                {
                    for (j = 0; j < M; j++)
                    {
                        if (checkBox2.Checked)
                        {
                            if (Text[i + j] != Pattern[j])
                            {
                                i = i + j - prefixArray[j] - 1;
                                break;
                            }
                        }
                        else
                        {
                            if (Char.ToLower(Text[i + j]) != Pattern[j] && Char.ToUpper(Text[i + j]) != Pattern[j])
                            {
                                i = i + j - prefixArray[j] - 1;
                                break;
                            }
                        }
                    }
                    if (j == M)
                    {
                        int x = i;
                        while (Text[x] != ' ')
                        {
                            x--;
                            if (x < 0)
                            {
                                break;
                            }
                        }
                        x++;
                        while (Text[x] != ' ')
                        {
                            tmp += Text[x];
                            x++;
                            if (x == N)
                            {
                                break;
                            }
                        }
                        if (checkBox1.Checked)
                        {
                            if (tmp != Pattern && tmp.ToLower() != Pattern.ToLower())
                            {
                                return -1;
                            }
                        }
                        textBox2.Text += "Word Found : " + tmp;
                        textBox2.Text += "\r\nColumn Number : " + i.ToString() + "\r\n";
                        checker = true;
                        i++;
                    }
                }
            }
            if (checker)
            {
                checker = false;
                return 1;
            }
            toolStripStatusLabel1.Text = "Command executed successfully!";
            return -1;
        }

        public int BruteForce(string Text, string Pattern, int N, int M)
        {
            if (Pattern.Length == 0)
            {
                textBox2.Text = "No content entered!";
                return -1;
            }
            String tmp = "";
            for (int i = 0; i <= N - M; i++)
            {
                tmp = "";
                int j;
                for (j = 0; j < M; j++)
                {
                    if (checkBox2.Checked)
                    {
                        if (Text[i + j] != Pattern[j]) // Not Pattern[i]
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (Char.ToUpper(Text[i + j]) != Pattern[j] && Char.ToLower(Text[i + j]) != Pattern[j]) // Not Pattern[i]
                        {
                            break;
                        }
                    }
                }
                if (j == M)
                {
                    int x = i;
                    while (Text[x] != ' ')
                    {
                        x--;
                        if (x < 0)
                        {
                            break;
                        }
                    }
                    x++;
                    while (Text[x] != ' ')
                    {
                        tmp += Text[x];
                        x++;
                        if (x == N)
                        {
                            break;
                        }
                    }
                    if (checkBox1.Checked)
                    {
                        if (tmp != Pattern && tmp.ToLower() != Pattern.ToLower())
                        {
                            return -1;
                        }
                    }
                    textBox2.Text += "Word Found : " + tmp;
                    textBox2.Text += "\r\nColumn Number : " + i.ToString() + "\r\n";
                    checker = true;
                    i++;
                }
            }
            if (checker)
            {
                checker = false;
                return 1;
            }
            toolStripStatusLabel1.Text = "Command executed successfully!";
            return -1;
        }

    }
}
