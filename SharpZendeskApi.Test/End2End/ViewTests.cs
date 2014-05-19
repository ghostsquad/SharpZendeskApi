namespace SharpZendeskApi.Test.End2End
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using FluentAssertions;

    using SharpZendeskApi.Management;
    using SharpZendeskApi.Test.Common;

    using Xunit;
    using Xunit.Should;

    public class ViewTests
    {
        [Fact(Timeout = 10000)]
        public void GetAvailableViews_CanDo()
        {
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Basic);
            var viewManager = new ViewManager(client);

            // act
            var actualViews = viewManager.GetAvailableViews(true).ToList();

            actualViews.Should().NotBeNull();
            actualViews.Count.Should().BeGreaterOrEqualTo(1);
            actualViews[0].Id.ShouldNotBeNull();
        }
    }
}
