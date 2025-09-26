using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
  
    public partial class Form6 : Form
    {
        private int selectedAssetCode;

        private ToolTip tooltip = new ToolTip();

        public Form6()
        {
            InitializeComponent();

            dgvAssets.DefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Regular);
            dgvAssets.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Bold);
            dgvAssets.RowHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Italic);

        }

        private void Form6_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadAssets();

            dgvAssets.SelectionChanged += dgvAssets_SelectionChanged;

            dgvAssets.CellContentClick += dgvAssets_CellContentClick;

            if (dgvAssets.Rows.Count > 0)
                dgvAssets.Rows[0].Selected = true;

            SetPlaceholder(txtSearch, "شماره دارایی یا شماره پلاک را وارد کنید");
            SetTextBoxesEnabled(false);

            tooltip.AutoPopDelay = 5000;
            tooltip.InitialDelay = 500;
            tooltip.ReshowDelay = 500;
            tooltip.ShowAlways = true;

            this.dgvAssets.CellMouseMove += dgvAssets_CellMouseMove;

        }

        private void SetTextBoxesEnabled(bool enabled)
        {
            txtAssetName.ReadOnly = !enabled;
            txtInumb.ReadOnly = !enabled;
            txtIassetcode.ReadOnly = !enabled;
            textSserial.ReadOnly = !enabled;
            textSmodel.ReadOnly = !enabled;
            textSplak.ReadOnly = !enabled;
            textIusercode.ReadOnly = !enabled;
            textSactivatedate.ReadOnly = !enabled;
        }

        private void SetupDataGridView()
        {

            dgvAssets.AutoGenerateColumns = false;
            dgvAssets.Columns.Clear();

            dgvAssets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Iassetcode",
                Name = "Iassetcode",
                HeaderText = "کد دارایی",
                Visible = false
            });
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
            dgvAssets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Sactivedate",
                Name = "Sactivedate",
                HeaderText = "تاریخ فعالسازی"
            });
            dgvAssets.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "Bstatus",
                Name = "Bstatus",
                HeaderText = "وضعیت",
                TrueValue = true,
                FalseValue = false,
                IndeterminateValue = false
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
                        query = @"SELECT Iassetcode, Snme, Inumb, Sserial, Smodel, Splak, Iusercode,Sactivedate, Bstatus
                          FROM Asset";
                        cmd = new OleDbCommand(query, conn);
                    }
                    else
                    {
                        query = @"SELECT Iassetcode, Snme, Inumb, Sserial, Smodel, Splak, Iusercode, Sactivedate, Bstatus
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

        private void dgvAssets_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAssets.CurrentRow != null && dgvAssets.CurrentRow.Index >= 0)
            {
                DataGridViewRow row = dgvAssets.CurrentRow;

                if (dgvAssets.Columns.Contains("Iassetcode"))
                    txtIassetcode.Text = row.Cells["Iassetcode"].Value?.ToString();

                if (dgvAssets.Columns.Contains("Snme"))
                    txtAssetName.Text = row.Cells["Snme"].Value?.ToString();

                if (dgvAssets.Columns.Contains("Inumb"))
                    txtInumb.Text = row.Cells["Inumb"].Value?.ToString();

                if (dgvAssets.Columns.Contains("Sserial"))
                    textSserial.Text = row.Cells["Sserial"].Value?.ToString();

                if (dgvAssets.Columns.Contains("Smodel"))
                    textSmodel.Text = row.Cells["Smodel"].Value?.ToString();

                if (dgvAssets.Columns.Contains("Splak"))
                    textSplak.Text = row.Cells["Splak"].Value?.ToString();

                if (dgvAssets.Columns.Contains("Iusercode"))
                    textIusercode.Text = row.Cells["Iusercode"].Value?.ToString();

              
                if (dgvAssets.Columns.Contains("Sactivedate"))
                    textSactivatedate.Text = row.Cells["Sactivedate"].Value?.ToString();

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





        private void dgvAssets_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvAssets.Columns[e.ColumnIndex].Name != "Bstatus") return;

            var row = dgvAssets.Rows[e.RowIndex];
            var chkCell = (DataGridViewCheckBoxCell)row.Cells["Bstatus"];

            bool wasActive = false;
            if (chkCell.Value != null && chkCell.Value != DBNull.Value)
                bool.TryParse(chkCell.Value.ToString(), out wasActive);

            chkCell.Value = wasActive;
            dgvAssets.CancelEdit();

            if (wasActive)
            {
                string dbPath = Properties.Settings.Default.DatabasePath;
                string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

                int iassetCode = Convert.ToInt32(dgvAssets.Rows[e.RowIndex].Cells["Iassetcode"].Value);
                string snme = dgvAssets.Rows[e.RowIndex].Cells["Snme"].Value.ToString();
                int inumb = Convert.ToInt32(dgvAssets.Rows[e.RowIndex].Cells["Inumb"].Value);
                string sserial = dgvAssets.Rows[e.RowIndex].Cells["Sserial"].Value.ToString();
                string smodel = dgvAssets.Rows[e.RowIndex].Cells["Smodel"].Value.ToString();
                string splak = dgvAssets.Rows[e.RowIndex].Cells["Splak"].Value?.ToString();
                int? iusercode = dgvAssets.Rows[e.RowIndex].Cells["Iusercode"].Value == DBNull.Value
                                 ? (int?)null
                                 : Convert.ToInt32(dgvAssets.Rows[e.RowIndex].Cells["Iusercode"].Value);
                string sactivedate = dgvAssets.Rows[e.RowIndex].Cells["Sactivedate"].Value?.ToString();

                Form7 f7 = new Form7(
                     connStr,
                     iassetCode,
                     inumb,
                    snme,
                    sserial,
                    smodel,
                    splak,
                    iusercode,
                    sactivedate
                    );
                if (f7.ShowDialog() == DialogResult.OK)
                {
                    LoadAssets();
                }


            }
            else
            {
                string dbPath = Properties.Settings.Default.DatabasePath;
                string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

                int iassetCode = Convert.ToInt32(dgvAssets.Rows[e.RowIndex].Cells["Iassetcode"].Value);
                string snme = dgvAssets.Rows[e.RowIndex].Cells["Snme"].Value.ToString();
                int inumb = Convert.ToInt32(dgvAssets.Rows[e.RowIndex].Cells["Inumb"].Value);
                string sserial = dgvAssets.Rows[e.RowIndex].Cells["Sserial"].Value.ToString();
                string smodel = dgvAssets.Rows[e.RowIndex].Cells["Smodel"].Value.ToString();
                string splak = dgvAssets.Rows[e.RowIndex].Cells["Splak"].Value?.ToString();
                int? iusercode = dgvAssets.Rows[e.RowIndex].Cells["Iusercode"].Value == DBNull.Value
                                 ? (int?)null
                                 : Convert.ToInt32(dgvAssets.Rows[e.RowIndex].Cells["Iusercode"].Value);
                string sactivedate = dgvAssets.Rows[e.RowIndex].Cells["Sactivedate"].Value?.ToString();

                using (var f5 = new Form5(
                    connStr,
                    iassetCode,
                    inumb,
                    snme,
                    sserial,
                    smodel,       
                    splak,
                    iusercode,
                    sactivedate
                    ))
                {
                    if (f5.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadAssets();
                    }
                }
            }


            string placeholder = "شماره دارایی یا شماره پلاک را وارد کنید";
            string searchText = (txtSearch.Text ?? "").Trim();
            if (searchText == placeholder) searchText = "";
            LoadAssets(searchText);
        }



        private void button3_Click(object sender, EventArgs e)
        {

            if (dgvAssets.CurrentRow == null)
            {
                MessageBox.Show("لطفا یک دستگاه را انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvAssets.CurrentRow;

            string userCode = row.Cells["Iusercode"].Value?.ToString();

            if (string.IsNullOrEmpty(userCode))
            {

                string dbPath = Properties.Settings.Default.DatabasePath;
                string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

                int iassetCode = Convert.ToInt32(row.Cells["Iassetcode"].Value);
                string snme = row.Cells["Snme"].Value?.ToString();
                int inumb = Convert.ToInt32(row.Cells["Inumb"].Value);
                string sserial = row.Cells["Sserial"].Value?.ToString();
                string smodel = row.Cells["Smodel"].Value?.ToString();
                string splak = row.Cells["Splak"].Value?.ToString();
                int? iusercode = row.Cells["Iusercode"].Value == DBNull.Value ? (int?)null : Convert.ToInt32(row.Cells["Iusercode"].Value);
                string sactivedate = row.Cells["Sactivedate"].Value?.ToString();

                using (Form5 frm = new Form5(
                    connStr,
                    iassetCode,
                    inumb,
                    snme,
                    sserial,
                    smodel,  
                    splak,
                    iusercode,   
                    sactivedate
                ))
                {
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadAssets();
                    }
                }



            }
            else
            {
                MessageBox.Show("این دستگاه در حال حاضر فعال است.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
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

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {

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


        private void buttonCheckStatus_Click(object sender, EventArgs e)
        {

            if (dgvAssets.CurrentRow == null)
            {
                MessageBox.Show("لطفا یک دستگاه را انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvAssets.CurrentRow;

            bool status = Convert.ToBoolean(row.Cells["Bstatus"].Value);
            if (status)
            {

                string dbPath = Properties.Settings.Default.DatabasePath;
                string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

                int iassetCode = Convert.ToInt32(row.Cells["Iassetcode"].Value);
                string snme = row.Cells["Snme"].Value?.ToString();
                int inumb = Convert.ToInt32(row.Cells["Inumb"].Value);
                string sserial = row.Cells["Sserial"].Value?.ToString();
                string smodel = row.Cells["Smodel"].Value?.ToString();
                string splak = row.Cells["Splak"].Value?.ToString();
                int? iusercode = row.Cells["Iusercode"].Value == null || row.Cells["Iusercode"].Value == DBNull.Value
                    ? (int?)null
                    : Convert.ToInt32(row.Cells["Iusercode"].Value);
                string sactivedate = row.Cells["Sactivedate"].Value?.ToString();

                using (Form7 frm = new Form7(
                     connStr,
                    iassetCode,
                    inumb,
                    snme,        
                    sserial,
                    smodel,
                    splak,
                    iusercode,  
                    sactivedate
                    ))
                {
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadAssets();
                    }
                }

            }
            else
            {
                MessageBox.Show("این دستگاه در حال حاضر غیرفعال است.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void dgvAssets_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        public void RefreshGrid()
        {
            string dbPath = Properties.Settings.Default.DatabasePath;
            string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string query = @"SELECT Iassetcode, Snme, Inumb, Sserial, Smodel, Splak, Iusercode, Sactivedate, Bstatus
                          FROM Asset";
                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvAssets.DataSource = dt;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string dbPath = Properties.Settings.Default.DatabasePath;
            string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";
            Form8 f8 = new Form8(connStr);
            if (f8.ShowDialog() == DialogResult.OK)
            {
                RefreshGrid();
            }
        }

        private void label3_Click(object sender, EventArgs e)
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

    }
}

