using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;

using scds = System.ComponentModel.DataAnnotations.Schema;
namespace SBD.AMS.Module.MYOB
{
    public abstract class BaseMyobBo : IXafEntityObject  //, IObjectSpaceLink  we should just declare it when we really need it... mainly we want out business objects to be like POCOs 
    {
        [Browsable(false)]
        [Key]
        public virtual int Id { get; set; }

        public virtual void OnCreated()
        {
            

        }

        public virtual void OnSaving()
        {
            Debug.Print("hi");
        }

        public virtual void OnLoaded()
        {
        }


    }
    [NavigationItem("MYOB")]
	[Table("MYOBAccounts")]
	[DisplayName("MYOB Accounts")]
	[DefaultProperty("Name")]
	[ImageName("BO_List")]
	[DefaultListViewOptions(false,0)] // first parameter is allowedit
	public class MYOBAccount : BaseMyobBo 
	{
		//http://developer.myob.com/api/accountright/v2/generalledger/account/
		[StringLength(30, ErrorMessage = "The field cannot exceed 30 characters. ")]
		

		public string Name { get; set; }

		[StringLength(6, ErrorMessage = "The field cannot exceed 6 characters. ")]
		[scds.Index("IX_MYOBAccounts_DisplayId", 1, IsUnique = true)]
		public string DisplayID { get; set; }

		[StringLength(60, ErrorMessage = "The field cannot exceed 60 characters. ")]
		public string Type  { get; set; }

		public virtual MYOBTaxCode TaxCode { get; set; }

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
 