namespace SharpZendeskApi.Models
{
    using System.Collections.Generic;

    public interface ITrackable : IZendeskThing
    {
        int? Id { get; }

        bool WasSubmitted { get; }

        IEnumerable<string> ChangedProperties { get; }
    }
}
