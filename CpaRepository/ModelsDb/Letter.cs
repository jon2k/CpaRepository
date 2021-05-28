﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.ModelsDb
{
    public class Letter:EntityBase
    {
        [Display(Name = "Номер письма о согласовании")]
        [Required(ErrorMessage = "Не указан номер письма о согласовании")]
        public string NumberLetter { get; set; }
        
        public string PathLetter { get; set; }
       
        [Display(Name = "Дата согласования")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Не указана дата согласования")]
        public DateTime DateOfLetter { get; set; }
       
        public int VendorId { get; set; }
        
        [Display(Name = "Вендор")]
        [Required(ErrorMessage = "Не указан вендор")]
        public virtual Vendor Vendor { get; set; }
        
        public virtual List<AgreedModule> AgreedModules { get; set; }
    }
}
