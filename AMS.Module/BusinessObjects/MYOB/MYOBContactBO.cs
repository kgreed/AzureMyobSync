using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
 
using scds = System.ComponentModel.DataAnnotations.Schema;

namespace SBD.AMS.Module.MYOB
{
    public class MYOBContactBO : BaseMyobBo, IObjectSpaceLink
    {
        [StringLength(50, ErrorMessage = "The field cannot exceed 50 characters. ")]
		[scds.Index("IX_MYOBContact_Name", 1, IsUnique = true)]
		public string CompanyName { get; set; }

        [StringLength(50, ErrorMessage = "The field cannot exceed 50 characters. ")]
	 
		public string LastName { get; set; }

        [StringLength(20, ErrorMessage = "The field cannot exceed 20 characters. ")]
		 
		public string FirstName { get; set; }

        public bool IsIndividual { get; set; }
        public DateTime? LastModified { get; set; }

        [StringLength(15, ErrorMessage = "The field cannot exceed 15 characters. ")]
        public string DisplayID { get; set; }

        public bool IsActive { get; set; }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public Guid Uid { get; set; }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [StringLength(30, ErrorMessage = "The field cannot exceed 30 characters. ")]
        public string RowVersion { get; set; }

        [Browsable(false)]
        public string URI { get; set; }


        [Browsable(false)]
        public int? DocumentPrintActionId { get; set; }

        [NotMapped]
        public DocumentAction DocumentAction
        {
            get => (DocumentAction)DocumentPrintActionId;
            set => DocumentPrintActionId = (int)value;
        }


        [NotMapped]
        [Browsable(false)]
        public IObjectSpace ObjectSpace { get; set; }
    }
    public enum DocumentAction
    {
        // Summary:
        //     Nothing to do or, already printed and/or sent
        Nothing = 0,
        //
        // Summary:
        //     To be printed
        Print = 1,
        //
        // Summary:
        //     To be emailed
        Email = 2,
        //
        // Summary:
        //     To be printed and emailed
        PrintAndEmail = 3,
    }
}