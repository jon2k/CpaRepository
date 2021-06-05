using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CpaRepository.ModelsDb;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CpaRepository.ViewModel.ActualVendorModule
{
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

        public virtual ModelsDb.VendorModule VendorModule { get; set; }

        [Display(Name = "Письмо о согласовании")]
        [Required(ErrorMessage = "Не указано письмо о согласовании")]
        public int LetterId { get; set; }

        public virtual ModelsDb.Letter Letter { get; set; }

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

        //[Display(Name = "Ссылка на модуль")]
        ////[Required(ErrorMessage = "Не указан путь к модулю")]
        //public string PathVendorModule { get; set; }

        //[Display(Name = "Ссылка на письмо")]
        ////[Required(ErrorMessage = "Не указан путь к модулю")]
        //public string PatchLetter { get; set; }

        [Display(Name = "Номер письма о согласовании")]
        [Required(ErrorMessage = "Не указан номер письма о согласовании")]
        public string NumberLetter { get; set; }

        //[Display(Name = "Изменения в модуле")]
        //[Required(ErrorMessage = "Не указаны причины изменения модуля")]
        //public string Changes { get; set; }

        [Display(Name = "Наличие модуля в хранилище")]
        public bool ExistModule { get; set; }

        //[Display(Name = "Выберите файл модуля (.zip)")]
        //[Required(ErrorMessage = "Не указан файл модуля")]
        //[NotMapped]
        //public IFormFile FileModule { get; set; }
        //[Display(Name = "Выберите файл письма (.pdf)")]
        //[Required(ErrorMessage = "Не указан файл письма")]
        //[NotMapped]
        //public IFormFile FileLetter { get; set; }
        //[Display(Name = "Связь с модулями  ТПР")]
        //[Required(ErrorMessage = "Не указана связь с МО")]
        //public int[] CpaModulesId { get; set; }
        //// public int CpaModuleId { get; set; }

        [Display(Name = "Связь с модулями  ТПР")]
        public virtual List<CpaModule> CpaModules { get; set; }
    }
}
