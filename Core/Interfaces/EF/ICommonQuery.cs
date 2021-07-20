using System.Collections.Generic;
using Core.Models;

namespace Core.Interfaces.EF
{
    public interface ICommonQuery
    {
        IEnumerable<Vendor> GetAllVendors();
        string GetNameVendor(int id);
        IEnumerable<VendorModule> GetVendorModulesOneVendor(int id);
        IEnumerable<CpaModule> GetAllCpaModules();
    }
}
