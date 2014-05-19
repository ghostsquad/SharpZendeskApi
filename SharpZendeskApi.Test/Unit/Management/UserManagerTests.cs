namespace SharpZendeskApi.Test.Unit.Management
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    using Xunit;

    public class UserManagerTests : ManagerTestBase<User, IUser, UserManager, IUserManager>
    {
    }
}
