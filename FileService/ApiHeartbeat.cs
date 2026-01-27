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
                }
                catch (OperationCanceledException)
                {
                    // to‘xtatildi — hech narsa yozmaymiz
                }
                catch (Exception ex)
                {
                    string logLine = $"[{DateTime.Now:HH:mm:ss}] ERROR | {ex.Message}";
                    await _files.AppendLineAsync(dailyFile, logLine);
                }

                await Task.Delay(100, token);
            }
        }
    }
}
