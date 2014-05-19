namespace SharpZendeskApi.Test.Integration
{
    using System;
    using System.Net;

    using Cavity;

    using FluentAssertions;

    using RestSharp;

    using SharpZendeskApi.Test.Common;

    using Xunit;

    public class AuthenticationTests
    {               
        #region Public Methods and Operators

        private IRestRequest GetNewRequest()
        {
            var request = new RestRequest { Method = Method.GET, Resource = "tickets/{id}.json" };
            request.AddUrlSegment("id", "1");

            return request;
        }

        /// <summary>
        /// The given incorrect password based credentials to known zendesk portal when ticket requested expect exception.
        /// </summary>
        [Fact(Timeout = 10000)]        
        public void GivenIncorrectPasswordBasedCredentialsToKnownZendeskPortalWhenTicketRequestedExpectUnauthorized()
        {           
            // arrange
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Basic, false);

            // act
            client.RequestHandler.Invoking(x => x.MakeRequest(this.GetNewRequest())).ShouldThrow<UnauthorizedAccessException>();
        }

        /// <summary>
        /// The given incorrect token based credentials to known zendesk portal when ticket requested expect exception.
        /// </summary>
        [Fact(Timeout = 10000)]
        public void GivenIncorrectTokenBasedCredentialsToKnownZendeskPortalWhenTicketRequestedExpectUnauthorized()
        {
            // arrange
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Token, false);

            // act
            client.RequestHandler.Invoking(x => x.MakeRequest(this.GetNewRequest())).ShouldThrow<UnauthorizedAccessException>();
        }

        /// <summary>
        /// The given password based credentials to known zendesk portal when ticket requested expect ok response.
        /// </summary>
        [Fact(Timeout = 10000)]
        public void GivenPasswordBasedCredentialsToKnownZendeskPortalWhenTicketRequestedExpectOkResponse()
        {
            // arrange
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Basic);

            // act
            client.RequestHandler.MakeRequest(this.GetNewRequest());
        }

        /// <summary>
        /// The given token based credentials to known zendesk portal when ticket requested expect ok response.
        /// </summary>
        [Fact(Timeout = 10000)]
        public void GivenTokenBasedCredentialsToKnownZendeskPortalWhenTicketRequestedExpectOkResponse()
        {
            // arrange
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Token);

            // act
            client.RequestHandler.MakeRequest(this.GetNewRequest());
        }

        #endregion
    }
}