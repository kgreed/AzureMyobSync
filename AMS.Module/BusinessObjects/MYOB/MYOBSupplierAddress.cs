using System.ComponentModel.DataAnnotations.Schema;

namespace SBD.AMS.Module.MYOB
{
    public class MYOBSupplierAddress : MYOBAddressBO
    {
        public virtual MYOBSupplier MyobSupplier { get; set; }

        [NotMapped]
        public bool TaggedToDelete { get; set; }
    }

}