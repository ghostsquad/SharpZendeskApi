namespace SharpZendeskApi.Test.Unit.Models
{
    using System.Linq;

    using FluentAssertions;

    using SharpZendeskApi.Models;
    using SharpZendeskApi.Models.Attributes;
    using SharpZendeskApi.Test.Common.JsonObjects;

    using Xunit;

    public class TicketTests : ModelTestBase<TicketJson, Ticket, ITicket>
    {
        [Fact]
        public override void CanCreateWithFilledMandatoryPropertiesUsingConstructor()
        {
            // arrange
            var properties = this.ModelFixture.Properties.Where(x => x.GetCustomAttributes(typeof(MandatoryAttribute), true).Any());

            // act
            var ticket = new Ticket(1, "test");

            // assert
            foreach (var property in properties)
            {
                property.GetValue(ticket).Should().NotBeNull();
            }
        }
    }
}
