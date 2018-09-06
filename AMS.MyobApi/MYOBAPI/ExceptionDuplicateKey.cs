using System;

namespace SBD.AMS.MYOB
{
	public class ExceptionDuplicateKey : Exception
	{
		public ExceptionDuplicateKey(string message)
			: base(message)
		{
		}
	}
}