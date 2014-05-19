namespace SharpZendeskApi.Test.Unit.Management
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using Moq;

    using Ploeh.AutoFixture;

    using RestSharp;

    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    using Xunit;
    using Xunit.Should;

    public class TicketManagerTests : ManagerTestBase<Ticket, ITicket, TicketManager>
    {        
        #region Public Methods and Operators

        [Fact]
        public void FromView_AssertRequest()
        {
            IRestRequest actualRequest = null;
            this.SetupListingResponse(x => actualRequest = x, 1);

            var expectedResourceParameter = "views/" + ExpectedId + "/tickets.json";

            // act
            this.testable.ClassUnderTest.FromView(1).ToList();

            // assert
            actualRequest.Should().NotBeNull();
            actualRequest.Resource.Should().Be(expectedResourceParameter);
            actualRequest.Method.Should().Be(Method.GET);
        }        

        #endregion
    }
}