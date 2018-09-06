using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMS.Module.BusinessObjects;
using MYOB.AccountRight.SDK;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.AccountRight.SDK.Contracts.Version2;
using MYOB.AccountRight.SDK.Contracts.Version2.GeneralLedger;
using MYOB.AccountRight.SDK.Services.Contact;
using MYOB.AccountRight.SDK.Services.GeneralLedger;
 
using SBD.AMS.Module.MYOB;
using MYOBApi.CSharp.Helpers;

namespace SBD.AMS.MYOB
{
	public class SyncAccounts   : IMyobHandler
	{
		public enum MYOBAccountType
		{
			// dont assume MYOB wont reorder these
			Bank,
			AccountReceivable,
			OtherCurrentAsset,
			FixedAsset,
			OtherAsset,
			CreditCard,
			AccountsPayable,
			OtherCurrentLiability,
			LongTermLiability,
			OtherLiability,
			AccountType,
			Equity,
			Income,
			CostOfSales,
			Expense,
			OtherIncome,
			OtherExpense
		}
		protected AccountService _myService;
        protected CustomerService _myCustomerService { get; set; }
		private List<Account> items;
		 
  

		private MYOBAccount GetMatchingJTItemByUid<T1>(AMSDbContext connect, T1 account) where T1 : BaseEntity
		{
			return connect.MYOBAccounts.SingleOrDefault(x => x.Uid == account.UID);
		}

	 

		

		private string RebuildMirrorTable(Account[] ar, string info, AMSDbContext connect, IProgress<int> progressCallback)
		{
			var stepNum = 0;
			var numSteps = ar.Count();

			foreach (var item in ar)
			{
				if (item.DisplayID == info)
				{
					throw new ExceptionDuplicateKey($"DisplayID {info}");
				}
				info = item.DisplayID;
				HandyFunctions.Log(info);
				var jtItem = GetMatchingJTItemByUid(connect, item);

				if (jtItem == null)
				{
					jtItem = new MYOBAccount();
					connect.MYOBAccounts.Add(jtItem);
				}

				CopyItemFromMYOB(jtItem, item);
				progressCallback.Report((100 * stepNum) / numSteps);

				stepNum++;
			}
			connect.SaveChanges();

			foreach (var acc in connect.MYOBAccounts)
			{
				var item = ar.SingleOrDefault(x => x.UID == acc.Uid);
				if (item == null)
				{
					connect.MYOBAccounts.Remove(acc);
				}
			}
			connect.SaveChanges();
			return info;
		}

		private static void CopyItemFromMYOB(MYOBAccount jtItem, Account item)
		{
			jtItem.Uid = item.UID;
			jtItem.Name = item.Name;
			jtItem.DisplayID = item.DisplayID;
			jtItem.Type = item.Type.ToString();
			jtItem.RowVersion = item.RowVersion;
		}

	  
		 

		public void InitService(ApiConfiguration myConfiguration, OAuthKeyService myOAuthKeyService)
		{
			_myService = new AccountService(myConfiguration, null, myOAuthKeyService);
			items = new List<Account>();
		    _myCustomerService = new CustomerService(myConfiguration, null, myOAuthKeyService);
		}

	  

	    public async Task<int> Addtems(CompanyFile myCompanyFile, string pageFilter, CompanyFileCredentials myCredentials, CancellationToken ct)
		{
			var tpc = _myService.GetRangeAsync(myCompanyFile, pageFilter, myCredentials, ct, null);

			var newItems = await tpc; // fails here
			items.AddRange(newItems.Items);
			return newItems.Items.Count();
		}

		public int ItemCount => items.Count;

	    public void SynchroniseItems( IProgress<int> progressCallback)
		{
			var info = "";

			try
			{
				// psuedo code
				// add item if it is missing.
				// update item if it needs updating
				// delete items that no longer match MYOB's UID

				var ar = items.OrderBy(x => x.DisplayID).ToArray();
				using (var connect = new AMSDbContext(HandyFunctions.ConnectionString()))
				{
					info = RebuildMirrorTable(ar, info, connect, progressCallback);
				}
			}
			catch (Exception ex)
			{
				ex.Data.Add("ItemName", info);
				throw;
			}
		}
		public string OrderBy => "DisplayID";
	}
}