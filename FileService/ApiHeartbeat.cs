using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FileService;

namespace FileService
{
    internal class ApiHeartbeat
    {
        private readonly IFileService _files;
        private readonly string _url;
        private readonly HttpClient _client = new HttpClient();

        public ApiHeartbeat(IFileService files, string url)
        {
            _files = files;
            _url = url;
        }

        // Har 5 sekundda ishlaydi, to‘xtatish uchun token
        public async Task RunAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                string dailyFile = $"{DateTime.Now:yyyy-MM-dd}.txt";

                try
                {
                    var resp = await _client.GetAsync(_url, token);
                    string body = await resp.Content.ReadAsStringAsync(token);

                    string logLine =
                        $"[{DateTime.Now:HH:mm:ss}] Status={(int)resp.StatusCode} {resp.StatusCode} | {body}";

                    await _files.AppendLineAsync(dailyFile, logLine);
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Yozildi -> {dailyFile}");
                }
                catch (OperationCanceledException)
                {
                    // to‘xtatildi
                }
                catch (Exception ex)
                {
                    string logLine = $"[{DateTime.Now:HH:mm:ss}] ERROR | {ex.Message}";
                    await _files.AppendLineAsync(dailyFile, logLine);
                    Console.WriteLine(logLine);
                }

                await Task.Delay(5000, token);
            }
        }
    }
}
