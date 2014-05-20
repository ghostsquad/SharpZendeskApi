namespace SharpZendeskApi.Test.End2End
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using FluentAssertions;

    using SharpZendeskApi.Management;
    using SharpZendeskApi.Test.Common;

    using Xunit;

    public class UserTests
    {
        private readonly IUserManager userManager;

        public UserTests()
        {
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Basic);
            this.userManager = new UserManager(client);
        }

        [Fact(Timeout = 10000)]
        public void Search_UsingSandboxCredentials()
        {
            var credentialData = TestHelpers.GetTestCredential(ZendeskAuthenticationMethod.Basic);

            // act
            var actualListing = this.userManager.Search(credentialData.Value.Username);

            actualListing.Should().NotBeNull();
            var items = actualListing.ToList();
            items.Should().HaveCount(1);

            items[0].Email.Should().Be(credentialData.Value.Username);
        }

        [Fact(Timeout = 10000)]
        public void Get_UsingKnownId()
        {
            var credentialData = TestHelpers.GetTestCredential(ZendeskAuthenticationMethod.Basic);
            var knownUser = this.userManager.Search(credentialData.Value.Username).First();

            // act
            var actualUser = this.userManager.Get(knownUser.Id.Value);

            actualUser.Should().NotBeNull();
            actualUser.Id.Should().Be(knownUser.Id);
        }
    }
}
