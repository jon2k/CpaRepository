using System.Collections.Generic;
using Core.Models;

namespace Core.Interfaces.EF
{
    public interface IAgreedModulesRepo : IRepository<AgreedModule>, ICommonQuery
    {    
        IEnumerable<Letter> GetLettersOneVendor(int id);
        Letter GetLetterById(int id);
        IEnumerable<AgreedModule> GetAgreedModulesOneVendor(int id);
        string GetNameVendorModule(int id);
    }
}
