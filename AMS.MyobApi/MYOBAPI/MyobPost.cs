using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SBD.AMS.MYOB
{
	public abstract class MyobPost  
	{
		public abstract Task<int> Post(CancellationToken ct, IProgress<int> progress);

		public virtual  string ErrorMessage(Exception ex)
		{
			var err =ex as global::MYOB.AccountRight.SDK.ApiValidationException;
			var s = new StringBuilder();
			if (err == null) return s.ToString();
			foreach (var e in err.Errors)
			{
				s.AppendLine(e.Message);
				s.AppendLine(e.AdditionalDetails);

			}
			s.AppendLine(err.ErrorInformation);
			return s.ToString();
		}
	}
}