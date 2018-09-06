using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMS.Module.BusinessObjects;
using MYOB.AccountRight.SDK;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.AccountRight.SDK.Contracts.Version2.Inventory;
using MYOB.AccountRight.SDK.Services.Inventory;
 
 
using SBD.AMS.Module.MYOB;
using MYOBApi.CSharp.Helpers;

namespace SBD.AMS.MYOB
{
	public class SyncInventory : IMyobHandler
	{
		 
		 
		protected ItemService _myService;
		private List<Item> items;
	 

		public void SynchroniseItems( IProgress<int> progressCallback)
		{
			var info = "";
			using (var connect = new AMSDbContext(HandyFunctions.ConnectionString()))
			{
		
				var ar = items.OrderBy(x => x.Number).ToArray();
			//	RebuildMirrorTable(ar, info, connect,progressCallback);

			//	RebuildMirrorLinks(connect, ar);
			}
		}
		 
		//private static void RebuildMirrorLinks(AMSDbContext connect, Item[] ar)
		//{
		//	DisconnectMismatches(connect);

		//	// Connect Matches Add 
		//	foreach (var item in ar)
		//	{
		//		var rec = connect.Products.SingleOrDefault(x => x.Code == item.Number);
		//		var jtItem = GetMatchingJTItemByUid(connect, item);

		//		if (rec == null)
		//		{
		//			var newRec = new Product
		//			{
		//				Code = jtItem.Number, MYOBItem = jtItem, Description = jtItem.Description 
						

		//			};
		//			connect.Products.Add(newRec);
		//		}
		//		else
		//		{
		//			if (rec.MYOBItem == null)
		//			{
		//				rec.MYOBItem = jtItem;
		//			}
		//		}
		//	}
		//	connect.SaveChanges();
		//}
 

	 
 

		//private static void AddNewUpdateExistingMirrorRecords(Item[] ar, string info, AMSDbContext connect, IProgress<int> progressCallback)
		//{
		//	var stepNum = 1;
		//	var numSteps = ar.Count();
		//	foreach (var item in ar)
		//	{
		//		if (item.Number == info)
		//		{
		//			throw new ExceptionDuplicateKey($"code {info}");
		//		}
		//		info = item.Number;
		//		HandyFunctions.Log(info);
		//		var jtItem = GetMatchingJTItemByUid(connect, item);

		//		if (jtItem == null)
		//		{
		//			jtItem = new MYOBItem();
		//			connect.MYOBItems.Add(jtItem);
		//		}
		//		CopyItemFromMYOB(jtItem, item);
		//		progressCallback.Report((100 * stepNum) / numSteps);

		//		stepNum++;

		//	}
		//	connect.SaveChanges();
		//}

		private static MYOBItem GetMatchingJTItemByUid(AMSDbContext connect, Item item)
		{
			var jtItem = connect.MYOBItems.SingleOrDefault(x => x.Uid == item.UID); //unique idex should ensure only 1
			return jtItem;
		}

 
		public void InitService(ApiConfiguration myConfiguration, OAuthKeyService myOAuthKeyService)
		{

			_myService = new ItemService(myConfiguration, null, myOAuthKeyService);
			items = new List<Item>();
		}

		public async Task<int> Addtems(CompanyFile myCompanyFile, string pageFilter, CompanyFileCredentials myCredentials, CancellationToken ct)
		{
			var tpc = _myService.GetRangeAsync(myCompanyFile, pageFilter, myCredentials, ct, null);

			var newItems = await tpc; // fails here
			items.AddRange(newItems.Items);
			return newItems.Items.Count();
		}


		public int ItemCount => items.Count;


	    public string OrderBy => "Number";
	}
}