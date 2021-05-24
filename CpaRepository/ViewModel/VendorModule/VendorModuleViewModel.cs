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
        [Required(ErrorMessage = "Не указано имя")]
        public string NameModule { get; set; }
        [Required(ErrorMessage = "Не указано описание")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Не указана связь с МО")]
        public int[] CpaModulesId { get; set; }
       // public int CpaModuleId { get; set; }
        public List<SelectListItem> CpaModules { get; set; }
    }
}
