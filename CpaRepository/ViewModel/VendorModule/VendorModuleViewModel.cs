using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModel.VendorModule
{
    public class VendorModuleVM
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public List<Core.Models.VendorModule> VendorModules { get; set; }
    }
    public class VendorModuleViewModel
    {
        public int Id { get; set; }

        public string VendorName { get; set; }

        [Required]
        public int VendorId { get; set; }
        
        [Display(Name = "Наименование модуля")]
        [Required(ErrorMessage = "Не указано имя")]
        public string NameModule { get; set; }
       
        [Display(Name = "Описание модуля")]
        [Required(ErrorMessage = "Не указано описание")]
        public string Description { get; set; }
       
        [Display(Name = "Связь с модулями  ТПР")]
        [Required(ErrorMessage = "Не указана связь с МО")]
       // public List<CpaModuleId> CpaModules { get; set; }
        public List<int> CpaModulesId { get; set; }

        [Display(Name = "Связь с модулями  ТПР")]
        public List<SelectListItem> CpaModulesSelectListItem { get; set; }
    }
    public class CpaModuleId
    {
        public int Id { get; set; }
    }
}
