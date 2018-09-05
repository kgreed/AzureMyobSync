using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.Persistent.Base;
 
 
using scds = System.ComponentModel.DataAnnotations.Schema;

namespace SBD.AMS.Module.MYOB
{
	[NavigationItem("MYOB")]
	[Table("MYOBItems")]
	[DisplayName("MYOB Items")]
	[DefaultProperty("Number")]
	[ImageName("BO_Product")]

	public class MYOBItem : BaseMyobBo
	{
		
		//http://developer.myob.com/api/essentials-accounting/endpoints/inventory/items/
		public MYOBItem()
		{
			 
		}
		public string Description { get; set; }
			[Browsable(false)]
		//[scds.Index("IX_MYOBItem_Uri", 1, IsUnique = true)]
		public string URI { get; set; }
		public DateTime? LastModified { get; set; }
		[Required]
		[StringLength(30, ErrorMessage = "The field cannot exceed 30 characters. ")]
		[scds.Index("IX_MYOBItem_Number", 1, IsUnique = true)]
		public string Number { get; set; }
		public bool IsBought { get; set; }
		public bool IsSold { get; set; }
		public bool IsInventoried { get; set; }
		public bool IsActive { get; set; }
		public bool UseDescription { get; set; }
		
		[StringLength(30, ErrorMessage = "The field cannot exceed 30 characters. ")]
		public string Name { get; set; }
	 

		[StringLength(30, ErrorMessage = "The field cannot exceed 30 characters. ")]
		public string UnitDescription { get; set; }
		public string UnitName { get; set; }
		public int Type { get; set; }

		[NotMapped] 
		public ItemTypeEnum ItemType {
			get => (ItemTypeEnum)Type;
		    set => Type = (int)value;
		}
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
    public enum ItemTypeEnum
    {
        Service,
        Item
    }
}