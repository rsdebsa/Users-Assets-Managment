namespace WindowsFormsApplication1
{
    partial class Form6
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvAssets = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonCheckStatus = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textSactivatedate = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textIusercode = new System.Windows.Forms.TextBox();
            this.textSplak = new System.Windows.Forms.TextBox();
            this.textSmodel = new System.Windows.Forms.TextBox();
            this.textSserial = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIassetcode = new System.Windows.Forms.TextBox();
            this.lblNationalCode = new System.Windows.Forms.Label();
            this.txtInumb = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtAssetName = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssets)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvAssets
            // 
            this.dgvAssets.AllowUserToAddRows = false;
            this.dgvAssets.AllowUserToDeleteRows = false;
            this.dgvAssets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAssets.Location = new System.Drawing.Point(24, 35);
            this.dgvAssets.MultiSelect = false;
            this.dgvAssets.Name = "dgvAssets";
            this.dgvAssets.ReadOnly = true;
            this.dgvAssets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAssets.Size = new System.Drawing.Size(905, 237);
            this.dgvAssets.TabIndex = 3;
            this.dgvAssets.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAssets_CellContentClick_1);
            this.dgvAssets.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAssets_CellMouseMove);
            this.dgvAssets.SelectionChanged += new System.EventHandler(this.dgvAssets_SelectionChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.buttonCheckStatus);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textSactivatedate);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textIusercode);
            this.groupBox1.Controls.Add(this.textSplak);
            this.groupBox1.Controls.Add(this.textSmodel);
            this.groupBox1.Controls.Add(this.textSserial);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtIassetcode);
            this.groupBox1.Controls.Add(this.lblNationalCode);
            this.groupBox1.Controls.Add(this.txtInumb);
            this.groupBox1.Controls.Add(this.lblLastName);
            this.groupBox1.Controls.Add(this.txtAssetName);
            this.groupBox1.Controls.Add(this.lblFirstName);
            this.groupBox1.Location = new System.Drawing.Point(24, 287);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox1.Size = new System.Drawing.Size(905, 181);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "اطلاعات دستگاه ها";
            // 
            // buttonCheckStatus
            // 
            this.buttonCheckStatus.Font = new System.Drawing.Font("B Titr", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.buttonCheckStatus.ForeColor = System.Drawing.Color.Black;
            this.buttonCheckStatus.Location = new System.Drawing.Point(142, 139);
            this.buttonCheckStatus.Name = "buttonCheckStatus";
            this.buttonCheckStatus.Size = new System.Drawing.Size(130, 35);
            this.buttonCheckStatus.TabIndex = 35;
            this.buttonCheckStatus.Text = "غیرفعالسازی دستگاه";
            this.buttonCheckStatus.UseVisualStyleBackColor = true;
            this.buttonCheckStatus.Click += new System.EventHandler(this.buttonCheckStatus_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label6.Location = new System.Drawing.Point(807, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "تاریخ فعالسازی";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // textSactivatedate
            // 
            this.textSactivatedate.Location = new System.Drawing.Point(654, 122);
            this.textSactivatedate.Name = "textSactivatedate";
            this.textSactivatedate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textSactivatedate.Size = new System.Drawing.Size(150, 20);
            this.textSactivatedate.TabIndex = 32;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("B Titr", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.button2.Location = new System.Drawing.Point(6, 98);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(130, 35);
            this.button2.TabIndex = 31;
            this.button2.Text = "افزودن دستگاه جدید";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("B Titr", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(6, 139);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 35);
            this.button1.TabIndex = 30;
            this.button1.Text = "بازگشت به منوی اصلی";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textIusercode
            // 
            this.textIusercode.Location = new System.Drawing.Point(390, 122);
            this.textIusercode.Name = "textIusercode";
            this.textIusercode.Size = new System.Drawing.Size(150, 20);
            this.textIusercode.TabIndex = 26;
            // 
            // textSplak
            // 
            this.textSplak.Location = new System.Drawing.Point(390, 53);
            this.textSplak.Name = "textSplak";
            this.textSplak.Size = new System.Drawing.Size(150, 20);
            this.textSplak.TabIndex = 27;
            // 
            // textSmodel
            // 
            this.textSmodel.Location = new System.Drawing.Point(654, 53);
            this.textSmodel.Name = "textSmodel";
            this.textSmodel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textSmodel.Size = new System.Drawing.Size(150, 20);
            this.textSmodel.TabIndex = 28;
            // 
            // textSserial
            // 
            this.textSserial.Location = new System.Drawing.Point(654, 83);
            this.textSserial.Name = "textSserial";
            this.textSserial.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textSserial.Size = new System.Drawing.Size(150, 20);
            this.textSserial.TabIndex = 29;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(546, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "کد پرسنلی تحویل";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(573, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "شماره پلاک";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(821, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "مدل دستگاه";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("B Titr", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.button3.ForeColor = System.Drawing.Color.Black;
            this.button3.Location = new System.Drawing.Point(142, 98);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(130, 35);
            this.button3.TabIndex = 21;
            this.button3.Text = "فعالسازی دستگاه";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(815, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "شماره سریال";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // txtIassetcode
            // 
            this.txtIassetcode.Location = new System.Drawing.Point(390, 87);
            this.txtIassetcode.Name = "txtIassetcode";
            this.txtIassetcode.Size = new System.Drawing.Size(150, 20);
            this.txtIassetcode.TabIndex = 13;
            // 
            // lblNationalCode
            // 
            this.lblNationalCode.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblNationalCode.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblNationalCode.Location = new System.Drawing.Point(554, 86);
            this.lblNationalCode.Name = "lblNationalCode";
            this.lblNationalCode.Size = new System.Drawing.Size(80, 20);
            this.lblNationalCode.TabIndex = 14;
            this.lblNationalCode.Text = "کد دارایی";
            // 
            // txtInumb
            // 
            this.txtInumb.Location = new System.Drawing.Point(390, 23);
            this.txtInumb.Name = "txtInumb";
            this.txtInumb.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtInumb.Size = new System.Drawing.Size(150, 20);
            this.txtInumb.TabIndex = 15;
            // 
            // lblLastName
            // 
            this.lblLastName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblLastName.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblLastName.Location = new System.Drawing.Point(554, 26);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(80, 20);
            this.lblLastName.TabIndex = 16;
            this.lblLastName.Text = "شماره دارایی";
            // 
            // txtAssetName
            // 
            this.txtAssetName.Location = new System.Drawing.Point(654, 23);
            this.txtAssetName.Name = "txtAssetName";
            this.txtAssetName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtAssetName.Size = new System.Drawing.Size(150, 20);
            this.txtAssetName.TabIndex = 17;
            // 
            // lblFirstName
            // 
            this.lblFirstName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblFirstName.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblFirstName.Location = new System.Drawing.Point(824, 26);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(60, 20);
            this.lblFirstName.TabIndex = 18;
            this.lblFirstName.Text = "نام دستگاه";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(637, 9);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(228, 20);
            this.txtSearch.TabIndex = 6;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(871, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "جست و جو";
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.ClientSize = new System.Drawing.Size(954, 473);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvAssets);
            this.Name = "Form6";
            this.Text = "مدیریت دستگاه ها";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form6_FormClosing);
            this.Load += new System.EventHandler(this.Form6_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssets)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvAssets;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIassetcode;
        private System.Windows.Forms.Label lblNationalCode;
        private System.Windows.Forms.TextBox txtInumb;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.TextBox txtAssetName;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textIusercode;
        private System.Windows.Forms.TextBox textSplak;
        private System.Windows.Forms.TextBox textSmodel;
        private System.Windows.Forms.TextBox textSserial;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textSactivatedate;
        private System.Windows.Forms.Button buttonCheckStatus;
    }
}