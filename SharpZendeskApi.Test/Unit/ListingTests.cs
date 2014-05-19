namespace SharpZendeskApi.Test.Unit
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using RestSharp;

    using SharpZendeskApi.Models;
    using SharpZendeskApi.Test.Common;
    using SharpZendeskApi.Test.Fakes;

    using Xunit;
    using Xunit.Should;

    public class ListingTests
    {
        #region Constants

        private const string Page2Uri = "https://account.zendesk.com/api/v2/users.json?page=2";        

        #endregion

        #region Fields

        private readonly Mock<ZendeskClientBase> clientMock = new Mock<ZendeskClientBase>(MockBehavior.Strict);

        private readonly IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());

        private readonly Mock<IRequestHandler> requestHandlerMock = new Mock<IRequestHandler>();

        private readonly List<IPage<Foo>> responses = new List<IPage<Foo>>();

        private readonly Testable<Listing<Foo, IFoo>> testable = new Testable<Listing<Foo, IFoo>>();

        private int responseIndex;

        #endregion

        #region Constructors and Destructors

        public ListingTests()
        {
            this.requestHandlerMock.Setup(x => x.MakeRequest<IPage<Foo>>(It.IsAny<IRestRequest>()))
                .Returns(() => this.responses[this.responseIndex])
                .Callback(() => this.responseIndex++);

            this.clientMock.SetupProperty(x => x.RequestHandler, this.requestHandlerMock.Object);

            // Listing ctor is internal, so we must create it here.
            this.testable.Fixture.Inject(
                new Listing<Foo, IFoo>(
                    this.clientMock.Object,
                    this.testable.Fixture.Create<IRestRequest>()));
        }

        #endregion

        #region Public Methods and Operators

        [Fact]
        public void EndOfPage_GivenMultipleItemsInMultiPageResponse_WhenBeforeLastItemEndOfPage_ShouldBeFalse()
        {
            this.AddPageOfItems(1, Page2Uri);
            this.AddPageOfItems(2);

            // act
            var enumerator = this.testable.ClassUnderTest.GetEnumerator();

            // assert
            enumerator.MoveNext();
            this.testable.ClassUnderTest.AtEndOfPage.Should().BeTrue("because we are on page 1 ticket 1 of 1.");
            enumerator.MoveNext();
            this.testable.ClassUnderTest.AtEndOfPage.Should().BeFalse("because we are on page 2 ticket 1 of 2.");
            enumerator.MoveNext();
            this.testable.ClassUnderTest.AtEndOfPage.Should().BeTrue("because we are on page 2 ticket 2 of 2.");
        }

        [Fact]
        public void EndOfPage_GivenMultipleItemsInResponse_WhenBeforeLastItem_ShouldBeFalse()
        {
            this.AddPageOfItems(2);

            // act
            var enumerator = this.testable.ClassUnderTest.GetEnumerator();

            // assert
            this.testable.ClassUnderTest.AtEndOfPage.Should().BeFalse("because we have not yet begun enumeration.");
            enumerator.MoveNext();
            this.testable.ClassUnderTest.AtEndOfPage.Should().BeFalse("because we are on ticket 1 of 2.");
            enumerator.MoveNext();
            this.testable.ClassUnderTest.AtEndOfPage.Should().BeTrue("because we are on ticket 2 of 2.");
        }

        [Fact]
        public void MoveNext_GivenMultipageMultiItem_WhenAtVeryEnd_ExpectFalse()
        {
            this.AddPageOfItems(2, Page2Uri);
            this.AddPageOfItems(2);

            // act
            var enumerator = this.testable.ClassUnderTest.GetEnumerator();

            // move to page 1 item 1
            enumerator.MoveNext().Should().BeTrue();

            // move to page 1 item 2
            enumerator.MoveNext().Should().BeTrue();

            // move to page 2 item 1
            enumerator.MoveNext().Should().BeTrue();

            // move to page 2 item 2
            enumerator.MoveNext().Should().BeTrue();

            // verify end
            enumerator.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void MoveNext_GivenMultiplePageResponse_CanPage()
        {
            this.AddPageOfItems(2, Page2Uri);
            this.AddPageOfItems(2);

            var expectedTickets = this.responses.SelectMany(p => p.Collection.Select(t => t)).ToArray();

            // act
            var enumerator = this.testable.ClassUnderTest.GetEnumerator();

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
        public void MoveNext_GivenResponseWithNoItems_ShouldReturnFalse()
        {
            this.AddPageOfItems(0);

            // act
            var enumerator = this.testable.ClassUnderTest.GetEnumerator();

            enumerator.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void MoveNext_GivenResponseWithOneItem_ExpectTrueThenFalse()
        {
            this.AddPageOfItems(1);

            // act            
            var enumerator = this.testable.ClassUnderTest.GetEnumerator();

            enumerator.MoveNext().Should().BeTrue();
            enumerator.MoveNext().Should().BeFalse();
        }        

        [Fact]
        public void PageNumbers_GivenTwoPageResponseWhenOnFirstPage_ExpectPreviousPageNullCurrentPage1NextPage2()
        {
            this.AddPageOfItems(1, Page2Uri);

            // act
            var enumerator = this.testable.ClassUnderTest.GetEnumerator();

            enumerator.MoveNext();

            // assert
            const string AssertMessage = "because we are currently on page 1 of 2.";

            this.testable.ClassUnderTest.PreviousPage.Should().NotHaveValue(AssertMessage);
            this.testable.ClassUnderTest.CurrentPage.Should().Be(1, AssertMessage);
            this.testable.ClassUnderTest.NextPage.Should().Be(2, AssertMessage);
        }

        [Fact]
        public void PageNumbers_GivenTwoPageResponseWhenOnSecondPage_ExpectPreviousPage1CurrentPage2NextPageNull()
        {
            this.AddPageOfItems(1, Page2Uri);
            this.AddPageOfItems(1);

            // act
            var enumerator = this.testable.ClassUnderTest.GetEnumerator();

            enumerator.MoveNext();
            enumerator.MoveNext();

            const string AssertMessage = "because we are currently on page 2 of 2.";

            this.testable.ClassUnderTest.PreviousPage.Should().Be(1, AssertMessage);
            this.testable.ClassUnderTest.CurrentPage.Should().Be(2, AssertMessage);
            this.testable.ClassUnderTest.NextPage.Should().NotHaveValue(AssertMessage);
        }

        [Fact]
        public void PageNumbers_GivenTwoPageResponse_WhenBeforeFirstMoveNext_ExpectAllPagesNull()
        {
            this.AddPageOfItems(1, Page2Uri);

            // assert
            const string AssertMessage = "because we have not yet started enumerating items or pages.";

            this.testable.ClassUnderTest.PreviousPage.Should().NotHaveValue(AssertMessage);
            this.testable.ClassUnderTest.CurrentPage.Should().NotHaveValue(AssertMessage);
            this.testable.ClassUnderTest.NextPage.Should().NotHaveValue(AssertMessage);
        }

        #endregion

        #region Methods

        private void AddPageOfItems(int numberOfItems, string nextpage = null)
        {
            var itemsList = new List<Foo>();
            if (numberOfItems > 0)
            {
                itemsList = this.fixture.CreateMany<Foo>(numberOfItems).ToList();
            }

            this.responses.Add(
                new Mock<IPage<Foo>>(MockBehavior.Strict)
                    .SetupProperty(x => x.NextPage, nextpage)
                    .SetupProperty(x => x.Count, numberOfItems)
                    .SetupProperty(x => x.Collection, itemsList)
                    .Object);
        }

        #endregion
    }
}