namespace SharpZendeskApi.Test.End2End
{
    using System.Linq;

    using FluentAssertions;

    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;
    using SharpZendeskApi.Test.Common;

    using Xunit;

    public class TicketTests
    {
        [Fact(Timeout = 10000)]
        public void Create_UsingNonParameterlessConstructor()
        {
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Basic);
            ITicket ticket = new Ticket(1, "test description");
            var ticketManager = new TicketManager(client);
            ticket = ticketManager.SubmitNew(ticket);
            ticket.Id.Should().HaveValue();
        }

        [Fact(Timeout = 10000)]
        public void Get_UsingKnownId()
        {
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Basic);
            var ticketManager = new TicketManager(client);
            var ticket = ticketManager.Get(1);
            ticket.Id.Should().Be(1);
            ticket.CreatedAt.Should().HaveValue();
        }

        [Fact(Timeout = 30000)]
        public void GetMany_UsingKnownIds()
        {
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Basic);
            var ticketManager = new TicketManager(client);
            var ids = new[] { 5, 6, 7 };

            var tickets = ticketManager.GetMany(ids).ToList();

            tickets.Should().HaveCount(3);
            tickets.Select(x => x.Id).Should().ContainInOrder(ids);
        }
    }
}
