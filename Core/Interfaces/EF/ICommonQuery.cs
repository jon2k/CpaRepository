using CpaRepository.ModelsDb;
using System.Collections.Generic;

namespace CpaRepository.Repository
{
    public interface ICommonQuery
    {
        IEnumerable<Vendor> GetAllVendors();
        string GetNameVendor(int id);
        IEnumerable<VendorModule> GetVendorModulesOneVendor(int id);
        IEnumerable<CpaModule> GetAllCpaModules();
    }
}
