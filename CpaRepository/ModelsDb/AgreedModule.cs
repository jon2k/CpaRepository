using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.ModelsDb
{
    public class AgreedModule : EntityBase
    {
        //public int VendorId { get; set; }
        //[Required]
        //public Vendor Vendor { get; set; }
        public int VendorModuleId { get; set; }
        [Required]
        public virtual VendorModule VendorModule { get; set; }
        public virtual Letter Letter { get; set; }
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
        [Display(Name = "Номер письма о согласовании")]
        [Required(ErrorMessage = "Не указан номер письма о согласовании")]
        public string NumberLetter { get; set; }
        [Display(Name = "Изменения в модуле")]
        [Required(ErrorMessage = "Не указаны причины изменения модуля")]
        public string Changes { get; set; }
    }

}
