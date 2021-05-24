﻿using System;
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
