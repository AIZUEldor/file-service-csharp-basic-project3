using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileService
{
    internal class FileService : IFileService
    {
        private const string DirectoryPath = @"D:\C#Work";

        public FileService()
        {
            
            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);
        }

        
        public Task<List<string>> GetAllFileNamesAsync()
        {
            var files = Directory.GetFiles(DirectoryPath, "*.txt")
                                 .Select(Path.GetFileName)
                                 .Select(name => $"M: {name}")
                                 .ToList();

            return Task.FromResult(files);
        }

  
        public async Task<string> ReadFileAsync(string fileName)
        {
            string filePath = Path.Combine(DirectoryPath, fileName);

            if (!File.Exists(filePath))
                return null;

            return await File.ReadAllTextAsync(filePath);
        }

        // 4) File bor bo'lsa yozmaymiz, false qaytaramiz
        public async Task<bool> WriteFileAsync(string fileName, string text)
        {
            string filePath = Path.Combine(DirectoryPath, fileName);

            if (File.Exists(filePath))
                return false;

            await File.WriteAllTextAsync(filePath, text);
            return true;
        }
    }
}
