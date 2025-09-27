using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;
using System.Data;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string connectionString;
        private Dictionary<int, string> jobCategories04;


        public Form1()
        {
            InitializeComponent();

            dgvData.DefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Regular);
            dgvData.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Bold);
            dgvData.RowHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Italic);

            button1.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            dgvData.BackgroundColor = Color.White;

            dgvData.DefaultCellStyle.BackColor = Color.White;
            dgvData.DefaultCellStyle.ForeColor = Color.Black;
            dgvData.DefaultCellStyle.SelectionBackColor = Color.DarkOrange;
            dgvData.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvData.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgvData.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvData.EnableHeadersVisualStyles = false; // لازمه که رنگ هدر عمل کنه



            errorProvider1.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            errorProvider1.BlinkRate = 250;

            string dbPath = Properties.Settings.Default.DatabasePath;
            connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";


            List<ComboBoxItem> departments = new List<ComboBoxItem>()
            {
                new ComboBoxItem { Text = "امور اداری", Value = 101 },
                new ComboBoxItem { Text = "امور مالی", Value = 102 },
                new ComboBoxItem { Text = "فناوری اطلاعات", Value = 103 },
                new ComboBoxItem { Text = "تدارکات", Value = 104 },
                new ComboBoxItem { Text = "امور حقوقی", Value = 105 },
                new ComboBoxItem { Text = "پژوهش و توسعه", Value = 106 },
                new ComboBoxItem { Text = "بازرگانی", Value = 107 },
                new ComboBoxItem { Text = "آموزش", Value = 108 },
                new ComboBoxItem { Text = "پشتیبانی فنی", Value = 109 },
                new ComboBoxItem { Text = "امور عمومی", Value = 110 }
            };
            comboBoxDepartment.DataSource = departments;
            comboBoxDepartment.DisplayMember = "Text";
            comboBoxDepartment.ValueMember = "Value";
            comboBoxDepartment.SelectedIndex = -1;

            List<ComboBoxItem> jobCategories = new List<ComboBoxItem>()
            {
                 new ComboBoxItem { Text = "مدیر", Value = 1 },
                 new ComboBoxItem { Text = "معاون", Value = 2 },
                 new ComboBoxItem { Text = "کارمند", Value = 3 }
            };
            jobCategories04 = new Dictionary<int, string>()
            {
                { 1, "مدیر" },
                { 2, "معاون" },
                { 3, "کارمند" },
            };

            dgvData.RightToLeft = RightToLeft.Yes;

            comboBoxJob.DataSource = jobCategories;
            comboBoxJob.DisplayMember = "Text";
            comboBoxJob.ValueMember = "Value";
            comboBoxJob.SelectedIndex = -1;

            LoadData();


            textBox1.TextChanged += ValidateForm;
            textBox2.TextChanged += ValidateForm;
            textBox3.TextChanged += ValidateForm;
            textBox5.TextChanged += ValidateForm;
            dateTimePicker1.ValueChanged += ValidateForm;
            dateTimePicker2.ValueChanged += ValidateForm;
            comboBoxDepartment.SelectedIndexChanged += ValidateForm;
            comboBoxJob.SelectedIndexChanged += ValidateForm;




        }



        public class ComboBoxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
            public override string ToString() { return Text; }
        }

        private void ValidateForm(object sender, EventArgs e)
        {
            button1.Enabled = IsFormValid();
        }

        private bool IsFormValid()
        {

            bool isValid = true;
            errorProvider1.Clear();

            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                !Regex.IsMatch(textBox1.Text, @"^[\u0600-\u06FF\s]+$"))
            {
                errorProvider1.SetError(textBox1, "لطفاً نام معتبر وارد کنید (فقط حروف فارسی)");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text) ||
                !Regex.IsMatch(textBox2.Text, @"^[\u0600-\u06FF\s]+$"))
            {
                errorProvider1.SetError(textBox2, "لطفاً نام خانوادگی معتبر وارد کنید (فقط حروف فارسی)");
                isValid = false;
            }

            if (!Regex.IsMatch(textBox3.Text, @"^\d{10}$"))
            {
                errorProvider1.SetError(textBox3, "کد ملی باید ۱۰ رقم باشد");
                isValid = false;
            }
            if (!IsValidIranianNationalCode(textBox3.Text))
             {
              errorProvider1.SetError(textBox3, "کد ملی نامعتبر است");
              isValid = false;
             }
            if (!Regex.IsMatch(textBox5.Text, @"^\d+$"))
            {
              errorProvider1.SetError(textBox5, "کد پرسنلی باید فقط شامل عدد باشد");
              isValid = false;
            }


            if (comboBoxDepartment.SelectedItem == null)
            {
                errorProvider1.SetError(comboBoxDepartment, "لطفاً یک اداره انتخاب کنید");
                isValid = false;
            }

            if (comboBoxJob.SelectedItem == null)
            {
                errorProvider1.SetError(comboBoxJob, "لطفاً دسته شغلی را انتخاب کنید");
                isValid = false;
            }

            DateTime birthDate = dateTimePicker1.Value;
            int age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.Date < birthDate.AddYears(age)) age--;
            if (age < 18)
            {
                errorProvider1.SetError(dateTimePicker1, "سن کاربر باید حداقل ۱۸ سال باشد");
                isValid = false;
            }

            return isValid;
        }
        //کد ملی با الگوریتم
        public bool IsValidIranianNationalCode(string nationalCode)
        {
            if (string.IsNullOrWhiteSpace(nationalCode))
                return false;

            if (!Regex.IsMatch(nationalCode, @"^\d{10}$"))
                return false;

            if (new string(nationalCode[0], 10) == nationalCode)
                return false;

            int checkDigit = int.Parse(nationalCode[9].ToString());
            int sum = 0;

            for (int i = 0; i < 9; i++)
            {
                sum += int.Parse(nationalCode[i].ToString()) * (10 - i);
            }

            int remainder = sum % 11;

            return (remainder < 2 && checkDigit == remainder) ||
                   (remainder >= 2 && checkDigit == (11 - remainder));
        }*/


        /*private bool CheckDuplicate(string inid, int iusercode)
        {
            string query = @"
            SELECT COUNT(*) 
            FROM Users 
            WHERE Inid = ? OR Iusercode = ?";
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("?", OleDbType.VarChar).Value = inid;
                        //cmd.Parameters.Add("?", OleDbType.Integer).Value = iusercode;

                        object result = cmd.ExecuteScalar();
                        int count = Convert.ToInt32(result);
                        return count > 0;
                    }
                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show("خطا در بررسی تکراری بودن: " + ex.Message);
                return true;
            }

            string query2 = @"SELECT COUNT(*) 
            FROM Users 
            WHERE Inid = ? OR Iusercode = ?;

            SELECT COUNT(*) 
            FROM UsersHis 
            WHERE Inid = ? OR Iusercode = ?";
            try
            {

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query2, conn))
                    {
                        //cmd.Parameters.Add("?", OleDbType.VarChar).Value = inid;
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = iusercode;

                        object result = cmd.ExecuteScalar();
                        int count = Convert.ToInt32(result);
                        return count > 0;
                    }
                }
            }
            catch(Exception ex)
            {

                MessageBox.Show("خطا در بررسی تکراری بودن: " + ex.Message);
                return true;
            }
        }
        private bool CheckDuplicate(string inid, int iusercode)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();

                    string queryInid = "SELECT COUNT(*) FROM Users WHERE Inid = ?";
                    using (OleDbCommand cmd = new OleDbCommand(queryInid, conn))
                    {
                        cmd.Parameters.Add("?", OleDbType.VarChar).Value = inid;
                        int countInid = Convert.ToInt32(cmd.ExecuteScalar());
                        if (countInid > 0)
                            return true;
                    }

                    string queryUser = "SELECT COUNT(*) FROM Users WHERE Iusercode = ?";
                    using (OleDbCommand cmd = new OleDbCommand(queryUser, conn))
                    {
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = iusercode;
                        int countUser = Convert.ToInt32(cmd.ExecuteScalar());
                        if (countUser > 0)
                            return true;
                    }

                    string queryUserHis = "SELECT COUNT(*) FROM UsersHis WHERE Iusercode = ?";
                    using (OleDbCommand cmd = new OleDbCommand(queryUserHis, conn))
                    {
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = iusercode;
                        int countUserHis = Convert.ToInt32(cmd.ExecuteScalar());
                        if (countUserHis > 0)
                            return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در بررسی تکراری بودن: " + ex.Message);
                return true;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string firstName = textBox1.Text.Trim();
            string lastName = textBox2.Text.Trim();
            string nationalCode = textBox3.Text.Trim();
            string personnelCode = textBox5.Text.Trim();

            if (!IsFormValid())
            {
                MessageBox.Show("لطفاً فیلدها را به‌درستی پر کنید ❌");
                return;
            }

            int parsedPersonnelCode = int.Parse(personnelCode);

            if (CheckDuplicate(nationalCode, parsedPersonnelCode))
            {
                MessageBox.Show("کد ملی یا کد پرسنلی قبلاً ثبت شده است ❌");
                return;
            }

            PersianCalendar pc = new PersianCalendar();

            string birthDateShamsi = string.Format("{0:0000}-{1:00}-{2:00}",
                pc.GetYear(dateTimePicker1.Value),
                pc.GetMonth(dateTimePicker1.Value),
                pc.GetDayOfMonth(dateTimePicker1.Value));

            string startWorkShamsi = string.Format("{0:0000}-{1:00}-{2:00}",
                pc.GetYear(dateTimePicker2.Value),
                pc.GetMonth(dateTimePicker2.Value),
                pc.GetDayOfMonth(dateTimePicker2.Value));

            int yearsOfWork = DateTime.Now.Year - dateTimePicker2.Value.Year;
            if (DateTime.Now.DayOfYear < dateTimePicker2.Value.DayOfYear)
                yearsOfWork--;

            ComboBoxItem departmentItem = comboBoxDepartment.SelectedItem as ComboBoxItem;
            int departmentCode = departmentItem.Value;

            ComboBoxItem jobItem = comboBoxJob.SelectedItem as ComboBoxItem;
            int jobTypeValue = jobItem != null ? jobItem.Value : 0;

            string query = "INSERT INTO Users (Snme, Sfnme, Inid, Sbirthdate, Iusercode, Idpcode, Sworkdate, Iexrp, Itype) " +
                           "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", firstName);
                        cmd.Parameters.AddWithValue("?", lastName);
                        cmd.Parameters.AddWithValue("?", nationalCode);
                        cmd.Parameters.AddWithValue("?", birthDateShamsi);
                        cmd.Parameters.AddWithValue("?", parsedPersonnelCode);
                        cmd.Parameters.AddWithValue("?", departmentCode);
                        cmd.Parameters.AddWithValue("?", startWorkShamsi);
                        cmd.Parameters.AddWithValue("?", yearsOfWork);
                        cmd.Parameters.AddWithValue("?", jobTypeValue);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("اطلاعات با موفقیت ذخیره شد ✅");

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox5.Clear();
                comboBoxDepartment.SelectedIndex = -1;
                comboBoxJob.SelectedIndex = -1;
                button1.Enabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در ذخیره اطلاعات: " + ex.Message);
            }
            LoadData();

            if (dgvData.Rows.Count > 0)
            {
                int lastRowIndex = dgvData.Rows.Count - 1;
                dgvData.ClearSelection();
                dgvData.Rows[lastRowIndex].Selected = true;
                dgvData.FirstDisplayedScrollingRowIndex = lastRowIndex;
            }

        }

        private void SetupGrid()
        {
            dgvData.Columns.Clear();
            dgvData.AutoGenerateColumns = false;   

            dgvData.Columns.Add(new DataGridViewTextBoxColumn { Name = "FirstName", HeaderText = "نام", DataPropertyName = "FirstName" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { Name = "LastName", HeaderText = "نام خانوادگی", DataPropertyName = "LastName" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { Name = "NationalCode", HeaderText = "کد ملی", DataPropertyName = "NationalCode" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { Name = "BirthDate", HeaderText = "تاریخ تولد", DataPropertyName = "BirthDate" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { Name = "PersonalCode", HeaderText = "کد پرسنلی", DataPropertyName = "PersonalCode" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { Name = "StartDate", HeaderText = "تاریخ اشتغال", DataPropertyName = "StartDate" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { Name = "Years", HeaderText = "سنوات", DataPropertyName = "Years" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { Name = "Department", HeaderText = "نام اداره", DataPropertyName = "Department" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { Name = "JobCategory", HeaderText = "دسته شغلی", DataPropertyName = "JobCategory" });
        }

        private void LoadData()
        {
            try
            {
                string dbPath = Properties.Settings.Default.DatabasePath;

                if (string.IsNullOrEmpty(dbPath))
                {
                    MessageBox.Show("مسیر دیتابیس تعریف نشده است.");
                    return;
                }

                if (dgvData == null)
                {
                    MessageBox.Show("DataGridView مقداردهی نشده است.");
                    return;
                }

                if (jobCategories04 == null)
                {
                    MessageBox.Show("لیست دسته‌های شغلی مقداردهی نشده است.");
                    return;
                }

                string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

                using (var localCon = new OleDbConnection(connStr))
                using (var da = new OleDbDataAdapter(
                    @"SELECT 
                U.Snme,
                U.Sfnme,
                U.Inid,
                U.Sbirthdate,
                U.Iusercode,
                U.Idpcode,
                D.Snme AS DepartmentName,
                U.Sworkdate,
                U.Iexrp,
                U.Itype
            FROM Users AS U
            LEFT JOIN Departmants AS D ON U.Idpcode = D.Idpcode"
                    , localCon))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dt.Columns.Add("JobCategoryName", typeof(string));

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Itype"] != DBNull.Value)
                        {
                            int code;
                            if (int.TryParse(row["Itype"].ToString(), out code) && jobCategories04.ContainsKey(code))
                            {
                                row["JobCategoryName"] = jobCategories04[code];
                            }
                        }
                    }

                    dgvData.AutoGenerateColumns = true;
                    dgvData.DataSource = dt;

                    dgvData.Columns["Snme"].HeaderText = "نام";
                    dgvData.Columns["Sfnme"].HeaderText = "نام خانوادگی";
                    dgvData.Columns["Inid"].HeaderText = "کد ملی";
                    dgvData.Columns["Sbirthdate"].HeaderText = "تاریخ تولد";
                    dgvData.Columns["Iusercode"].HeaderText = "کد پرسنلی";
                    dgvData.Columns["Idpcode"].Visible = false;
                    dgvData.Columns["DepartmentName"].HeaderText = "نام اداره";
                    dgvData.Columns["Sworkdate"].HeaderText = "تاریخ اشتغال";
                    dgvData.Columns["Iexrp"].HeaderText = "سنوات";
                    dgvData.Columns["Itype"].Visible = false;
                    dgvData.Columns["JobCategoryName"].HeaderText = "دسته شغلی";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در بارگذاری داده‌ها: " + ex.Message);
            }
        }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = dateTimePicker2.Value.Date;
                DateTime today = DateTime.Today;

                int totalMonths = (today.Year - startDate.Year) * 12 + (today.Month - startDate.Month);
                int years = totalMonths / 12;

                textBox4.Text = $"{years}";
            }
            catch
            {
                textBox4.Text = "تاریخ معتبر نیست";
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("آیا می‌خواهید به مدیریت کاربران برگردید؟",
                                                     "بازگشت",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                this.Hide();
                Form4 form4 = new Form4();
                form4.Show();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime birthDate = dateTimePicker1.Value;
            int age = DateTime.Now.Year - birthDate.Year;

            if (DateTime.Now.Date < birthDate.AddYears(age))
            {
                age--;
            }

            if (age < 18)
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("آیا می‌خواهید به مدیریت کاربران برگردید؟",
                                                        "بازگشت",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                this.Hide();
                Form4 form4 = new Form4();
                form4.Show();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}
