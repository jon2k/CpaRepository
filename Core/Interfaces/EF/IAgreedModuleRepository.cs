using System;
using System.Collections.Generic;
using Core.Models;

namespace Core.Interfaces.EF
{
    public interface IAgreedModuleRepository : IRepository<AgreedModule>
    {
        // Get all last agreed modules of concrete vendor
        IEnumerable<AgreedModule> GetLastModuleVendor(Vendor vendor);
        // Get all agreed modules of a concrete vendor for a period
        IEnumerable<AgreedModule> GetModuleForPeriod(DateTime dateStart, DateTime dateStop, Vendor vendor);
        // Get all agreed modules of a concrete vendor for a concrete date
        IEnumerable<AgreedModule> GetModuleForDate(DateTime date, Vendor vendor);
    }

}
