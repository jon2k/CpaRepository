using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModel.Letter
{
    public class LetterViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Номер письма о согласовании")]
        [Required(ErrorMessage = "Не указан номер письма о согласовании")]      
        public string NumberLetter { get; set; }
       
        [Display(Name = "Дата согласования")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Не указана дата согласования")]
        public DateTime DateOfLetter { get; set; }
       
        [Display(Name = "Вендор")]
        [Required(ErrorMessage = "Не указан вендор")]
        public int VendorId { get; set; }
       
        public virtual Vendor Vendor { get; set; }

        [Display(Name = "Наличие письма в хранилище")]
        public bool ExistLetter { get; set; }

        [Display(Name = "Выберите файл письма (.pdf)")]

        [NotMapped]
        public IFormFile FileLetter { get; set; }

        public IEnumerable<SelectListItem> Vendors { get; set; }
    }
}
