using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.Persistent.Base;

namespace SBD.AMS.Module.MYOB
{
	[NavigationItem("MYOB")]
	[Table("MYOBSuppliers")]
	[DisplayName("MYOB Suppliers")]
	[DefaultProperty("CompanyName")]
	[ImageName("BO_Vendor")]
	 
	public class MYOBSupplier : MYOBContactBO
	{
		//http://developer.myob.com/api/accountright/v2/contact/supplier/
		public MYOBSupplier()
		{
			Addresses = new List<MYOBSupplierAddress>();
		}
	 
      

        public virtual List<MYOBSupplierAddress> Addresses { get; set; }
      
	}
}