namespace SharpZendeskApi.Test.Unit.Management
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using RestSharp;

    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    using Xunit;
    using Xunit.Should;

    public class UserManagerTests : ManagerTestBase<User, IUser, UserManager, IUserManager>
    {
        [Fact]
        public void Search_AssertRequest()
        {
            IRestRequest actualRequest = null;
            this.SetupListingResponse(x => actualRequest = x, 1);

            const string SearchString = "foo";

            const string ExpectedResource = "users/search.json";

            // act
            this.TestableInterface.Search(SearchString);

            actualRequest.Method.Should().Be(Method.GET);
            actualRequest.Resource.Should().Be(ExpectedResource);
            actualRequest.Parameters.Should().HaveCount(1);

            var actualParameter = actualRequest.Parameters[0];
            actualParameter.Name.Should().Be("query");
            actualParameter.Type.Should().Be(ParameterType.QueryString);
            actualParameter.Value.Should().Be(SearchString);
        }

        [Fact]
        public void Search_AssertReturnValue()
        {
            var expectedListing = this.SetupListingResponse(x => { }, 1);

            // act
            var actualListing = this.TestableInterface.Search("foo");

            actualListing.ShouldBeSameAs(expectedListing);
        }
    }
}
