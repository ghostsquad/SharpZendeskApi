namespace SharpZendeskApi.Core.Management
{
    using System.Collections.Generic;

    using SharpZendeskApi.Core.Models;

    public interface IManager<TInterface>
        where TInterface : IZendeskThing, ITrackable
    {        
        IZendeskClient Client { get; set; }

        IListing<TInterface> GetMany(IEnumerable<int> ids);

        bool TryGet(int id, out TInterface value);

        TInterface Get(int id);

        void SubmitUpdatesFor(TInterface obj);

        TInterface SubmitNew(TInterface obj);
    }
}