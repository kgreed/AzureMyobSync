using System;
using System.Threading;
using System.Threading.Tasks;
using MYOB.AccountRight.SDK;
using MYOB.AccountRight.SDK.Contracts;
using MYOBApi.CSharp.Helpers;
 

namespace SBD.AMS.MYOB
 {
 

	public interface IMyobHandler
	{
		void InitService(ApiConfiguration myConfiguration,  OAuthKeyService myOAuthKeyService);
		Task<int> Addtems(CompanyFile myCompanyFile, string pageFilter, CompanyFileCredentials myCredentials, CancellationToken ct);
		int ItemCount { get; }

		 
		void SynchroniseItems( IProgress<int> progress);
		string OrderBy { get; }
	}
}