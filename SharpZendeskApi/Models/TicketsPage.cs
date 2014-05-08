namespace SharpZendeskApi.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    internal class TicketsPage : IPage<Ticket>
    {
        public List<Ticket> Collection
        {
            get
            {
                return this.Tickets;
            }

            // this set is used exclusively by RestSharp when populating this class
            [ExcludeFromCodeCoverage]
            set
            {
                this.Tickets = value;
            }
        }

        public List<Ticket> Tickets { get; set; } 

        public string NextPage { get; set; }

        public int Count { get; set; }        

        public string PreviousPage { get; set; }
    }
}
