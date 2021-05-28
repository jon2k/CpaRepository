using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;
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
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path);
                    }
                    else
                        throw new Exception(message: $"Не существует пути: {path}.");
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
                StringBuilder builder = new StringBuilder();
                var res=destFileName.Split('\\');
                for (int i = 0; i < res.Length-1; i++)
                {
                    builder.Append(res[i]);
                    builder.Append("\\");
                }
                if (!Directory.Exists(builder.ToString()))
                {
                    Directory.CreateDirectory(destFileName);
                }
                File.Move(sourceFileName, destFileName);
              
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
    }
}
