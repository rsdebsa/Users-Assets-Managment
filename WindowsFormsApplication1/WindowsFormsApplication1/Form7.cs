using System;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Globalization;


namespace WindowsFormsApplication1
{
    public partial class Form7 : Form
    {
        private string connStr;
        private int assetCode;
        private string snme;
        private int inumb;
        private string sserial;
        private string smodel;
        private string splak;
        private int? iusercode;
        private string sactivedate;

        public Form7(string connStr, int iassetCode, int inumb, string snme,
                     string sserial, string smodel, string splak, int? iusercode, string sactivedate)
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterParent;

            this.connStr = connStr;
            this.inumb = inumb;
            this.snme = snme;
            this.assetCode = iassetCode;
            this.sserial = sserial;
            this.smodel = smodel;
            this.splak = splak;
            this.iusercode = iusercode;
            this.sactivedate = sactivedate;
        }
        private string GetPersianDate(DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            return $"{pc.GetYear(dt)}/{pc.GetMonth(dt):00}/{pc.GetDayOfMonth(dt):00}";
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("لطفا دلیل غیرفعال‌سازی را انتخاب کنید.");
                return;
            }

            string reason = comboBox1.SelectedItem.ToString();
            string deactiveDate = GetPersianDate(DateTime.Now);

            DialogResult dr = MessageBox.Show(
                $"آیا از غیرفعال‌سازی دستگاه:\n" +
                $"📌 نام دستگاه: {snme}-{smodel}\n" +
                $"📌 شماره دارایی: {inumb}\n" +
                $"📌 شماره سریال: {sserial}\n" +
                $"📌 شماره پلاک: {splak}\n\n" +
                $"اطمینان دارید؟",
                "تایید غیرفعال‌سازی",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr != DialogResult.Yes)
                return;

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
                using (OleDbTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        string updateAsset = "UPDATE Asset SET Iusercode = NULL, Splak = NULL, Sactivedate = NULL, Bstatus = False WHERE Inumb = ?";
                        using (OleDbCommand cmd = new OleDbCommand(updateAsset, conn, tran))
                        {
                            cmd.Parameters.Add("?", OleDbType.Integer).Value = inumb;
                            cmd.ExecuteNonQuery();
                        }

                        string insertHistory = @"INSERT INTO AssetHis 
                                (Snme, Inumb, Sserial, Smodel, Splak, Iusercode, Iassetcode, Sactivedate, Bstatus) 
                                VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

                        using (OleDbCommand cmd = new OleDbCommand(insertHistory, conn, tran))
                        {
                            cmd.Parameters.Add("?", OleDbType.VarWChar).Value = snme;
                            cmd.Parameters.Add("?", OleDbType.Integer).Value = inumb;
                            cmd.Parameters.Add("?", OleDbType.VarWChar).Value = sserial;
                            cmd.Parameters.Add("?", OleDbType.VarWChar).Value = smodel;
                            cmd.Parameters.Add("?", OleDbType.VarWChar).Value = string.IsNullOrEmpty(splak) ? (object)DBNull.Value : splak;
                            cmd.Parameters.Add("?", OleDbType.Integer).Value = iusercode.HasValue ? (object)iusercode.Value : DBNull.Value;
                            cmd.Parameters.Add("?", OleDbType.Integer).Value = assetCode;
                            cmd.Parameters.Add("?", OleDbType.VarWChar).Value = deactiveDate;
                            cmd.Parameters.Add("?", OleDbType.Boolean).Value = false;

                            cmd.ExecuteNonQuery();
                        }


                        tran.Commit();
                        MessageBox.Show("دستگاه با موفقیت غیرفعال شد و در تاریخچه ثبت گردید.");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("خطا در ثبت تغییرات: " + ex.Message);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

