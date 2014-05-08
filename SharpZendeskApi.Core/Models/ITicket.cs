namespace SharpZendeskApi.Core.Models
{
    using System;
    using System.Collections.Generic;

    public interface ITicket : IZendeskThing, ITrackable
    {
        #region Public Properties

        int? AssigneeId { get; set; }

        int? BrandId { get; }

        IEnumerable<int> CollaboratorIds { get; set; }

        string Comment { get; set; }

        DateTime? CreatedAt { get; }

        IEnumerable<CustomField> CustomFields { get; set; }

        string Description { get; }

        DateTime? DueAt { get; set; }

        string ExternalId { get; set; }

        List<int> FollowupIds { get; }

        int? ForumTopicId { get; set; }

        int? GroupId { get; set; }

        bool? HasIncidents { get; }

        int? OrganizationId { get; }

        string Priority { get; set; }

        int? ProblemId { get; set; }

        string Recipient { get; }

        int? RequesterId { get; set; }

        SatisfactionRating SatisfactionRating { get; }

        List<int> SharingAgreementIds { get; }

        string Status { get; set; }

        string Subject { get; set; }

        int? SubmitterId { get; set; }

        IEnumerable<string> Tags { get; set; }

        int? TicketFormId { get; }

        string Type { get; set; }

        DateTime? UpdatedAt { get; }

        string Url { get; }

        Via Via { get; }

        #endregion
    }
}