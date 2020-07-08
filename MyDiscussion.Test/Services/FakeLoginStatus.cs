using MyDiscussion.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDiscussion.Test.Services
{
	class FakeLoginStatus : ILoginStatus
	{
		public bool IsLoggedIn => RealUserId != null;

		public Guid UserId => RealUserId.Value;

		public Guid? RealUserId { get; set; }
	}
}
