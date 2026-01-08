namespace YandexDiskBackup
{
    partial class ConfigForm
    {
        private System.ComponentModel.IContainer components = null;
        
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkAutoBackup;
        private System.Windows.Forms.Label lblRetention;
        private System.Windows.Forms.NumericUpDown numRetentionDays;
        private System.Windows.Forms.CheckBox chkNotifications;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblLastBackup;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.TextBox txtFolder;
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
        
        private void InitializeComponent()
        {
            this.lblLogin = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkAutoBackup = new System.Windows.Forms.CheckBox();
            this.lblRetention = new System.Windows.Forms.Label();
            this.numRetentionDays = new System.Windows.Forms.NumericUpDown();
            this.chkNotifications = new System.Windows.Forms.CheckBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblLastBackup = new System.Windows.Forms.Label();
            this.lblFolder = new System.Windows.Forms.Label();
            this.txtFolder = new System.Windows.Forms.TextBox();
            
            ((System.ComponentModel.ISupportInitialize)(this.numRetentionDays)).BeginInit();
            this.SuspendLayout();
            
            // lblLogin
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new System.Drawing.Point(12, 15);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(87, 13);
            this.lblLogin.TabIndex = 0;
            this.lblLogin.Text = "Yandex.Disk Login:";
            
            // txtLogin
            this.txtLogin.Location = new System.Drawing.Point(120, 12);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(200, 20);
            this.txtLogin.TabIndex = 1;
            this.txtLogin.TextChanged += new System.EventHandler(this.txtLogin_TextChanged);
            
            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(12, 45);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(106, 13);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Yandex.Disk Password:";
            
            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(120, 42);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(200, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            
            // lblFolder
            this.lblFolder.AutoSize = true;
            this.lblFolder.Location = new System.Drawing.Point(12, 75);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(103, 13);
            this.lblFolder.TabIndex = 0;
            this.lblFolder.Text = "Remote Folder Name:";
            
            // txtFolder
            this.txtFolder.Location = new System.Drawing.Point(120, 72);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(200, 20);
            this.txtFolder.TabIndex = 3;
            this.txtFolder.Text = "KeePassBackups";
            
            // chkAutoBackup
            this.chkAutoBackup.AutoSize = true;
            this.chkAutoBackup.Location = new System.Drawing.Point(15, 105);
            this.chkAutoBackup.Name = "chkAutoBackup";
            this.chkAutoBackup.Size = new System.Drawing.Size(133, 17);
            this.chkAutoBackup.TabIndex = 4;
            this.chkAutoBackup.Text = "Automatic daily backup";
            this.chkAutoBackup.UseVisualStyleBackColor = true;
            
            // chkNotifications
            this.chkNotifications.AutoSize = true;
            this.chkNotifications.Location = new System.Drawing.Point(15, 130);
            this.chkNotifications.Name = "chkNotifications";
            this.chkNotifications.Size = new System.Drawing.Size(122, 17);
            this.chkNotifications.TabIndex = 5;
            this.chkNotifications.Text = "Show notifications";
            this.chkNotifications.UseVisualStyleBackColor = true;
            
            // lblRetention
            this.lblRetention.AutoSize = true;
            this.lblRetention.Location = new System.Drawing.Point(12, 160);
            this.lblRetention.Name = "lblRetention";
            this.lblRetention.Size = new System.Drawing.Size(155, 13);
            this.lblRetention.TabIndex = 6;
            this.lblRetention.Text = "Keep backups for (days, 0=forever):";
            
            // numRetentionDays
            this.numRetentionDays.Location = new System.Drawing.Point(170, 158);
            this.numRetentionDays.Maximum = new decimal(new int[] {3650, 0, 0, 0});
            this.numRetentionDays.Name = "numRetentionDays";
            this.numRetentionDays.Size = new System.Drawing.Size(60, 20);
            this.numRetentionDays.TabIndex = 6;
            this.numRetentionDays.Value = new decimal(new int[] {30, 0, 0, 0});
            
            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatus.Location = new System.Drawing.Point(12, 190);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(105, 13);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Status: Unknown";
            
            // lblLastBackup
            this.lblLastBackup.AutoSize = true;
            this.lblLastBackup.Location = new System.Drawing.Point(12, 210);
            this.lblLastBackup.Name = "lblLastBackup";
            this.lblLastBackup.Size = new System.Drawing.Size(72, 13);
            this.lblLastBackup.TabIndex = 9;
            this.lblLastBackup.Text = "Last backup: ";
            
            // btnTest
            this.btnTest.Location = new System.Drawing.Point(15, 235);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(120, 30);
            this.btnTest.TabIndex = 7;
            this.btnTest.Text = "Test Connection";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            
            // btnOK
            this.btnOK.Location = new System.Drawing.Point(164, 235);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            
            // btnCancel
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(245, 235);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
            // ConfigForm
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(334, 280);
            this.Controls.Add(this.lblFolder);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.lblLastBackup);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.numRetentionDays);
            this.Controls.Add(this.lblRetention);
            this.Controls.Add(this.chkNotifications);
            this.Controls.Add(this.chkAutoBackup);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.lblLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Yandex.Disk Backup Settings";
            
            ((System.ComponentModel.ISupportInitialize)(this.numRetentionDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}