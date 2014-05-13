namespace SharpZendeskApi.Models
{
    using System.Collections.Generic;

    using SharpZendeskApi.Models.Attributes;

    public interface IAttachment : IZendeskThing, ITrackable
    {
        /// <summary>
        /// Gets the content type.
        /// </summary>
        [ReadOnly]
        string ContentType { get; }

        /// <summary>
        /// Gets the content url.
        /// </summary>
        [ReadOnly]
        string ContentUrl { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        [ReadOnly]
        string Name { get; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        [ReadOnly]
        int? Size { get; }

        /// <summary>
        /// Gets the thumbnails.
        /// </summary>
        [ReadOnly]
        IList<IThumbnail> Thumbnails { get; }
    }
}