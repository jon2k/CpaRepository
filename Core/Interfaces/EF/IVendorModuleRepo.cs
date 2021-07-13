using CpaRepository.ModelsDb;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.Repository
{
    public interface IVendorModuleRepo:IRepository<VendorModule>, ICommonQuery
    {
       
    }
}
