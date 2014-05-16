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
            this.SetupPageResponse(x => actualRequest = x, 1);

            var expectedResourceParameter = "views/" + ExpectedId + "/tickets.json";

            // act
            this.testable.ClassUnderTest.FromView(1).ToList();

            // assert
            actualRequest.Should().NotBeNull();
            actualRequest.Resource.Should().Be(expectedResourceParameter);
            actualRequest.Method.Should().Be(Method.GET);
        }

        [Fact]
        public void FromViewOverload_AssertCallsFromViewWithViewId()
        {
            var managerMock = this.testable.InjectMock<TicketManager>(this.ClientMock.Object);
            managerMock.CallBase = true;

            int? actualId = null;
            managerMock.Setup(x => x.FromView(It.IsAny<int>()))
                .Callback<int>(x => actualId = x);

            var viewObject = Mock.Of<View>(x => x.Id == TicketManagerTests.ExpectedId);

            // act
            testable.ClassUnderTest.FromView(viewObject);

            // assert
            actualId.ShouldNotBeNull();
            actualId.Should().Be(TicketManagerTests.ExpectedId);
        }

        [Fact]
        public override void SubmitNew_AssertRequestConstruction()
        {
            // arrange
            var ticket = new Ticket(1, "test");

            IRestRequest actualRequest = null;
            this.SetupSingleResponse(x => actualRequest = x, ticket);

            const string JsonBodyInput = "{\"test\"=1}";
            const string ExpectedJsonBody = "application/json=" + JsonBodyInput;

            TrackableZendeskThingBase actualSerializedObject = null;
            var serializerMock = testable.InjectMock<IZendeskSerializer>();
            serializerMock.Setup(x => x.Serialize(It.IsAny<TrackableZendeskThingBase>()))
                    .Callback<TrackableZendeskThingBase>(x => actualSerializedObject = x)
                    .Returns(ExpectedJsonBody)
                    .Verifiable();

            const string ExpectedResource = "tickets.json";

            // act
            this.testable.ClassUnderTest.SubmitNew(ticket);

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
            this.testable.ClassUnderTest.Invoking(x => x.FromView(null)).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void FromView_GivenViewWithNullId_ExpectArgumentException()
        {
            // arrange
            var view = new View();

            // act & assert
            this.testable.ClassUnderTest.Invoking(x => x.FromView(view))
                .ShouldThrow<ArgumentException>()
                .WithMessage("View Id is null!");
        }

        #endregion
    }
}