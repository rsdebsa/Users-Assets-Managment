using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using System.Text.RegularExpressions;




namespace WindowsFormsApplication1
{

    public partial class Form4 : Form
    {
        private DataTable dtData;
        private Dictionary<int, string> jobCategories;

        private bool _isEditMode = false;
        private int? _editingUserCode = null;



        public Form4()
        {


            InitializeComponent();

            dgvData.DefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Regular);

            dgvData.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Bold);

            dgvData.RowHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Italic);

            this.Load += Form4_Load;
            dgvData.MouseEnter += (s, e) => { if (dgvData.Enabled) dgvData.Focus(); };
            dgvData.MouseDown += (s, e) => dgvData.Focus();


            txtSearch.Enter += textBox1_Enter;
            txtSearch.Leave += textBox1_Leave;
            txtSearch.KeyDown += textBox1_KeyDown;
            comboBoxSort.SelectedIndexChanged += comboBoxSort_SelectedIndexChanged;
            dgvData.CellClick += dgvData_CellClick;
        }

        private void ClearFields()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtNationalCode.Clear();
            txtEmployeeCode.Clear();
            txtExperience.Clear();
            dtpBirthDate.Value = DateTime.Today;
            dtpStartDate.Value = DateTime.Today;
            cmbDepartment.SelectedIndex = -1;
        }

         public static string GetPersianDate(DateTime date)
            {
                PersianCalendar pc = new PersianCalendar();
                return $"{pc.GetYear(date):0000}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}";
            }

    private void SetGroupBoxEnabled(bool enabled)
        {
            foreach (Control ctrl in groupBox1.Controls)
            {
                if (ctrl is TextBox) ((TextBox)ctrl).ReadOnly = !enabled;
                else if (ctrl is ComboBox || ctrl is DateTimePicker || ctrl is NumericUpDown)
                    ctrl.Enabled = enabled;
            }
        }

        private void SetEditMode(bool on)
        {
            _isEditMode = on;
            SetGroupBoxEnabled(on);
            dgvData.Enabled = !on;
        }
        private int CalculateExperience(DateTime startDate)
        {
            DateTime today = DateTime.Today;
            int years = today.Year - startDate.Year;

            if (startDate.Date > today.AddYears(-years))
                years--;

            return years < 0 ? 0 : years;
        }


        private void Form4_Load(object sender, EventArgs e)
        {

            dgvData.BackgroundColor = Color.White;

            dgvData.DefaultCellStyle.BackColor = Color.White;
            dgvData.DefaultCellStyle.ForeColor = Color.Black;      
            dgvData.DefaultCellStyle.SelectionBackColor = Color.DarkOrange;
            dgvData.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvData.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgvData.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

            dgvData.EnableHeadersVisualStyles = false;

            txtFirstName.Enabled = false;
            txtLastName.Enabled = false;
            txtNationalCode.Enabled = false;
            txtExperience.Enabled = false;
            txtExperience.ReadOnly = true;
            txtEmployeeCode.Enabled = false;




            dgvData.RightToLeft = RightToLeft.Yes;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            SetGroupBoxEnabled(false);
            SetEditMode(false);

            SetPlaceholder(txtSearch, "کد ملی یا کد پرسنلی یا نام را وارد کنید");

            string dbPath = Properties.Settings.Default.DatabasePath;
            if (string.IsNullOrEmpty(dbPath))
            {
                MessageBox.Show("مسیر دیتابیس مشخص نشده است.");
                return;
            }
            jobCategories = new Dictionary<int, string>()
            {
                { 1, "مدیر" },
                { 2, "معاون" },
                { 3, "کارمند" },
            };

            cmbJobCategory.DataSource = jobCategories.ToList();
            cmbJobCategory.DisplayMember = "Value";
            cmbJobCategory.ValueMember = "Key";
            cmbJobCategory.SelectedIndex = -1;

            SetupDataGridView();
            LoadData();
            LoadDepartments();

            cmbJobCategory.DataSource = jobCategories.ToList();
            cmbJobCategory.DisplayMember = "Value";
            cmbJobCategory.ValueMember = "Key";
            cmbJobCategory.SelectedIndex = -1;



            comboBoxSort.Items.Clear();
            comboBoxSort.Items.AddRange(new string[]
            {
                "نام خانوادگی : به ترتیب حروف الفبا",
                "سنوات : بیشترین به کمترین",
                "سنوات : کمترین به بیشترین",
                "سن : از کمترین به بیشترین",
                "سن : از بیشترین به کمترین"
            });
            comboBoxSort.SelectedIndex = 0;
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "کد ملی یا کد پرسنلی را وارد کنید";
                txtSearch.ForeColor = Color.Gray;
            }
        }
        private void SetupDataGridView()
        {
            dgvData.AutoGenerateColumns = false;
            dgvData.Columns.Clear();

            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Snme", Name = "Snme", HeaderText = "نام" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Sfnme", Name = "Sfnme", HeaderText = "نام خانوادگی" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Inid", Name = "Inid", HeaderText = "کد ملی" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Sbirthdate", Name = "Sbirthdate", HeaderText = "تاریخ تولد" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Iusercode", Name = "Iusercode", HeaderText = "کد پرسنلی" }); // تایپی قبلی اصلاح شد
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DepartmentName", Name = "DepartmentName", HeaderText = "نام اداره" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Sworkdate", Name = "Sworkdate", HeaderText = "تاریخ اشتغال" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Iexrp", Name = "Iexrp", HeaderText = "سنوات" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "JobCategory", Name = "JobCategory", HeaderText = "دسته شغلی" });

            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Idpcode", Name = "Idpcode", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Itype", Name = "Itype", Visible = false });
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string searchValue = txtSearch.Text.Trim();

                if (string.IsNullOrEmpty(searchValue) || searchValue == "کد ملی یا کد پرسنلی یا نام را وارد کنید")
                    return;

                int num;
                bool isNumber = int.TryParse(searchValue, out num);
                IEnumerable<DataRow> rows;

                if (isNumber)
                {
                    rows = dtData.AsEnumerable().Where(r =>
                    {
                        int inidValue, userCodeValue;
                        bool match = false;

                        if (int.TryParse(r["Inid"]?.ToString(), out inidValue) && inidValue == num)
                            match = true;

                        if (int.TryParse(r["Iusercode"]?.ToString(), out userCodeValue) && userCodeValue == num)
                            match = true;

                        return match;
                    });
                }
                else
                {
                    rows = dtData.AsEnumerable().Where(r =>
                    {
                        string name = r["Snme"]?.ToString();
                        string family = r["Sfnme"]?.ToString();

                        return (!string.IsNullOrEmpty(name) && name.Contains(searchValue)) ||
                               (!string.IsNullOrEmpty(family) && family.Contains(searchValue));
                    });
                }

                if (rows.Any())
                    dgvData.DataSource = rows.CopyToDataTable();
                else
                {
                    MessageBox.Show("کاربر یافت نشد.");
                    dgvData.DataSource = null;
                }
                if (e.KeyCode == Keys.Down && dgvData.Rows.Count > 0)
                {
                    dgvData.Focus();
                    int i = dgvData.CurrentCell?.RowIndex ?? -1;
                    int next = Math.Min(i + 1, dgvData.Rows.Count - 1);
                    if (next >= 0) dgvData.CurrentCell = dgvData.Rows[next].Cells[0];
                }
                if (e.KeyCode == Keys.Up && dgvData.Rows.Count > 0)
                {
                    dgvData.Focus();
                    int i = dgvData.CurrentCell?.RowIndex ?? dgvData.Rows.Count;
                    int prev = Math.Max(i - 1, 0);
                    dgvData.CurrentCell = dgvData.Rows[prev].Cells[0];
                }


            }
        }

        private bool IsFormValid()
        {

            bool isValid = true;
            errorProvider1.Clear();

            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                !Regex.IsMatch(txtFirstName.Text, @"^[\u0600-\u06FF\s]+$"))
            {
                errorProvider1.SetError(txtFirstName, "لطفاً نام معتبر وارد کنید (فقط حروف فارسی)");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                !Regex.IsMatch(txtLastName.Text, @"^[\u0600-\u06FF\s]+$"))
            {
                errorProvider1.SetError(txtLastName, "لطفاً نام خانوادگی معتبر وارد کنید (فقط حروف فارسی)");
                isValid = false;
            }

   
            if (!IsValidIranianNationalCode(txtNationalCode.Text))
            {
                errorProvider1.SetError(txtNationalCode, "کد ملی نامعتبر است");
                isValid = false;
            }
            if (!Regex.IsMatch(txtEmployeeCode.Text, @"^\d+$"))
            {
                errorProvider1.SetError(txtEmployeeCode, "کد پرسنلی باید فقط شامل عدد باشد");
                isValid = false;
            }


            if (cmbDepartment.SelectedItem == null)
            {
                errorProvider1.SetError(cmbDepartment, "لطفاً یک اداره انتخاب کنید");
                isValid = false;
            }

            if (cmbJobCategory.SelectedItem == null)
            {
                errorProvider1.SetError(cmbJobCategory, "لطفاً دسته شغلی را انتخاب کنید");
                isValid = false;
            }

            DateTime birthDate = dtpBirthDate.Value;
            int age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.Date < birthDate.AddYears(age)) age--;
            if (age < 18)
            {
                errorProvider1.SetError(dtpBirthDate, "سن کاربر باید حداقل ۱۸ سال باشد");
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
        }


        private bool CheckDuplicate(string inid, int iusercode)
        {
            string dbPath = Properties.Settings.Default.DatabasePath;
            string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

            string query = "SELECT COUNT(*) FROM Users WHERE Inid = ? OR Iusercode = ?";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("?", OleDbType.VarChar).Value = inid;
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = iusercode;

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
        }


        private void SetPlaceholder(TextBox txt, string placeholder)
        {
            txt.Text = placeholder;
            txt.ForeColor = Color.Gray;

            txt.GotFocus += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };

            txt.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                }
            };
        }




        private void LoadData(string searchText = "")
        {
            try
            {
                string dbPath = Properties.Settings.Default.DatabasePath;
                string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    conn.Open();

                    string sql = @"
                SELECT 
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
                LEFT JOIN Departmants AS D ON U.Idpcode = D.Idpcode";

                    OleDbCommand cmd;

                    if (string.IsNullOrWhiteSpace(searchText))
                    {
                        cmd = new OleDbCommand(sql, conn);
                    }
                    else
                    {
                        sql += " WHERE U.Snme LIKE ? OR U.Sfnme LIKE ? OR U.Inid LIKE ? OR U.Iusercode LIKE ?";
                        cmd = new OleDbCommand(sql, conn);
                        cmd.Parameters.AddWithValue("?", searchText + "%");
                        cmd.Parameters.AddWithValue("?", searchText + "%");
                        cmd.Parameters.AddWithValue("?", searchText + "%");
                        cmd.Parameters.AddWithValue("?", searchText + "%");
                    }

                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    dtData = new DataTable();
                    da.Fill(dtData);

                    if (!dtData.Columns.Contains("JobCategory"))
                        dtData.Columns.Add("JobCategory", typeof(string));

                    foreach (DataRow r in dtData.Rows)
                    {
                        if (r["Itype"] != DBNull.Value)
                        {
                            int code;
                            string title;

                            if (int.TryParse(r["Itype"].ToString(), out code) &&
                                jobCategories.TryGetValue(code, out title))
                            {
                                r["JobCategory"] = title;
                            }
                        }
                    }

                    dgvData.DataSource = dtData;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در بارگذاری داده‌ها: " + ex.Message);
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "کد ملی یا کد پرسنلی یا نام را وارد کنید")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }



 


        private void comboBoxSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dtData == null || dtData.Rows.Count == 0) return;

            DataView dv = dtData.DefaultView;
            switch (comboBoxSort.SelectedIndex)
            {
                case 0: dv.Sort = "Sfnme ASC"; break;
                case 1: dv.Sort = "Iexrp DESC"; break;
                case 2: dv.Sort = "Iexrp ASC"; break;
                case 3: dv.Sort = "Sbirthdate DESC"; break;
                case 4: dv.Sort = "Sbirthdate ASC"; break;
            }
            dgvData.DataSource = dv.ToTable();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_isEditMode)
            {
                MessageBox.Show("در حال ویرایش هستید. لطفاً ابتدا ذخیره یا انصراف کنید.");
                if (_editingUserCode.HasValue)
                {
                    foreach (DataGridViewRow r in dgvData.Rows)
                    {
                        if (r.Cells["Iusercode"].Value != null &&
                            Convert.ToInt32(r.Cells["Iusercode"].Value) == _editingUserCode.Value)
                        {
                            dgvData.CurrentCell = r.Cells[0];
                            r.Selected = true;
                            break;
                        }
                    }
                }
                return;
            }


            if (e.RowIndex >= 0 && dgvData.CurrentRow != null)
            {
                DataGridViewRow row = dgvData.Rows[e.RowIndex];

                txtFirstName.Text = row.Cells["Snme"].Value?.ToString();
                txtLastName.Text = row.Cells["Sfnme"].Value?.ToString();
                txtNationalCode.Text = row.Cells["Inid"].Value?.ToString();
                txtEmployeeCode.Text = row.Cells["Iusercode"].Value?.ToString();
                txtExperience.Text = row.Cells["Iexrp"].Value?.ToString();

                DateTime birthDate;
                if (DateTime.TryParse(row.Cells["Sbirthdate"].Value?.ToString(), out birthDate))
                    dtpBirthDate.Value = birthDate;
                else
                    dtpBirthDate.Value = DateTime.Today;

                DateTime startDate;
                if (DateTime.TryParse(row.Cells["Sworkdate"].Value?.ToString(), out startDate))
                {
                    dtpStartDate.Value = startDate;
                    txtExperience.Text = CalculateExperience(startDate).ToString(); // اینجا سنوات نشون داده میشه
                }
                else
                {
                    dtpStartDate.Value = DateTime.Today;
                    txtExperience.Text = "0";
                }



                if (row.Cells["Idpcode"].Value != DBNull.Value)
                    cmbDepartment.SelectedValue = Convert.ToInt32(row.Cells["Idpcode"].Value);
                else
                    cmbDepartment.SelectedIndex = -1;

                if (row.Cells["Itype"].Value != DBNull.Value)
                {
                    int code = Convert.ToInt32(row.Cells["Itype"].Value);
                    cmbJobCategory.SelectedValue = code;
                }

          
                else
                {
                    cmbJobCategory.SelectedIndex = -1;
                }
            }
        }

        private void LoadDepartments()
        {
            string dbPath = Properties.Settings.Default.DatabasePath;
            string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

            try
            {
                using (var localCon = new OleDbConnection(connStr))
                using (var da = new OleDbDataAdapter("SELECT Idpcode, Snme FROM Departmants", localCon))
                {
                    DataTable dtDept = new DataTable();
                    da.Fill(dtDept);

                    cmbDepartment.DataSource = dtDept;
                    cmbDepartment.DisplayMember = "Snme";
                    cmbDepartment.ValueMember = "Idpcode";
                    cmbDepartment.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در بارگذاری ادارات: " + ex.Message);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 form3 = new Form3();
            form3.Show();
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            if (_isEditMode)
            {
                DialogResult dr = MessageBox.Show("آیا می‌خواهید از حالت ویرایش خارج شوید؟",
                                                  "خروج از ویرایش",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    SetEditMode(false);
                    _isEditMode = false;
                }
            }
            else
            {
                DialogResult dr = MessageBox.Show("آیا می‌خواهید به منوی اصلی برگردید؟",
                                                  "بازگشت به فرم اصلی",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    this.Hide();
                    Form3 form3 = new Form3();
                    form3.Show();
                }
            }
        }


        int selectedUserCode = -1;

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0)
            {
                MessageBox.Show("لطفاً یک ردیف را برای ویرایش انتخاب کنید");
                return;
            }

            DataGridViewRow row = dgvData.SelectedRows[0];

            if (row.Cells["Iusercode"].Value == null || !int.TryParse(row.Cells["Iusercode"].Value.ToString(), out selectedUserCode))
            {
                MessageBox.Show("کد پرسنلی معتبر یافت نشد");
                return;
            }

            txtFirstName.Enabled = txtLastName.Enabled = txtNationalCode.Enabled = txtEmployeeCode.Enabled = true;

            txtFirstName.Text = row.Cells["Snme"].Value?.ToString();
            txtLastName.Text = row.Cells["Sfnme"].Value?.ToString();
            txtNationalCode.Text = row.Cells["Inid"].Value?.ToString();
            txtEmployeeCode.Text = row.Cells["Iusercode"].Value?.ToString();
            txtExperience.Text = row.Cells["Iexrp"].Value?.ToString();

            DateTime birthDate;
            if (DateTime.TryParse(row.Cells["Sbirthdate"].Value?.ToString(), out birthDate))
                dtpBirthDate.Value = birthDate;
            else
                dtpBirthDate.Value = DateTime.Today;

            DateTime startDate;
            if (DateTime.TryParse(row.Cells["Sworkdate"].Value?.ToString(), out startDate))
            {
                dtpStartDate.Value = startDate;
                txtExperience.Text = CalculateExperience(startDate).ToString();
            }
            else
            {
                dtpStartDate.Value = DateTime.Today;
                txtExperience.Text = "0";
            }
            int dep;
            if (row.Cells["Idpcode"].Value != DBNull.Value && int.TryParse(row.Cells["Idpcode"].Value.ToString(), out dep))
                cmbDepartment.SelectedValue = dep;
            else
                cmbDepartment.SelectedIndex = -1;

            int type;
            if (row.Cells["Itype"].Value != DBNull.Value && int.TryParse(row.Cells["Itype"].Value.ToString(), out type))
                cmbJobCategory.SelectedValue = type;
            else
                cmbJobCategory.SelectedIndex = -1;


            _isEditMode = true;
            _editingUserCode = selectedUserCode;

            groupBox1.Enabled = true;
            SetEditMode(true);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                MessageBox.Show("لطفاً خطاهای مشخص شده را برطرف کنید ⚠️");
                return;
            }
            if (selectedUserCode == -1)
            {
                MessageBox.Show("لطفاً ابتدا ردیفی را برای ویرایش انتخاب کنید");
                return;
            }

            DialogResult dr = MessageBox.Show("آیا از ذخیره تغییرات مطمئن هستید؟",
                                              "تأیید ذخیره",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Question);

            if (dr == DialogResult.No) return;

            DateTime startDate = dtpStartDate.Value;
            txtExperience.Text = CalculateExperience(startDate).ToString();




            try
            {
                string dbPath = Properties.Settings.Default.DatabasePath;
                string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    PersianCalendar pc = new PersianCalendar();

                    string birthDateShamsi = string.Format("{0:0000}/{1:00}/{2:00}",
                        pc.GetYear(dtpBirthDate.Value),
                        pc.GetMonth(dtpBirthDate.Value),
                        pc.GetDayOfMonth(dtpBirthDate.Value));

                    string workDateShamsi = string.Format("{0:0000}/{1:00}/{2:00}",
                        pc.GetYear(dtpStartDate.Value),
                        pc.GetMonth(dtpStartDate.Value),
                        pc.GetDayOfMonth(dtpStartDate.Value));

                    conn.Open();
                    string query = @"UPDATE Users 
                                    SET Snme=?, 
                                    Sfnme=?, 
                                    Inid=?, 
                                    Sbirthdate=?, 
                                    Sworkdate=?,
                                    Iexrp=?,
                                    Iusercode=?, 
                                    Idpcode=?, 
                                    Itype=? 
                                        WHERE Iusercode=?";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {

                        cmd.Parameters.AddWithValue("?", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtNationalCode.Text.Trim());
                        cmd.Parameters.AddWithValue("?", birthDateShamsi);
                        cmd.Parameters.AddWithValue("?", workDateShamsi);
                        cmd.Parameters.AddWithValue("?", int.Parse(txtExperience.Text));
                        cmd.Parameters.AddWithValue("?", int.Parse(txtEmployeeCode.Text.Trim()));
                        cmd.Parameters.AddWithValue("?", cmbDepartment.SelectedValue ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("?", cmbJobCategory.SelectedValue ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("?", selectedUserCode);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("تغییرات با موفقیت ذخیره شد ✅");

                LoadData();

                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Cells["Iusercode"].Value != null &&
                        Convert.ToInt32(row.Cells["Iusercode"].Value) == selectedUserCode)
                    {
                        row.Selected = true;
                        dgvData.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                }

                SetEditMode(false);

                selectedUserCode = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در ذخیره تغییرات: " + ex.Message);
            }
        }

        private void cmbJobCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("آیا می‌خواهید به منوی اصلی برگردید؟",
                                                    "بازگشت به فرم اصلی",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                this.Hide();
                Form3 form3 = new Form3();
                form3.Show();
            }
        }

        private void dgvData_MouseEnter(object sender, EventArgs e)
        {
            if (dgvData != null && dgvData.Rows.Count > 0)
                dgvData.Focus();
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string placeholder = "کد ملی یا کد پرسنلی را وارد کنید";
            string searchText = txtSearch.Text.Trim();

            if (searchText == placeholder)
            {
                searchText = "";
            }

            LoadData(searchText);
        }


        private void txtExperience_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string sdeletionDate = GetPersianDate(DateTime.Now);

            if (dgvData.CurrentRow == null)
            {
                MessageBox.Show("لطفاً یک ردیف را برای حذف انتخاب کنید");
                return;
            }

            int userCode = Convert.ToInt32(dgvData.CurrentRow.Cells["Iusercode"].Value);

            DialogResult dr = MessageBox.Show(
                "آیا مطمئن هستید که می‌خواهید این کاربر را حذف کنید؟",
                "تأیید حذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (dr == DialogResult.No) return;

            try
            {
                string dbPath = Properties.Settings.Default.DatabasePath;
                string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    conn.Open();

                    string checkQuery = "SELECT COUNT(*) FROM Asset WHERE Iusercode=?";
                    using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("?", userCode);
                        int assetCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (assetCount > 0)
                        {
                            MessageBox.Show("این کاربر دارای دستگاه فعال است و نمی‌توان آن را حذف کرد ❌");
                            return;
                        }
                    }

                    string snme = dgvData.CurrentRow.Cells["Snme"].Value?.ToString();
                    string sfname = dgvData.CurrentRow.Cells["Sfnme"].Value?.ToString();
                    int inid = Convert.ToInt32(dgvData.CurrentRow.Cells["Inid"].Value);
                    string sbirthdate = dgvData.CurrentRow.Cells["Sbirthdate"].Value?.ToString();
                    int idpcode = Convert.ToInt32(dgvData.CurrentRow.Cells["Idpcode"].Value);
                    string sworkdate = dgvData.CurrentRow.Cells["Sworkdate"].Value?.ToString();
                    int iexpr = Convert.ToInt32(dgvData.CurrentRow.Cells["Iexrp"].Value);
                    string itype = dgvData.CurrentRow.Cells["Itype"].Value?.ToString();

                    string insertQuery = @"INSERT INTO UsersHis 
                (Snme, Sfnme, Inid, Sbirthdate, Iusercode, Idpcode, Sworkdate, Iexrp, Itype, Sdeletiondate) 
                VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                    using (OleDbCommand insertCmd = new OleDbCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("?", snme);
                        insertCmd.Parameters.AddWithValue("?", sfname);
                        insertCmd.Parameters.AddWithValue("?", inid);
                        insertCmd.Parameters.AddWithValue("?", sbirthdate);
                        insertCmd.Parameters.AddWithValue("?", userCode);
                        insertCmd.Parameters.AddWithValue("?", idpcode);
                        insertCmd.Parameters.AddWithValue("?", sworkdate);
                        insertCmd.Parameters.AddWithValue("?", iexpr);
                        insertCmd.Parameters.AddWithValue("?", itype);
                        insertCmd.Parameters.AddWithValue("?", sdeletionDate);

                        insertCmd.ExecuteNonQuery();
                    }

                    string deleteQuery = "DELETE FROM Users WHERE Iusercode=?";
                    using (OleDbCommand deleteCmd = new OleDbCommand(deleteQuery, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("?", userCode);
                        deleteCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("کاربر با موفقیت حذف گردید ✅");

                LoadData(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در حذف کاربر: " + ex.Message);
            }
        }




        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime startDate = dtpStartDate.Value;
            txtExperience.Text = CalculateExperience(startDate).ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            try
            {
                LoadData();
                dgvData.ClearSelection();
                MessageBox.Show("اطلاعات با موفقیت بروز شد ✅");
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در بروزرسانی اطلاعات: " + ex.Message);
            }
        }
    }
}