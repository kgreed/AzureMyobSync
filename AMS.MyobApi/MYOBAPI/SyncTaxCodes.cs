using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AMS.Module.BusinessObjects;
using MYOB.AccountRight.SDK;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.AccountRight.SDK.Contracts.Version2.GeneralLedger;
using MYOB.AccountRight.SDK.Services.GeneralLedger;
 
using SBD.AMS.Module.MYOB;
using MYOBApi.CSharp.Helpers;
using t = System.Threading.Tasks;

namespace SBD.AMS.MYOB
{

	public class SyncTaxCodes : IMyobHandler
	{
		protected TaxCodeService _myService;
		private List<TaxCode> items;
	 
		public void InitService(ApiConfiguration myConfiguration, OAuthKeyService myOAuthKeyService)
		{
			_myService = new TaxCodeService(myConfiguration, null, myOAuthKeyService);
			items = new List<TaxCode>();
		}

		public async t.Task<int> Addtems(CompanyFile myCompanyFile, string pageFilter, CompanyFileCredentials myCredentials, CancellationToken ct)
		{
			var tpc = _myService.GetRangeAsync(myCompanyFile, pageFilter, myCredentials, ct, null);
			var newItems = await tpc;
			items.AddRange(newItems.Items);
			return newItems.Items.Count();
		}

		public int ItemCount => items.Count;


		public string OrderBy => "Code";


		private MYOBTaxCode GetMatchingJTItemByUid(AMSDbContext connect, TaxCode taxcode)
		{
			return connect.MYOBTaxCodes.SingleOrDefault(x => x.Uid == taxcode.UID);
		}

		public void SynchroniseItems( IProgress<int> progressCallback)
		{
			var ar = items.OrderBy(x => x.Code).ToArray();
			using (var connect = new AMSDbContext(HandyFunctions.ConnectionString()))
			{
				RebuildMirrorTable(ar, connect, progressCallback);

				//RebuildMirrorLinks(connect, ar);
			}
			 
		}

		//private void RebuildMirrorLinks(AMSDbContext connect, TaxCode[] ar)
		//{
		//	// disconnect mismatches
		//	foreach (var taxrate in connect.TaxRates.Where(x => x.MYOBTaxCode != null && x.Code != x.MYOBTaxCode.Code))
		//	{
		//		taxrate.MYOBTaxCode = null;
		//	}
		//	connect.SaveChanges();

		//	// add missing TaxRates
		//	foreach (var item in ar)
		//	{
		//		HandyFunctions.Log("checking {0}", item.Code);
		//		var taxRate = connect.TaxRates.SingleOrDefault(x => x.Code == item.Code);

		//		if (taxRate == null)
		//		{
		//			var jtItem = GetMatchingJTItemByUid(connect, item);

		//			taxRate = new TaxRate { Code = jtItem.Code, Percentage = jtItem.Rate, MYOBTaxCode = jtItem };
		//			connect.TaxRates.Add(taxRate);
		//			taxRate.MYOBTaxCode = jtItem;
		//		}
		//		else
		//		{
		//			if (taxRate.MYOBTaxCode == null)
		//			{
		//				var jtItem = GetMatchingJTItemByUid(connect, item);
		//				taxRate.MYOBTaxCode = jtItem;
		//			}
		//		}
		//	}
		//	connect.SaveChanges();
		//}

		private void RebuildMirrorTable(TaxCode[] ar, AMSDbContext connect, IProgress<int> progressCallback)
		{
			var info = "";
			var stepNum = 1;
			var numSteps = ar.Count();
			try
			{
				foreach (var item in ar)
				{
					if (item.Code == info)
					{
						throw new ExceptionDuplicateKey($"Taxcode {info}");
					}
					info = item.Code;
					HandyFunctions.Log(info);
					var jtItem = GetMatchingJTItemByUid(connect, item);

					if (jtItem == null)
					{
						jtItem = new MYOBTaxCode();
						connect.MYOBTaxCodes.Add(jtItem);
					}

					CopyItemFromMYOB(connect, jtItem, item);
					progressCallback.Report((100 * stepNum) / numSteps);
				
					stepNum++;


				}
				connect.SaveChanges();

				// delete missing
				foreach (var taxcode in connect.MYOBTaxCodes)
				{
					var item = ar.SingleOrDefault(x => x.UID == taxcode.Uid);
					if (item == null)
					{
						//var codeString = item.Code;
						//foreach (var rate in connect.TaxRates.Where(x => x.MYOBTaxCode != null && x.MYOBTaxCode.Code == codeString))
						//{
						//	rate.MYOBTaxCode = null;
						//}
						connect.MYOBTaxCodes.Remove(taxcode);
					}
				}
				connect.SaveChanges();
			}
			catch (Exception ex)
			{
				ex.Data.Add("TaxCode", info);
				throw;
			}
		}

		private static void CopyItemFromMYOB(AMSDbContext connect, MYOBTaxCode jtItem, TaxCode item)
		{
			jtItem.Uid = item.UID;
			jtItem.URI = item.URI.ToString();
			jtItem.Code = item.Code;
			jtItem.Description = item.Description;
			jtItem.Rate = item.Rate;
			jtItem.TypeCode = item.Type.ToString();

			if (item.TaxCollectedAccount == null)
			{
				jtItem.TaxCollectedAccount = null;
			}
			else
			{
				jtItem.TaxCollectedAccount = connect.MYOBAccounts.SingleOrDefault(x => x.Uid == item.TaxCollectedAccount.UID);
			}

			if (item.TaxPaidAccount == null)
			{
				jtItem.TaxPaidAccount = null;
			}
			else
			{
				jtItem.TaxPaidAccount = connect.MYOBAccounts.SingleOrDefault(x => x.Uid == item.TaxPaidAccount.UID);
			}

			jtItem.RowVersion = item.RowVersion;
		}

	 

	}
}