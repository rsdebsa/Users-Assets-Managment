namespace WindowsFormsApplication1
{
    using System.Drawing;

    partial class Form3
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
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelReports = new System.Windows.Forms.Panel();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.bntUsersReport = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panelReports.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(274, 42);
            this.button1.TabIndex = 0;
            this.button1.Text = "انتخاب دیتابیس";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(21, 119);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(274, 42);
            this.button3.TabIndex = 2;
            this.button3.Text = "مدیریت کاربران";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(21, 174);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(274, 42);
            this.button4.TabIndex = 3;
            this.button4.Text = "مدیریت دستگاه ها";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(21, 229);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(274, 42);
            this.button5.TabIndex = 4;
            this.button5.Text = "دریافت گزارش";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            this.button5.MouseHover += new System.EventHandler(this.button5_MouseHover);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("B Titr", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label1.Location = new System.Drawing.Point(97, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 23);
            this.label1.TabIndex = 5;
            this.label1.Text = "برنامه مدیریت دیتا بیس";
            // 
            // button2
            // 
            this.button2.ForeColor = System.Drawing.Color.Red;
            this.button2.Location = new System.Drawing.Point(21, 298);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 40);
            this.button2.TabIndex = 6;
            this.button2.Text = "خروج از برنامه";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button6
            // 
            this.button6.ForeColor = System.Drawing.Color.Red;
            this.button6.Location = new System.Drawing.Point(207, 298);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(88, 40);
            this.button6.TabIndex = 7;
            this.button6.Text = "پشتیبانی";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(596, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(316, 364);
            this.panel1.TabIndex = 9;
            // 
            // panelReports
            // 
            this.panelReports.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.panelReports.Controls.Add(this.button9);
            this.panelReports.Controls.Add(this.button8);
            this.panelReports.Controls.Add(this.button7);
            this.panelReports.Controls.Add(this.bntUsersReport);
            this.panelReports.Location = new System.Drawing.Point(283, 160);
            this.panelReports.Name = "panelReports";
            this.panelReports.Size = new System.Drawing.Size(316, 270);
            this.panelReports.TabIndex = 10;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(18, 198);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(274, 42);
            this.button9.TabIndex = 14;
            this.button9.Text = "گزارش دستگاه های کاربران";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(18, 138);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(274, 42);
            this.button8.TabIndex = 13;
            this.button8.Text = "گزارش دستگاه های فعال";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(18, 78);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(274, 42);
            this.button7.TabIndex = 12;
            this.button7.Text = "گزارش تاریخچه کاربران";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // bntUsersReport
            // 
            this.bntUsersReport.Location = new System.Drawing.Point(18, 19);
            this.bntUsersReport.Name = "bntUsersReport";
            this.bntUsersReport.Size = new System.Drawing.Size(274, 42);
            this.bntUsersReport.TabIndex = 11;
            this.bntUsersReport.Text = "گزارش کاربران مشغول به کار";
            this.bntUsersReport.UseVisualStyleBackColor = true;
            this.bntUsersReport.Click += new System.EventHandler(this.bntUsersReport_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Coral;
            this.BackgroundImage = global::WindowsFormsApplication1.Properties.Resources.gjbibvn;
            this.ClientSize = new System.Drawing.Size(954, 473);
            this.Controls.Add(this.panelReports);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form3";
            this.Text = "منوی اصلی";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form3_FormClosing);
            this.MouseHover += new System.EventHandler(this.Form3_MouseHover);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelReports.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelReports;
        private System.Windows.Forms.Button bntUsersReport;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
    }
}