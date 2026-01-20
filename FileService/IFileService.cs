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
        Task<bool> WriteFileAsync(string fileName, string text);
        Task<string> ReadFileAsync(string fileName);
        

    }
}
