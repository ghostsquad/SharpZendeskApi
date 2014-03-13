// Tickets are represented as JSON objects which have the following keys:
// Name                  Type    ReadOnly Mandatory Comment
// ----                  ----    -------- --------- -------
// assignee_id           integer no       no        What agent is currently assigned to the ticket
// brand_id              integer yes      no        The id of the brand this ticket is associated with - only applicable for enterprise accounts
// collaborator_ids      array   no       no        Who are currently CC'ed on the ticket
// created_at            date    yes      no        When this record was created
// custom_fields         array   no       no        The custom fields of the ticket
// description           string  yes      yes        The first comment on the ticket
// due_at                date    no       no        If this is a ticket of type "task" it has a due date. Due date format uses ISO 8601 format.
// external_id           string  no       no        A unique external id, you can use this to link Zendesk tickets to local records
// followup_ids          array   yes      no        The ids of the followups created from this ticket - only applicable for closed tickets
// forum_topic_id        integer no       no        The topic this ticket originated from, if any
// group_id              integer no       no        The group this ticket is assigned to
// has_incidents         boolean yes      no        Is true of this ticket has been marked as a problem, false otherwise
// id                    integer yes      no        Automatically assigned when creating tickets
// organization_id       integer yes      no        The organization of the requester
// priority              string  no       no        Priority, defines the urgency with which the ticket should be addressed: "urgent", "high",
//                                                  "normal", "low"
// problem_id            integer no       no        The problem this incident is linked to, if any
// recipient             string  yes      no        The original recipient e-mail address of the ticket
// requester_id          integer no       yes       The user who requested this ticket
// satisfaction_rating   object  yes      no        The satisfaction rating of the ticket, if it exists
// sharing_agreement_ids array   yes      no        The ids of the sharing agreements used for this ticket
// status                string  no       no        The state of the ticket, "new", "open", "pending", "hold", "solved", "closed"
// subject               string  no       no        The value of the subject field for this ticket
// submitter_id          integer no       no        The user who submitted the ticket; The submitter always becomes the author of the first comment on
//                                                  the ticket.
// tags                  array   no       no        The array of tags applied to this ticket
// ticket_form_id        integer yes      no        The id of the ticket form to render for this ticket - only applicable for enterprise accounts
// type                  string  no       no        The type of this ticket, i.e. "problem", "incident", "question" or "task"
// updated_at            date    yes      no        When this record last got updated
// url                   string  yes      no        The API url of this ticket
// via                   Via     yes      no        This object explains how the ticket was created
// The POST request takes one parameter, a ticket object that lists the values to set when the ticket is created.
// Name             Description
// ----             -----------
// assignee_id      The numeric ID of the agent to assign the ticket to.
// collaborator_ids An array of the numeric IDs of agents or end-users to CC on the ticket. An email notification is sent to them when the ticket is
//                  created.
// comment          Required. A comment object that describes the problem, incident, question, or task. See Ticket comments in Audit Events.
// custom_fields    An array of the custom fields of the ticket.
// due_at           For tickets of type "task", the due date of the task. Accepts the ISO 8601 date format (yyyy-mm-dd).
// external_id      A unique external ID to link Zendesk tickets to local records.
// forum_topic_id   The numeric ID of the topic the ticket originated from, if any.
// group_id         The numeric ID of the group to assign the ticket to.
// organization_id  The numeric ID of the organization to assign the ticket to.
// priority         Allowed values are urgent, high, normal, or low.
// problem_id       For tickets of type "incident", the numeric ID of the problem the incident is linked to, if any.
// requester_id     The numeric ID of the user asking for support through the ticket.
// status           Allowed values are new, open, pending, hold, solved or closed. Is set to open if status is not specified.
// subject          Required. The subject of the ticket.
// submitter_id     The numeric ID of the user submitting the ticket.
// tags             An array of tags to add to the ticket.
// type             Allowed values are problem, incident, question, or task.
// The PUT request takes one parameter, a ticket object that lists the values to update. All properties are optional.
// Name             Description
// ----             -----------
// assignee_id      The numeric ID of the agent to assign the ticket to.
// collaborator_ids An array of the numeric IDs of agents or end-users to CC. Note that this replaces any existing collaborators. An email notification
//                  is sent to them when the ticket is created.
// comment          An object that adds a comment to the ticket. See Ticket comments in Audit Events.
// custom_fields    An array of the custom field objects consisting of ids and values. Any tags defined with the custom field replace existing tags.
// due_at           For tickets of type "task", the due date of the task. Accepts the ISO 8601 date format (yyyy-mm-dd).
// external_id      A unique external ID to link Zendesk tickets to local records.
// forum_topic_id   The numeric ID of the topic the ticket originated from, if any.
// group_id         The numeric ID of the group to assign the ticket to.
// organization_id  The numeric ID of the organization to assign the ticket to.
// priority         Allowed values are urgent, high, normal, or low.
// problem_id       For tickets of type "incident", the numeric ID of the problem the incident is linked to, if any.
// requester_id     The numeric ID of the user asking for support through the ticket.
// status           Allowed values are open, pending, hold, solved or closed.
// subject          The subject of the ticket.
// tags             An array of tags to add to the ticket. Note that the tags replace any existing tags.
// type             Allowed values are problem, incident, question, or task.
namespace SharpZendeskApi.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SharpZendeskApi.Core.Models.Attributes;

    /// <summary>
    ///     The ticket object
    ///     http://developer.zendesk.com/documentation/rest_api/tickets.html
    /// </summary>
    public class Ticket : TrackableZendeskThingBase, ITicket
    {
        #region Fields

        private int? assigneeId;

        private int[] collaboratorIds;

        private string comment;

        private CustomField[] customFields;

        private DateTime? dueAt;

        private string externalId;

        private int? forumTopicId;

        private int? groupId;

        private string priority;

        private int? problemId;

        private int? requesterId;

        private string status;

        private string subject;

        private int? submitterId;

        private string[] tags;

        private string type;

        #endregion

        #region Constructors and Destructors

        public Ticket(int? requesterId, string description)
        {
            this.requesterId = requesterId;
            this.Description = description;
        }

        public Ticket()
        {
        }

        #endregion

        #region Public Properties

        public int? AssigneeId
        {
            get
            {
                return this.assigneeId;
            }

            set
            {
                this.assigneeId = value;
                this.NotifyPropertyChanged();
            }
        }

        [ReadOnly]
        public int? BrandId { get; set; }

        /// <summary>
        ///     Gets or sets the collaborator ids.
        /// </summary>
        public IEnumerable<int> CollaboratorIds
        {
            get
            {
                return this.collaboratorIds != null ? this.collaboratorIds.ToArray() : null;
            }

            set
            {
                this.collaboratorIds = value != null ? value.ToArray() : new int[0];

                this.NotifyPropertyChanged();
            }
        }

        public string Comment
        {
            get
            {
                return this.comment;
            }

            set
            {
                this.comment = value;
                this.NotifyPropertyChanged();
            }
        }

        [ReadOnly]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        ///     Gets or sets the custom fields.
        /// </summary>
        public IEnumerable<CustomField> CustomFields
        {
            get
            {
                return this.customFields != null ? this.customFields.ToArray() : null;
            }

            set
            {
                this.customFields = value != null ? value.ToArray() : new CustomField[0];
                this.NotifyPropertyChanged();
            }
        }

        [ReadOnly, Mandatory]
        public string Description { get; set; }

        public DateTime? DueAt
        {
            get
            {
                return this.dueAt;
            }

            set
            {
                this.dueAt = value;
                this.NotifyPropertyChanged();
            }
        }

        public string ExternalId
        {
            get
            {
                return this.externalId;
            }

            set
            {
                this.externalId = value;
                this.NotifyPropertyChanged();
            }
        }

        [ReadOnly]
        public List<int> FollowupIds { get; set; }

        public int? ForumTopicId
        {
            get
            {
                return this.forumTopicId;
            }

            set
            {
                this.forumTopicId = value;
                this.NotifyPropertyChanged();
            }
        }

        public int? GroupId
        {
            get
            {
                return this.groupId;
            }

            set
            {
                this.groupId = value;
                this.NotifyPropertyChanged();
            }
        }

        [ReadOnly]
        public bool? HasIncidents { get; set; }

        [ReadOnly]
        public int? OrganizationId { get; set; }

        public string Priority
        {
            get
            {
                return this.priority;
            }

            set
            {
                this.priority = value;
                this.NotifyPropertyChanged();
            }
        }

        public int? ProblemId
        {
            get
            {
                return this.problemId;
            }

            set
            {
                this.problemId = value;
                this.NotifyPropertyChanged();
            }
        }

        [ReadOnly]
        public string Recipient { get; set; }

        [Mandatory]
        public int? RequesterId
        {
            get
            {
                return this.requesterId;
            }

            set
            {
                this.requesterId = value;
                this.NotifyPropertyChanged();
            }
        }

        [ReadOnly]
        public SatisfactionRating SatisfactionRating { get; set; }

        [ReadOnly]
        public List<int> SharingAgreementIds { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        public string Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.status = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the subject.
        /// </summary>
        public string Subject
        {
            get
            {
                return this.subject;
            }

            set
            {
                this.subject = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the submitter id.
        /// </summary>
        public int? SubmitterId
        {
            get
            {
                return this.submitterId;
            }

            set
            {
                this.submitterId = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the tags.
        /// </summary>
        public IEnumerable<string> Tags
        {
            get
            {
                return this.tags != null ? this.tags.ToArray() : null;
            }

            set
            {
                this.tags = value != null ? value.ToArray() : new string[0];
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the ticket form id.
        /// </summary>
        [ReadOnly]
        public int? TicketFormId { get; set; }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        public string Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the updated at.
        /// </summary>        
        [ReadOnly]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        ///     Gets or sets the url.
        /// </summary>
        [ReadOnly]
        public string Url { get; set; }

        /// <summary>
        ///     Gets or sets the via.
        /// </summary>
        [ReadOnly]
        public Via Via { get; set; }

        #endregion
    }
}