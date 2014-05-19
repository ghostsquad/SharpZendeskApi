// .\GetZendeskModelDataFromUrl.ps1 http://developer.zendesk.com/documentation/rest_api/ticket_comments.html
// Ticket comments have the following keys:
// 
// Name        Type    ReadOnly Mandatory Comment
// ----        ----    -------- --------- -------
// attachments array   yes      no        The attachments on this comment as Attachment objects
// author_id   integer yes      no        The id of the author of this comment
// body        string  yes      no        The actual comment made by the author. Use the return (\r) and newline (\n) characters for line breaks.
// html_body   string  yes      no        The actual comment made by the author formatted to HTML
// id          integer yes      no        Automatically assigned when creating events
// public      boolean no       no        If this is a public comment or an internal agents only note (can be made private via PUT
//                                        api/v2/tickets//comments//make_private)
// type        string  yes      no        Has the value Comment
namespace SharpZendeskApi.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// The ticket comment.
    /// </summary>
    public class TicketComment : TrackableZendeskThingBase, ITicketComment
    {
        #region Public Properties      

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        public IList<IAttachment> Attachments { get; set; }

        /// <summary>
        /// Gets or sets the author id.
        /// </summary>
        public int? AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the html body.
        /// </summary>
        public string HtmlBody { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether public.
        /// </summary>
        public bool? Public { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string Type { get; set; }

        #endregion
    }
}