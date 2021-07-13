using CpaRepository.ModelsDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.Repository
{
    public interface IAgreedModulesRepo : IRepository<AgreedModule>, ICommonQuery
    {    
        IEnumerable<Letter> GetLettersOneVendor(int id);
        Letter GetLetterById(int id);
        IEnumerable<AgreedModule> GetAgreedModulesOneVendor(int id);
        string GetNameVendorModule(int id);
    }
}
