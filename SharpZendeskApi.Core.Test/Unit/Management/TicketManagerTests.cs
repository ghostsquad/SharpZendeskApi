namespace SharpZendeskApi.Core.Test.Unit.Management
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using Moq;

    using RestSharp;

    using SharpZendeskApi.Core.Management;
    using SharpZendeskApi.Core.Models;

    using Xunit;
    using Xunit.Should;

    public class TicketManagerTests : ManagerTestBase<Ticket, ITicket, TicketManager>
    {
        #region Public Methods and Operators

        [Fact]
        public void CanGetFromViewWhenGivenId()
        {
            // arrange
            var pageResponse = this.GetPageResponse(2);

            IRestRequest actualRequest = null;
            this.ClientMock.Setup(x => x.Execute<IPage<Ticket>>(It.IsAny<IRestRequest>()))
                .Returns(pageResponse)
                .Callback<IRestRequest>(r => actualRequest = r);

            // http://developer.zendesk.com/documentation/rest_api/views.html#getting-tickets-from-a-view
            const string ExpectedResourceParameter = "views/1/tickets.json";

            // act
            var actualTickets = this.Manager.As<TicketManager>().FromView(1).Take(2).ToList();

            // assert
            actualRequest.Should().NotBeNull();

            actualRequest.Resource.Should().Be(ExpectedResourceParameter);
            actualRequest.Method.Should().Be(Method.GET);

            actualTickets.Should().NotBeEmpty().And.HaveCount(2).And.ContainInOrder(pageResponse.Data.Collection);
        }

        [Fact]
        public void CanGetFromViewWhenGivenViewObject()
        {
            // arrange
            var pageResponse = this.GetPageResponse(2);

            IRestRequest actualRequest = null;
            this.ClientMock.Setup(x => x.Execute<IPage<Ticket>>(It.IsAny<IRestRequest>()))
                .Returns(pageResponse)
                .Callback<IRestRequest>(r => actualRequest = r);

            var viewObject = Mock.Of<View>(x => x.Id == 1);

            // http://developer.zendesk.com/documentation/rest_api/views.html#getting-tickets-from-a-view
            const string ExpectedResourceParameter = "views/1/tickets.json";

            // act
            var actualTickets = this.Manager.As<TicketManager>().FromView(viewObject).Take(2).ToList();

            // assert
            actualRequest.Should().NotBeNull();

            actualRequest.Resource.Should().Be(ExpectedResourceParameter);
            actualRequest.Method.Should().Be(Method.GET);

            actualTickets.Should().NotBeEmpty().And.HaveCount(2).And.ContainInOrder(pageResponse.Data.Collection);
        }

        [Fact]
        public override void SubmitNew_UsingParameterizedConstructor_ExpectSuccess()
        {
            // arrange
            var ticket = new Ticket(1, "test");
            this.SetupOkResponse();

            IRestRequest actualRequest = null;
            this.SetupVerifiableClientExecuteGetActualRequest(x => actualRequest = x);

            const string JsonBodyInput = "{\"test\"=1}";
            const string ExpectedJsonBody = "application/json=" + JsonBodyInput;

            TrackableZendeskThingBase actualSerializedObject = null;
            var fakeSerializerMock = new Mock<IZendeskSerializer>();
            fakeSerializerMock.Setup(x => x.Serialize(It.IsAny<TrackableZendeskThingBase>()))
                    .Callback<TrackableZendeskThingBase>(x => actualSerializedObject = x)
                    .Returns(ExpectedJsonBody)
                    .Verifiable();

            this.ClientMock.Object.Container.Register(fakeSerializerMock.Object, SerializationScenario.Create.ToString());

            const string ExpectedResource = "tickets.json";

            // act
            this.Manager.SubmitNew(ticket);

            // assert
            actualRequest.Should().NotBeNull();
            actualRequest.Method.Should().Be(Method.POST);
            actualRequest.Resource.Should().Be(ExpectedResource);
            actualRequest.Parameters.First(x => x.Type == ParameterType.RequestBody).Value.Should().Be(ExpectedJsonBody);
            actualSerializedObject.ShouldBeSameAs(ticket);
        }

        [Fact]
        public void FromView_GivenNull_ExpectArgumentNullException()
        {
            // act & assert
            this.Manager.As<TicketManager>().Invoking(x => x.FromView(null)).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void FromView_GivenViewWithNullId_ExpectArgumentException()
        {
            // arrange
            var view = new View();

            // act & assert
            this.Manager.As<TicketManager>()
                .Invoking(x => x.FromView(view))
                .ShouldThrow<ArgumentException>()
                .WithMessage("View Id is null!");
        }

        #endregion
    }
}