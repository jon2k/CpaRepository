using CpaRepository.ModelsDb;
using System;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Загрузить модуль")]
        [Required(ErrorMessage = "Не указан путь к модулю")]
        public string PatchVendorModule { get; set; }
        [Display(Name = "Письмо о согласовании")]
        [Required(ErrorMessage = "Не указан путь к письму о согласовании")]
        public string PatchLetter { get; set; }
        [Display(Name ="Изменения в модуле")]
        [Required(ErrorMessage = "Не указаны причины изменения модуля")]
        public string Changes { get; set; }
    }
}
