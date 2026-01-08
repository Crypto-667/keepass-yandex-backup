using System;
using System.Windows.Forms;

namespace YandexDiskBackup
{
    public partial class ConfigForm : Form
    {
        private readonly YandexDiskConfig m_config;
        private readonly BackupManager m_backupManager;
        
        public ConfigForm(YandexDiskConfig config, BackupManager backupManager)
        {
            InitializeComponent();
            m_config = config;
            m_backupManager = backupManager;
            LoadConfig();
        }
        
        private void LoadConfig()
        {
            txtLogin.Text = m_config.Login;
            txtPassword.Text = m_config.Password;
            chkAutoBackup.Checked = m_config.AutoBackup;
            numRetentionDays.Value = m_config.RetentionDays;
            chkNotifications.Checked = m_config.ShowNotifications;
            txtFolder.Text = m_config.CustomFolder;
            
            UpdateStatus();
        }
        
        private void SaveConfig()
        {
            m_config.Login = txtLogin.Text.Trim();
            m_config.Password = txtPassword.Text;
            m_config.AutoBackup = chkAutoBackup.Checked;
            m_config.RetentionDays = (int)numRetentionDays.Value;
            m_config.ShowNotifications = chkNotifications.Checked;
            m_config.CustomFolder = txtFolder.Text.Trim();
            
            m_config.Save();
        }
        
        private void UpdateStatus()
        {
            bool configured = !string.IsNullOrEmpty(m_config.Login) && 
                            !string.IsNullOrEmpty(m_config.Password);
            
            lblStatus.Text = configured ? "Configured" : "Not configured";
            lblStatus.ForeColor = configured ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            
            if (m_config.LastBackupDate > DateTime.MinValue)
            {
                lblLastBackup.Text = $"Last backup: {m_config.LastBackupDate:yyyy-MM-dd HH:mm}";
            }
            else
            {
                lblLastBackup.Text = "Last backup: Never";
            }
        }
        
        private async void btnTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter login and password first.", 
                    "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            btnTest.Enabled = false;
            btnTest.Text = "Testing...";
            
            try
            {
                using (var webDav = new WebDavClient(txtLogin.Text, txtPassword.Text, txtFolder.Text))
                {
                    bool success = await webDav.TestConnectionAsync();
                    
                    if (success)
                    {
                        MessageBox.Show("Connection successful!", 
                            "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Connection failed. Check your credentials and internet connection.", 
                            "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection test error: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnTest.Enabled = true;
                btnTest.Text = "Test Connection";
            }
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter Yandex.Disk login and password.", 
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            SaveConfig();
            DialogResult = DialogResult.OK;
            Close();
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        
        private void txtLogin_TextChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }
        
        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }
    }
}