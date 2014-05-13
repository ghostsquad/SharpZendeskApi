// .\GetZendeskModelDataFromUrl.ps1 http://developer.zendesk.com/documentation/rest_api/satisfaction_ratings.html
// Name         Type    ReadOnly Mandatory Comment
// ----         ----    -------- --------- -------
// assignee_id  integer yes      yes       The id of agent assigned to at the time of rating
// comment      string  yes      no        The comment received with this rating, if available
// created_at   date    yes      no        The time the satisfaction rating got created
// group_id     integer yes      yes       The id of group assigned to at the time of rating
// id           integer yes      no        Automatically assigned upon creation
// requester_id integer yes      yes       The id of ticket requester submitting the rating
// score        string  yes      yes       The rating: "offered", "unoffered", "good" or "bad"
// ticket_id    integer yes      yes       The id of ticket being rated
// updated_at   date    yes      no        The time the satisfaction rating got updated
// url          string  yes      no        The API url of this rating
namespace SharpZendeskApi.Models
{
    using System;

    /// <summary>
    ///     The satisfaction rating.
    /// </summary>
    public class SatisfactionRating : TrackableZendeskThingBase, ISatisfactionRating
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the assignee id.
        /// </summary>
        public int? AssigneeId { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the group id.
        /// </summary>
        public int? GroupId { get; set; }        

        /// <summary>
        /// Gets or sets the requester id.
        /// </summary>
        public int? RequesterId { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        public string Score { get; set; }

        /// <summary>
        /// Gets or sets the ticket id.
        /// </summary>
        public int? TicketId { get; set; }

        /// <summary>
        /// Gets or sets the update at.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url { get; set; }

        #endregion
    }
}