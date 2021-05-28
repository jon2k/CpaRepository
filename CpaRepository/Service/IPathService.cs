using System;

namespace CpaRepository.Service
{
    public interface IPathService
    {
        public string GetPathFolderForModule(string nameVendor, string nameVendorModule, DateTime date);
        public string GetPathFolderForLetter(string nameVendor, DateTime date);

    }
}
