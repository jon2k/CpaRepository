using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.ModelsDb
{
    public class CpaModule : EntityBase
    {
        [Required(ErrorMessage = "Не указано имя")]
        public string NameModule { get; set; }
        [Required(ErrorMessage = "Не указано описание")]
        public string Description { get; set; }
       // public List<CpaSubModule> CpaSubModules { get; set; }
        public virtual List<VendorModule> VendorModules { get; set; }
    }

}
