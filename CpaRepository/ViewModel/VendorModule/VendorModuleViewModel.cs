using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.ViewModel.VendorModule
{
    public class VendorModuleViewModel
    {
        public int VendorId { get; set; }     
        public string NameModule { get; set; }
        public string Description { get; set; }
        public int[] CpaModulesId { get; set; }
        public int CpaModuleId { get; set; }
        public List<SelectListItem> CpaModules { get; set; }
    }
}
