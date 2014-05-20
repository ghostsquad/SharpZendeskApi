namespace SharpZendeskApi.Test.End2End
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;
    using SharpZendeskApi.Test.Common;

    using Xunit;

    public class TicketTests
    {
        private readonly ITicketManager ticketManager;

        public TicketTests()
        {
            var client = TestHelpers.GetClient(ZendeskAuthenticationMethod.Basic);
            this.ticketManager = new TicketManager(client);
        }

        [Fact(Timeout = 10000)]
        public void SubmitNew_UsingNonParameterlessConstructor()
        {
            ITicket ticket = new Ticket(1, "test description");

            // act
            ticket = this.ticketManager.SubmitNew(ticket);

            ticket.Id.Should().HaveValue();
        }

        [Fact(Timeout = 10000)]
        public void Get_UsingKnownId()
        {
            // act
            var ticket = this.ticketManager.Get(1);

            ticket.Id.Should().Be(1);
            ticket.CreatedAt.Should().HaveValue();
            ticket.WasSubmitted.Should().BeTrue();
        }

        [Fact(Timeout = 30000)]
        public void GetMany_UsingKnownIds()
        {
            var ids = new[] { 5, 6, 7 };

            // act
            var tickets = this.ticketManager.GetMany(ids).ToList();

            tickets.Should().HaveCount(3);
            tickets.Select(x => x.Id).Should().ContainInOrder(ids);
            tickets.All(x => x.WasSubmitted).Should().BeTrue();
        }

        [Fact(Timeout = 30000)]
        public void SubmitUpdatesFor_CanUpdateSubject()
        {
            var ticket = this.ticketManager.Get(1);
            var expectedSubject = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");
            ticket.Subject = expectedSubject;

            // act
            this.ticketManager.SubmitUpdatesFor(ticket);
            var updatedTicket = this.ticketManager.Get(1);

            updatedTicket.Subject.Should().Be(expectedSubject);
        }
    }
}
