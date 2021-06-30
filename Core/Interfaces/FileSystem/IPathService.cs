using System;

namespace CpaRepository.Service
{
    public interface IPathService
    {
        public string GetPathFolderForModule(string envPath, string nameVendor, string nameVendorModule, DateTime date);
        public string GetPathFolderForLetter(string envPath, string nameVendor, DateTime date);

    }
}
