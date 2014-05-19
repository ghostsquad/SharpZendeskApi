namespace SharpZendeskApi.Models
{
    public interface IThumbnail : IZendeskThing, ITrackable
    {
        /// <summary>
        ///     Gets the content type.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        ///     Gets the content url.
        /// </summary>
        string ContentUrl { get; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Gets the size.
        /// </summary>
        int? Size { get; }
    }
}