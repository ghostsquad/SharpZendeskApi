namespace SharpZendeskApi.Management
{
    using System.Collections.Generic;

    using SharpZendeskApi.Models;

    public interface ITicketManager
    {
        IListing<ITicket> FromView(int viewId);

        IListing<ITicket> FromView(IView view);

        IListing<ITicket> GetMany(IEnumerable<int> ids);

        ITicket Get(int id);

        void SubmitUpdatesFor(ITicket obj);

        ITicket SubmitNew(ITicket obj);

        ZendeskClientBase Client { get; set; }

        bool TryGet(int id, out ITicket value);
    }
}