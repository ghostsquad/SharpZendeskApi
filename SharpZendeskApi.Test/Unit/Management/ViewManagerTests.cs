namespace SharpZendeskApi.Test.Unit.Management
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Moq;

    using Ploeh.AutoFixture;

    using RestSharp;

    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    using Xunit;
    using Xunit.Should;

    public class ViewManagerTests : ManagerTestBase<View, IView, ViewManager>
    {
        [Fact(Skip = "get many not implemented")]
        public override void GetMany_WithValidRequestAndExistingObject_ShouldReturnWithObject()
        {
        }

        [Fact]
        public void GetAvailableViews_AssertRequest()
        {            
            // arrange
            IRestRequest actualRequest = null;
            this.ClientMock.Setup(x => x.GetListing<View, IView>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(x => actualRequest = x);

            const string ExpectedResource = "views.json";

            // act
            this.testable.ClassUnderTest.GetAvailableViews(true);

            // assert
            actualRequest.Should().NotBeNull();
            actualRequest.Resource.Should().Be(ExpectedResource);
            actualRequest.Method.Should().Be(Method.GET);
        }

        [Fact]
        public void GetAvailableViews_ReturnsExpectedItems()
        {
            var expectedItems = this.testable.Fixture.CreateMany<IView>();
            var listingMock = Mock.Of<IListing<IView>>(x => x.GetEnumerator() == expectedItems.GetEnumerator());

            this.ClientMock.Setup(x => x.GetListing<View, IView>(It.IsAny<IRestRequest>()))
                .Returns(listingMock);

            // act
            var actualItems = this.testable.ClassUnderTest.GetAvailableViews(true).ToList();

            actualItems.ShouldBeEquivalentTo(expectedItems.ToList());
        }

        [Fact]
        public void GetActiveViews_AssertRequest()
        {
            IRestRequest actualRequest = null;
            this.ClientMock.Setup(x => x.GetListing<View, IView>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(x => actualRequest = x);

            const string ExpectedResource = "views/active.json";

            // act
            this.testable.ClassUnderTest.GetActiveViews();

            actualRequest.Should().NotBeNull();
            actualRequest.Resource.Should().Be(ExpectedResource);
            actualRequest.Method.Should().Be(Method.GET);
        }

        [Fact]
        public void GetActiveView_ReturnsExpectedItems()
        {
            var expectedItems = this.testable.Fixture.CreateMany<IView>();
            var listingMock = Mock.Of<IListing<IView>>(x => x.GetEnumerator() == expectedItems.GetEnumerator());

            this.ClientMock.Setup(x => x.GetListing<View, IView>(It.IsAny<IRestRequest>()))
                .Returns(listingMock);

            // act
            var actualItems = this.testable.ClassUnderTest.GetActiveViews().ToList();

            actualItems.ShouldBeEquivalentTo(expectedItems.ToList());
        }
    }
}
