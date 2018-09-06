using System.Configuration;
using System.Diagnostics;
using MYOB.AccountRight.SDK.Contracts.Version2.Contact;
using SBD.AMS.Module.MYOB;

namespace SBD.AMS.MYOB
{
    public static class HandyFunctions
    {
        public static void Log(string format, params object[] values)
        {
            var text = string.Format(format, values);

            Trace.WriteLine(text);
        }

        public static void Log(string info)
        {
            Trace.WriteLine(info);
        }

        public static string ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["ApplicationDatabase"].ConnectionString;
        }

        public static Address MakeAddress(Address address)
        {
            var addr = new Address
            {
                ContactName = address.ContactName,
                Street = address.Street,
                City = address.City,
                State = address.State,
                PostCode = address.PostCode,
                Country =  address.Country,
                Email = address.Email,
                Phone1 = address.Phone1,
                Phone2 = address.Phone2
            };
            return addr;
        }

        public static void PopulateMyobAddress(
            MYOBAddressBO newAddress,
            Address address)
        {
            newAddress.ContactName = address.ContactName;
            newAddress.Email = address.Email;
            newAddress.Fax = address.Fax;
            newAddress.Location = address.Location;
            newAddress.Phone1 = address.Phone1;
            newAddress.Phone2 = address.Phone2;
            newAddress.Phone3 = address.Phone3;
            newAddress.Salutation = address.Salutation;
            newAddress.Country = address.Country;
            newAddress.Street = address.Street;
            newAddress.City = address.City;
            newAddress.State = address.State;
            newAddress.Postcode = address.PostCode;
            newAddress.WebSite = address.Website;

           
        }
    }
}