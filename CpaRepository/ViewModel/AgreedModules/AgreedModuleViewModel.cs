using CpaRepository.ModelsDb;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CpaRepository.ViewModel.AgreedModules
{
    public class AgreedModuleViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Вендор")]
        [Required(ErrorMessage ="Не указан вендор")]
        public int VendorId { get; set; }
      //  [Required]
        public virtual Vendor Vendor { get; set; }
        [Display(Name = "Вендорный модуль")]
        [Required(ErrorMessage = "Не указан вендорный модуль")]
        public int VendorModuleId { get; set; }
       
        public virtual CpaRepository.ModelsDb.VendorModule VendorModule { get; set; }
        [Display(Name = "Контрольная сумма")]
        [Required(ErrorMessage = "Не указана контрольная сумма")]
        public string CRC { get; set; }
        [Display(Name = "Версия модуля")]
        [Required(ErrorMessage = "Не указана версия")]
        public string Version { get; set; }
        [Display(Name = "Дата согласования")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Не указана дата согласования")]
        public DateTime DateOfAgreement { get; set; }
        [Display(Name = "Ссылка на модуль")]
        //[Required(ErrorMessage = "Не указан путь к модулю")]
        public string PatchVendorModule { get; set; }
        [Display(Name = "Ссылка на письмо")]
        //[Required(ErrorMessage = "Не указан путь к модулю")]
        public string PatchLetter { get; set; }
        [Display(Name = "Номер письма о согласовании (Пример: ТВВ-Ц10-04/24747)")]
        [Required(ErrorMessage = "Не указан номер письма о согласовании")]
        public string NumberLetter { get; set; }
        [Display(Name ="Изменения в модуле")]
        [Required(ErrorMessage = "Не указаны причины изменения модуля")]
        public string Changes { get; set; }
        [Display(Name = "Выберите файл модуля (.zip)")]
        [Required(ErrorMessage = "Не указан файл модуля")]
        [NotMapped]
        public IFormFile FileModule { get; set; }
        [Display(Name = "Выберите файл письма (.pdf)")]
        [Required(ErrorMessage = "Не указан файл письма")]
        [NotMapped]
        public IFormFile FileLetter { get; set; }
    }
}
