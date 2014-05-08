namespace SharpZendeskApi.Test.Integration
{
    using System.Net;

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
            request.AddHeader("Keep-Alive", "max=1, timeout=1");

            return request;
        }

        /// <summary>
        /// The given incorrect password based credentials to known zendesk portal when ticket requested expect exception.
        /// </summary>
        [Fact]        
        public void GivenIncorrectPasswordBasedCredentialsToKnownZendeskPortalWhenTicketRequestedExpectUnauthorized()
        {           
            // arrange
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Basic, false);
            
            // act
            var actualResponse = client.Execute(this.GetNewRequest());                        

            // assert
            actualResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized, "because the password given is incorrect");            
        }

        /// <summary>
        /// The given incorrect token based credentials to known zendesk portal when ticket requested expect exception.
        /// </summary>
        [Fact]
        public void GivenIncorrectTokenBasedCredentialsToKnownZendeskPortalWhenTicketRequestedExpectUnauthorized()
        {
            // arrange
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Token, false);

            // act
            var actualResponse = client.Execute(this.GetNewRequest());                       

            // assert
            actualResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized, "because the token givecn is incorrect");            
        }

        /// <summary>
        /// The given password based credentials to known zendesk portal when ticket requested expect ok response.
        /// </summary>
        [Fact]
        public void GivenPasswordBasedCredentialsToKnownZendeskPortalWhenTicketRequestedExpectOkResponse()
        {
            // arrange
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Basic);

            // act
            var actualResponse = client.Execute(this.GetNewRequest());            

            // assert
            actualResponse.StatusCode.Should().Be(HttpStatusCode.OK, "because the password is correct");           
        }

        /// <summary>
        /// The given token based credentials to known zendesk portal when ticket requested expect ok response.
        /// </summary>
        [Fact]
        public void GivenTokenBasedCredentialsToKnownZendeskPortalWhenTicketRequestedExpectOkResponse()
        {
            // arrange
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Token);

            // act
            var actualResponse = client.Execute(this.GetNewRequest());                        

            // assert
            actualResponse.StatusCode.Should().Be(HttpStatusCode.OK, "because the token is correct");            
        }

        #endregion
    }
}