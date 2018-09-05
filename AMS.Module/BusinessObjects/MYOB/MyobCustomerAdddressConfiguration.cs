using System.Data.Entity.ModelConfiguration;

namespace SBD.AMS.Module.MYOB
{
	public class MyobCustomerAdddressConfiguration : EntityTypeConfiguration<MYOBCustomerAddress>
	{
		public MyobCustomerAdddressConfiguration()
		{
			HasRequired(x => x.MyobCustomer).WithMany(y => y.Addresses).WillCascadeOnDelete(true);

			//HasRequired(x => x.InverseContact)
			//    .WithMany(y => y.RelatedFrom)
			//    .HasForeignKey(z => z.RelatedContactId)
			//    .WillCascadeOnDelete(false);
		}
	}
}