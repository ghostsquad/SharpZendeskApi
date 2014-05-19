namespace SharpZendeskApi.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using RestSharp;

    using SharpZendeskApi.Exceptions;

    using Xunit;
    using Xunit.Extensions;

    public class ResponseExtensionTests
    {
        #region Public Properties

        public static IEnumerable<object[]> ProblemScenarios
        {
            get
            {
                IRestResponse nullResponse = null;
                IRestResponse unauthorizedResponse = new RestResponse
                                                         {
                                                             StatusCode = HttpStatusCode.Unauthorized, 
                                                             Content = "unauthorized"
                                                         };
                IRestResponse tooManyRequests = new RestResponse
                                                    {
                                                        StatusCode = (HttpStatusCode)429, 
                                                        Content = "toomanyrequests"
                                                    };
                IRestResponse otherException = new RestResponse
                                                   {
                                                       ErrorException =
                                                           new InvalidOperationException("otherexception")
                                                   };
                IRestResponse nullRequest = new RestResponse();
                IRestResponse getNotFound = new RestResponse
                                                {
                                                    Request = new RestRequest { Method = Method.GET }, 
                                                    StatusCode = HttpStatusCode.NotFound, 
                                                    Content = "getnotfound"
                                                };

                IRestResponse getOther = new RestResponse
                                             {
                                                 Request = new RestRequest { Method = Method.GET }, 
                                                 StatusCode = HttpStatusCode.NoContent, 
                                                 Content = "getother"
                                             };

                IRestResponse postNotCreated = new RestResponse
                                                   {
                                                       Request = new RestRequest { Method = Method.POST }, 
                                                       StatusCode = HttpStatusCode.NotAcceptable, 
                                                       Content = "postnotcreated"
                                                   };

                IRestResponse putNotOkNotNotModified = new RestResponse
                                                           {
                                                               Request =
                                                                   new RestRequest { Method = Method.PUT }, 
                                                               StatusCode = HttpStatusCode.NotAcceptable, 
                                                               Content = "putNotOkNotNotModified"
                                                           };

                IRestResponse deleteNotOkNotNoContent = new RestResponse
                                                            {
                                                                Request =
                                                                    new RestRequest
                                                                        {
                                                                            Method =
                                                                                Method.DELETE
                                                                        }, 
                                                                StatusCode = HttpStatusCode.NotAcceptable, 
                                                                Content = "deleteNotOkNotNoContent"
                                                            };

                // Or this could read from a file. :)
                return new[]
                           {
                               new object[] { nullResponse, typeof(SharpZendeskException), "The response was null." }, 
                               new object[] { unauthorizedResponse, typeof(UnauthorizedAccessException), "unauthorized" }, 
                               new object[] { tooManyRequests, typeof(TooManyRequestsException), "toomanyrequests" }, 
                               new object[] { otherException, typeof(InvalidOperationException), "otherexception" }, 
                               new object[]
                                   {
                                       nullRequest, typeof(SharpZendeskException), 
                                       "The response object does not contain the original request."
                                   }, 
                               new object[] { getNotFound, typeof(NotFoundException), "getnotfound" }, 
                               new object[] { getOther, typeof(ZendeskRequestException), "getother" }, 
                               new object[] { postNotCreated, typeof(ZendeskRequestException), "postnotcreated" }, 
                               new object[]
                                   {
                                      putNotOkNotNotModified, typeof(ZendeskRequestException), "putNotOkNotNotModified" 
                                   }, 
                               new object[]
                                   {
                                      deleteNotOkNotNoContent, typeof(ZendeskRequestException), "deleteNotOkNotNoContent" 
                                   }, 
                           };
            }
        }

        #endregion

        #region Public Methods and Operators

        [Fact]
        public void ThrowIfProblem_GivenGenericNullResponse_ExpectNonGenericMethodCallAndSharpZendeskException()
        {
            // arrange
            IRestResponse<string> givenResponse = null;

            // act & assert
            //var actualException = Assert.Throws<SharpZendeskException>(() => givenResponse.ThrowIfProblem());

            //actualException.Message.Should().Be("The response was null.");
        }

        [Fact]
        public void ThrowIfProblem_GivenGenericResponseAndNullData_ExpectSharpZendeskException()
        {
            // arrange
            IRestResponse<string> givenResponse = new RestResponse<string>
                                                      {
                                                          StatusCode = HttpStatusCode.OK, 
                                                          Request = new RestRequest()
                                                      };

            // act & assert
            //var actualException = Assert.Throws<SharpZendeskException>(() => givenResponse.ThrowIfProblem());

            //actualException.Message.Should()
                //.Be("Response data was null or not in the correct format. Expected type: " + typeof(string));
        }

        [Fact]
        public void ThrowIfProblem_GivenGenericResponseNoProblem_ExpectNoThrow()
        {
            // arrange
            IRestResponse<string> givenResponse = new RestResponse<string>
                                                      {
                                                          StatusCode = HttpStatusCode.OK, 
                                                          Request = new RestRequest(), 
                                                          Data = "test"
                                                      };

            // act & assert
            //givenResponse.Invoking(x => x.ThrowIfProblem()).ShouldNotThrow("because there is no problem");
        }

        [Theory]
        [PropertyData("ProblemScenarios")]
        public void ThrowIfProblem_GivenProblemScenarios_ExpectProperExceptionAndMessage(
            IRestResponse response, 
            Type expectedException, 
            string expectedMessage)
        {
            // act & assert
            //var actualException = Assert.Throws(expectedException, () => response.ThrowIfProblem());
            //actualException.Message.Should().Be(expectedMessage);
        }

        #endregion
    }
}