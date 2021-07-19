using System;
using Core.Interfaces.FileSystem;

namespace Infrastructure.FileSystem
{
    public class PathService : IPathService
    {
        //private IWebHostEnvironment _environment;
        public PathService()
        {
          //  _environment = webHostEnvironment;
        }
        public string GetPathFolderForLetter(string envPath, string nameVendor, DateTime date)
        {
            if (envPath != null && nameVendor!=null )
            {
                /*return _environment.WebRootPath + "\\Root\\Letters\\"
                    + nameVendor + "\\"
                     + date.Date.ToString("dd.MM.yyyy");*/
                return envPath + "\\Root\\Letters\\"
                   + nameVendor + "\\"
                    + date.Date.ToString("dd.MM.yyyy");
            }
            else
                throw new Exception("Недостаточно данных для создания пути сохранения файла письма");
        }

        public string GetPathFolderForModule(string envPath, string nameVendor, string nameVendorModule, DateTime date)
        {
            if (envPath != null && nameVendor != null && nameVendorModule != null)
            {
                return envPath + "\\Root\\Modules\\" 
                    + nameVendor + "\\" 
                    + nameVendorModule + "\\" 
                    + date.Date.ToString("dd.MM.yyyy");
            }
            else
                throw new Exception("Недостаточно данных для создания пути сохранения архива модуля");
        }
    }
}
