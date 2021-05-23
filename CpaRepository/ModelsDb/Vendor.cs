using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.ModelsDb
{
    public class Vendor : EntityBase
    {
        [Required]
        public string Name { get; set; }
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
