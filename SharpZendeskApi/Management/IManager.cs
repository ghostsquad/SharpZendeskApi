namespace SharpZendeskApi.Management
{
    using System.Collections.Generic;

    using SharpZendeskApi.Models;

    public interface IManager<TInterface>
        where TInterface : IZendeskThing, ITrackable
    {
        ZendeskClientBase Client { get; }

        IListing<TInterface> GetMany(IEnumerable<int> ids);

        bool TryGet(int id, out TInterface value);

        TInterface Get(int id);

        void SubmitUpdatesFor(TInterface obj);

        TInterface SubmitNew(TInterface obj);
    }
}