using System;

namespace SBD.AMS.MYOB
{
	public class ExceptionMYOBContactMissMatch : Exception
	{
		 
		public ExceptionMYOBContactMissMatch(string message)
			: base(message)
		{
		}
	}
}