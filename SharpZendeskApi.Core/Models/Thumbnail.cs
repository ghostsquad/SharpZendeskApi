// .\GetZendeskModelDataFromUrl.ps1 http://developer.zendesk.com/documentation/rest_api/attachments.html
// A <see cref="thumbnail"/> shares all of the same fields as an <see cref="attachment"/> object, except an attachment will have a list of "thumbnails".
// Name         Type    ReadOnly Mandatory Comment
// ----         ----    -------- --------- -------
// content_type string  yes      no        The content type of the image. Example value: image/png
// content_url  string  yes      no        A full URL where the attachment image file can be downloaded
// file_name    string  yes      no        The name of the image file
// id           integer yes      no        Automatically assigned when created
// size         integer yes      no        The size of the image file in bytes
// thumbnails   array   yes      no        An array of Photo objects. Note that thumbnails do not have thumbnails.
namespace SharpZendeskApi.Core.Models
{
    using System.Collections.ObjectModel;

    using RestSharp;

    /// <summary>
    ///     The thumbnail.
    /// </summary>
    public class Thumbnail : TrackableZendeskThingBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     Gets or sets the content url.
        /// </summary>
        public string ContentUrl { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the size.
        /// </summary>
        public int? Size { get; set; }

        #endregion
    }
}