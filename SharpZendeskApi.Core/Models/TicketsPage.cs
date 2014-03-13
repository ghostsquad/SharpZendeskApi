namespace SharpZendeskApi.Core.Models
{
    using System;
    using System.Collections.Generic;

    internal class TicketsPage : IPage<Ticket>
    {
        public List<Ticket> Collection
        {
            get
            {
                return this.Tickets;
            }

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
