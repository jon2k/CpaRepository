﻿using CpaRepository.EF;
using CpaRepository.ModelsDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.Repository
{
    public class LetterRepo: Repository<Letter>, ILetterRepo
    {
        public LetterRepo(ApplicationContext context) : base(context)
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
        public string GetVendorModule(int id)
        {
            return _db.VendorModules.Find(id).NameModule;
        }

        public IEnumerable<VendorModule> GetVendorModulesOneVendor(int id)
        {
            return _db.VendorModules.Where(n => n.VendorId == id).ToList();
        }
    }
}
