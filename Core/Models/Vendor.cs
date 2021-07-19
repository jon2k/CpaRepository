using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Vendor : EntityBase
    {
        [Display(Name = "Наименование вендора")]
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }
        [Display(Name = "Описание вендора")]
        [Required(ErrorMessage = "Не указано описание")]
        public string Describtion { get; set; }
        public virtual List<VendorModule> VendorModules { get; set; }

        public override bool Equals(object obj)
        {

            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Vendor v = (Vendor)obj;
                return Name.Equals(v.Name);
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return Name;
        }
    }

}
