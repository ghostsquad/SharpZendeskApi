// .\GetZendeskModelDataFromUrl.ps1 http://developer.zendesk.com/documentation/rest_api/attachments.html
// Attachments are represented as JSON objects with the following keys:
// 
// Name         Type    ReadOnly Mandatory Comment
// ----         ----    -------- --------- -------
// content_type string  yes      no        The content type of the image. Example value: image/png
// content_url  string  yes      no        A full URL where the attachment image file can be downloaded
// file_name    string  yes      no        The name of the image file
// id           integer yes      no        Automatically assigned when created
// size         integer yes      no        The size of the image file in bytes
// thumbnails   array   yes      no        An array of Photo objects. Note that thumbnails do not have thumbnails.
namespace SharpZendeskApi.Models
{
    using System.Collections.Generic;

    using SharpZendeskApi.Models.Attributes;

    /// <summary>
    /// The attachment.
    /// </summary>
    public class Attachment : TrackableZendeskThingBase
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        [ReadOnly]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the content url.
        /// </summary>
        [ReadOnly]
        public string ContentUrl { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [ReadOnly]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        [ReadOnly]
        public int? Size { get; set; }

        /// <summary>
        /// Gets or sets the thumbnails.
        /// </summary>
        [ReadOnly]
        public List<Thumbnail> Thumbnails { get; set; }

        #endregion
    }
}