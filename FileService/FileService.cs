using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileService;

namespace FileService
{
    internal class FileService : IFileService
    {
        private const string DirectoryPath = @"D:\C#Work";
        private readonly SemaphoreSlim _gate = new SemaphoreSlim(1, 1);

        public FileService()
        {
            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);
        }

        public Task<List<string>> GetAllFileNamesAsync()
        {
            var files = Directory.GetFiles(DirectoryPath, "*.txt")
                                 .Select(Path.GetFileName)
                                 .OrderBy(x => x)
                                 .Select(x => $"M: {x}")
                                 .ToList();

            return Task.FromResult(files);
        }

        public async Task<string?> ReadFileAsync(string fileName)
        {
            string filePath = Path.Combine(DirectoryPath, fileName);
            if (!File.Exists(filePath))
                return null;

            await _gate.WaitAsync();
            try
            {
                return await File.ReadAllTextAsync(filePath);
            }
            finally
            {
                _gate.Release();
            }
        }

        // API javobini faylga "qo'shib borish" (append)
        public async Task AppendLineAsync(string fileName, string line)
        {
            string filePath = Path.Combine(DirectoryPath, fileName);

            await _gate.WaitAsync();
            try
            {
                // Har yozishda oxiriga yangi qator qo‘shiladi
                await File.AppendAllTextAsync(filePath, line + System.Environment.NewLine);
            }
            finally
            {
                _gate.Release();
            }
        }
    }
}
