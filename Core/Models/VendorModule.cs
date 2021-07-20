using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class VendorModule : EntityBase
    {
        public int VendorId { get; set; }

        [Required]
        public virtual Vendor Vendor { get; set; }

        [Display(Name = "Наименование модуля")]
        [Required(ErrorMessage = "Не указано имя")]
        public string NameModule { get; set; }

        [Display(Name = "Описание модуля")]
        [Required(ErrorMessage = "Не указано описание")]

        public string Description { get; set; }

        public virtual List<AgreedModule> AgreedModules { get; set; }

        [Display(Name = "Связь с модулями  ТПР")]
        [Required]
        public virtual List<CpaModule> CpaModules { get; set; }
    }
}
