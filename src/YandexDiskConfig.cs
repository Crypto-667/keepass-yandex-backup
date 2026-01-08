using System;
using System.IO;
using System.Xml;

namespace YandexDiskBackup
{
    public class YandexDiskConfig
    {
        private const string CONFIG_PREFIX = "YandexDiskBackup.";
        private readonly string m_configPath;
        
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
        public bool AutoBackup { get; set; } = true;
        public int RetentionDays { get; set; } = 30;
        public DateTime LastBackupDate { get; set; } = DateTime.MinValue;
        public bool ShowNotifications { get; set; } = true;
        public string CustomFolder { get; set; } = "KeePassBackups";
        
        public YandexDiskConfig()
        {
            m_configPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "KeePass", "YandexDiskBackup.config");
        }
        
        public void Load()
        {
            try
            {
                if (!File.Exists(m_configPath))
                    return;
                    
                var xml = new XmlDocument();
                xml.Load(m_configPath);
                
                var root = xml.DocumentElement;
                if (root == null)
                    return;
                
                Login = GetXmlValue(root, "Login", "");
                Password = GetXmlValue(root, "Password", "");
                AutoBackup = bool.Parse(GetXmlValue(root, "AutoBackup", "true"));
                RetentionDays = int.Parse(GetXmlValue(root, "RetentionDays", "30"));
                ShowNotifications = bool.Parse(GetXmlValue(root, "ShowNotifications", "true"));
                CustomFolder = GetXmlValue(root, "CustomFolder", "KeePassBackups");
                
                string lastBackupStr = GetXmlValue(root, "LastBackupDate", "");
                if (!string.IsNullOrEmpty(lastBackupStr) && DateTime.TryParse(lastBackupStr, out DateTime lastDate))
                {
                    LastBackupDate = lastDate;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Config load error: {ex.Message}");
            }
        }
        
        public void Save()
        {
            try
            {
                var xml = new XmlDocument();
                var root = xml.CreateElement("YandexDiskBackupConfig");
                xml.AppendChild(root);
                
                SetXmlValue(root, "Login", Login);
                SetXmlValue(root, "Password", Password);
                SetXmlValue(root, "AutoBackup", AutoBackup.ToString());
                SetXmlValue(root, "RetentionDays", RetentionDays.ToString());
                SetXmlValue(root, "ShowNotifications", ShowNotifications.ToString());
                SetXmlValue(root, "CustomFolder", CustomFolder);
                SetXmlValue(root, "LastBackupDate", LastBackupDate.ToString("o"));
                
                // Создаем директорию если нужно
                string configDir = Path.GetDirectoryName(m_configPath);
                if (!Directory.Exists(configDir))
                    Directory.CreateDirectory(configDir);
                
                xml.Save(m_configPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Config save error: {ex.Message}");
            }
        }
        
        private string GetXmlValue(XmlElement root, string key, string defaultValue)
        {
            var node = root.SelectSingleNode(key);
            return node?.InnerText ?? defaultValue;
        }
        
        private void SetXmlValue(XmlElement root, string key, string value)
        {
            var node = root.OwnerDocument.CreateElement(key);
            node.InnerText = value;
            root.AppendChild(node);
        }
        
        public bool IsConfigured()
        {
            return !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);
        }
        
        public bool NeedAutoBackup()
        {
            if (!AutoBackup || !IsConfigured())
                return false;
                
            // Проверяем, делали ли бэкап сегодня
            return LastBackupDate.Date < DateTime.Today;
        }
    }
}