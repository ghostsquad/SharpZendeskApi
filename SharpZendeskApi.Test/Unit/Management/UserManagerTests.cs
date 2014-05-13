using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpZendeskApi.Test.Unit.Management
{
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    public class UserManagerTests : ManagerTestBase<User, IUser, UserManager>
    {
        public override void SubmitNew_UsingParameterizedConstructor_ExpectSuccess()
        {
            throw new NotImplementedException();
        }
    }
}
