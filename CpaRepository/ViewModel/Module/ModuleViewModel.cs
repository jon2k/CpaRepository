using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModel.Module
{
    public class ModuleVM
    {
        public List<ModuleViewModel> AllModule { get; set; }
        public List<SelectListItem> AllVendorId { get; set; }
        public List<SelectListItem> AllCpaModuleId { get; set; }
        public bool IsArchive { get; set; }
    }
    public class ModuleViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Вендор")]
        [Required(ErrorMessage = "Не указан вендор")]
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }

        [Display(Name = "Вендорный модуль")]
        [Required(ErrorMessage = "Не указан вендорный модуль")]
        public int VendorModuleId { get; set; }

        public virtual Core.Models.VendorModule VendorModule { get; set; }

        [Display(Name = "Письмо о согласовании")]
        [Required(ErrorMessage = "Не указано письмо о согласовании")]
        public int LetterId { get; set; }

        public virtual Core.Models.Letter Letter { get; set; }

        [Display(Name = "Контрольная сумма")]
        [Required(ErrorMessage = "Не указана контрольная сумма")]
        public string CRC { get; set; }

        [Display(Name = "Версия модуля")]
        [Required(ErrorMessage = "Не указана версия")]
        public string Version { get; set; }

        [Display(Name = "Дата согласования")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Не указана дата согласования")]
        public DateTime DateOfLetter { get; set; }      

        [Display(Name = "Номер письма о согласовании")]
        [Required(ErrorMessage = "Не указан номер письма о согласовании")]
        public string NumberLetter { get; set; }

        [Display(Name = "Наличие модуля в хранилище")]
        public bool ExistModule { get; set; }      

        [Display(Name = "Связь с модулями  ТПР")]
        public virtual List<CpaModule> CpaModules { get; set; }
    }
}
