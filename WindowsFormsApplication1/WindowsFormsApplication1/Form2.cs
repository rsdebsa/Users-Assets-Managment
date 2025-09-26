using System;
using System.Windows.Forms;
using System.Drawing;
namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(150, 150);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Access Database (*.accdb)|*.accdb";
            openFileDialog.Title = "انتخاب فایل دیتابیس";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string dbPath = openFileDialog.FileName;

                textBox1.Text = dbPath;

                Properties.Settings.Default.DatabasePath = dbPath;
                Properties.Settings.Default.Save();

                MessageBox.Show("مسیر دیتابیس ذخیره شد ✅", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.DatabasePath;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();

        }
    }
}
