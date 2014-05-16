using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpZendeskApi.Test.Unit.Management
{
    using FluentAssertions;

    using Moq;

    using RestSharp;

    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    using Xunit;

    public class ViewManagerTests : ManagerTestBase<View, IView, ViewManager>
    {
        [Fact(Skip = "not implemented")]
        public override void GetMany_WithValidRequestAndExistingObject_ShouldReturnWithObject()
        {
            throw new NotImplementedException();
        }

        [Fact(Skip = "not implemented")]
        public override void SubmitNew_AssertRequestConstruction()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GetAvailableViews_AssertRequestConstruction()
        {            
            // arrange
            IRestRequest actualRequest = null;
            var page = this.SetupPageResponse(x => actualRequest = x, 2);

            this.RequestHandlerMock.Setup(x => x.MakeRequest<IPage<View>>(It.IsAny<IRestRequest>()))
                .Returns(page)
                .Callback<IRestRequest>(r => actualRequest = r);

            var expectedResourceParameter = string.Format(
                "{0}.json",
                typeof(View).GetTypeNameAsCPlusPlusStyle().Pluralize());

            var manager = this.testable.ClassUnderTest;

            // act
            manager.GetAvailableViews(true).Take(2).ToList();

            // assert
            actualRequest.Should().NotBeNull();
            actualRequest.Resource.Should().Be(expectedResourceParameter);
            actualRequest.Method.Should().Be(Method.GET);
        }
    }
}
