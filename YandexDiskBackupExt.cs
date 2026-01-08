using KeePass.Plugins;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace YandexDiskBackup
{
    public sealed class YandexDiskBackupExt : Plugin
    {
        private IPluginHost m_host = null;
        private ToolStripMenuItem m_tsmiToolsMenu = null;
        private YandexDiskConfig m_config = null;
        private BackupManager m_backupManager = null;
        
        public override bool Initialize(IPluginHost host)
        {
            Debug.WriteLine("=== YandexDiskBackup: Initialize called ===");
            
            if (host == null) 
            {
                Debug.WriteLine("Host is null!");
                return false;
            }
            
            m_host = host;
            
            try
            {
                // Инициализируем конфигурацию и менеджер
                m_config = new YandexDiskConfig();
                m_config.Load();
                
                m_backupManager = new BackupManager(m_host, m_config);
                
                // Создаем меню
                CreateMenu();
                
                Debug.WriteLine("Plugin initialized successfully!");
                
                // Проверяем авто-бэкап
                System.Threading.Tasks.Task.Delay(3000).ContinueWith(t =>
                {
                    CheckAutoBackup();
                });
                
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Initialization error: {ex}");
                MessageBox.Show($"Plugin initialization error: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        private void CreateMenu()
        {
            // Создаем корневое меню
            m_tsmiToolsMenu = new ToolStripMenuItem();
            m_tsmiToolsMenu.Text = "Yandex.Disk Backup";
            
            // Добавляем подпункты
            var backupItem = new ToolStripMenuItem();
            backupItem.Text = "Backup Now";
            backupItem.Click += OnBackupNow;
            m_tsmiToolsMenu.DropDownItems.Add(backupItem);
            
            var settingsItem = new ToolStripMenuItem();
            settingsItem.Text = "Settings";
            settingsItem.Click += OnSettings;
            m_tsmiToolsMenu.DropDownItems.Add(settingsItem);
            
            var testItem = new ToolStripMenuItem();
            testItem.Text = "Test Connection";
            testItem.Click += OnTestConnection;
            m_tsmiToolsMenu.DropDownItems.Add(testItem);
            
            var separator = new ToolStripSeparator();
            m_tsmiToolsMenu.DropDownItems.Add(separator);
            
            var aboutItem = new ToolStripMenuItem();
            aboutItem.Text = "About";
            aboutItem.Click += OnAbout;
            m_tsmiToolsMenu.DropDownItems.Add(aboutItem);
            
            // Добавляем в главное меню Tools
            m_host.MainWindow.ToolsMenu.DropDownItems.Add(m_tsmiToolsMenu);
        }
        
        private async void OnBackupNow(object sender, EventArgs e)
        {
            var backupItem = sender as ToolStripMenuItem;
            if (backupItem != null)
            {
                backupItem.Enabled = false;
                backupItem.Text = "Backing up...";
                
                try
                {
                    bool success = await m_backupManager.CreateBackupAsync();
                }
                finally
                {
                    backupItem.Enabled = true;
                    backupItem.Text = "Backup Now";
                }
            }
        }
        
        private void OnSettings(object sender, EventArgs e)
        {
            using (var form = new ConfigForm(m_config, m_backupManager))
            {
                form.ShowDialog();
            }
        }
        
        private async void OnTestConnection(object sender, EventArgs e)
        {
            if (!m_config.IsConfigured())
            {
                MessageBox.Show("Please configure Yandex.Disk credentials first in Settings.", 
                    "Configuration Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var testItem = sender as ToolStripMenuItem;
            if (testItem != null)
            {
                testItem.Enabled = false;
                testItem.Text = "Testing...";
                
                try
                {
                    bool success = await m_backupManager.TestConnectionAsync();
                    
                    if (success)
                    {
                        MessageBox.Show("Connection successful!", 
                            "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Connection failed!", 
                            "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                finally
                {
                    testItem.Enabled = true;
                    testItem.Text = "Test Connection";
                }
            }
        }
        
        private void OnAbout(object sender, EventArgs e)
        {
            string message = @"Yandex.Disk Backup Plugin v1.1

        Features / Возможности:
        • Backup to Yandex.Disk via WebDAV
        • Automatic daily backups
        • Configurable retention period
        • Connection testing

        • Резервное копирование на Яндекс.Диск через WebDAV
        • Автоматическое ежедневное резервное копирование
        • Настраиваемый период хранения
        • Тестирование подключения

        Связь/Сommunication: https://t.me/Crypto_667";
    
            MessageBox.Show(message, 
                "About / О программе", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }
        
        private void CheckAutoBackup()
        {
            try
            {
                m_backupManager.CheckAutoBackup();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Auto-backup check error: {ex.Message}");
            }
        }
        
        public override void Terminate()
        {
            Debug.WriteLine("=== YandexDiskBackup: Terminate called ===");
            
            // Сохраняем конфигурацию
            m_config?.Save();
            
            // Очищаем меню
            if (m_tsmiToolsMenu != null)
            {
                m_tsmiToolsMenu.Dispose();
                m_tsmiToolsMenu = null;
            }
        }
        
        public override string UpdateUrl
        {
            get { return "https://example.com/keepass/update_check/"; }
        }
    }
}