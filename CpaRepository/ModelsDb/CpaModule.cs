using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.ModelsDb
{
    public class CpaModule : EntityBase
    {
        [Required]
        public string NameModule { get; set; }
        public string Description { get; set; }
        public List<CpaSubModule> CpaSubModules { get; set; }
    }

}
