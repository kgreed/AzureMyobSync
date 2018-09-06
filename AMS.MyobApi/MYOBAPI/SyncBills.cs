using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.AccountRight.SDK.Contracts.Version2.Purchase;
using MYOB.AccountRight.SDK.Services;
using MYOB.AccountRight.SDK.Services.Purchase;
 
 
using MYOBApi.CSharp.Helpers;
using SDK = MYOB.AccountRight.SDK;

namespace SBD.AMS.MYOB
{
	public class SyncBills  
	{
		protected ItemBillService myService;
		private CompanyFile myCompanyFile;
		private SDK.CompanyFileCredentials myCredentials;


		public void InitService(
			CompanyFile companyFile,
			SDK.CompanyFileCredentials credentials,
			SDK.ApiConfiguration myConfiguration,
			OAuthKeyService myOAuthKeyService)
		{
			myCompanyFile = companyFile;
			myCredentials = credentials;

			myService = new ItemBillService(myConfiguration, null, myOAuthKeyService);


		}

	 
	}
}
