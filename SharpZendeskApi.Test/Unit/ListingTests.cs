namespace SharpZendeskApi.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using RestSharp;

    using SharpZendeskApi.Exceptions;
    using SharpZendeskApi.Models;

    using Xunit;
    using Xunit.Should;

    public class ListingTests
    {
        #region Constants

        private const string Page2Uri = "https://account.zendesk.com/api/v2/users.json?page=2";

        private const string TestExceptionMessage = "test exception";

        #endregion

        #region Fields

        private Mock<IRestClient> clientMock = new Mock<IRestClient>(MockBehavior.Strict);

        private IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());

        private int responseIndex = 0;

        private List<IRestResponse<IPage<Ticket>>> responses = new List<IRestResponse<IPage<Ticket>>>();

        #endregion

        #region Constructors and Destructors

        public ListingTests()
        {
            this.clientMock.Setup(x => x.Execute<IPage<Ticket>>(It.IsAny<IRestRequest>()))
                .Returns(() => this.responses[this.responseIndex])
                .Callback(() => this.responseIndex++);
        }

        #endregion

        #region Public Methods and Operators

        [Fact]
        public void EndOfPage_GivenMultipleItemsInMultiPageResponse_WhenBeforeLastItemEndOfPage_ShouldBeFalse()
        {
            // arrange
            var page1 = this.GetTicketPage(1, Page2Uri);
            this.AddPageResponse(page1);
            var page2 = this.GetTicketPage(2);
            this.AddPageResponse(page2);

            // act
            var actualTickets = new Listing<Ticket, ITicket>(this.clientMock.Object, Mock.Of<IRestRequest>());
            var enumerator = actualTickets.GetEnumerator();

            // assert
            // move to page 1 item 1
            enumerator.MoveNext();
            actualTickets.AtEndOfPage.Should().BeTrue("because we are on page 1 ticket 1 of 1.");

            // move to page 2 item 1
            enumerator.MoveNext();
            actualTickets.AtEndOfPage.Should().BeFalse("because we are on page 2 ticket 1 of 2.");

            // move to page 2 item 2 (final item)
            enumerator.MoveNext();
            actualTickets.AtEndOfPage.Should().BeTrue("because we are on page 2 ticket 2 of 2.");
        }

        [Fact]
        public void EndOfPage_GivenMultipleItemsInResponse_WhenBeforeLastItem_ShouldBeFalse()
        {
            // arrange
            var page1 = this.GetTicketPage(2);
            this.AddPageResponse(page1);

            // act
            var actualTickets = new Listing<Ticket, ITicket>(this.clientMock.Object, Mock.Of<IRestRequest>());
            var enumerator = actualTickets.GetEnumerator();

            // assert
            actualTickets.AtEndOfPage.Should().BeFalse("because we have not yet begun enumeration.");
            enumerator.MoveNext();
            actualTickets.AtEndOfPage.Should().BeFalse("because we are on ticket 1 of 2.");
            enumerator.MoveNext();
            actualTickets.AtEndOfPage.Should().BeTrue("because we are on ticket 2 of 2.");
        }

        [Fact]
        public void MoveNext_GivenMultiplePageResponse_CanPage()
        {
            // arrange
            var page1 = this.GetTicketPage(2, Page2Uri);
            this.AddPageResponse(page1);
            var page2 = this.GetTicketPage(2);
            this.AddPageResponse(page2);

            var expectedTickets = this.responses.SelectMany(p => p.Data.Collection.Select(t => t)).ToArray();

            // act
            var actualTickets = new Listing<Ticket, ITicket>(this.clientMock.Object, Mock.Of<IRestRequest>());
            var enumerator = actualTickets.GetEnumerator();

            var ticketsMovedThrough = 0;
            while (enumerator.MoveNext())
            {
                var currentTicket = enumerator.Current;
                currentTicket.ShouldBeSameAs(expectedTickets[ticketsMovedThrough]);
                ticketsMovedThrough++;
            }

            ticketsMovedThrough.Should().Be(4, "because there are 4 total tickets.");
        }

        [Fact]
        public void MoveNext_GivenNonOkStatus_ExpectZendeskRequestException()
        {
            // arrange
            var response =
                new Mock<IRestResponse<IPage<Ticket>>>(MockBehavior.Strict).SetupProperty(
                    x => x.StatusCode, 
                    HttpStatusCode.BadRequest)
                    .SetupProperty(x => x.ErrorException, null)
                    .SetupProperty(x => x.Request, new RestRequest())
                    .SetupProperty(x => x.Content, TestExceptionMessage);

            this.responses.Add(response.Object);

            // act
            var actualTickets = new Listing<Ticket, ITicket>(this.clientMock.Object, Mock.Of<IRestRequest>());
            var enumerator = actualTickets.GetEnumerator();

            // assert
            enumerator.Invoking(x => x.MoveNext())
                .ShouldThrow<ZendeskRequestException>()
                .WithMessage(TestExceptionMessage);
        }

        [Fact]
        public void MoveNext_GivenResponseWithNoItems_ShouldReturnFalse()
        {
            // arrange
            var page1 = this.GetTicketPage(0);
            this.AddPageResponse(page1);

            // act
            var actualTickets = new Listing<Ticket, ITicket>(this.clientMock.Object, Mock.Of<IRestRequest>());
            var enumerator = actualTickets.GetEnumerator();

            enumerator.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void MoveNext_GivenResponseWithOneItem_ExpectTrueThenFalse()
        {
            // arrange
            var page1 = this.GetTicketPage(1);
            this.AddPageResponse(page1);

            // act
            var actualTickets = new Listing<Ticket, ITicket>(this.clientMock.Object, Mock.Of<IRestRequest>());
            var enumerator = actualTickets.GetEnumerator();

            enumerator.MoveNext().Should().BeTrue();
            enumerator.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void MoveNext_GivenRestSharpException_ExpectSameException()
        {
            // arrange            
            var response =
                new Mock<IRestResponse<IPage<Ticket>>>(MockBehavior.Strict).SetupProperty(
                    x => x.StatusCode, 
                    HttpStatusCode.OK)
                    .SetupProperty(x => x.Content, "test content")
                    .SetupProperty(x => x.Request, Mock.Of<IRestRequest>())
                    .SetupProperty(x => x.ErrorException, new ApplicationException(TestExceptionMessage));

            this.responses.Add(response.Object);

            // act
            var actualTickets = new Listing<Ticket, ITicket>(this.clientMock.Object, Mock.Of<IRestRequest>());
            var enumerator = actualTickets.GetEnumerator();

            // assert
            enumerator.Invoking(x => x.MoveNext()).ShouldThrow<ApplicationException>().WithMessage(TestExceptionMessage);
        }

        [Fact]
        public void MoveNext_GivenUnauthorizedRequest_ExpectUnauthorizedException()
        {
            // arrange
            var response =
                new Mock<IRestResponse<IPage<Ticket>>>(MockBehavior.Strict).SetupProperty(
                    x => x.StatusCode, 
                    HttpStatusCode.Unauthorized).SetupProperty(x => x.Content, "unauthorized");

            this.responses.Add(response.Object);

            // act
            var actualTickets = new Listing<Ticket, ITicket>(this.clientMock.Object, Mock.Of<IRestRequest>());
            var enumerator = actualTickets.GetEnumerator();

            // assert
            enumerator.Invoking(x => x.MoveNext()).ShouldThrow<UnauthorizedAccessException>();
        }

        [Fact(Timeout = 200)]
        public void MoveNext_GivenMultipageMultiItem_WhenAtVeryEnd_ExpectFalse()
        {
            // arrange
            var page1 = this.GetTicketPage(2, Page2Uri);
            this.AddPageResponse(page1);
            var page2 = this.GetTicketPage(2);
            this.AddPageResponse(page2);

            // act
            var actualTickets = new Listing<Ticket, ITicket>(this.clientMock.Object, Mock.Of<IRestRequest>());
            var enumerator = actualTickets.GetEnumerator();

            // move to page 1 item 1
            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be(page1.Collection[0]);
            // move to page 1 item 2
            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be(page1.Collection[1]);
            // move to page 2 item 1
            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be(page2.Collection[0]);
            // move to page 2 item 2
            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be(page2.Collection[1]);
            // verify end
            enumerator.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void PageNumbers_GivenTwoPageResponseWhenOnFirstPage_ExpectPreviousPageNullCurrentPage1NextPage2()
        {
            // arrange
            var page1 = this.GetTicketPage(1, Page2Uri);
            this.AddPageResponse(page1);

            // act
            var actualTickets = new Listing<Ticket, ITicket>(this.clientMock.Object, Mock.Of<IRestRequest>());
            var enumerator = actualTickets.GetEnumerator();

            enumerator.MoveNext();

            // assert
            const string AssertMessage = "because we are currently on page 1 of 2.";

            actualTickets.PreviousPage.Should().NotHaveValue(AssertMessage);
            actualTickets.CurrentPage.Should().Be(1, AssertMessage);
            actualTickets.NextPage.Should().Be(2, AssertMessage);
        }

        [Fact]
        public void PageNumbers_GivenTwoPageResponseWhenOnSecondPage_ExpectPreviousPage1CurrentPage2NextPageNull()
        {
            // arrange
            var page1 = this.GetTicketPage(1, Page2Uri);
            var page2 = this.GetTicketPage(1);
            this.AddPageResponse(page1);
            this.AddPageResponse(page2);

            // act
            var actualTickets = new Listing<Ticket, ITicket>(this.clientMock.Object, Mock.Of<IRestRequest>());
            var enumerator = actualTickets.GetEnumerator();

            enumerator.MoveNext();
            enumerator.MoveNext();

            const string AssertMessage = "because we are currently on page 2 of 2.";

            actualTickets.PreviousPage.Should().Be(1, AssertMessage);
            actualTickets.CurrentPage.Should().Be(2, AssertMessage);
            actualTickets.NextPage.Should().NotHaveValue(AssertMessage);
        }

        [Fact]
        public void PageNumbers_GivenTwoPageResponse_WhenBeforeFirstMoveNext_ExpectAllPagesNull()
        {
            // arrange
            var page1 = this.GetTicketPage(1, Page2Uri);
            this.AddPageResponse(page1);

            // act
            var actualTickets = new Listing<Ticket, ITicket>(this.clientMock.Object, Mock.Of<IRestRequest>());

            // assert
            const string AssertMessage = "because we have not yet started enumerating items or pages.";

            actualTickets.PreviousPage.Should().NotHaveValue(AssertMessage);
            actualTickets.CurrentPage.Should().NotHaveValue(AssertMessage);
            actualTickets.NextPage.Should().NotHaveValue(AssertMessage);
        }

        #endregion

        #region Methods

        private void AddPageResponse(IPage<Ticket> page)
        {
            var response = this.GetTicketPageResponse(page);
            this.responses.Add(response);
        }

        private IPage<Ticket> GetTicketPage(int numberOfTickets, string nextpage = null)
        {
            var ticketsList = new List<Ticket>();
            if (numberOfTickets > 0)
            {
                ticketsList = this.fixture.Build<Ticket>().CreateMany(numberOfTickets).ToList();
            }

            return
                new Mock<IPage<Ticket>>(MockBehavior.Strict).SetupProperty(x => x.NextPage, nextpage)
                    .SetupProperty(x => x.Count, numberOfTickets)
                    .SetupProperty(x => x.Collection, ticketsList)
                    .Object;
        }

        private IRestResponse<IPage<Ticket>> GetTicketPageResponse(IPage<Ticket> page)
        {
            return
                new Mock<IRestResponse<IPage<Ticket>>>(MockBehavior.Strict).SetupProperty(
                    x => x.StatusCode, 
                    HttpStatusCode.OK)
                    .SetupProperty(x => x.Data, page)
                    .SetupProperty(x => x.ErrorException, null)
                    .SetupProperty(x => x.Request, new RestRequest(Method.GET))
                    .Object;
        }

        #endregion
    }
}