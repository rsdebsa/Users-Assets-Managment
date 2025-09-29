using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            panelReports.Visible = false;

        }
        private void Form3_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.DatabasePath = "";
            Properties.Settings.Default.Save();
            ContextMenuStrip cms = new ContextMenuStrip();
            button5.ContextMenuStrip = cms;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            using (Form2 frm = new Form2())
            {
                frm.ShowDialog(this); 
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string dbPath = Properties.Settings.Default.DatabasePath;

            if (string.IsNullOrWhiteSpace(dbPath) || !System.IO.File.Exists(dbPath))
            {
                MessageBox.Show("لطفاً ابتدا دیتابیس خود را انتخاب کنید ❌",
                                "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Hide();

            Form4 form4 = new Form4();
            form4.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string dbPath = Properties.Settings.Default.DatabasePath;

            if (string.IsNullOrWhiteSpace(dbPath) || !System.IO.File.Exists(dbPath))
            {
                MessageBox.Show("لطفاً ابتدا دیتابیس خود را انتخاب کنید ❌",
                                "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Hide();

            Form6 form6 = new Form6();
            form6.Show();

        }
        private void button5_Click(object sender, EventArgs e)
        {
            string dbPath = Properties.Settings.Default.DatabasePath;

            if (string.IsNullOrWhiteSpace(dbPath) || !System.IO.File.Exists(dbPath))
            {
                MessageBox.Show("لطفاً ابتدا دیتابیس خود را انتخاب کنید ❌",
                                "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
        
            DialogResult result = MessageBox.Show("آیا مطمئن هستید که می‌خواهید خارج شوید؟",
                                                  "خروج از برنامه",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button5_MouseHover(object sender, EventArgs e)
        {
            panelReports.Visible = true;
        }



        private void Form3_MouseHover(object sender, EventArgs e)
        {
            if (panelReports.Visible)
            {
                Point mousePos = this.PointToClient(MousePosition);
                if (!panelReports.Bounds.Contains(mousePos) && !button5.Bounds.Contains(mousePos))
                {
                    panelReports.Visible = false;
                }
            }
        }

        private void bntUsersReport_Click(object sender, EventArgs e)
        {
            string dbPath = Properties.Settings.Default.DatabasePath;

            if (string.IsNullOrWhiteSpace(dbPath) || !System.IO.File.Exists(dbPath))
            {
                MessageBox.Show("لطفاً ابتدا دیتابیس خود را انتخاب کنید ❌",
                                "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Hide();

            Form10 form10 = new Form10();
            form10.Show();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            string dbPath = Properties.Settings.Default.DatabasePath;

            if (string.IsNullOrWhiteSpace(dbPath) || !System.IO.File.Exists(dbPath))
            {
                MessageBox.Show("لطفاً ابتدا دیتابیس خود را انتخاب کنید ❌",
                                "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Hide();

            Form12 form12 = new Form12();
            form12.Show();
        }
        private void button8_Click(object sender, EventArgs e)
        {

            string dbPath = Properties.Settings.Default.DatabasePath;

            if (string.IsNullOrWhiteSpace(dbPath) || !System.IO.File.Exists(dbPath))
            {
                MessageBox.Show("لطفاً ابتدا دیتابیس خود را انتخاب کنید ❌",
                                "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Hide();

            Form11 form11 = new Form11();
            form11.Show();
        }
        private void button9_Click(object sender, EventArgs e)
        {


            string dbPath = Properties.Settings.Default.DatabasePath;

            if (string.IsNullOrWhiteSpace(dbPath) || !System.IO.File.Exists(dbPath))
            {
                MessageBox.Show("لطفاً ابتدا دیتابیس خود را انتخاب کنید ❌",
                                "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Hide();

            Form9 form9 = new Form9();
            form9.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("آیا مطمئن هستید که می‌خواهید خارج شوید؟",
                                                "خروج از برنامه",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
