using System;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form8 : Form
    {
        private string connStr;

        public Form8(string connStr)
        {
            InitializeComponent();
            this.connStr = connStr;

            this.StartPosition = FormStartPosition.CenterParent;

            comboAssetnme.SelectedIndexChanged += new EventHandler(ValidateForm);
            textInumb.TextChanged += new EventHandler(ValidateForm);
            textSmodel.TextChanged += new EventHandler(ValidateForm);
            textSserial.TextChanged += new EventHandler(ValidateForm);

        }

        private void ValidateForm(object sender, EventArgs e)
        {
            bool isValid = true;
            errorProvider1.Clear();

            if (comboAssetnme.SelectedItem == null)
            {
                errorProvider1.SetError(comboAssetnme, "لطفاً نام دستگاه را انتخاب کنید.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(textInumb.Text) || !Regex.IsMatch(textInumb.Text, @"^\d+$"))
            {
                errorProvider1.SetError(textInumb, "شماره دارایی فقط عدد و اجباری است.");
                isValid = false;
            }
            else if (IsInumbDuplicate(textInumb.Text))
            {
                errorProvider1.SetError(textInumb, "دستگاه با این شماه دارایی قبلا ثبت شده است.");
                isValid = false;
            }
            if (!Regex.IsMatch(textSserial.Text, @"^[A-Za-z0-9 ]+$"))
            {
                errorProvider1.SetError(textSserial, "فقط حروف و اعداد انگلیسی مجاز است.");
                isValid = false;
            }
            else if (IsSerialExists(textSserial.Text))
            {
                errorProvider1.SetError(textSserial, "دستگاه با این شماه سریال قبلا ثبت شده است.");
                isValid = false;
            }
            if (!Regex.IsMatch(textSmodel.Text, @"^[A-Za-z0-9 ]+$"))
            {
                errorProvider1.SetError(textSmodel, "فقط حروف و اعداد انگلیسی مجاز است.");
                isValid = false;
            }

            button1.Enabled = isValid;
        }
        private bool IsInumbDuplicate(string inumb)
        {
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Asset WHERE Inumb = ?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("?", OleDbType.Integer).Value = Convert.ToInt32(inumb);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        private bool IsSerialExists(string sserial)
        {
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Asset WHERE Sserial = ?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("?", OleDbType.VarChar).Value = sserial;
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        private int GetAssetCode(string deviceName)
        {
            if (deviceName == "Case") return 1;
            if (deviceName == "Keyboard") return 2;
            if (deviceName == "Speaker") return 3;
            if (deviceName == "Mouse") return 4;
            if (deviceName == "Monitor") return 5;
            return 0;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string selectedDevice = comboAssetnme.SelectedItem.ToString();
            int assetCode = GetAssetCode(selectedDevice);
            string snme = selectedDevice;
            int inumb = Convert.ToInt32(textInumb.Text);
            string smodel = textSmodel.Text;
            string sserial = textSserial.Text;

            DialogResult dr = MessageBox.Show(
                "آیا از ثبت اطلاعات زیر مطمئن هستید؟\n\n" +
                "نام دستگاه: " + snme + "\n" +
                "شماره دارایی: " + inumb + "\n" +
                "مدل دستگاه: " + smodel + "\n" +
                "شماره سریال: " + sserial,
                "تأیید ثبت",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr == DialogResult.No) return;

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
                string insertQuery = "INSERT INTO Asset " +
                    "(Iassetcode, Snme, Inumb, Sserial, Smodel, Splak, Iusercode, Sactivedate, Bstatus) " +
                    "VALUES (?, ?, ?, ?, ?, NULL, NULL, NULL, 0)";

                using (OleDbCommand cmd = new OleDbCommand(insertQuery, conn))
                {
                    cmd.Parameters.Add("?", OleDbType.Integer).Value = assetCode;
                    cmd.Parameters.Add("?", OleDbType.VarChar).Value = snme;
                    cmd.Parameters.Add("?", OleDbType.Integer).Value = inumb;
                    cmd.Parameters.Add("?", OleDbType.VarChar).Value = sserial;
                    cmd.Parameters.Add("?", OleDbType.VarChar).Value = smodel;

                    cmd.ExecuteNonQuery();
                }
            }

           
            DialogResult drActivate = MessageBox.Show(
                 "آیا مایل به فعال‌سازی دستگاه هستید؟",
                 "فعال‌سازی",
                    MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question
                        );

            if (drActivate == DialogResult.Yes)
            {
                Form5 f5 = new Form5(connStr, assetCode, inumb, snme, sserial, smodel, null, null, null);
                f5.ShowDialog(this);

                if (f5.DialogResult == DialogResult.OK)
                {
                    MessageBox.Show("فعال‌سازی دستگاه انجام شد ✅", "موفقیت",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("دستگاه با موفقیت اضافه گردید ✅", "موفقیت",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }



            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
