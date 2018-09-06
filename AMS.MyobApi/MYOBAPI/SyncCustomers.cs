using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AMS.Module.BusinessObjects;
using MYOB.AccountRight.SDK;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.AccountRight.SDK.Contracts.Version2.Contact;
using MYOB.AccountRight.SDK.Services;
using MYOB.AccountRight.SDK.Services.Contact;
 
using SBD.AMS.Module.MYOB;
using MYOBApi.CSharp.Helpers;


namespace SBD.AMS.MYOB
{
    public class SyncCustomers : IMyobHandler
    {
        protected CustomerService _myService;
        private List<Customer> items;

        public void InitService(ApiConfiguration myConfiguration, OAuthKeyService myOAuthKeyService)
        {
            _myService = new CustomerService(myConfiguration, null, myOAuthKeyService);
            items = new List<Customer>();
        }

        public async Task<int> Addtems(
            CompanyFile myCompanyFile,
            string pageFilter,
            CompanyFileCredentials myCredentials,
            CancellationToken ct)
        {
            var tpc = _myService.GetRangeAsync(myCompanyFile, pageFilter, myCredentials, ct, null);
            var newItems = await tpc;
            items.AddRange(newItems.Items);
            return newItems.Items.Count();
        }

        public int ItemCount => items.Count;

        public void SynchroniseItems(IProgress<int> progress)
        {
            using (var connect = new AMSDbContext(HandyFunctions.ConnectionString()))
            {
                RebuildMirrorTable(connect, progress);
               // RebuildMirrorLinks(connect);
            }
        }

        public string OrderBy => "CompanyName,LastName,FirstName";

        //private void RebuildMirrorLinks(AMSDbContext connect)
        //{
        //  //  DisconnectMismatches(connect);  keep links we set up, for example for cash sale and for head office
        //    var ar = items.OrderBy(x => x.CompanyName).ThenBy(x => x.LastName).ThenBy(x => x.FirstName).ToArray();

        //    var sb = new StringBuilder();
        //    // Connect Matches Add 
        //    foreach (var item in ar)
        //    {
        //        var jtItem = connect.MYOBCustomers.FirstOrDefault(x => x.Uid == item.UID); //unique idex should ensure only 1
        //        if (jtItem == null)
        //        {
        //            sb.AppendLine(item.CompanyName);
        //            //throw new Exception($"Unable to find  {item.CompanyName}");
        //            continue;
        //        }
        //        if (item.LastName == "Aikens")
        //        {
        //            Trace.WriteLine("hi");
        //        }

        //        // locate contect using Name or FirstName, LastName depending on whether IsIndividual
        //        var key = MakeKey(item);
        //        var rec = connect.Contacts.SingleOrDefault(x => x.Name == key);
        //        if (rec == null)
        //        {
        //            var alternateKey = MakeAlternateKey(item);
        //            rec = connect.Contacts.SingleOrDefault(x => x.Name == alternateKey);
        //            if (rec != null)
        //            {
        //                throw new ExceptionMYOBContactMissMatch(
	       //                 $"Customer: CompanyName:{item.CompanyName}, LastName{item.LastName}, IsIndividual{item.IsIndividual} conflicts with Organisation {rec.Name}");
        //            }
        //        }

        //        // if not found then add it.
        //        if (rec == null)
        //        {
        //            var newRec = new Contact
        //            {
        //                MYOBCustomer = jtItem,
        //                IsIndividual = jtItem.IsIndividual,
        //                Name =
        //                    jtItem.IsIndividual
        //                        ? HandyContactFunctions.FormattedLastAndFirstName( jtItem.LastName, jtItem.FirstName)
        //                        : jtItem.CompanyName,
        //                Person = { FirstName = jtItem.FirstName, LastName = jtItem.LastName }
        //            };
        //            if (item.Addresses != null)
        //            {
        //                var billAddress = item.Addresses.SingleOrDefault(x => x.Location == 1);
        //                if (billAddress != null)
        //                {
        //                    newRec.BillingAddress = HandyFunctions.MakeAddress(billAddress);
        //                }
        //                var shipAddress = item.Addresses.SingleOrDefault(x => x.Location == 2);
        //                if (shipAddress != null)
        //                {
        //                    newRec.ShippingAddress = HandyFunctions.MakeAddress(shipAddress);
        //                }
        //            }
        //            HandyFunctions.Log("added {0}", newRec.Name);
        //            connect.Contacts.Add(newRec);
        //        }
        //        else
        //        {
        //            // if found then link it
        //            if (rec.MYOBCustomer == null || rec.MYOBCustomer.Id != jtItem.Id)
        //            {
        //                rec.MYOBCustomer = jtItem;
        //            }
        //        }
        //    }
        //    connect.SaveChanges();
        //    if (sb.Length > 0)
        //    {
        //        MessageBox.Show("Unable to locate " + sb);
        //    }
        //}

        //private void DisconnectMismatches(b.JobTalkDbContext connect)
        //{
        //    foreach (var rec in 
        //        connect.Contacts.Where(
        //            x =>
        //                x.MYOBCustomer != null &&
        //                !((x.MYOBCustomer.IsIndividual &&
        //                   (x.MYOBCustomer.LastName + ", " + x.MYOBCustomer.FirstName) == x.Name) ||
        //                  ((!x.MYOBCustomer.IsIndividual) && x.MYOBCustomer.CompanyName == x.Name))))
        //    {
        //        rec.MYOBCustomer = null;
        //    }
        //    connect.SaveChanges();
        //}

        private void RebuildMirrorTable(AMSDbContext connect, IProgress<int> progressCallback)
        {
            var ar = items.OrderBy(x => x.CompanyName).ThenBy(x => x.LastName).ThenBy(x => x.FirstName).ToArray();
            AddNewUpdateExistingRecords(ar, connect, progressCallback);
            DeleteMissingMirrorRecords(connect, ar);
        }

        private static void AddNewUpdateExistingRecords(
            Customer[] ar,
            AMSDbContext connect,
            IProgress<int> progressCallback)
        {
            var sb = new StringBuilder();
            var info = "";
            var stepNum = 1;
            var numSteps = ar.Count();
            foreach (var item in ar)
            {
                if (MakeKey(item) == info)
                {
                   // skip this one
                    //throw new ExceptionDuplicateKey($"Unable to import. Duplicate key exists in MYOB for: {info}");
                    sb.AppendLine(info);
                    continue;
                }
                info = MakeKey(item);
                HandyFunctions.Log(info);
                var jtCustomer = connect.MYOBCustomers.SingleOrDefault(x => x.Uid == item.UID);

                //unique idex should ensure only 1
                if (jtCustomer == null)
                {
                    jtCustomer = new MYOBCustomer();
                    connect.MYOBCustomers.Add(jtCustomer);
                }
                CopyRecordFromMyob(connect, jtCustomer, item);
                progressCallback.Report((100 * stepNum) / numSteps);
                stepNum++;
            }
            connect.SaveChanges();
            if (sb.Length > 0)
            {
                MessageBox.Show("Duplicate customers skipped /n" + sb );
            }
        }

        private static string MakeKey(Customer customer)
        {
            return customer.IsIndividual ? HandyContactFunctions.FormattedLastAndFirstName( customer.LastName, customer.FirstName) : customer.CompanyName;
        }

        private static string MakeAlternateKey(Customer customer)
        {
            return customer.IsIndividual ? customer.CompanyName : HandyContactFunctions.FormattedLastAndFirstName(customer.LastName, customer.FirstName);
        }


        private static void DeleteMissingMirrorRecords(AMSDbContext connect, Customer[] ar)
        {
            foreach (var rec in connect.MYOBCustomers)
            {
                var oItem = ar.SingleOrDefault(x => x.UID == rec.Uid);
                if (oItem != null) continue;
                {
                    //var key = rec.IsIndividual ? HandyContactFunctions.FormattedLastAndFirstName(rec.LastName, rec.FirstName) : rec.CompanyName;
                    //foreach (var contact in connect.Contacts.Where(x => x.MYOBCustomer != null && x.Name == key))
                    //{
                    //    contact.MYOBCustomer = null; // unlink
                    //}
                    connect.MYOBCustomers.Remove(rec);
                }
            }
            connect.SaveChanges();
        }

        private static void CopyRecordFromMyob(AMSDbContext connect, MYOBCustomer jtItem, Customer item)
        {
            jtItem.CompanyName = item.IsIndividual ? HandyContactFunctions.FormattedLastAndFirstName(item.LastName, item.FirstName) : item.CompanyName;
            jtItem.DisplayID = item.DisplayID;
            jtItem.FirstName = item.FirstName;
            jtItem.LastName = item.LastName;
            jtItem.IsIndividual = item.IsIndividual;
            jtItem.IsActive = item.IsActive;
            jtItem.RowVersion = item.RowVersion;
            jtItem.Uid = item.UID;
            jtItem.LastModified = item.LastModified;
            jtItem.URI = item.URI.ToString();

	       

	        if (item.SellingDetails?.InvoiceDelivery != null)
		        jtItem.DocumentAction = (DocumentAction)item.SellingDetails.InvoiceDelivery;
	        SyncAddresses(connect, jtItem, item);
        }

        private static void SyncAddresses(AMSDbContext connect, MYOBCustomer jtItem, Customer item)
        {
            foreach (var a in jtItem.Addresses)
            {
                a.TaggedToDelete = true;
            }
            if (item.Addresses != null)
            {
                foreach (var address in item.Addresses)
                {
                    var addresses = jtItem.Addresses.AsQueryable();
                    var tempAddress = address;
                    var existingAddresses = addresses.Where(x => x.Location == tempAddress.Location).ToArray();
                    if (existingAddresses.Count() == 1)
                    {
                        var existingAddress = existingAddresses[0];
                        ModifyIfNeeded(
                            connect,
                            existingAddress,
                            "ContactName",
                            existingAddress.ContactName,
                            tempAddress.ContactName);
                        ModifyIfNeeded(connect, existingAddress, "Street", existingAddress.Street, tempAddress.Street);
                        ModifyIfNeeded(connect, existingAddress, "City", existingAddress.City, tempAddress.City);
                        ModifyIfNeeded(connect, existingAddress, "State", existingAddress.State, tempAddress.State);
                        ModifyIfNeeded(
                            connect,
                            existingAddress,
                            "Postcode",
                            existingAddress.Postcode,
                            tempAddress.PostCode);
                        ModifyIfNeeded(
                            connect,
                            existingAddress,
                            "Country",
                            existingAddress.Country,
                            tempAddress.Country);
                        ModifyIfNeeded(connect, existingAddress, "Email", existingAddress.Email, tempAddress.Email);
                        ModifyIfNeeded(connect, existingAddress, "Phone1", existingAddress.Phone1, tempAddress.Phone1);
                        ModifyIfNeeded(connect, existingAddress, "Phone2", existingAddress.Phone2, tempAddress.Phone2);
                        ModifyIfNeeded(connect, existingAddress, "Fax", existingAddress.Fax, tempAddress.Fax);
                        ModifyIfNeeded(connect, existingAddress, "WebSite", existingAddress.WebSite, tempAddress.Website);

                        existingAddress.TaggedToDelete = false;
                        if (existingAddress.ContactName != tempAddress.ContactName)
                        {
                            existingAddress.ContactName = tempAddress.ContactName;
                            connect.Entry(existingAddress).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        foreach (var duplAddress in existingAddresses)
                        {
                            duplAddress.TaggedToDelete = true;
                        }
                        if (string.IsNullOrEmpty(address.Street))
                        {
                            continue;
                        }
                        var newAddress = new MYOBCustomerAddress();
                        HandyFunctions.PopulateMyobAddress(newAddress, address);
                        jtItem.Addresses.Add(newAddress);
                        newAddress.MyobCustomer = jtItem;
                    }
                }
            }
            var deleteQueue = jtItem.Addresses.Where(x => x.TaggedToDelete).ToArray();
            foreach (var a in deleteQueue)
            {
                var entry = connect.Entry(a);
                entry.State = EntityState.Deleted;
            }
        }

        private static void ModifyIfNeeded(
            AMSDbContext connect,
            MYOBCustomerAddress existingAddress,
            string propertyName,
            string oldValue,
            string newValue)
        {
            if (oldValue == null && newValue == null)
            {
                return;
            }
            if (oldValue == newValue)
            {
                return;
            }
            var entry = connect.Entry(existingAddress);
            entry.Property(propertyName).CurrentValue = newValue;
            entry.Property(propertyName).IsModified = true;
        }

        public async void AddItemToMyob(
            AMSDbContext connect,
            MYOBCustomer jtMyobCustomer,
            CompanyFile myCompanyFile,
            CompanyFileCredentials myCredentials,
            CancellationToken ct)
        {
            var item = new Customer
            {
                IsActive = jtMyobCustomer.IsActive,
                CompanyName = jtMyobCustomer.CompanyName,
                FirstName = jtMyobCustomer.FirstName,
                LastName = jtMyobCustomer.LastName
            };
            HandyFunctions.Log(item.ToJson2());
            var task = _myService.InsertAsync(myCompanyFile, item, myCredentials, ct, ErrorLevel.WarningsAsErrors);
            await task;
            jtMyobCustomer.Uid = item.UID;
            connect.SaveChanges();
        }
    }
}