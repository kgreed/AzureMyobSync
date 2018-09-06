using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MYOB.AccountRight.SDK;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.AccountRight.SDK.Contracts.Version2.Contact;
using MYOB.AccountRight.SDK.Contracts.Version2.GeneralLedger;
using MYOB.AccountRight.SDK.Services;
using MYOB.AccountRight.SDK.Services.GeneralLedger;
using MYOBApi.CSharp.Helpers;
using SBD.AMS.Module.MYOB;
using t = System.Threading.Tasks;


namespace SBD.AMS.MYOB
{
    public class MyobHandler : IDisposable
    {
        private CompanyFile _companyFile;
        private ApiConfiguration _configurationCloud;
        private CompanyFileCredentials _credentials;
        private OAuthKeyService _oAuthKeyService;


        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public static void ListFiles()
        {
            var configuration = new ApiConfiguration("http://localhost:8080/accountright");
            var cfService = new CompanyFileService(configuration);
            var companyFiles = cfService.GetRange();
            var companyFile =
                companyFiles.FirstOrDefault(x => new Version(x.ProductVersion) >= new Version("2015.3"));
            var credentials = new CompanyFileCredentials("Administrator", "");
            var accountService = new AccountService(configuration);
            var accounts = accountService.GetRange(companyFile, null, credentials);
            MessageBox.Show($"{accounts.Count} accounts");
        }

        // <summary>
        /// This public method may be very long running, so it returns a Task which allows calling
        /// application to 'await' the asynchronous return of a future result (or an exception).
        /// </summary>
        /// <remarks>
        ///     This is an example of a public library method that may run for a long time
        ///     By passing a CancellationToken we can test if the caller wants us to cancel.
        ///     By passing an IProgress we can pass information back to the caller (conveniently it
        ///     happens on the original thread if the caller is a Forms or WPF app).
        /// </remarks>
        public void PrepareToCommunicateWithMyob(string MYOBLogin)
        {
            //http://stackoverflow.com/questions/11879967/best-way-to-convert-callback-based-async-method-to-awaitable-task


            var developerKey = ConfigurationManager.AppSettings["MyobDeveloperKey"];


            var developerSecret = ConfigurationManager.AppSettings["MyobDeveloperSecret"];

            _configurationCloud = new ApiConfiguration(developerKey, developerSecret, "http://desktop");

            // you many get a benign error here on the fist time - if the tokens have not been stored
            try
            {
                TryGetTokens();
            }
            catch (Exception)
            {
                try
                {
                    TryGetTokens();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            var task2 = GetCompanyFiles(_configurationCloud, _oAuthKeyService);
            var companyFiles = task2.Result; // waits for the files
            _companyFile = companyFiles.First();
            _credentials = AskForCredentials(_companyFile, MYOBLogin);
        }

        private void TryGetTokens()
        {
            _oAuthKeyService = new OAuthKeyService();
            if (_oAuthKeyService.OAuthResponse != null) return;
            var oauthService = new OAuthService(_configurationCloud);
            var code = OAuthLogin.GetAuthorizationCode(_configurationCloud);
            var tokens = oauthService.GetTokens(code);
            _oAuthKeyService.OAuthResponse = tokens;
        }

        public async t.Task<int> RunSyncs(
            CancellationToken ct,
            Progress<int> progress,
            List<MYOBTypeEnum> myobTypes)
        {
            var sw = new Stopwatch();
            sw.Start();
            var s = new StringBuilder();
            try
            {
                foreach (var myobType in myobTypes.Where(x =>
                    x != MYOBTypeEnum.SalesInvoice && x != MYOBTypeEnum.SalesOrder))
                {
                    var num =
                        await
                            SyncTable(
                                myobType,
                                _configurationCloud,
                                _companyFile,
                                _credentials,
                                _oAuthKeyService,
                                ct,
                                progress);
                    s.AppendLine($"{num} {myobType} synchronised");
                }


                sw.Stop();
                s.AppendLine($"{Convert.ToInt32(sw.ElapsedMilliseconds / 1000)} seconds");
                MessageBox.Show(s.ToString(), "MYOB Communication");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return 0;
        }

        //http://codereview.stackexchange.com/questions/42652/chaining-tasks-with-async-await
        public t.Task<CompanyFile[]> GetCompanyFiles(ApiConfiguration configuration, IOAuthKeyService authKeyService)
        {
            var tcs = new t.TaskCompletionSource<CompanyFile[]>();
            t.Task.Run(
                async () =>
                {
                    var cfsCloud = new CompanyFileService(configuration, null, authKeyService);
                    var files = await cfsCloud.GetRangeAsync();
                    tcs.SetResult(files);
                });
            return tcs.Task;
        }

        private static CompanyFileCredentials AskForCredentials(CompanyFile companyFile, string MYOBLogin)
        {
            //var prompt = string.Format( "Password for login {0} " ,& MYOBLogin
            var o = new MYOBLogin {Login = MYOBLogin};
            CompanyFileCredentials credentials = null;
            var dlg = o.ShowDialog();
            if (dlg == DialogResult.OK)
            {
                var login = o.Login;
                var password = o.Password();
                credentials = new CompanyFileCredentials(login, password);
            }

            return credentials;
        }

        private t.Task<OAuthTokens> GetOAuthTokens()
        {
            var tcs = new t.TaskCompletionSource<OAuthTokens>();
            t.Task.Run(
                async () =>
                {
                    var oauthService = new OAuthService(_configurationCloud);
                    var code = OAuthLogin.GetAuthorizationCode(_configurationCloud);
                    var response = await oauthService.GetTokensAsync(code);
                    tcs.SetResult(response);
                });
            return tcs.Task;
        }

        //public MyobPost MakePostServiceHandler(
        //    MYOBTypeEnum type,
        //    ApiConfiguration myConfiguration,
        //    OAuthKeyService myOAuthKeyService,
        //    CancellationToken ct)
        //{
        //    switch (type)
        //    {
        //        case MYOBTypeEnum.SalesInvoice:
        //            var syncinv = new SyncInvoices();
        //            syncinv.InitService(_companyFile, _credentials, myConfiguration, myOAuthKeyService);
        //            return syncinv;
        //        case MYOBTypeEnum.SalesOrder:
        //            var o = new SyncSalesOrders();
        //            o.InitService(_companyFile, _credentials, myConfiguration, myOAuthKeyService);
        //            return o;


        //        default:
        //            throw new ArgumentOutOfRangeException("type");
        //    }
        //}

        public IMyobHandler MakeServiceHandler(
            MYOBTypeEnum type,
            ApiConfiguration myConfiguration,
            OAuthKeyService myOAuthKeyService)
        {
            IMyobHandler o = null;
            switch (type)
            {
                case MYOBTypeEnum.TaxRate:
                    o = new SyncTaxCodes();
                    break;
                case MYOBTypeEnum.GeneralLedgerAccount:
                    o = new SyncAccounts();
                    break;
                case MYOBTypeEnum.Inventory:
                    o = new SyncInventory();
                    break;

                case MYOBTypeEnum.Customer:
                    o = new SyncCustomers();
                    break;
                case MYOBTypeEnum.Supplier:
                    o = new SyncSuppliers();
                    break;

                //case MYOBTypeEnum.NotApplicable:
                //	break;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }

            o.InitService(myConfiguration, myOAuthKeyService);
            return o;
        }

        public static SupplierLink MakeSupplierCardLink(MYOBSupplier supplier)
        {
            var c = new SupplierLink
            {
                DisplayID = supplier.DisplayID,
                Name = supplier.CompanyName,
                UID = supplier.Uid,
                URI = new Uri(supplier.URI)
            };
            if (c.UID == null) throw new Exception($"UID not set for customer {supplier.DisplayID}");
            if (c.URI == null) throw new Exception($"URI not set for {supplier.DisplayID}  ");
            return c;
        }

        public static CardLink MakeCustomerCardLink(MYOBCustomer customer)
        {
            var c = new CardLink
            {
                DisplayID = customer.DisplayID,
                Name = customer.CompanyName,
                UID = customer.Uid,
                URI = new Uri(customer.URI)
            };
            if (c.UID == null) throw new Exception($"UID not set for customer {customer.DisplayID}");
            if (c.URI == null) throw new Exception($"URI not set for {customer.DisplayID}  ");
            return c;
        }

        public static CustomerLink MakeCustomerLink(MYOBCustomer customer)
        {
            var c = new CustomerLink
            {
                DisplayID = customer.DisplayID,
                Name = customer.CompanyName,
                UID = customer.Uid,
                URI = new Uri(customer.URI)
            };
            if (c.UID == null) throw new Exception($"UID not set for customer {customer.DisplayID}");
            if (c.URI == null) throw new Exception($"URI not set for {customer.DisplayID}  ");
            return c;
        }

        public async t.Task<int> SyncTable(
            MYOBTypeEnum type,
            ApiConfiguration myConfiguration,
            CompanyFile myCompanyFile,
            CompanyFileCredentials myCredentials,
            OAuthKeyService myOAuthKeyService,
            CancellationToken ct,
            IProgress<int> progress)
        {
            var _currentPage = 1;
            const double PageSize = 400;
            var myMYOBhandler = MakeServiceHandler(type, myConfiguration, myOAuthKeyService);
            try
            {
                var totalPages = 0;
                var numAdded = 0;
                do
                {
                    var pageFilter =
                        $"$top={PageSize}&$skip={PageSize * (_currentPage - 1)}&$orderby={myMYOBhandler.OrderBy}";
                    numAdded = await myMYOBhandler.Addtems(myCompanyFile, pageFilter, myCredentials, ct);
                    if (totalPages == 0) totalPages = (int) Math.Ceiling(myMYOBhandler.ItemCount / PageSize);
                    _currentPage++;
                } while (numAdded > 0); //(_currentPage <= totalPages);

                myMYOBhandler.SynchroniseItems(progress);
                return myMYOBhandler.ItemCount;
            }
            catch (ApiCommunicationException ex)
            {
                HandyFunctions.Log(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                HandyFunctions.Log(ex.ToString());
                throw;
            }
        }


        public static AccountLink MakeAccountLink(MYOBAccount account)
        {
            var lk = new AccountLink {UID = account.Uid, DisplayID = account.DisplayID, Name = account.Name};
            if (lk.UID == null) throw new Exception($"UID not set for account {account.DisplayID}");
            if (lk.URI == null) throw new Exception($"URI not set for account {account.DisplayID}");
            return lk;
        }

        protected void OnError(Uri uri, Exception ex)
        {
            //Display the formatted message
            switch (ex.GetType().Name)
            {
                case "WebException":
                    MessageBox.Show(FormatMessage((WebException) ex));
                    break;
                case "ApiCommunicationException":
                    MessageBox.Show(FormatMessage((WebException) ex.InnerException));
                    break;
                case "ApiOperationException":
                    MessageBox.Show(ex.Message);
                    break;
                default:
                    MessageBox.Show(ex.Message);
                    break;
            }

            //	HideSpinner();
        }


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
            var read = new char[257];

            // Read 256 charcters at a time    . 
            var count = readStream.Read(read, 0, 256);
            responseText.AppendLine("HTML...");

            while (count > 0)
            {
                // Dump the 256 characters on a string and display the string onto the console. 
                var str = new string(read, 0, count);
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
    }
}