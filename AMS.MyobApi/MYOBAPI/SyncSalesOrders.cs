using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using MYOB.AccountRight.SDK;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.AccountRight.SDK.Contracts.Version2.Contact;
using MYOB.AccountRight.SDK.Contracts.Version2.Sale;
using MYOB.AccountRight.SDK.Extensions;
using MYOB.AccountRight.SDK.Services;
using MYOB.AccountRight.SDK.Services.Contact;
using MYOB.AccountRight.SDK.Services.Sale;
 
 
using SBD.AMS.Module.MYOB;
using MYOBApi.CSharp.Helpers;
using MYOBDocumentAction = MYOB.AccountRight.SDK.Contracts.Version2.Sale.DocumentAction;
using MYOBCustomerContact = MYOB.AccountRight.SDK.Contracts.Version2.Contact.Customer;

namespace SBD.AMS.MYOB
{
	public class SyncSalesOrders  
	{
		private CompanyFile myCompanyFile;
		private CompanyFileCredentials myCredentials;
		protected CustomerService myCustomerService;
		protected ItemOrderService myService;

         private string FormatMessage(WebException webEx)
        {
            var responseText = new StringBuilder();
            responseText.AppendLine(webEx.Message);
            responseText.AppendLine();

            // Call method 'GetResponseStream' to obtain stream associated with the response object 
            var response = webEx.Response;
            var receiveStream = response.GetResponseStream();

            var encode = Encoding.GetEncoding("utf-8");

            // Pipe the stream to a higher level stream reader with the required encoding format. 
            var readStream = new StreamReader(receiveStream, encode);
            var read = new Char[257];

            // Read 256 charcters at a time    . 
            var count = readStream.Read(read, 0, 256);
            responseText.AppendLine("HTML...");

            while (count > 0)
            {
                // Dump the 256 characters on a string and display the string onto the console. 
                var str = new String(read, 0, count);
                responseText.Append(str);
                count = readStream.Read(read, 0, 256);
            }
            responseText.Append("");

            // Release the resources of stream object.
            readStream.Close();

            // Release the resources of response object.
            response.Close();

            return responseText.ToString();
        }

        public bool AlreadyPosted(CancellationToken ct, IProgress<int> progressCallback, string OrderNumber)
		{
			var queryString = "$filter=Number eq '" + OrderNumber + "'";
			var orders = myService.GetRange(myCompanyFile, queryString, myCredentials);
			return orders.Count > 0;

		
		}

		private MYOBCustomer MakeMyobCustomer(MYOBCustomerContact myobCust)
		{
			var myobCustomer = new MYOBCustomer
			{
				CompanyName = myobCust.CompanyName,
				IsIndividual = myobCust.IsIndividual,
				FirstName = myobCust.FirstName,
				LastName = myobCust.LastName,
				Uid = myobCust.UID
			};
			return myobCustomer;
		}

		private static MYOBCustomerContact MakeMyobCustomerContact(MYOBCustomer myobCustomer)
		{
			var myobContact = new MYOBCustomerContact
			{
				CompanyName = myobCustomer.CompanyName,
				IsIndividual = myobCustomer.IsIndividual,
				FirstName = myobCustomer.FirstName,
				LastName = myobCustomer.LastName,
				UID = myobCustomer.Uid
			};
			return myobContact;
		}

		public void InitService(
			CompanyFile companyFile,
			CompanyFileCredentials credentials,
			ApiConfiguration myConfiguration,
			OAuthKeyService myOAuthKeyService)
		{
			myCompanyFile = companyFile;
			myCredentials = credentials;
			myService = new ItemOrderService(myConfiguration, null, myOAuthKeyService);
			myCustomerService = new CustomerService(myConfiguration, null, myOAuthKeyService);
		}
	}
}