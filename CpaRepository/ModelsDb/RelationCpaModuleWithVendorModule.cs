using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.ModelsDb
{
    public class RelationCpaModuleWithVendorModule : EntityBase
    {
        public int CpaSubModuleId { get; set; }
        [Required]
        public CpaSubModule CpaSubModule { get; set; }
        public int VendorModuleId { get; set; }
        [Required]
        public VendorModule VendorModule { get; set; }
    }

}
