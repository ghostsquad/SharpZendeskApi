namespace SharpZendeskApi.Core.Management
{
    using System;
    using System.Collections.Generic;

    using RestSharp;

    using SharpZendeskApi.Core.Models;

    public class TicketManager : ManagerBase<Ticket, ITicket>
    {
        private const string SingleTicketEndpoint = "tickets/{0}.json";

        private const string ManyTicketsEndpoint = "tickets/show_many.json?ids={0}";

        private const string TicketsFromViewEndpoint = "views/{0}/tickets.json";

        private const string SubmitTicketEndpoint = "tickets.json";

        public TicketManager(IZendeskClient client) : base(client)
        {                       
        }        

        public IListing<ITicket> FromView(int viewId)
        {
            // http://developer.zendesk.com/documentation/rest_api/views.html#getting-tickets-from-a-view
            var request = new RestRequest(
                string.Format(TicketsFromViewEndpoint, viewId),
                Method.GET) { RootElement = typeof(Ticket).GetTypeNameAsCPlusPlusStyle().Pluralize() };

            return new Listing<Ticket, ITicket>(this.Client, request);
        }

        public IListing<ITicket> FromView(View view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            if (view.Id.HasValue)
            {
                return this.FromView(view.Id.Value);
            }

            throw new ArgumentException("View Id is null!");
        }

        public override IListing<ITicket> GetMany(IEnumerable<int> ids)
        {
            return this.GetMany(string.Format(ManyTicketsEndpoint, string.Join(",", ids)));
        }

        public override ITicket Get(int id)
        {
            var url = string.Format(SingleTicketEndpoint, id);
            return this.Get(url, id);
        }

        public override void SubmitUpdatesFor(ITicket obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            var objAsTicket = obj as Ticket;
            // ReSharper disable once PossibleNullReferenceException
            // objAsTicket will always implement ITicket due to generic constraints on base class
            var url = string.Format(SingleTicketEndpoint, objAsTicket.Id);
            this.SubmitUpdatesFor(url, objAsTicket);
        }

        public override ITicket SubmitNew(ITicket obj)
        {
            return this.SubmitNew(SubmitTicketEndpoint, (Ticket)obj);
        }
    }
}
