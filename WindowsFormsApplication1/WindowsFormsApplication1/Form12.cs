using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Stimulsoft.Report;

namespace WindowsFormsApplication1
{
    public partial class Form12 : Form
    {
        private DataTable dtData;
        private Dictionary<int, string> jobCategories;

        public Form12()
        {




            InitializeComponent();

            // تغییر فونت سلول‌ها
            dgvData.DefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Regular);

            // تغییر فونت هدر ستون‌ها
            dgvData.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Bold);

            // تغییر فونت هدر ردیف‌ها
            dgvData.RowHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Italic);


            this.Load += Form12_Load;
            dgvData.MouseEnter += (s, e) => { if (dgvData.Enabled) dgvData.Focus(); };
            dgvData.MouseDown += (s, e) => dgvData.Focus();


        }
        private string GetConnString()
        {
            string dbPath = Properties.Settings.Default.DatabasePath;
            return $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

        }

        private void Form12_Load(object sender, EventArgs e)
        {
            // رنگ پس زمینه جدول
            dgvData.BackgroundColor = Color.White;

            // رنگ پیش فرض سلول‌ها
            dgvData.DefaultCellStyle.BackColor = Color.White;
            dgvData.DefaultCellStyle.ForeColor = Color.Black;

            // رنگ انتخاب (وقتی کاربر روی سلول کلیک می‌کنه)
            dgvData.DefaultCellStyle.SelectionBackColor = Color.DarkOrange;
            dgvData.DefaultCellStyle.SelectionForeColor = Color.White;

            // رنگ هدر (سربرگ ستون‌ها)
            dgvData.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgvData.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvData.EnableHeadersVisualStyles = false; // لازمه که رنگ هدر عمل کنه



            dgvData.RightToLeft = RightToLeft.Yes;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;

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

            comboBox1.Items.Clear();
            comboBox1.Items.Add("همه کاربران");
            comboBox1.Items.Add("بر اساس اداره");
            comboBox1.Items.Add("بر اساس دسته شغلی");
            comboBox1.Items.Add("بر اساس اداره و دسته شغلی");
            comboBox1.SelectedIndex = 0;
            //LoadFilteredData();

            SetupDataGridView();

            LoadData();
            LoadDepartments();
            LoadJobTypes();

            SetPlaceholder(txtSearch, "کد ملی یا کد پرسنلی یا نام را وارد کنید");


            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
        }
        private void txtSearch_Leave(object sender, EventArgs e)
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
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Sdeletiondate", Name = "Sdeletiondate", HeaderText = "تاریخ غیرفعالسازی کاربر" });

            // اگر خواستی کدها را هم داشته باشی ولی مخفی:
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Idpcode", Name = "Idpcode", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Itype", Name = "Itype", Visible = false });
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string searchValue = txtSearch.Text.Trim();

                // بررسی اینکه ورودی خالی یا متن پیش‌فرض نباشه
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




        private int CalculateExperience(DateTime startDate)
        {
            DateTime today = DateTime.Today;
            int years = today.Year - startDate.Year;

            if (startDate.Date > today.AddYears(-years))
                years--;

            return years < 0 ? 0 : years;
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
                    U.ID,
                    U.Snme,
                    U.Sfnme,
                    U.Inid,
                    U.Sbirthdate,
                    U.Iusercode,
                    U.Idpcode,
                    D.Snme AS DepartmentName,
                    U.Sworkdate,
                    U.Iexrp,
                    U.Itype,
                    U.Sdeletiondate
                FROM UsersHis AS U
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

                    // نام دسته شغلی (به‌جای عدد)
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

                    // 👇 درستش اینه که بری داخل comboBox3
                    comboBox3.DataSource = dtDept;
                    comboBox3.DisplayMember = "Snme";   // اسم اداره
                    comboBox3.ValueMember = "Idpcode";  // کد اداره
                    comboBox3.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در بارگذاری ادارات: " + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "کد ملی یا کد پرسنلی یا نام را وارد کنید";

            // اگه می‌خوای همزمان گرید هم دوباره بارگذاری بشه
            LoadFilteredData(null);
            // هر بار انتخاب کاربر عوض شد، مقادیر بقیه کمبوها ریست بشن
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;

            if (comboBox1.SelectedIndex == 0) // همه کاربران
            {
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                LoadData();
            }
            else if (comboBox1.SelectedIndex == 1) // بر اساس اداره
            {
                comboBox2.Enabled = false;
                comboBox3.Enabled = true;
            }
            else if (comboBox1.SelectedIndex == 2) // بر اساس دسته شغلی
            {
                comboBox2.Enabled = true;
                comboBox3.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 3) // بر اساس اداره + دسته شغلی
            {
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
            }
        }





        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private Button button2;
        private Label label2;

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private ComboBox comboBox2;
        private ComboBox comboBox3;
        private Label label4;

        private void LoadJobTypes()
        {
            // دیتای دسته شغلی رو از همون دیکشنری خودمون می‌گیریم
            DataTable dt = new DataTable();
            dt.Columns.Add("Itype", typeof(int));
            dt.Columns.Add("JobTypeName", typeof(string));

            foreach (var kvp in jobCategories)
            {
                dt.Rows.Add(kvp.Key, kvp.Value);
            }

            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "JobTypeName";
            comboBox2.ValueMember = "Itype";

            comboBox2.SelectedIndex = -1;

        }


        private void FilterByDepartment(string idpcode)
        {
            string dbPath = Properties.Settings.Default.DatabasePath;
            string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

            using (OleDbConnection con = new OleDbConnection(connStr))
            {
                string query = @"
            SELECT 
                U.ID,
                U.Snme,
                U.Sfnme,
                U.Inid,
                U.Sbirthdate,
                U.Iusercode,
                U.Idpcode,
                D.Snme AS DepartmentName,
                U.Sworkdate,
                U.Iexrp,
                U.Itype,
                U.Sdeletiondate
            FROM UsersHis AS U
            LEFT JOIN Departmants AS D ON U.Idpcode = D.Idpcode
            WHERE U.Idpcode = @id";

                OleDbDataAdapter da = new OleDbDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@id", idpcode);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (!dt.Columns.Contains("JobCategory"))
                    dt.Columns.Add("JobCategory", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    if (row["Itype"] != DBNull.Value)
                    {
                        int code;
                        if (int.TryParse(row["Itype"].ToString(), out code) && jobCategories.ContainsKey(code))
                            row["JobCategory"] = jobCategories[code];
                    }
                }

                dgvData.DataSource = dt;
            }
        }

        private void FilterByJobType(string itype)
        {
            string dbPath = Properties.Settings.Default.DatabasePath;
            string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

            using (OleDbConnection con = new OleDbConnection(connStr))
            {
                string query = @"
           SELECT 
                U.ID,
                U.Snme,
                U.Sfnme,
                U.Inid,
                U.Sbirthdate,
                U.Iusercode,
                U.Idpcode,
                D.Snme AS DepartmentName,
                U.Sworkdate,
                U.Iexrp,
                U.Itype,
                U.Sdeletiondate
            FROM UsersHis AS U
            LEFT JOIN Departmants AS D ON U.Idpcode = D.Idpcode
            WHERE U.Itype = @type";

                OleDbDataAdapter da = new OleDbDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@type", itype);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (!dt.Columns.Contains("JobCategory"))
                    dt.Columns.Add("JobCategory", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    if (row["Itype"] != DBNull.Value)
                    {
                        int code;
                        if (int.TryParse(row["Itype"].ToString(), out code) && jobCategories.ContainsKey(code))
                            row["JobCategory"] = jobCategories[code];
                    }
                }

                dgvData.DataSource = dt;
            }
        }

        private void FilterByDepartmentAndJobType(string idpcode, string itype)
        {
            string dbPath = Properties.Settings.Default.DatabasePath;
            string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

            using (OleDbConnection con = new OleDbConnection(connStr))
            {
                string query = @"
           SELECT 
                U.ID,
                U.Snme,
                U.Sfnme,
                U.Inid,
                U.Sbirthdate,
                U.Iusercode,
                U.Idpcode,
                D.Snme AS DepartmentName,
                U.Sworkdate,
                U.Iexrp,
                U.Itype,
                U.Sdeletiondate
            FROM UsersHis AS U
            LEFT JOIN Departmants AS D ON U.Idpcode = D.Idpcode
            WHERE U.Idpcode = @idp AND U.Itype = @itype";

                OleDbDataAdapter da = new OleDbDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@idp", idpcode);
                da.SelectCommand.Parameters.AddWithValue("@itype", itype);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (!dt.Columns.Contains("JobCategory"))
                    dt.Columns.Add("JobCategory", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    if (row["Itype"] != DBNull.Value)
                    {
                        int code;
                        if (int.TryParse(row["Itype"].ToString(), out code) && jobCategories.ContainsKey(code))
                            row["JobCategory"] = jobCategories[code];
                    }
                }

                dgvData.DataSource = dt;
            }
        }

        private void LoadFilteredData(string searchText = null)
        {
            string dbPath = Properties.Settings.Default.DatabasePath;
            string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();

                string query = @"
            SELECT 
                U.ID,
                U.Snme,
                U.Sfnme,
                U.Inid,
                U.Sbirthdate,
                U.Iusercode,
                U.Idpcode,
                D.Snme AS DepartmentName,
                U.Sworkdate,
                U.Iexrp,
                U.Itype,
                U.Sdeletiondate
            FROM UsersHis AS U
            LEFT JOIN Departmants AS D ON U.Idpcode = D.Idpcode
            WHERE 1=1";

                if (comboBox1.SelectedIndex == 1 && comboBox3.SelectedValue != null)
                    query += " AND U.Idpcode = ?";
                else if (comboBox1.SelectedIndex == 2 && comboBox2.SelectedValue != null)
                    query += " AND U.Itype = ?";
                else if (comboBox1.SelectedIndex == 3 && comboBox2.SelectedValue != null && comboBox3.SelectedValue != null)
                    query += " AND U.Idpcode = ? AND U.Itype = ?";

                if (!string.IsNullOrWhiteSpace(searchText) && searchText != "کد ملی یا کد پرسنلی یا نام را وارد کنید")
                    query += " AND (U.Inid LIKE ? OR U.Iusercode LIKE ? OR U.Snme LIKE ? OR U.Sfnme LIKE ?)";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    if (comboBox1.SelectedIndex == 1 && comboBox3.SelectedValue != null)
                        cmd.Parameters.AddWithValue("?", comboBox3.SelectedValue);
                    else if (comboBox1.SelectedIndex == 2 && comboBox2.SelectedValue != null)
                        cmd.Parameters.AddWithValue("?", comboBox2.SelectedValue);
                    else if (comboBox1.SelectedIndex == 3 && comboBox2.SelectedValue != null && comboBox3.SelectedValue != null)
                    {
                        cmd.Parameters.AddWithValue("?", comboBox3.SelectedValue);
                        cmd.Parameters.AddWithValue("?", comboBox2.SelectedValue);
                    }

                    if (!string.IsNullOrWhiteSpace(searchText) && searchText != "کد ملی یا کد پرسنلی یا نام را وارد کنید")
                    {
                        cmd.Parameters.AddWithValue("?", "%" + searchText + "%");
                        cmd.Parameters.AddWithValue("?", "%" + searchText + "%");
                        cmd.Parameters.AddWithValue("?", "%" + searchText + "%");
                        cmd.Parameters.AddWithValue("?", "%" + searchText + "%");
                    }

                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (!dt.Columns.Contains("JobCategory"))
                        dt.Columns.Add("JobCategory", typeof(string));

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Itype"] != DBNull.Value)
                        {
                            int code;
                            if (int.TryParse(row["Itype"].ToString(), out code) && jobCategories.ContainsKey(code))
                                row["JobCategory"] = jobCategories[code];
                        }
                    }

                    dgvData.DataSource = dt;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
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


        private TextBox txtSearch;
        private Label label5;

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 2 && comboBox2.SelectedValue != null)
            {
                FilterByJobType(comboBox2.SelectedValue.ToString());
            }
            else if (comboBox1.SelectedIndex == 3 && comboBox2.SelectedValue != null && comboBox3.SelectedValue != null)
            {
                FilterByDepartmentAndJobType(comboBox3.SelectedValue.ToString(), comboBox2.SelectedValue.ToString());
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1 && comboBox3.SelectedValue != null)
            {
                FilterByDepartment(comboBox3.SelectedValue.ToString());
            }
            else if (comboBox1.SelectedIndex == 3 && comboBox3.SelectedValue != null && comboBox2.SelectedValue != null)
            {
                FilterByDepartmentAndJobType(comboBox3.SelectedValue.ToString(), comboBox2.SelectedValue.ToString());
            }
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string placeholder = "کد ملی یا کد پرسنلی یا نام را وارد کنید";
            string searchText = txtSearch.Text.Trim();

            if (searchText == placeholder)
                return;

            LoadFilteredData(searchText);
        }
        private void btnRefresh_Click_1(object sender, EventArgs e)
        {

            try
            {
                LoadData();   // همون متدی که داده‌ها رو از دیتابیس میاره
                dgvData.ClearSelection(); // انتخاب‌ها پاک بشه
                MessageBox.Show("اطلاعات با موفقیت بروز شد ✅");
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در بروزرسانی اطلاعات: " + ex.Message);
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {

            try
            {
                string reportPath = Path.Combine(Application.StartupPath, "Reports", "UsersHisOnly.mrt");
                StiReport report = new StiReport();
                report.Load(reportPath);
                report.Dictionary.Databases.Clear();

                DataTable dt = new DataTable();


                // ست کردن نام دقیق ستون‌ها (مطابق فایل mrt)
                dt.Columns.Add("Sdeletiondate");
                dt.Columns.Add("Itype");
                dt.Columns.Add("Iexrp");
                dt.Columns.Add("Sworkdate");
                dt.Columns.Add("Idpcode");
                dt.Columns.Add("Iusercode");
                dt.Columns.Add("Sbirthdate");
                dt.Columns.Add("Inid");
                dt.Columns.Add("Sfnme");
                dt.Columns.Add("Snme");

                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.IsNewRow) continue;

                    DataRow dr = dt.NewRow();
                    dr["Sdeletiondate"] = row.Cells["Sdeletiondate"].Value.ToString();
                    dr["Itype"] = row.Cells["JobCategory"].Value.ToString();
                    dr["Iexrp"] = row.Cells["Iexrp"].Value.ToString();
                    dr["Sworkdate"] = row.Cells["Sworkdate"].Value.ToString();
                    dr["Idpcode"] = row.Cells["DepartmentName"].Value.ToString();
                    dr["Iusercode"] = row.Cells["Iusercode"].Value.ToString();
                    dr["Sbirthdate"] = row.Cells["Sbirthdate"].Value.ToString();
                    dr["Inid"] = row.Cells["Inid"].Value.ToString();
                    dr["Sfnme"] = row.Cells["Sfnme"].Value.ToString();
                    dr["Snme"] = row.Cells["Snme"].Value.ToString();
                    dt.Rows.Add(dr);
                }


                report.RegData("UsersHis0", dt);
                report.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در گزارش: " + ex.Message);
            }
        }



    // متد کمکی: جدول فعلی گریدویو رو برمی‌گردونه
    private DataTable GetCurrentGridTable()
        {
            if (dgvData.DataSource is DataTable)
            {
                return (DataTable)dgvData.DataSource;
            }
            else if (dgvData.DataSource is DataView)
            {
                return ((DataView)dgvData.DataSource).ToTable();
            }
            return dtData; // در صورتی که مستقیم دیتا از dtData باشه
        }


        // متد کمکی برای استخراج داده‌های انتخاب‌شده
        private DataTable GetSelectedUsersTable()
        {
            DataTable dtSelected = dtData.Clone(); // همان ساختار جدول اصلی

            foreach (DataGridViewRow row in dgvData.SelectedRows)
            {
                DataRowView drv = row.DataBoundItem as DataRowView;
                if (drv != null)
                {
                    dtSelected.ImportRow(drv.Row);
                }
            }

            return dtSelected;
        }

        private void Form12_FormClosing(object sender, FormClosingEventArgs e)
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
}

