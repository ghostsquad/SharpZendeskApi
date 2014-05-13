namespace SharpZendeskApi.Models
{
    using System;

    using SharpZendeskApi.Models.Attributes;

    public interface ITrackable
    {
        [ReadOnly]
        int? Id { get; }        
    }
}
