using CpaRepository.ModelsDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.Repository
{
    public interface ILetterRepo : IRepository<Letter>
    {
        IEnumerable<Vendor> GetAllVendors();
        string GetNameVendor(int id);
        IEnumerable<VendorModule> GetVendorModulesOneVendor(int id);
        IEnumerable<CpaModule> GetAllCpaModules();

    }
}
