using System.Collections.Generic;
using System.Linq;
using Core.Interfaces.EF;
using Core.Models;
using Infrastructure.EF;

namespace Infrastructure.Repository
{
    public class VendorModuleRepo:Repository<VendorModule>, IVendorModuleRepo
    {
        public VendorModuleRepo(ApplicationContext context) : base(context)
        {

        }

        public IEnumerable<CpaModule> GetAllCpaModules()
        {
            return _db.CpaModules.ToList();
        }

        public IEnumerable<Vendor> GetAllVendors()
        {
            return _db.Vendors.ToList();
        }

        public string GetNameVendor(int id)
        {
            return _db.Vendors.Find(id).Name;
        }

        public IEnumerable<VendorModule> GetVendorModulesOneVendor(int id)
        {
            return _db.VendorModules.Where(n => n.VendorId == id).ToList();
        }
    }
}
