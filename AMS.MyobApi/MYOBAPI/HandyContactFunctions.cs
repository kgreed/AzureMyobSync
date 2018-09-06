using System.Text;
using DevExpress.ExpressApp;
using MYOB.AccountRight.SDK.Contracts.Version2.Contact;
using SBD.AMS.Module.MYOB;

namespace SBD.AMS.MYOB
{
    public static class HandyContactFunctions
    {
        private static void AddToString(StringBuilder builder, string prompt, string firstValue, string secondValue)
        {
            if (firstValue != secondValue)
            {
                if (NullToEmpty(firstValue) != NullToEmpty(secondValue))
                {

                    builder.AppendLine($"{prompt}: [{firstValue}] vs [{secondValue}]");
                }
            }
        }

        private static string NullToEmpty(string s)
        {
            if (s == null)
            {
                return string.Empty;
            }
            return s;
        }

        public static string FormattedLastAndFirstName(string lastName, string firstName)
        {
            return $"{lastName}, {firstName}";
        }

        public static string MakeKey(bool isIndividual, string lastName, string firstName, string companyName)
        {
            if (isIndividual)
            {
                return FormattedLastAndFirstName(lastName, firstName);
            }

            return companyName;


        }
        public static string MakeAlternateKey(bool isIndividual, string lastName, string firstName, string companyName)
        {
            if (isIndividual)
            {
                return companyName;
            }
            return FormattedLastAndFirstName(lastName, firstName);
        }

        //private static string CompareAddress(Address jtAddr, MYOBAddressBO myobAddr)
        //{
        //    var s = new StringBuilder();
        //    AddToString(s, "ContactName", jtAddr.ContactName, myobAddr.ContactName);
        //    AddToString(s, "Street", jtAddr.Street, myobAddr.Street);
        //    AddToString(s, "Suburb", jtAddr.Suburb, myobAddr.City);
        //    AddToString(s, "State", jtAddr.State, myobAddr.State);
        //    AddToString(s, "Postcode", jtAddr.Postcode, myobAddr.Postcode);
        //    AddToString(s, "Country", jtAddr.Country, myobAddr.Country);
        //    AddToString(s, "Phone1", jtAddr.Phone1, myobAddr.Phone1);
        //    AddToString(s, "Phone2", jtAddr.Phone2, myobAddr.Phone2);
        //    AddToString(s, "Fax", jtAddr.Fax, myobAddr.Fax);
        //    AddToString(s, "Email", jtAddr.Email, myobAddr.Email);
        //    AddToString(s, "WebSite", jtAddr.WebSite, myobAddr.WebSite);
        //    return s.ToString();
        //}

        //public static void UpdateFromAccountingCopy(IObjectSpace os, Contact c)
        //{
        //    // c.FlagToUpdateAccounting = false;
        //    UpdateContactFromMirror(os, c);
        //    os.SetModified(c);
        //}

        //private static void UpdateContactFromMirror(IObjectSpace os, Contact c)
        //{
        //    MYOBContactBO myobbo = null;
        //    MYOBAddressBO billingAddress = null;
        //    MYOBAddressBO shippingAddress = null;
        //    if (c.MYOBCustomer != null)
        //    {
        //        myobbo = c.MYOBCustomer;
        //        if (c.MYOBCustomer.Addresses.Count > 0)
        //        {
        //            billingAddress = c.MYOBCustomer.Addresses[0];
        //            UpdateAddress(os, c.BillingAddress, billingAddress);
        //        }
        //        if (c.MYOBCustomer.Addresses.Count > 1)
        //        {
        //            shippingAddress = c.MYOBCustomer.Addresses[1];
        //            UpdateAddress(os, c.ShippingAddress, shippingAddress);
        //        }


        //    }
        //    else
        //    {
        //        if (c.MYOBSupplier != null)
        //        {
        //            myobbo = c.MYOBSupplier;
        //            if (c.MYOBSupplier.Addresses.Count > 0)
        //            {
        //                billingAddress = c.MYOBSupplier.Addresses[0];
        //                UpdateAddress(os, c.BillingAddress, billingAddress);
        //            }
        //            if (c.MYOBSupplier.Addresses.Count > 1)
        //            {
        //                shippingAddress = c.MYOBSupplier.Addresses[1];
        //                UpdateAddress(os, c.ShippingAddress, shippingAddress);
        //            }


        //        }
        //    }
        //    if (myobbo == null)
        //    {
        //        return;
        //    }
        //    c.IsIndividual = myobbo.IsIndividual;
        //    if (c.IsIndividual)
        //    {
        //        c.Name = FormattedLastAndFirstName(myobbo.LastName, myobbo.FirstName);
        //    }
        //    else
        //    {
        //        c.Name = myobbo.CompanyName;
        //    }
        //}

        //private static void UpdateAddress(IObjectSpace os, Address a, MYOBAddressBO p)
        //{
        //    a.ContactName = p.ContactName;
        //    a.Street = p.Street;
        //    a.Suburb = p.City;
        //    a.Postcode = p.Postcode;
        //    a.Country = p.Country;
        //    a.Phone1 = p.Phone1;
        //    a.Phone2 = p.Phone2;
        //    a.Phone3 = p.Phone3;
        //    a.Fax = p.Fax;
        //    a.Email = p.Email;
        //    a.WebSite = p.WebSite;
        //    os.SetModified(a);
        //}

        //private static string CompareContact(
        //    Contact c,
        //    MYOBContactBO myobbo,
        //    MYOBAddressBO billingAddress,
        //    MYOBAddressBO shippingAddress)
        //{
        //    var s = new StringBuilder();
        //    if (myobbo == null)
        //    {
        //        return string.Empty;
        //    }
        //    AddToString(s, "IsIndividual", c.IsIndividual.ToString(), myobbo.IsIndividual.ToString());
        //    if (c.IsIndividual)
        //    {
        //        AddToString(s, "Last,First", c.Name, FormattedLastAndFirstName(myobbo.LastName, myobbo.FirstName));
        //    }
        //    else
        //    {
        //        AddToString(s, "Company", c.Name, myobbo.CompanyName);
        //    }
        //    if (billingAddress != null)
        //    {
        //        var sBillingAddress = CompareAddress(c.BillingAddress, billingAddress);
        //        if (sBillingAddress.Length > 0)
        //        {
        //            s.AppendLine($"BillingAddress: {sBillingAddress}");
        //        }
        //    }
        //    if (shippingAddress != null)
        //    {
        //        var sShippingAddress = CompareAddress(c.ShippingAddress, shippingAddress);
        //        if (sShippingAddress.Length > 0)
        //        {
        //            s.AppendLine($"ShippingAddress: {sShippingAddress}");
        //        }
        //    }
        //    return s.ToString();
        //}

        //public static string SynchronisationComparisonMessage(Contact c)
        //{
        //    var s = new StringBuilder();
        //    if (c.MYOBCustomer != null)
        //    {
        //        MYOBCustomerAddress billingAddress = null;
        //        MYOBCustomerAddress shippingAddress = null;
        //        if (c.MYOBCustomer.Addresses.Count > 0)
        //        {
        //            billingAddress = c.MYOBCustomer.Addresses[0];
        //        }
        //        if (c.MYOBCustomer.Addresses.Count > 1)
        //        {
        //            shippingAddress = c.MYOBCustomer.Addresses[1];
        //        }
        //        var sCustomer = CompareContact(c, c.MYOBCustomer, billingAddress, shippingAddress);
        //        if (sCustomer.Length > 0)
        //        {
        //            s.AppendLine($"Customer: {sCustomer}");
        //        }
        //    }
        //    if (c.MYOBSupplier != null)
        //    {
        //        MYOBSupplierAddress billingAddress = null;
        //        MYOBSupplierAddress shippingAddress = null;
        //        if (c.MYOBSupplier.Addresses.Count > 0)
        //        {
        //            billingAddress = c.MYOBSupplier.Addresses[0];
        //        }
        //        if (c.MYOBSupplier.Addresses.Count > 1)
        //        {
        //            shippingAddress = c.MYOBSupplier.Addresses[1];
        //        }
        //        var sSupplier = CompareContact(c, c.MYOBSupplier, billingAddress, shippingAddress);
        //        if (sSupplier.Length > 0)
        //        {
        //            s.AppendLine($"Supplier: {sSupplier}");
        //        }
        //    }
        //    return s.ToString();
        //}
    }
}