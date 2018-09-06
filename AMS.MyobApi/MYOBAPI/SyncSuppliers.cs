using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public class SyncSuppliers : IMyobHandler
    {
        protected SupplierService _myService;
        private List<Supplier> items;

        public void InitService(ApiConfiguration myConfiguration, OAuthKeyService myOAuthKeyService)
        {
            _myService = new SupplierService(myConfiguration, null, myOAuthKeyService);
            items = new List<Supplier>();
        }

        public async Task<int> Addtems(
            CompanyFile myCompanyFile,
            string pageFilter,
            CompanyFileCredentials myCredentials,
            CancellationToken ct)
        {
            var tpc = _myService.GetRangeAsync(myCompanyFile, pageFilter, myCredentials, ct, null);
            var newItems = await tpc; // fails here
            items.AddRange(newItems.Items);
            return newItems.Items.Count();
        }

        public int ItemCount => items.Count;

        public void SynchroniseItems(IProgress<int> progressCallback)
        {
            using (var connect = new AMSDbContext(HandyFunctions.ConnectionString()))
            {
                var ar = items.OrderBy(x => x.CompanyName).ThenBy(x => x.LastName).ThenBy(x => x.FirstName).ToArray();
                RebuildMirrorTable(ar, connect, progressCallback);
               // RebuildMirrorLinks(connect, ar);
            }
        }

        public string OrderBy => "CompanyName,LastName,FirstName";


        //private void RebuildMirrorLinks(AMSDbContext connect, Supplier[] ar)
        //{
        //    DisconnectMismatches(connect);

        //    // Connect Matches Add 
        //    foreach (var item in ar)
        //    {
        //        var jtItem = connect.MYOBSuppliers.Single(x => x.Uid == item.UID); //unique idex should ensure only 1
        //        var key = HandyContactFunctions.MakeKey(item.IsIndividual, item.LastName, item.FirstName,item.CompanyName);
        //        var rec = connect.Contacts.SingleOrDefault(x => x.Name == key);
        //        if (rec == null)
        //        {
        //            var alternateKey = HandyContactFunctions.MakeAlternateKey(item.IsIndividual, item.LastName, item.FirstName, item.CompanyName);
        //            rec = connect.Contacts.SingleOrDefault(x => x.Name == alternateKey);
        //            if (rec != null)
        //            {
        //                throw new ExceptionMYOBContactMissMatch(
        //                    $"Supplier: CompanyName:{item.CompanyName}, LastName{item.LastName}, IsIndividual{item.IsIndividual} conflicts with Organisation {rec.Name}");
        //            }
        //        }
        //        if (rec == null)
        //        {
        //            var newRec = new Contact
        //            {
        //                MYOBSupplier = jtItem,
        //                Name = jtItem.IsIndividual ? HandyContactFunctions.FormattedLastAndFirstName( jtItem.LastName, jtItem.FirstName) : jtItem.CompanyName
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
        //            if (rec.MYOBSupplier == null || rec.MYOBSupplier.Id != rec.Id)
        //            {
        //                rec.MYOBSupplier = jtItem;
        //            }
        //        }
        //    }
        //    connect.SaveChanges();
        //}

        //private static void DisconnectMismatches(AMSDbContext connect)
        //{
        //    foreach (var rec in
        //        connect.Contacts.Where(
        //            x =>
        //                x.MYOBSupplier != null &&
        //                !((x.MYOBSupplier.IsIndividual && (x.MYOBSupplier.LastName + ", " + x.MYOBSupplier.FirstName) == x.Name) ||
        //                  ((!x.MYOBSupplier.IsIndividual) && x.MYOBSupplier.CompanyName == x.Name))))
        //    {
        //        rec.MYOBSupplier = null;
        //    }
        //    connect.SaveChanges();
        //}

        private static void RebuildMirrorTable(
            Supplier[] ar,
            AMSDbContext connect,
            IProgress<int> progressCallback)
        {
            AddNewUpdateExistingRecords(ar, connect, progressCallback);
            DeleteMissingMirrorRecords(connect, ar);
        }

        private static void AddNewUpdateExistingRecords(
            Supplier[] ar,
            AMSDbContext connect,
            IProgress<int> progressCallback)
        {
            var info = "";
            var stepNum = 1;
            var numSteps = ar.Count();
            foreach (var item in ar)
            {
                var key = HandyContactFunctions.MakeKey(
                    item.IsIndividual,
                    item.LastName,
                    item.FirstName,
                    item.CompanyName);
                if (key == info)
                {
                    throw new ExceptionDuplicateKey($"{info}");
                }
                info = key;
                HandyFunctions.Log(info);
                var jtSupplier = connect.MYOBSuppliers.SingleOrDefault(x => x.Uid == item.UID);

                    //unique idex should ensure only 1
                if (jtSupplier == null)
                {
                    jtSupplier = new MYOBSupplier();
                    connect.MYOBSuppliers.Add(jtSupplier);
                }
                CopyRecordFromMYOB(connect,jtSupplier, item);
                progressCallback.Report((100 * stepNum) / numSteps);
                stepNum++;
            }
            connect.SaveChanges();
        }

      

       

        private static void DeleteMissingMirrorRecords(AMSDbContext connect, Supplier[] ar)
        {
            foreach (var rec in connect.MYOBSuppliers)
            {
                var oItem = ar.SingleOrDefault(x => x.UID == rec.Uid);
                if (oItem == null)
                {
                    var key = rec.IsIndividual ? rec.LastName : rec.CompanyName;
                    //foreach (
                    //    var sup in
                    //        connect.Contacts.Where(
                    //            x =>
                    //                x.MYOBSupplier != null && x.MYOBSupplier.CompanyName == key ||
                    //                x.MYOBSupplier.LastName == key))
                    //{
                    //    sup.MYOBSupplier = null; // unlink
                    //}
                    connect.MYOBSuppliers.Remove(rec);
                }
            }
            connect.SaveChanges();
        }

        private static void CopyRecordFromMYOB(AMSDbContext connect, MYOBSupplier jtItem, Supplier item)
        {
            jtItem.CompanyName = HandyContactFunctions.MakeKey(
                item.IsIndividual,
                item.LastName,
                item.FirstName,
                item.CompanyName);
            
            jtItem.DisplayID = item.DisplayID;
            jtItem.FirstName = item.FirstName;
            jtItem.LastName = item.LastName;
            jtItem.IsIndividual = item.IsIndividual;
            jtItem.IsActive = item.IsActive;
            jtItem.RowVersion = item.RowVersion;
            jtItem.Uid = item.UID;
            jtItem.LastModified = item.LastModified;
            jtItem.URI = item.URI.ToString();
            jtItem.IsActive = item.IsActive;
         
            SyncAddresses(connect,jtItem, item);
        }
        private static void ModifyIfNeeded(
          AMSDbContext connect,
          MYOBSupplierAddress existingAddress,
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
        private static void SyncAddresses(AMSDbContext connect, MYOBSupplier jtItem, Supplier item)
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
                        ModifyIfNeeded(connect, existingAddress, "Fax", existingAddress.Phone2, tempAddress.Fax);
                        ModifyIfNeeded(connect, existingAddress, "WebSite", existingAddress.Phone2, tempAddress.Website);
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
                        if (!string.IsNullOrEmpty(address.Street))
                        {
                            var newAddress = new MYOBSupplierAddress();
                            HandyFunctions.PopulateMyobAddress(newAddress, address);
                            jtItem.Addresses.Add(newAddress);
                            newAddress.MyobSupplier = jtItem;
                        }
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

   
        public async void AddItemToMYOB(
            AMSDbContext connect,
            MYOBSupplier jtMyobSupplier,
            CompanyFile myCompanyFile,
            CompanyFileCredentials myCredentials,
            CancellationToken ct)
        {
            var item = new Supplier
            {
                IsActive = jtMyobSupplier.IsActive,
                CompanyName = jtMyobSupplier.CompanyName,
                FirstName = jtMyobSupplier.FirstName,
                LastName = jtMyobSupplier.LastName
            };
            HandyFunctions.Log(item.ToJson2());
            var task = _myService.InsertAsync(myCompanyFile, item, myCredentials, ct, ErrorLevel.WarningsAsErrors);
            await task;
            jtMyobSupplier.Uid = item.UID;
            connect.SaveChanges();
        }
    }
}