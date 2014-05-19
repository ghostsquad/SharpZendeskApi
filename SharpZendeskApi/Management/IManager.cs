namespace SharpZendeskApi.Management
{
    using System.Collections.Generic;

    using SharpZendeskApi.Models;

    public interface IManager<TInterface>
        where TInterface : class, IZendeskThing, ITrackable
    {
        TInterface Get(int id);

        IListing<TInterface> GetMany(IEnumerable<int> ids);

        TInterface SubmitNew(TInterface obj);

        void SubmitUpdatesFor(TInterface obj);

        bool TryGet(int id, out TInterface value);
    }
}