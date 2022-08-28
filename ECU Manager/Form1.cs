using System;
using System.IO;
//using System.IO.MemoryMappedFiles;
using System.Windows.Forms;


namespace ECU_Manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /*
        int s = 20; // Ширина столбцов с 1 по 16
        int s2 = 12; // Ширина столбцов с 18 по 34
        dataGridView1.Columns[0].Width = 50;

        for (int i = 1; i < 17; i++)
        {
            dataGridView1.Columns[i].Width = s; // Присвоение ширины
            dataGridView1.Columns[i + 17].Width = s2; // Присвоение ширины
            ((DataGridViewTextBoxColumn)dataGridView1.Columns[i]).MaxInputLength = 2; // Ограничение на ввод 2 знаками
            ((DataGridViewTextBoxColumn)dataGridView1.Columns[i + 17]).MaxInputLength = 1; //Ограничение на ввод 1 знаком
        }

        dataGridView1.Columns[17].Width = 10;

        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.AllowUserToDeleteRows = false;
        dataGridView1.AllowUserToResizeColumns = false;
        dataGridView1.AllowUserToResizeRows = false;
        */

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileInfo file = new System.IO.FileInfo(openFileDialog.FileName);
                long size = file.Length;
                byte[] shc = new byte[size];
                using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    fs.Read(shc, 0, System.Convert.ToInt32(size));
                }
                dataGridView1.Rows.Clear();
                long kolStrok = size / 16;
                for (long i = 0; i < kolStrok; i++)
                {
                    string id = (i * 16).ToString("X") + ":";
                    id = id.PadLeft(7, '0');
                    dataGridView1.Rows.Add(id);
                }
                long c = 0;
                for (int i = 0; i < kolStrok; i++)
                {
                    for (byte b = 1; b < 17; b++)
                    {
                        dataGridView1[b, i].Value = string.Format("{0:X2}", shc[c]); // Заполнение HEX поля 
                        c++;
                    }
                }
                c = 0;
                for (int i = 0; i < kolStrok; i++)
                {
                    for (byte b = 18; b < 34; b++)
                    {
                        dataGridView1[b, i].Value = System.Convert.ToChar(shc[c]); // Заполнение ASCII поля
                        c++;
                    }
                }
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //Создаем обьекты
            
            MappedMemory.Mmf mmf = new MappedMemory.Mmf();

            //путь до файла
            string pathToFile = @"C:\2.BIN";

            //Получаем ссылку на MappedMemory
            
            var buffer=mmf.MoveToMmf(pathToFile);
            

            
            // Конвертируем байты в HEX
            string hex = Convert.ToHexString(buffer.Read(0, byte[]));

            // Заполняем RTB строкой в HEX формате
            richTextBox1.Text = hex;

            //Строка поиска в HEX формате
            string strSearch = "0188";

            //Offset вхождения в строку
            int b = hex.IndexOf(strSearch, 0);

            // offset и сколько вхождений
            while (b > -1)
            {
                listBox1.Items.Add(b.ToString() + Environment.NewLine);
                b = hex.IndexOf(strSearch, b + strSearch.Length);
            }


            //Количество вхождений
            MessageBox.Show(listBox1.Items.Count.ToString());
        }
    }
}







