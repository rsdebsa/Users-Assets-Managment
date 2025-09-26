using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Stimulsoft.Report;


namespace WindowsFormsApplication1
{
    public partial class Form11 : Form
    {

        private int selectedAssetCode;

        private ToolTip tooltip = new ToolTip();


        public Form11()
        {
            InitializeComponent();

            dgvAssets.DefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Regular);
            dgvAssets.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Bold);
            dgvAssets.RowHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Italic);

        }
        private void Form11_Load(object sender, EventArgs e)
        {
            SetupDataGridView();

            comboBox1.Items.Clear();
            comboBox1.Items.Add("همه دستگاه‌ها");
            comboBox1.Items.Add("بر اساس نوع دستگاه");
            comboBox1.Items.Add("بر اساس اداره");
            comboBox1.Items.Add("بر اساس نوع دستگاه و اداره");
            comboBox1.SelectedIndex = 0;

            comboBox2.Enabled = false;
            comboBox3.Enabled = false;

            LoadAssets();
            LoadDepartments();
            LoadDeviceTypes();




            if (dgvAssets.Rows.Count > 0)
                dgvAssets.Rows[0].Selected = true;

            SetPlaceholder(txtSearch, "شماره دارایی یا شماره پلاک را وارد کنید");

            tooltip.AutoPopDelay = 5000;
            tooltip.InitialDelay = 500;
            tooltip.ReshowDelay = 500;
            tooltip.ShowAlways = true;

            this.dgvAssets.CellMouseMove += dgvAssets_CellMouseMove;


        }
        private void SetupDataGridView()
        {

            dgvAssets.AutoGenerateColumns = false;
            dgvAssets.Columns.Clear();

            dgvAssets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Snme",
                Name = "Snme",
                HeaderText = "نام دستگاه"
            });
            dgvAssets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Inumb",
                Name = "Inumb",
                HeaderText = "شماره دارایی"
            });
            dgvAssets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Sserial",
                Name = "Sserial",
                HeaderText = "شماره سریال"
            });
            dgvAssets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Smodel",
                Name = "Smodel",
                HeaderText = "مدل دستگاه"
            });
            dgvAssets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Splak",
                Name = "Splak",
                HeaderText = "شماره پلاک"
            });
            dgvAssets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Iusercode",
                Name = "Iusercode",
                HeaderText = "کد پرسنلی تحویل"
            });
            dgvAssets.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "Bstatus",
                Name = "Bstatus",
                HeaderText = "وضعیت دستگاه",
                TrueValue = true,
                FalseValue = false,
                IndeterminateValue = false
            });
            dgvAssets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Sactivedate",
                Name = "Sactivedate",
                HeaderText = "تاریخ فعالسازی"
            });



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
        private void LoadAssets(string searchText = "")
        {
            try
            {
                string dbPath = Properties.Settings.Default.DatabasePath;
                string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    conn.Open();

                    string query;
                    OleDbCommand cmd;

                    if (string.IsNullOrEmpty(searchText))
                    {
                        query = @"SELECT Snme, Inumb, Sserial, Smodel, Splak, Iusercode, Iassetcode, Bstatus, Sactivedate
                          FROM Asset";
                        cmd = new OleDbCommand(query, conn);
                    }
                    else
                    {
                        query = @"SELECT Snme, Inumb, Sserial, Smodel, Splak, Iusercode, Iassetcode, Bstatus, Sactivedate
                          FROM Asset
                          WHERE Splak LIKE ? OR Inumb LIKE ?";
                        cmd = new OleDbCommand(query, conn);
                        cmd.Parameters.AddWithValue("?", searchText + "%");
                        cmd.Parameters.AddWithValue("?", searchText + "%");
                    }

                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Bstatus"] != DBNull.Value)
                        {
                            bool val = false;
                            if (row["Bstatus"] is bool)
                                val = (bool)row["Bstatus"];
                            else
                                val = row["Bstatus"].ToString() == "1" ||
                                      row["Bstatus"].ToString().ToLower() == "true" ||
                                      row["Bstatus"].ToString() == "-1";

                            row["Bstatus"] = val;
                        }
                    }

                    dgvAssets.DataSource = dt;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در بارگذاری داده‌ها: " + ex.Message);
            }
        }
        private void LoadDepartments()
        {
            try
            {
                string dbPath = Properties.Settings.Default.DatabasePath;
                string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    conn.Open();
                    string query = "SELECT Idpcode, Snme FROM Departmants";
                    OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    DataRow row = dt.NewRow();
                    row["Idpcode"] = 0;
                    row["Snme"] = "همه ادارات";
                    dt.Rows.InsertAt(row, 0);

                    comboBox3.DataSource = dt;
                    comboBox3.DisplayMember = "Snme";
                    comboBox3.ValueMember = "Idpcode";

                    comboBox3.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در بارگذاری ادارات: " + ex.Message);
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string placeholder = "شماره دارایی یا شماره پلاک را وارد کنید";
            string searchText = txtSearch.Text.Trim();

            if (searchText == placeholder)
            {
                searchText = "";
            }

            LoadAssets(searchText);
        }

        private void button1_Click(object sender, EventArgs e)
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
        private void Form11_FormClosing(object sender, FormClosingEventArgs e)
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
        private void dgvAssets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAssets.Rows[e.RowIndex];
                if (row.Cells["Iassetcode"].Value != null)
                {
                    selectedAssetCode = Convert.ToInt32(row.Cells["Iassetcode"].Value);
                }
            }
        }
        public void RefreshGrid()
        {
            string dbPath = Properties.Settings.Default.DatabasePath;
            string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string query = @"SELECT Snme, Inumb, Sserial, Smodel, Splak, Iusercode, Iassetcode, Bstatus, Sactivedate
                          FROM Asset";
                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvAssets.DataSource = dt;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;

            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    LoadAssets();
                    break;

                case 1:
                    comboBox2.Enabled = true;
                    LoadDeviceTypes();
                    break;

                case 2:
                    comboBox3.Enabled = true;
                    LoadDepartments();
                    break;

                case 3:
                    comboBox2.Enabled = true;
                    comboBox3.Enabled = true;
                    LoadDeviceTypes();
                    LoadDepartments();
                    break;
            }
        }
        private void LoadDeviceTypes()
        {
            var deviceTypes = new Dictionary<int, string>
    {
        {1, "Case"},
        {2, "Keyboard"},
        {3, "Speaker"},
        {4, "Mouse"},
        {5, "Monitor"}
    };

            comboBox2.DataSource = new BindingSource(deviceTypes, null);
            comboBox2.DisplayMember = "Value";
            comboBox2.ValueMember = "Key";
        }



        private void LoadAssetsByDevice(int assetCode)
        {
            try
            {
                string dbPath = Properties.Settings.Default.DatabasePath;
                string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    conn.Open();

                    string query = @"SELECT Snme, Inumb, Sserial, Smodel, Splak, Iusercode, Iassetcode, Bstatus, Sactivedate
                             FROM Asset
                             WHERE Iassetcode = ?";

                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("?", assetCode);

                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvAssets.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در فیلتر نوع دستگاه: " + ex.Message);
            }
        }
        private void LoadAssetsByDepartment(int deptCode)
        {
            try
            {
                string dbPath = Properties.Settings.Default.DatabasePath;
                string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    conn.Open();

                    string query = @"SELECT a.Snme, a.Inumb, a.Sserial, a.Smodel, a.Splak, a.Iusercode, a.Iassetcode, a.Bstatus, a.Sactivedate
                             FROM Asset a
                             INNER JOIN Users u ON a.Iusercode = u.Iusercode
                             WHERE u.Idpcode = ?";

                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("?", deptCode);

                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvAssets.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در فیلتر بر اساس اداره: " + ex.Message);
            }
        }

        private void LoadAssetsByDeviceAndDepartment(int assetCode, int deptCode)
        {
            try
            {
                string dbPath = Properties.Settings.Default.DatabasePath;
                string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    conn.Open();

                    string query = @"SELECT a.Snme, a.Inumb, a.Sserial, a.Smodel, a.Splak, a.Iusercode, a.Iassetcode, a.Bstatus, a.Sactivedate
                             FROM Asset a
                             INNER JOIN Users u ON a.Iusercode = u.Iusercode
                             WHERE a.Iassetcode = ? AND u.Idpcode = ?";

                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("?", assetCode);
                    cmd.Parameters.AddWithValue("?", deptCode);

                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvAssets.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در فیلتر ترکیبی: " + ex.Message);
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedValue != null)
            {
                int assetCode = ((KeyValuePair<int, string>)comboBox2.SelectedItem).Key;
                FilterByDeviceType(assetCode);
            }
        }

        private void FilterByDeviceType(int assetCode)
        {
            string dbPath = Properties.Settings.Default.DatabasePath;
            string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
                string query = "SELECT * FROM Asset WHERE Iassetcode=?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", assetCode);

                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvAssets.DataSource = dt;
            }
        }


        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedValue != null && comboBox3.SelectedValue is int)
            {
                int deptId = (int)comboBox3.SelectedValue;

                if (deptId == 0)
                    LoadAssets();
                else
                    FilterByDepartment(deptId);
            }
        }

        private void FilterByDepartment(int deptId)
        {
            string dbPath = Properties.Settings.Default.DatabasePath;
            string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
                string query = @"SELECT A.* 
                         FROM Asset A 
                         INNER JOIN Users U ON A.Iusercode = U.Iusercode
                         WHERE U.Idpcode = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", deptId);

                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvAssets.DataSource = dt;
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
        }

        private void dgvAssets_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    var dgv = sender as DataGridView;
                    var column = dgv.Columns[e.ColumnIndex];

                    if (column.Name == "Iusercode")
                    {
                        var cellValue = dgv.Rows[e.RowIndex].Cells["Iusercode"].Value;
                        if (cellValue != null)
                        {
                            string userCode = cellValue.ToString();
                            string info = GetUserInfoByCode(userCode);

                            if (!string.IsNullOrEmpty(info))
                            {
                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = info;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        private string GetUserInfoByCode(string userCode)
        {
            string dbPath = Properties.Settings.Default.DatabasePath;
            string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";
            string result = "";

            using (OleDbConnection con = new OleDbConnection(connStr))
            {
                string query = @"
            SELECT U.Snme, U.Sfnme, D.Snme AS DepartmentName
            FROM Users AS U
            LEFT JOIN Departmants AS D ON U.Idpcode = D.Idpcode
            WHERE U.Iusercode = ?";

                OleDbCommand cmd = new OleDbCommand(query, con);
                cmd.Parameters.AddWithValue("?", userCode);

                con.Open();
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = $"نام: {reader["Snme"]} | خانوادگی: {reader["Sfnme"]} | اداره: {reader["DepartmentName"]}";
                }
            }

            return result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string reportPath = Path.Combine(Application.StartupPath, "Reports", "AssetOnly.mrt");
                StiReport report = new StiReport();
                report.Load(reportPath);
                report.Dictionary.Databases.Clear();

                // جدول دستگاه‌ها
                DataTable dtAsset = new DataTable();
                dtAsset.Columns.Add("Snme");
                dtAsset.Columns.Add("Inumb");
                dtAsset.Columns.Add("Sserial");
                dtAsset.Columns.Add("Smodel");
                dtAsset.Columns.Add("Splak");
                dtAsset.Columns.Add("Sactivedate");
                dtAsset.Columns.Add("Bstatus");
                dtAsset.Columns.Add("Iusercode");

                // جدول کاربران
                DataTable dtUsers0 = new DataTable();
                dtUsers0.Columns.Add("Iusercode");
                dtUsers0.Columns.Add("Sfnme");
                dtUsers0.Columns.Add("Snme");
                dtUsers0.Columns.Add("Sbirthdate");
                dtUsers0.Columns.Add("Sworkdate");
                dtUsers0.Columns.Add("JobCategory");  // تبدیل به متن انجام میشه
                dtUsers0.Columns.Add("DepartmentName");

                HashSet<string> addedUsers = new HashSet<string>();
                string dbPath = Properties.Settings.Default.DatabasePath;
                string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

                foreach (DataGridViewRow row in dgvAssets.Rows)
                {
                    if (row.IsNewRow) continue;

                    // اضافه کردن دستگاه
                    DataRow dr = dtAsset.NewRow();
                    dr["Snme"] = row.Cells["Snme"].Value.ToString();
                    dr["Inumb"] = row.Cells["Inumb"].Value.ToString();
                    dr["Sserial"] = row.Cells["Sserial"].Value.ToString();
                    dr["Smodel"] = row.Cells["Smodel"].Value.ToString();
                    dr["Splak"] = row.Cells["Splak"].Value.ToString();
                    dr["Sactivedate"] = row.Cells["Sactivedate"].Value.ToString();
                    dr["Bstatus"] = row.Cells["Bstatus"].Value;
                    dr["Iusercode"] = row.Cells["Iusercode"].Value.ToString();
                    dtAsset.Rows.Add(dr);

                    // اگر کاربر هنوز اضافه نشده
                    string usercode = row.Cells["Iusercode"].Value.ToString();
                    if (!addedUsers.Contains(usercode))
                    {
                        addedUsers.Add(usercode);

                        using (OleDbConnection conn = new OleDbConnection(connStr))
                        {
                            conn.Open();

                            string sql = @"
                       SELECT u.Iusercode, u.Sfnme, u.Snme, u.Sbirthdate, u.Sworkdate,
                                 u.Itype, d.Snme AS DepartmentName
                                    FROM Users u
                                    LEFT JOIN Departmants d ON u.Idpcode = d.Idpcode
                                     WHERE u.Iusercode = ?";

                            using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("?", usercode);

                                using (OleDbDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        DataRow du = dtUsers0.NewRow();
                                        du["Iusercode"] = reader["Iusercode"].ToString();
                                        du["Sfnme"] = reader["Sfnme"].ToString();
                                        du["Snme"] = reader["Snme"].ToString();
                                        du["Sbirthdate"] = reader["Sbirthdate"].ToString();
                                        du["Sworkdate"] = reader["Sworkdate"].ToString();
                                        string jobCode = reader["Itype"].ToString();
                                        switch (jobCode)
                                        {
                                            case "1": du["JobCategory"] = "مدیر"; break;
                                            case "2": du["JobCategory"] = "معاون"; break;
                                            case "3": du["JobCategory"] = "کارمند"; break;
                                            default: du["JobCategory"] = "نامشخص"; break;
                                        }

                                        du["DepartmentName"] = reader["DepartmentName"].ToString();
                                        dtUsers0.Rows.Add(du);
                                    }
                                }
                            }
                        }
                    }
                }

                // پاس دادن دیتا به ریپورت
                report.RegData("Asset", dtAsset);
                report.RegData("Users0", dtUsers0);

                report.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در گزارش: " + ex.Message);
            }
        }


    }
}