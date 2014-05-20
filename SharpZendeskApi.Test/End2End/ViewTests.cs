namespace SharpZendeskApi.Test.End2End
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using FluentAssertions;

    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;
    using SharpZendeskApi.Test.Common;

    using Xunit;
    using Xunit.Should;

    public class ViewTests
    {
        private IViewManager viewManager;

        public ViewTests()
        {
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Basic);
            this.viewManager = new ViewManager(client);
        }

        [Fact(Timeout = 10000)]
        public void GetAvailableViews_Full_CanDo()
        {            
            // act
            var actualViews = this.viewManager.GetAvailableViews(true).ToList();

            actualViews.Should().NotBeNull();
            actualViews.Count.Should().BeGreaterOrEqualTo(1);
            foreach (var view in actualViews)
            {
                view.Id.Should().HaveValue();
                view.WasSubmitted.Should().BeTrue();
            }
        }

        [Fact(Timeout = 10000)]
        public void GetActiveViews_CanDo()
        {
            // act
            var actualViews = this.viewManager.GetActiveViews().ToList();

            actualViews.Should().NotBeNull();
            actualViews.Count.Should().BeGreaterOrEqualTo(1);
            foreach (var view in actualViews)
            {
                view.Id.Should().HaveValue();
                view.WasSubmitted.Should().BeTrue();
            }
        }

        [Fact(Timeout = 10000)]
        public void SubmitNew_UsingParameterizedViewConstructor()
        {
            var expectedViewTitle = "TestView" + Guid.NewGuid();

            var view = new View(
                new Condition[] { new Condition { Field = "status", Operator = "is", Value = "new" } },
                expectedViewTitle);

            // act
            var actualView = this.viewManager.SubmitNew(view);

            actualView.Id.Should().HaveValue();
            actualView.Title.Should().Be(expectedViewTitle);
            actualView.WasSubmitted.Should().BeTrue();
        }

        [Fact(Timeout = 10000)]
        public void SubmitUpdatesFor_UsingExistingView()
        {
            var views = this.viewManager.GetAvailableViews(true);
            var viewToUpdate = views.First(x => x.Title.StartsWith("TestView"));
            var expectedTitle = "TestView" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");
            viewToUpdate.Title = expectedTitle;

            // act
            this.viewManager.SubmitUpdatesFor(viewToUpdate);
            var actualView = this.viewManager.Get(viewToUpdate.Id.Value);

            actualView.Title.Should().Be(expectedTitle);            
        }

        [Fact(Timeout = 10000)]
        public void Get_UsingKnownId()
        {
            var views = this.viewManager.GetAvailableViews();
            var viewIdToGet = views.Select(x => x.Id.Value).First();

            // act
            var actualView = this.viewManager.Get(viewIdToGet);

            actualView.Id.Should().Be(viewIdToGet);
            actualView.WasSubmitted.Should().BeTrue();
        }
    }
}
