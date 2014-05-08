namespace SharpZendeskApi.Test.End2End
{
    using FluentAssertions;

    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;
    using SharpZendeskApi.Test.Common;

    using Xunit;

    public class TicketTests
    {
        [Fact]
        public void Create_UsingNonParameterlessConstructor()
        {
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Token);
            ITicket ticket = new Ticket(1, "test description");
            var ticketManager = new TicketManager(client);
            ticket = ticketManager.SubmitNew(ticket);
            ticket.Id.Should().HaveValue();
        }

        [Fact]
        public void Get_UsingKnownId()
        {
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Token);
            var ticketManager = new TicketManager(client);
            var ticket = ticketManager.Get(1);
            ticket.Id.Should().Be(1);
            ticket.CreatedAt.Should().HaveValue();
        }
    }
}
