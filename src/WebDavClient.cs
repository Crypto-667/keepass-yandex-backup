using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YandexDiskBackup
{
    public class WebDavClient : IDisposable
    {
        private const string YANDEX_WEBDAV_URL = "https://webdav.yandex.ru";
        private readonly System.Net.Http.HttpClient _httpClient;
        private readonly string _basePath;
        
        public WebDavClient(string login, string password, string baseFolder = "KeePassBackups")
        {
            var handler = new System.Net.Http.HttpClientHandler
            {
                Credentials = new NetworkCredential(login, password),
                PreAuthenticate = true
            };
            
            _httpClient = new System.Net.Http.HttpClient(handler)
            {
                Timeout = TimeSpan.FromMinutes(5),
                BaseAddress = new Uri(YANDEX_WEBDAV_URL)
            };
            
            _basePath = baseFolder.Trim('/');
            
            // Добавляем заголовки
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "KeePass-YandexDiskBackup/1.0");
        }
        
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var response = await PropFindAsync("/");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[WebDAV] Connection test failed: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> EnsureDirectoryExistsAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_basePath))
                    return true;
                    
                // Проверяем существование директории
                var checkResponse = await PropFindAsync($"/{_basePath}");
                if (checkResponse.IsSuccessStatusCode)
                    return true;
                
                // Создаем директорию
                var request = new System.Net.Http.HttpRequestMessage(new System.Net.Http.HttpMethod("MKCOL"), $"/{_basePath}");
                var response = await _httpClient.SendAsync(request);
                
                // 409 Conflict означает, что директория уже существует
                if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    return true;
                    
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to create directory: {response.StatusCode} - {error}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[WebDAV] EnsureDirectory failed: {ex.Message}");
                throw;
            }
        }
        
        public async Task<bool> UploadFileAsync(string localFilePath, string remoteFileName)
        {
            try
            {
                // Гарантируем существование директории
                await EnsureDirectoryExistsAsync();
                
                // Читаем файл
                byte[] fileData;
                using (var fs = File.OpenRead(localFilePath))
                {
                    fileData = new byte[fs.Length];
                    await fs.ReadAsync(fileData, 0, (int)fs.Length);
                }
                
                // Формируем путь
                string remotePath = string.IsNullOrEmpty(_basePath) 
                    ? $"/{remoteFileName}" 
                    : $"/{_basePath}/{remoteFileName}";
                
                // Загружаем файл
                var content = new System.Net.Http.ByteArrayContent(fileData);
                var response = await _httpClient.PutAsync(remotePath, content);
                
                if (response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"[WebDAV] Upload successful: {remoteFileName}");
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Upload failed ({response.StatusCode}): {error}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[WebDAV] Upload error: {ex.Message}");
                throw;
            }
        }
        
        private async Task<System.Net.Http.HttpResponseMessage> PropFindAsync(string path, string depth = "0")
        {
            var request = new System.Net.Http.HttpRequestMessage(new System.Net.Http.HttpMethod("PROPFIND"), path);
            request.Headers.Add("Depth", depth);
            
            // Запрашиваем минимальный набор свойств
            request.Content = new System.Net.Http.StringContent(
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <propfind xmlns=""DAV:"">
                    <prop>
                        <resourcetype/>
                        <displayname/>
                        <getcontentlength/>
                        <getlastmodified/>
                    </prop>
                </propfind>",
                Encoding.UTF8,
                "application/xml");
            
            return await _httpClient.SendAsync(request);
        }
        
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}