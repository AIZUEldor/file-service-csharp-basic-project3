using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FileService
{
    internal interface IFileService
    {
       
        Task<List<string>> GetAllFileNamesAsync();
        Task  AppendLineAsync(string fileName, string line);
        Task<string?> ReadFileAsync(string fileName);
        

    }
}
