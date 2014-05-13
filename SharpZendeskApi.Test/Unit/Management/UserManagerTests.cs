using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpZendeskApi.Test.Unit.Management
{
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    using Xunit;

    public class UserManagerTests : ManagerTestBase<User, IUser, UserManager>
    {
        [Fact(Skip = "not implemented")]
        public override void SubmitNew_UsingParameterizedConstructor_ExpectSuccess()
        {
            throw new NotImplementedException();
        }
    }
}
