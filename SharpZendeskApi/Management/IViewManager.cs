namespace SharpZendeskApi.Management
{
    using SharpZendeskApi.Models;

    public interface IViewManager : IManager<IView>
    {
        IListing<IView> GetAvailableViews(bool full = false);

        IListing<IView> GetActiveViews();
    }
}