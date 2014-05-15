namespace SharpZendeskApi.Test.Unit
{
    using System;
    using System.Net;

    using FluentAssertions;

    using Moq;

    using RestSharp;

    using SharpZendeskApi.Exceptions;
    using SharpZendeskApi.Test.Common;
    using SharpZendeskApi.Test.Fakes;

    using Xunit;

    public class RequestHandlerTests
    {
        #region Public Methods and Operators

        [Fact]
        public void MakeRequestT_GivenGoodResponseNoDataExpectSharpZendeskException()
        {
            var goodResponse = GetResponseMock<Foo>();
            goodResponse.SetupProperty(x => x.Data, null);

            var testable = new Testable<RequestHandler>();
            InjectClientResponse(testable, goodResponse.Object);

            const string ExpectedMessage =
                "Response data was null or not in the correct format. Expected type: SharpZendeskApi.Test.Fakes.Foo";

            // act
            testable.ClassUnderTest.Invoking(x => x.MakeRequest<Foo>(new RestRequest()))
                .ShouldThrow<SharpZendeskException>()
                .WithMessage(ExpectedMessage);
        }

        [Fact]
        public void MakeRequestT_GivenGoodResponseWithDataExpectDataReturned()
        {
            var goodResponse = GetResponseMock<Foo>();
            var testable = new Testable<RequestHandler>();
            InjectClientResponse(testable, goodResponse.Object);

            // act
            var results = testable.ClassUnderTest.MakeRequest<Foo>(new RestRequest());

            results.Should().BeSameAs(goodResponse.Object.Data);
        }

        [Fact]
        public void MakeRequest_BadRequest()
        {
            const string ExpectedMessage = "badrequest";
            AssertMakeRequest(HttpStatusCode.BadRequest, typeof(ZendeskRequestException), ExpectedMessage);
        }

        [Fact]
        public void MakeRequest_DeleteNotOkNotNoContent()
        {
            const string ExpectedMessage = "deletenotoknotnocontent";
            var response =
                GetResponseMock(Method.DELETE, HttpStatusCode.Conflict)
                    .SetupProperty(x => x.Content, ExpectedMessage)
                    .Object;
            AssertMakeRequest(response, typeof(ZendeskRequestException), ExpectedMessage);
        }

        [Fact]
        public void MakeRequest_HasException()
        {
            const string ExpectedMessage = "hasexception";
            var response = GetResponseMock().SetupProperty(x => x.ErrorException, new Exception(ExpectedMessage)).Object;
            AssertMakeRequest(response, typeof(Exception), ExpectedMessage);
        }

        [Fact]
        public void MakeRequest_NotFound()
        {
            const string ExpectedMessage = "notfound";
            AssertMakeRequest(HttpStatusCode.NotFound, typeof(NotFoundException), ExpectedMessage);
        }

        [Fact]
        public void MakeRequest_NotOk()
        {
            const string ExpectedMessage = "notok";
            AssertMakeRequest(HttpStatusCode.Conflict, typeof(ZendeskRequestException), ExpectedMessage);
        }

        [Fact]
        public void MakeRequest_NullResponse()
        {
            const string ExpectedMessage = "The response was null.";
            IRestResponse response = null;
            AssertMakeRequest(response, typeof(SharpZendeskException), ExpectedMessage);
        }

        [Fact]
        public void MakeRequest_PostNotCreated()
        {
            const string ExpectedMessage = "postnotcreated";
            var response = GetResponseMock(Method.POST, HttpStatusCode.Conflict)
                .SetupProperty(x => x.Content, ExpectedMessage).Object;
            AssertMakeRequest(response, typeof(ZendeskRequestException), ExpectedMessage);
        }

        [Fact]
        public void MakeRequest_PutNotOkNotNotModified()
        {
            const string ExpectedMessage = "putnotoknotnotmodified";
            var response =
                GetResponseMock(Method.PUT, HttpStatusCode.Conflict)
                    .SetupProperty(x => x.Content, ExpectedMessage)
                    .Object;
            AssertMakeRequest(response, typeof(ZendeskRequestException), ExpectedMessage);
        }

        [Fact]
        public void MakeRequest_ResponseOriginalRequestNull()
        {
            const string ExpectedMessage = "The response object does not contain the original request.";
            var response = GetResponseMock().SetupProperty(x => x.Request, null).Object;
            AssertMakeRequest(response, typeof(SharpZendeskException), ExpectedMessage);
        }

        [Fact]
        public void MakeRequest_TooManyRequest()
        {
            const string ExpectedMessage = "toomanyrequests";
            AssertMakeRequest((HttpStatusCode)429, typeof(TooManyRequestsException), ExpectedMessage);
        }

        [Fact]
        public void MakeRequest_Unauthorized()
        {
            const string ExpectedMessage = "unauthorized";
            AssertMakeRequest(HttpStatusCode.Unauthorized, typeof(UnauthorizedAccessException), ExpectedMessage);
        }

        #endregion

        #region Methods

        private static void AssertMakeRequest(IRestResponse response, Type expectedException, string expectedMessage)
        {
            var testable = new Testable<RequestHandler>();
            InjectClientResponse(testable, response);

            // act
            var actualException = Assert.Throws(
                expectedException, () => testable.ClassUnderTest.MakeRequest(Mock.Of<IRestRequest>()));
            actualException.Message.Should().Be(expectedMessage);
        }

        private static void AssertMakeRequest(HttpStatusCode code, Type expectedException, string expectedMessage)
        {
            var response = GetResponseMock(code: code).SetupProperty(x => x.Content, expectedMessage).Object;
            AssertMakeRequest(response, expectedException, expectedMessage);
        }

        private static Mock<IRestResponse> GetResponseMock(
            Method method = Method.GET, 
            HttpStatusCode code = HttpStatusCode.OK)
        {
            var response = new Mock<IRestResponse>();
            response.SetupProperty(x => x.StatusCode, code)
                .SetupProperty(x => x.Request, Mock.Of<IRestRequest>(x => x.Method == method));

            return response;
        }

        private static Mock<IRestResponse<T>> GetResponseMock<T>(
            Method method = Method.GET, 
            HttpStatusCode code = HttpStatusCode.OK) where T : class, new()
        {
            var response = new Mock<IRestResponse<T>>();
            response.SetupProperty(x => x.StatusCode, code)
                .SetupProperty(x => x.Data, new T())
                .SetupProperty(x => x.Request, Mock.Of<IRestRequest>(x => x.Method == method));

            return response;
        }

        private static void InjectClientResponse<T, TData>(Testable<T> testable, IRestResponse<TData> response)
            where T : class where TData : class
        {
            testable.InjectMock<IRestClient>().Setup(x => x.Execute<TData>(It.IsAny<IRestRequest>())).Returns(response);
        }

        private static void InjectClientResponse<T>(Testable<T> testable, IRestResponse response) where T : class
        {
            testable.InjectMock<IRestClient>().Setup(x => x.Execute(It.IsAny<IRestRequest>())).Returns(response);
        }

        #endregion
    }
}