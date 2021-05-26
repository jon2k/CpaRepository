using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CpaRepository.ViewModel.VendorModule
{
    public class VendorModuleViewModel
    {
        public int Id { get; set; }
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
        public int[] CpaModulesId { get; set; }
        // public int CpaModuleId { get; set; }
        [Display(Name = "Связь с модулями  ТПР")]
        public List<SelectListItem> CpaModules { get; set; }
    }
}
