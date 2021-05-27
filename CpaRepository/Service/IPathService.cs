using System;

namespace CpaRepository.Service
{
    public interface IPathService
    {
        public string GetPathModule(string nameVendor, string nameVendorModule, DateTime date);
        public string GetPathLetter(string nameVendor, string nameFile);

    }
}
