using CpaRepository.EF;
using CpaRepository.ModelsDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.Repository
{
    public class AgreedModuleRepository : Repository<AgreedModule>, IAgreedModuleRepository
    {
        public AgreedModuleRepository(ApplicationContext context) : base(context)
        {
        }



        public IEnumerable<AgreedModule> GetLastModuleVendor(Vendor vendor)
        {
            if (vendor == null)
                throw new ArgumentNullException($"{nameof(GetModuleForPeriod)} entity must not be null");
            else
            {
                var result = Context.AgreedModules.Where(v => v.VendorModule.Vendor.Equals(vendor))
                    .GroupBy(m => m.VendorModule)
                    //.Select(n => new { dd = n.Key, ww = n.OrderBy(o => o.DateOfAgreement).FirstOrDefault() });
                    .Select(group => new { name = group.Key, All = group.OrderByDescending(x => x.DateOfAgreement) })
                    .OrderBy(group => group.All.FirstOrDefault())
                    .SelectMany(r => r.All);


                return result;
            }
        }

        public IEnumerable<AgreedModule> GetModuleForDate(DateTime date, Vendor vendor)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AgreedModule> GetModuleForPeriod(DateTime dateStart, DateTime dateStop, Vendor vendor)
        {
            if (vendor == null)
                throw new ArgumentNullException($"{nameof(GetModuleForPeriod)} entity must not be null");
            else if (dateStart <= dateStop)
                throw new Exception($"{nameof(GetModuleForPeriod)} incorrect dates");
            else
                return Context.AgreedModules.Where(n => n.VendorModule.Vendor.Equals(vendor))
                    .Where(d => d.DateOfAgreement >= dateStart && d.DateOfAgreement <= dateStop);
        }
    }

}
