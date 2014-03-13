namespace SharpZendeskApi.Core.Management
{
    using System.Collections.Generic;

    using SharpZendeskApi.Core.Models;

    public interface IManager<TInterface>
        where TInterface : IZendeskThing        
    {
        Dictionary<int, TInterface> Cache { get; set; }

        IZendeskClient Client { get; set; }

        void RefreshCache();

        IListing<TInterface> GetMany(IEnumerable<int> ids);

        bool Exists(int id);

        bool TryGet(int id, out TInterface value);

        TInterface Get(int id, bool force);

        void SubmitUpdatesFor(TInterface obj);

        TInterface SubmitNew(TInterface obj);
    }
}