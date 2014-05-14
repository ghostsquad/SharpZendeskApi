namespace SharpZendeskApi.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Moq;

    using RestSharp;

    using SharpZendeskApi.Exceptions;
    using SharpZendeskApi.Test.Common;

    using Xunit;

    public class RequestHandlerTests
    {        
        public class Foo
        {
        }

        private static void InjectClientResponse<T, TData>(Testable<T> testable, IRestResponse<TData> response)
            where T : class
            where TData : class
        {
            testable.InjectMock<IRestClient>()
                .Setup(x => x.Execute<TData>(It.IsAny<IRestRequest>()))
                .Returns(response);
        }

        private static Mock<IRestResponse<T>> GetGoodResponseMock<T>() where T : class, new()
        {
            var goodResponse = new Mock<IRestResponse<T>>();
            goodResponse.SetupProperty(x => x.StatusCode, HttpStatusCode.OK)
                        .SetupProperty(x => x.ErrorException, null)
                        .SetupProperty(x => x.Data, new T())
                        .SetupProperty(x => x.Request, Mock.Of<IRestRequest>(x => x.Method == Method.GET));

            return goodResponse;
        }        

        [Fact]
        public void MakeRequestT_GivenGoodResponseWithDataExpectDataReturned()
        {
            var goodResponse = GetGoodResponseMock<Foo>();
            var testable = new Testable<RequestHandler>();
            InjectClientResponse(testable, goodResponse.Object);

            // act
            var results = testable.ClassUnderTest.MakeRequest<Foo>(new RestRequest());

            results.Should().BeSameAs(goodResponse.Object.Data);
        }

        [Fact]
        public void MakeRequestT_GivenGoodResponseNoDataExpectSharpZendeskException()
        {
            var goodResponse = GetGoodResponseMock<Foo>();
            goodResponse.SetupProperty(x => x.Data, null);

            var testable = new Testable<RequestHandler>();
            InjectClientResponse(testable, goodResponse.Object);

            const string ExpectedMessage =
                "Response data was null or not in the correct format. Expected type: SharpZendeskApi.Test.Unit.RequestHandlerTests+Foo";

            // act
            testable.ClassUnderTest.Invoking(x => x.MakeRequest<Foo>(new RestRequest()))
                .ShouldThrow<SharpZendeskException>()
                .WithMessage(ExpectedMessage);
        }
    }
}
