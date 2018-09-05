using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.Persistent.Base;

namespace SBD.AMS.Module.MYOB
{
	[NavigationItem("MYOB")]
	[Table("MYOBCustomers")]
	[DisplayName("MYOB Customers")]
	[DefaultProperty("CompanyName")]
	[ImageName("BO_Customer")]
 
	public class MYOBCustomer : MYOBContactBO
	{
		
		//http://developer.myob.com/api/accountright/v2/contact/customer/
		public MYOBCustomer()
		{
			Addresses = new List<MYOBCustomerAddress>();
		}
		 
		
	 	public virtual List<MYOBCustomerAddress> Addresses { get; set; }
	   
	}


}