using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.AccountRight.SDK.Contracts.Version2.Sale;
using MYOB.AccountRight.SDK.Services;
using MYOB.AccountRight.SDK.Services.Sale;
 
 
using MYOBApi.CSharp.Helpers;
using  SDK =MYOB.AccountRight.SDK  ;

namespace SBD.AMS.MYOB
{
	// http://developer.myob.com/api/accountright/v2/sale/invoice/
    public class SyncInvoices  
    {
        protected ItemInvoiceService myService;
        private CompanyFile myCompanyFile;
        private SDK.CompanyFileCredentials myCredentials;
       

        public void InitService(
            CompanyFile companyFile,
            SDK.CompanyFileCredentials credentials,
            SDK.ApiConfiguration myConfiguration,
            OAuthKeyService myOAuthKeyService )
        {
            myCompanyFile = companyFile;
            myCredentials = credentials;
          
            myService = new ItemInvoiceService(myConfiguration, null, myOAuthKeyService);


        }
    
  
    }
     
}
