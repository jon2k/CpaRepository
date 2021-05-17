using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.ModelsDb
{
    public class CpaSubModule : EntityBase
    {
        public int ModuleId { get; set; }
        [Required]
        public CpaModule Module { get; set; }
        [Required]
        public string NameSubModule { get; set; }
        [Required]
        public string Description { get; set; }
        public List<RelationCpaModuleWithVendorModule> Relations { get; set; }
    }

}
