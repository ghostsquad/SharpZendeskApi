namespace SharpZendeskApi.Models
{
    using System.Collections.Generic;

    public interface ITicketComment : IZendeskThing, ITrackable
    {
        /// <summary>
        /// Gets the attachments.
        /// </summary>
        IList<IAttachment> Attachments { get; }

        /// <summary>
        /// Gets the author id.
        /// </summary>
        int? AuthorId { get; }

        /// <summary>
        /// Gets the body.
        /// </summary>
        string Body { get; }

        /// <summary>
        /// Gets the html body.
        /// </summary>
        string HtmlBody { get; }

        /// <summary>
        /// Gets or sets a value indicating whether public.
        /// </summary>
        bool? Public { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        string Type { get; }
    }
}