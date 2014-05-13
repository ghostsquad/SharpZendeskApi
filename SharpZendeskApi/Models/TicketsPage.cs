namespace SharpZendeskApi.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    internal class TicketsPage : IPage<ITicket>
    {
        public IList<ITicket> Collection
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

        public IList<ITicket> Tickets { get; set; } 

        public string NextPage { get; set; }

        public int Count { get; set; }        

        public string PreviousPage { get; set; }
    }
}
