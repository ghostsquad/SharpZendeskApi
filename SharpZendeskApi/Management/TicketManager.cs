namespace SharpZendeskApi.Management
{
    using System;
    using System.Collections.Generic;

    using RestSharp;

    using SharpZendeskApi.Models;

    public sealed class TicketManager : ManagerBase<Ticket, ITicket>, ITicketManager
    {
        private const string SingleTicketEndpoint = "tickets/{0}.json";

        private const string ManyTicketsEndpoint = "tickets/show_many.json?ids={0}";

        private const string TicketsFromViewEndpoint = "views/{0}/tickets.json";

        private const string SubmitTicketEndpoint = "tickets.json";

        public TicketManager(ZendeskClientBase client) : base(client)
        {                       
        }        

        public IListing<ITicket> FromView(int viewId)
        {
            // http://developer.zendesk.com/documentation/rest_api/views.html#getting-tickets-from-a-view
            var url = string.Format(TicketsFromViewEndpoint, viewId);            

            var request = new RestRequest(url, Method.GET) { RootElement = this.PluralizedModelName };

            return this.Client.GetListing<Ticket, ITicket>(request);
        }

        public IListing<ITicket> FromView(IView view)
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

            var url = string.Format(SingleTicketEndpoint, obj.Id.Value);
            this.SubmitUpdatesFor(url, obj);
        }

        public override ITicket SubmitNew(ITicket obj)
        {
            return this.SubmitNew(SubmitTicketEndpoint, obj);
        }
    }
}
