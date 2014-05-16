namespace SharpZendeskApi.Management
{
    using System.Collections.Generic;

    using SharpZendeskApi.Models;

    public interface IViewManager
    {
        IView Get(int id);

        void SubmitUpdatesFor(IView obj);

        IListing<IView> GetAvailableViews(bool full = false);

        IListing<IView> GetActiveViews();

        IListing<IView> GetMany(IEnumerable<int> ids);

        IView SubmitNew(IView obj);

        ZendeskClientBase Client { get; set; }

        bool TryGet(int id, out IView value);
    }
}