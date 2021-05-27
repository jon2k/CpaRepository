using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.ModelsDb
{
    public class Letter:EntityBase
    {
        public string PathLetter { get; set; }
        public List<AgreedModule> AgreedModules { get; set; }
    }
}
