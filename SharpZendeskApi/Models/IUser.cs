namespace SharpZendeskApi.Models
{
    using System;
    using System.Collections.Generic;

    using SharpZendeskApi.Models.Attributes;

    public interface IUser : ITrackable
    {
        /// <summary>
        /// Gets a value indicating whether active.
        /// </summary>
        bool? Active { get; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        string Alias { get; set; }

        /// <summary>
        /// Gets or sets the custom role id.
        /// </summary>
        int? CustomRoleId { get; set; }

        /// <summary>
        /// Gets or sets the details.
        /// </summary>
        string Details { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Gets or sets the external id.
        /// </summary>
        string ExternalId { get; set; }

        /// <summary>
        /// Gets the last login at.
        /// </summary>
        DateTime? LastLoginAt { get; }

        /// <summary>
        /// Gets the locale.
        /// </summary>
        string Locale { get; }

        /// <summary>
        /// Gets or sets the locale id.
        /// </summary>
        int? LocaleId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether moderator.
        /// </summary>
        bool? Moderator { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        string Notes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether only private comments.
        /// </summary>
        bool? OnlyPrivateComments { get; set; }

        /// <summary>
        /// Gets or sets the organization id.
        /// </summary>
        int? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        string Phone { get; set; }

        /// <summary>
        /// Gets or sets the photo.
        /// </summary>
        IAttachment Photo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether restricted agent.
        /// </summary>
        bool? RestrictedAgent { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        string Role { get; set; }

        /// <summary>
        /// Gets a value indicating whether shared.
        /// </summary>
        bool? Shared { get; }

        /// <summary>
        /// Gets a value indicating whether shared agent.
        /// </summary>
        bool? SharedAgent { get; }

        /// <summary>
        /// Gets or sets the signature.
        /// </summary>
        string Signature { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether suspended.
        /// </summary>
        bool? Suspended { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the ticket restriction.
        /// </summary>
        string TicketRestriction { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the user fields.
        /// </summary>
        IEnumerable<CustomField> UserFields { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether verified.
        /// </summary>
        bool? Verified { get; set; }

        [ReadOnly]
        string Url { get; }

        [ReadOnly]
        DateTime? UpdatedAt { get; }

        [ReadOnly]
        DateTime? CreatedAt { get; }
    }
}