namespace SharpZendeskApi.Models
{
    using System;

    public interface ISatisfactionRating : IZendeskThing, ITrackable
    {
        /// <summary>
        /// Gets the assignee id.
        /// </summary>
        int? AssigneeId { get; }

        /// <summary>
        /// Gets the comment.
        /// </summary>
        string Comment { get; }

        /// <summary>
        /// Gets the created at.
        /// </summary>
        DateTime? CreatedAt { get; }

        /// <summary>
        /// Gets the group id.
        /// </summary>
        int? GroupId { get; }

        /// <summary>
        /// Gets or the requester id.
        /// </summary>
        int? RequesterId { get; }

        /// <summary>
        /// Gets the score.
        /// </summary>
        string Score { get; }

        /// <summary>
        /// Gets the ticket id.
        /// </summary>
        int? TicketId { get; }

        /// <summary>
        /// Gets the update at.
        /// </summary>
        DateTime? UpdatedAt { get; }

        /// <summary>
        /// Gets the url.
        /// </summary>
        string Url { get; }
    }
}