using CpaRepository.EF;
using CpaRepository.ModelsDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.Repository
{
    public class AgreedModulesRepo : Repository<AgreedModule>, IAgreedModulesRepo
    {
        public AgreedModulesRepo(ApplicationContext context) : base(context)
        {

        }    
        public IEnumerable<AgreedModule> GetAgreedModulesOneVendor(int id)
        {
            return _db.AgreedModules.Where(n => n.VendorModule.VendorId == id).ToList();
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
        public string GetNameVendorModule(int id)
        {
            return _db.VendorModules.Find(id).NameModule;
        }

        public IEnumerable<VendorModule> GetVendorModulesOneVendor(int id)
        {
            return _db.VendorModules.Where(n => n.VendorId == id).ToList();
        }
        public IEnumerable<Letter> GetLettersOneVendor(int id)
        {
            return _db.Letters.Where(n => n.VendorId == id).ToList();
        }
        public Letter GetLetterById(int id)
        {
            return _db.Letters.Find(id);
        }


    }
}

