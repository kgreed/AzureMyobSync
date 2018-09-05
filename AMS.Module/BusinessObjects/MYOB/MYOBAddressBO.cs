using System.ComponentModel.DataAnnotations;
using AMS.Module.BusinessObjects;
 

namespace SBD.AMS.Module.MYOB
{
	public class MYOBAddressBO : BasicBo
    {


        public int Location { get; set; }
        [StringLength(255, ErrorMessage = "The field cannot exceed 255 characters. ")]
        public string Street { get; set; }
        [StringLength(255, ErrorMessage = "The field cannot exceed 255 characters. ")]
        public string City { get; set; }
        [StringLength(255, ErrorMessage = "The field cannot exceed 255 characters. ")]
        public string State { get; set; }
        [StringLength(15, ErrorMessage = "The field cannot exceed 15 characters. ")]
        public string Postcode { get; set; }
        [StringLength(21, ErrorMessage = "The field cannot exceed 21 characters. ")]
        public string Phone1 { get; set; }
        [StringLength(255, ErrorMessage = "The field cannot exceed 255 characters. ")]
        public string Country { get; set; }


        [StringLength(21, ErrorMessage = "The field cannot exceed 21 characters. ")]
        public string Phone2 { get; set; }

        [StringLength(21, ErrorMessage = "The field cannot exceed 21 characters. ")]
        public string Phone3 { get; set; }
        [StringLength(21, ErrorMessage = "The field cannot exceed 21 characters. ")]
        public string Fax { get; set; }


        [StringLength(255, ErrorMessage = "The field cannot exceed 255 characters. ")]
        public string Email { get; set; }

        [StringLength(255, ErrorMessage = "The field cannot exceed 255 characters. ")]
        public string WebSite { get; set; }
        [StringLength(25, ErrorMessage = "The field cannot exceed 25 characters. ")]
        public string ContactName { get; set; }
        [StringLength(15, ErrorMessage = "The field cannot exceed 15 characters. ")]
        public string Salutation { get; set; }

        
    }
}