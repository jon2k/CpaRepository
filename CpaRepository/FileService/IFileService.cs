using CpaRepository.ModelsDb;
using CpaRepository.ViewModel.AgreedModules;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.FileService
{

    public interface IPathService
    {
        public string GetPathModule(AgreedModuleViewModel model);
        public string GetPathLetter(AgreedModuleViewModel model);

    }
    public class PathServise : IPathService
    {
        private IWebHostEnvironment _environment;
        public PathServise(IWebHostEnvironment webHostEnvironment)
        {
            _environment = webHostEnvironment;
        }
        public string GetPathLetter(AgreedModuleViewModel model)
        {
            throw new NotImplementedException();
        }

        public string GetPathModule(AgreedModuleViewModel model)
        {
            throw new NotImplementedException();
        }
    }
    public interface IFileService
    {
        public Task SaveFileAsync(IFormFile file, string path);
        public bool DeleteFile(string path);

    }
    public class FileService : IFileService
    {
        public bool DeleteFile(string path)
        {
            throw new NotImplementedException();
        }

        public async Task SaveFileAsync(IFormFile file, string path)
        {
            try
            {

                if (path != null)
                {
                    // проверка на наличие папок
                }
                else
                    throw new Exception(message: "Путь не можеть быть null");


                if (file != null)
                {
                    using (var fileStream = new FileStream(path, FileMode.Create))
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
