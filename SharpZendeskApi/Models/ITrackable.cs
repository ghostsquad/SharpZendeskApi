namespace SharpZendeskApi.Models
{
    using System;
    using System.Collections.Generic;

    using SharpZendeskApi.Models.Attributes;

    public interface ITrackable
    {
        int? Id { get; }

        bool WasSubmitted { get; }

        IEnumerable<string> ChangedProperties { get; }
    }
}
