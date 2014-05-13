namespace SharpZendeskApi.Models
{
    using System;
    using System.Collections.Generic;

    using SharpZendeskApi.Models.Attributes;

    public interface ITicket : IZendeskThing, ITrackable
    {
        #region Public Properties

        int? AssigneeId { get; set; }

        int? BrandId { get; }

        IEnumerable<int> CollaboratorIds { get; set; }

        string Comment { get; set; }

        IEnumerable<CustomField> CustomFields { get; set; }

        string Description { get; }

        DateTime? DueAt { get; set; }

        string ExternalId { get; set; }

        IList<int> FollowupIds { get; }

        int? ForumTopicId { get; set; }

        int? GroupId { get; set; }

        bool? HasIncidents { get; }

        int? OrganizationId { get; }

        string Priority { get; set; }

        int? ProblemId { get; set; }

        string Recipient { get; }

        int? RequesterId { get; set; }

        ISatisfactionRating SatisfactionRating { get; }

        IList<int> SharingAgreementIds { get; }

        string Status { get; set; }

        string Subject { get; set; }

        int? SubmitterId { get; set; }

        IEnumerable<string> Tags { get; set; }

        int? TicketFormId { get; }

        string Type { get; set; }

        Via Via { get; }

        [ReadOnly]
        string Url { get; }

        [ReadOnly]
        DateTime? UpdatedAt { get; }

        [ReadOnly]
        DateTime? CreatedAt { get; }

        #endregion
    }
}