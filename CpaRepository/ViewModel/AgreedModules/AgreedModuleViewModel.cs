using CpaRepository.ModelsDb;
using System;
using System.ComponentModel.DataAnnotations;

namespace CpaRepository.ViewModel.AgreedModules
{
    public class AgreedModuleViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Не указан вендор")]
        public int VendorId { get; set; }
        [Required]
        public virtual Vendor Vendor { get; set; }
        [Required(ErrorMessage = "Не указан вендоргый модуль")]
        public int VendorModuleId { get; set; }
        [Required]
        public virtual CpaRepository.ModelsDb.VendorModule VendorModule { get; set; }
        [Required(ErrorMessage = "Не указана контрольная сумма")]
        public string CRC { get; set; }
        [Required(ErrorMessage = "Не указана версия")]
        public string Version { get; set; }
        [Required(ErrorMessage = "Не указана дата согласования")]
        public DateTime DateOfAgreement { get; set; }
        [Required(ErrorMessage = "Не указан путь к модулю")]
        public string PatchVendorModule { get; set; }
        [Required(ErrorMessage = "Не указан путь к письму о согласовании")]
        public string PatchLetter { get; set; }
        [Required(ErrorMessage = "Не указаны причины изменения модуля")]
        public string Changes { get; set; }
    }
}
