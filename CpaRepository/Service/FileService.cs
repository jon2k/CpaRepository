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
            if (Directory.Exists(sourceFileName))
            {
                if (Directory.Exists(destFileName))
                {
                    File.Move(sourceFileName, destFileName);
                }
                else
                {
                    throw new Exception(message: $"Не существует пути: {destFileName}.");
                }
            }
            else
            {
                throw new Exception(message: $"Не существует пути: {sourceFileName}.");
            }
        }

        public async Task SaveFileAsync(IFormFile file, string path)
        {
            try
            {

                if (path != null)
                {
                    // проверка на наличие папок
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
                else
                    throw new Exception(message: "Путь не можеть быть null");


                if (file != null)
                {
                    using (var fileStream = new FileStream(path+"\\"+ file.FileName, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
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
