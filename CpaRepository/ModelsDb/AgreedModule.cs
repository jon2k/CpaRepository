using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.ModelsDb
{
    public class AgreedModule : EntityBase
    {
        //public int VendorId { get; set; }
        //[Required]
        //public Vendor Vendor { get; set; }
        public int VendorModuleId { get; set; }
        [Required]
        public virtual VendorModule VendorModule { get; set; }
        [Required]
        public string CRC { get; set; }
        [Required]
        public string Version { get; set; }
        [Required]
        public DateTime DateOfAgreement { get; set; }
        [Required]
        public string PatchVendorModule { get; set; }
        public string PatchCpaModule { get; set; }
        public string PatchCpaCommit { get; set; }
        public string PatchLetter { get; set; }
    }

}
