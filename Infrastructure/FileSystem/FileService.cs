using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CpaRepository.Service
{
    public class FileService : IFileService
    {
        public void DeleteFile(string path)
        {
            try
            {
                if (path != null)
                {
                    // проверка на наличие папок
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                        DeleteEmptyFolder(path);
                    }
                    else
                        DeleteEmptyFolder(path);
                }
                else
                    throw new Exception(message: "Путь не можеть быть null");
            }
            catch
            { 
                throw;
            }
        }

        public void Move(string sourceFileName, string destFileName)
        {
            if (File.Exists(sourceFileName))
            {              
                var destPath=Path.GetDirectoryName(destFileName);
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }
                File.Move(sourceFileName, destFileName);
                DeleteEmptyFolder(sourceFileName);
            }
            else
            {
                throw new Exception(message: $"Не существует пути: {sourceFileName}.");
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file, string pathFolder)
        {
            try
            {
                if (pathFolder != null)
                {
                    // проверка на наличие папок
                    if (!Directory.Exists(pathFolder))
                    {
                        Directory.CreateDirectory(pathFolder);
                    }
                }
                else
                    throw new Exception(message: "Путь не можеть быть null");


                if (file != null)
                {
                    using (var fileStream = new FileStream(pathFolder+"\\"+ file.FileName, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return pathFolder + "\\" + file.FileName;
                }
                else
                    throw new Exception(message: "Отсуствует файл для загрузки");
            }
            catch
            {
                throw;
            }

        }
        private void DeleteEmptyFolder(string path)
        {
            string pathFolder = Path.GetDirectoryName(path);
            var directoryInfo = new DirectoryInfo(pathFolder);
            Delete(directoryInfo);

            void Delete(DirectoryInfo di)
            {
                try
                {
                    var ee = di.GetFiles();
                    var ww = di.GetDirectories().Length;
                    if (di.GetFiles().Length == 0 && di.GetDirectories().Length == 0)
                    {
                        di.Delete();
                        Delete(di.Parent);
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    Delete(di.Parent);
                }
            }
        }
    }
}
