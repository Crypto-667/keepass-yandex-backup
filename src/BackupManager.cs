using KeePass.Plugins;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YandexDiskBackup
{
    public class BackupManager
    {
        private readonly IPluginHost m_host;
        private readonly YandexDiskConfig m_config;
        
        public BackupManager(IPluginHost host, YandexDiskConfig config)
        {
            m_host = host;
            m_config = config;
        }
        
        public async Task<bool> CreateBackupAsync(bool silent = false)
        {
            try
            {
                if (!m_config.IsConfigured())
                {
                    if (!silent)
                    {
                        MessageBox.Show("Please configure Yandex.Disk credentials first in Settings.", 
                            "Configuration Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    return false;
                }
                
                var db = m_host.Database;
                if (db == null || db.IOConnectionInfo == null)
                {
                    if (!silent)
                    {
                        MessageBox.Show("No database is open. Please open a database first.", 
                            "No Database", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    return false;
                }
                
                string dbPath = db.IOConnectionInfo.Path;
                if (string.IsNullOrEmpty(dbPath) || !File.Exists(dbPath))
                {
                    if (!silent)
                    {
                        MessageBox.Show("Database file not found or path is empty.", 
                            "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return false;
                }
                
                // Create backup name
                string dbName = Path.GetFileNameWithoutExtension(dbPath);
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string backupName = $"{dbName}_{timestamp}.kdbx";
                
                // Для начала сохраняем локально
                string localBackupDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "KeePass", "YandexBackups");
                
                if (!Directory.Exists(localBackupDir))
                    Directory.CreateDirectory(localBackupDir);
                
                string localBackupPath = Path.Combine(localBackupDir, backupName);
                File.Copy(dbPath, localBackupPath, true);
                
                // Обновляем дату последнего бэкапа
                m_config.LastBackupDate = DateTime.Now;
                m_config.Save();
                
                // Пробуем загрузить на Яндекс.Диск
                bool webDavSuccess = false;
                try
                {
                    using (var webDav = new WebDavClient(m_config.Login, m_config.Password, m_config.CustomFolder))
                    {
                        webDavSuccess = await webDav.UploadFileAsync(localBackupPath, backupName);
                    }
                }
                catch (Exception webDavEx)
                {
                    System.Diagnostics.Debug.WriteLine($"[BackupManager] WebDAV upload failed: {webDavEx.Message}");
                    webDavSuccess = false;
                }
                
                if (m_config.ShowNotifications && !silent)
                {
                    if (webDavSuccess)
                    {
                        MessageBox.Show($"Backup created successfully!\n\nFile: {backupName}\nLocation: Yandex.Disk", 
                            "Backup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Backup saved locally (Yandex.Disk upload failed).\n\nFile: {backupName}\nLocation: {localBackupDir}", 
                            "Backup Complete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                
                // Clean old backups
                CleanOldBackups();
                
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BackupManager] Error: {ex}");
                
                if (!silent)
                {
                    MessageBox.Show($"Backup failed: {ex.Message}", 
                        "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
        }
        
        private void CleanOldBackups()
        {
            if (m_config.RetentionDays <= 0)
                return;
                
            string localBackupDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "KeePass", "YandexBackups");
            
            if (!Directory.Exists(localBackupDir))
                return;
                
            var cutoffDate = DateTime.Now.AddDays(-m_config.RetentionDays);
            
            foreach (var file in Directory.GetFiles(localBackupDir, "*.kdbx"))
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.LastWriteTime < cutoffDate)
                    {
                        File.Delete(file);
                    }
                }
                catch { }
            }
        }
        
        public void CheckAutoBackup()
        {
            if (m_config.NeedAutoBackup())
            {
                Task.Run(async () =>
                {
                    await CreateBackupAsync(true);
                });
            }
        }
        
        public async Task<bool> TestConnectionAsync()
        {
            if (!m_config.IsConfigured())
                return false;
                
            try
            {
                using (var webDav = new WebDavClient(m_config.Login, m_config.Password, m_config.CustomFolder))
                {
                    return await webDav.TestConnectionAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BackupManager] Connection test error: {ex.Message}");
                return false;
            }
        }
    }
}