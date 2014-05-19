namespace SharpZendeskApi.Management
{
    using SharpZendeskApi.Models;

    public interface ITicketManager : IManager<ITicket>
    {
        IListing<ITicket> FromView(int viewId);
    }
}