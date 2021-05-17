using CpaRepository.ModelsDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.Models
{
    public class AddedModule
    { 
        
        public Vendor Vendor { get; set; }        
      
        public VendorModule VendorModule { get; set; }
        [Required]
        public string CRC { get; set; }
        [Required]
        public string Version { get; set; }
        [Required]
        public DateTime DateOfAgreement { get; set; }
        [Required]
        public string PatchVendorModule { get; set; }    
    }
}
