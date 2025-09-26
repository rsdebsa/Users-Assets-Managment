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
    public partial class Form5 : Form
    {

        private DataTable dtData;
        private Dictionary<int, string> jobCategories;

        private string connStr;
        private int assetCode;
        private string snme;
        private int inumb;
        private string sserial;
        private string smodel;
        private string splak;
        private int? iusercode;
        private string sactivedate;

        public Form5(string connStr, int iassetCode, int inumb, string snme,
                 string sserial, string smodel, string splak, int? iusercode, string sactivedate)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.connStr = connStr;
            this.inumb = inumb;
            this.assetCode = iassetCode;
            this.snme = snme;
            this.inumb = inumb;
            this.sserial = sserial;
            this.smodel = smodel;
            this.splak = splak;
            this.iusercode = iusercode;
            this.sactivedate = sactivedate;

            dgvData.DefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Regular);

            dgvData.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Bold);

            dgvData.RowHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Italic);

            this.Load += Form5_Load;
            dgvData.MouseEnter += (s, e) => { if (dgvData.Enabled) dgvData.Focus(); };
            dgvData.MouseDown += (s, e) => dgvData.Focus();
            dgvData.DataBindingComplete += dgvData_DataBindingComplete;


            txtSearch.TextChanged += txtSearch_TextChanged_1;
            txtSearch.Enter += textBox1_Enter;
            txtSearch.Leave += textBox1_Leave;
            txtSearch.KeyDown += textBox1_KeyDown;
            dgvData.CellContentClick += dgvData_CellContentClick;

        }
        private string GetPersianDate(DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            return $"{pc.GetYear(dt)}/{pc.GetMonth(dt):00}/{pc.GetDayOfMonth(dt):00}";
        }


        private void dgvData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.Cells["Itype"].Value != null)
                {
                    int typeCode;
                    if (int.TryParse(row.Cells["Itype"].Value.ToString(), out typeCode)
                        && jobCategories.ContainsKey(typeCode))
                    {
                        row.Cells["Itype"].Value = jobCategories[typeCode];
                    }
                }
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
            if (txtSearch.Text == "کد ملی یا کد پرسنلی را وارد کنید")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {

            dgvData.BackgroundColor = Color.White;

            dgvData.DefaultCellStyle.BackColor = Color.White;
            dgvData.DefaultCellStyle.ForeColor = Color.Black;
            dgvData.DefaultCellStyle.SelectionBackColor = Color.DarkOrange;
            dgvData.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvData.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgvData.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvData.EnableHeadersVisualStyles = false;


            dgvData.RightToLeft = RightToLeft.Yes;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;

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

            SetupDataGridView();
            LoadData();
            LoadDepartments();

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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در بارگذاری ادارات: " + ex.Message);
            }
        }



        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            string placeholder = "کد ملی یا کد پرسنلی را وارد کنید";
            string searchText = txtSearch.Text.Trim();

            if (searchText == placeholder)
            {
                searchText = "";
            }

            LoadData(searchText);

        }



        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
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
        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
        {

        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            string dbPath = Properties.Settings.Default.DatabasePath;
            string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";
            string plak = textBox1.Text.Trim();
            string activeDate = GetPersianDate(DateTime.Now);

            if (dgvData.SelectedRows.Count == 0)
            {
                MessageBox.Show("لطفا یک کاربر انتخاب کنید.");
                return;
            }

            int userCode = Convert.ToInt32(dgvData.SelectedRows[0].Cells["Iusercode"].Value);
            string userName = dgvData.SelectedRows[0].Cells["Snme"].Value?.ToString();

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();

                if (!string.IsNullOrWhiteSpace(plak))
                {
                    int total = 0;
                    using (var cmd1 = new OleDbCommand("SELECT COUNT(*) FROM Asset WHERE Splak = ? AND Iassetcode <> ?", conn))
                    {
                        cmd1.Parameters.AddWithValue("?", plak);
                        cmd1.Parameters.AddWithValue("?", assetCode);
                        total += Convert.ToInt32(cmd1.ExecuteScalar() ?? 0);
                    }
                    using (var cmd2 = new OleDbCommand("SELECT COUNT(*) FROM AssetHis WHERE Splak = ?", conn))
                    {
                        cmd2.Parameters.AddWithValue("?", plak);
                        total += Convert.ToInt32(cmd2.ExecuteScalar() ?? 0);
                    }
                    if (total > 0)
                    {
                        MessageBox.Show("این پلاک قبلا ثبت شده است.");
                        return;
                    }
                }

                // --- پیام اول ---
                DialogResult dr = MessageBox.Show(
                    $"آیا از فعال‌سازی دستگاه:\n" +
                    $"📌 نام دستگاه: {snme}\n" +
                    $"📌 شماره دارایی: {inumb}\n" +
                    $"📌 سریال: {sserial}\n\n" +
                    $"برای کاربر:\n" +
                    $"👤 نام و نام خانوادگی: {userName}\n" +
                    $"👤 کد پرسنلی: {userCode}\n\n" +
                    (string.IsNullOrWhiteSpace(plak) ? "⚠️ بدون پلاک" : $"با پلاک\n {plak}") +
                    "\n\nمطمئن هستید؟",
                    "تایید فعال‌سازی",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr != DialogResult.Yes) return;

                if (string.IsNullOrWhiteSpace(plak))
                {
                    DialogResult dr2 = MessageBox.Show(
                        "شما پلاکی وارد نکردید.\nآیا مطمئن هستید می‌خواهید دستگاه را بدون پلاک فعال کنید؟",
                        "هشدار",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (dr2 != DialogResult.Yes) return;
                }

                using (OleDbTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        string updateAsset = "UPDATE Asset SET Iusercode = ?, Splak = ?, Sactivedate = ?, Bstatus = True WHERE Inumb = ?";
                        using (OleDbCommand cmd = new OleDbCommand(updateAsset, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("?", userCode);
                            if (string.IsNullOrWhiteSpace(plak))
                                cmd.Parameters.AddWithValue("?", DBNull.Value);
                            else
                                cmd.Parameters.AddWithValue("?", plak);
                            cmd.Parameters.AddWithValue("?", activeDate);
                            cmd.Parameters.AddWithValue("?", inumb);
                            cmd.ExecuteNonQuery();
                        }

                        string insertHistory = "INSERT INTO AssetHis (Snme, Inumb, Sserial, Smodel, Splak, Iusercode, Iassetcode, Sactivedate, Bstatus) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";
                        using (OleDbCommand cmd = new OleDbCommand(insertHistory, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("?", snme);
                            cmd.Parameters.AddWithValue("?", inumb);
                            cmd.Parameters.AddWithValue("?", sserial);
                            cmd.Parameters.AddWithValue("?", smodel);
                            if (string.IsNullOrWhiteSpace(plak))
                                cmd.Parameters.AddWithValue("?", DBNull.Value);
                            else
                                cmd.Parameters.AddWithValue("?", plak);
                            cmd.Parameters.AddWithValue("?", userCode);
                            cmd.Parameters.AddWithValue("?", assetCode);
                            cmd.Parameters.AddWithValue("?", activeDate);
                            cmd.Parameters.Add("?", OleDbType.Boolean).Value = true;
                            cmd.ExecuteNonQuery();

                            int transactionId = 0;
                            using (OleDbCommand cmdGetId = new OleDbCommand("SELECT @@IDENTITY", conn, tran))
                            {
                                transactionId = Convert.ToInt32(cmdGetId.ExecuteScalar());
                            }
                            MessageBox.Show("کد تراکنش ثبت‌شده: " + transactionId);
                        }

                        tran.Commit();
                        MessageBox.Show("دستگاه با موفقیت فعال شد.");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("خطا در ذخیره‌سازی: " + ex.Message);
                    }
                }
            }
    }
  }
}



