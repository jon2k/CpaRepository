using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces.FileSystem
{
    public interface IFileService
    {
        public Task<string> SaveFileAsync(IFormFile file, string pathFolder);
        public void DeleteFile(string path);
        public void Move(string sourceFileName, string destFileName);

    }
}
