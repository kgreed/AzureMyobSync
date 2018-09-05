using System.ComponentModel.DataAnnotations.Schema;

namespace SBD.AMS.Module.MYOB
{
    public class MYOBCustomerAddress : MYOBAddressBO
    {
        public virtual MYOBCustomer MyobCustomer { get; set; }
        [NotMapped]
        public bool TaggedToDelete { get; set; }
    }
}