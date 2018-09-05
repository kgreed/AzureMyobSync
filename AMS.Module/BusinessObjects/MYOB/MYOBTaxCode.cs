using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.Persistent.Base;
 

namespace SBD.AMS.Module.MYOB
{
	[NavigationItem("MYOB")]
	[Table("MYOBTaxCodes")]
	[DisplayName("MYOB TaxCodes")]
	[DefaultProperty("Code")]
	[ImageName("BO_List")]
	public class MYOBTaxCode :BaseMyobBo
	{
		

		[StringLength(3, ErrorMessage = "The field cannot exceed 3 characters. ")]
		public string Code { get; set; }
		[StringLength(30, ErrorMessage = "The field cannot exceed 30 characters. ")]
		public string Description { get; set; }
		public double Rate { get; set; }
			[Browsable(false)]
		public string URI { get; set; }
		public MYOBAccount TaxCollectedAccount { get; set; }
		public MYOBAccount TaxPaidAccount { get; set; }
	 
		public string TypeCode { get; set; }
		[VisibleInListView(false)]
		[VisibleInDetailView(false)]
		[VisibleInLookupListView(false)]
		public Guid Uid { get; set; }

		[VisibleInListView(false)]
		[VisibleInDetailView(false)]
		[VisibleInLookupListView(false)]
		[StringLength(30, ErrorMessage = "The field cannot exceed 30 characters. ")]

		public string RowVersion { get; set; }
	}
}