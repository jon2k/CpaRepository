using CpaRepository.ModelsDb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.ViewModel.Letter
{
    public class LetterViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Номер письма о согласовании")]
        [Required(ErrorMessage = "Не указан номер письма о согласовании")]      
        public string NumberLetter { get; set; }
      //  public string PathLetter { get; set; }
       
        [Display(Name = "Дата согласования")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Не указана дата согласования")]
        public DateTime DateOfLetter { get; set; }
       
        [Display(Name = "Вендор")]
        [Required(ErrorMessage = "Не указан вендор")]
        public int VendorId { get; set; }
        // public int CpaModuleId { get; set; }
       
       // [Display(Name = "Вендор")]
        public virtual Vendor Vendor { get; set; }
        
        [Display(Name = "Выберите файл письма (.pdf)")]
       // [Required(ErrorMessage = "Не указан файл письма")]
        [NotMapped]
        public IFormFile FileLetter { get; set; }
    }
}
