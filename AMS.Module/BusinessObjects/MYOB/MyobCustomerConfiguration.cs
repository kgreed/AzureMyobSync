using System.Data.Entity.ModelConfiguration;

namespace SBD.AMS.Module.MYOB
{
	public class MyobCustomerConfiguration : EntityTypeConfiguration<MYOBCustomer>
	{
		public MyobCustomerConfiguration()
		{
			HasMany(x => x.Addresses).WithRequired(y => y.MyobCustomer).WillCascadeOnDelete(true);

			//HasRequired(x => x.InverseContact)
			//    .WithMany(y => y.RelatedFrom)
			//    .HasForeignKey(z => z.RelatedContactId)
			//    .WillCascadeOnDelete(false);
		}
	}
}