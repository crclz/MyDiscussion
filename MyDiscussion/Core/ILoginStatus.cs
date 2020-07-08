using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDiscussion.Core
{
	public interface ILoginStatus
	{
		public bool IsLoggedIn { get; }
		public Guid UserId { get; }
	}
}
