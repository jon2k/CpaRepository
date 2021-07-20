using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class AgreedModule : EntityBase
    {
        public int VendorModuleId { get; set; }

        [Required]
        public virtual VendorModule VendorModule { get; set; }

        public int LetterId { get; set; }

        public virtual Letter Letter { get; set; }
        
        [Display(Name = "Контрольная сумма")]
        [Required(ErrorMessage = "Не указана контрольная сумма")]
        public string CRC { get; set; }
        
        [Display(Name = "Версия модуля")]
        [Required(ErrorMessage = "Не указана версия")]
        public string Version { get; set; }      
       
        [Display(Name = "Загрузить модуль")]
        [Required(ErrorMessage = "Не указан путь к модулю")]
        public string PathVendorModule { get; set; }      
       
        [Display(Name = "Изменения в модуле")]
        [Required(ErrorMessage = "Не указаны причины изменения модуля")]
        public string Changes { get; set; }
    }

}
