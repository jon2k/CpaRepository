using Microsoft.AspNetCore.Hosting;
using System;

namespace CpaRepository.Service
{
    public class PathService : IPathService
    {
        private IWebHostEnvironment _environment;
        public PathService(IWebHostEnvironment webHostEnvironment)
        {
            _environment = webHostEnvironment;
        }
        public string GetPathFolderForLetter(string nameVendor, DateTime date)
        {
            if (_environment != null && nameVendor!=null )
            {
                return _environment.WebRootPath + "\\Root\\Letters\\"
                    + nameVendor + "\\"
                     + date.Date.ToString("dd.MM.yyyy");
            }
            else
                throw new Exception("Недостаточно данных для создания пути сохранения файла письма");
        }

        public string GetPathFolderForModule(string nameVendor, string nameVendorModule, DateTime date)
        {
            if (_environment != null && nameVendor != null && nameVendorModule != null)
            {
                return _environment.WebRootPath + "\\Root\\" 
                    + nameVendor + "\\" 
                    + nameVendorModule + "\\" 
                    + date.Date.ToString("dd.MM.yyyy");
            }
            else
                throw new Exception("Недостаточно данных для создания пути сохранения архива модуля");
        }
    }
}
