namespace SharpZendeskApi.Test.Unit.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SharpZendeskApi.Models;
    using SharpZendeskApi.Test.Common.JsonObjects;

    using Xunit;

    public class UserTests : ModelTestBase<UserJson, User, IUser>
    {
        [Fact(Skip = "not implemented")]
        public override void CanCreateWithFilledMandatoryPropertiesUsingConstructor()
        {
            throw new NotImplementedException();
        }
    }
}
