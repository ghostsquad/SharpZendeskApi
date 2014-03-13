// .\GetZendeskModelDataFromUrl.ps1 http://developer.zendesk.com/documentation/rest_api/ticket_fields.html
// Ticket fields have the following attributes
// Name                  Type    ReadOnly Mandatory Comment
// ----                  ----    -------- --------- -------
// active                boolean no       no        Whether this field is available
// collapsed_for_agents  string  no       no        If this field should be shown to agents by default or be hidden alongside infrequently used fields
// created_at            date    yes      no        The time the ticket field was created
// custom_field_options  array   no       yes       Required and presented for a ticket field of type "tagger"
// description           string  no       no        The description of the purpose of this ticket field, shown to users
// editable_in_portal    boolean no       no        Whether this field is editable by end users
// id                    integer yes      no        Automatically assigned upon creation
// position              integer no       no        A relative position for the ticket fields, determines the order of ticket fields on a ticket
// regexp_for_validation string  no       no        Regular expression field only. The validation pattern for a field value to be deemed valid.
// removable             boolean yes      no        If this field is not a system basic field that must be present for all tickets on the account
// required              boolean no       no        If it's required for this field to have a value when updated by agents
// required_in_portal    boolean no       no        If it's required for this field to have a value when updated by end users
// system_field_options  array   yes      no        Presented for a ticket field of type "tickettype", "priority" or "status"
// tag                   string  no       no        A tag value to set for checkbox fields when checked
// title                 string  no       yes       The title of the ticket field
// title_in_portal       string  no       no        The title of the ticket field when shown to end users
// type                  string  no       yes       The type of the ticket field
// updated_at            date    yes      no        The time of the last update of the ticket field
// url                   string  yes      no        The URL for this resource
// visible_in_portal     boolean no       no        Whether this field is available to end users
namespace SharpZendeskApi.Core.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;

    using RestSharp;

    /// <summary>
    ///     The ticket field.
    /// </summary>
    public class TicketField : TrackableZendeskThingBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether active.
        /// </summary>
        public bool? Active { get; set; }

        /// <summary>
        ///     Gets or sets the collapsed for agents.
        /// </summary>
        public string CollapsedForAgents { get; set; }

        /// <summary>
        ///     Gets or sets the created at.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /* for now, I'm commenting this out, as this object is not described in the documentation
         * http://developer.zendesk.com/documentation/rest_api/ticket_fields.html
        /// <summary>
        /// Gets or sets the custom field options.
        /// </summary>
        public List<object> CustomFieldOptions { get; set; }
         */

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether editable in portal.
        /// </summary>
        public bool? EditableInPortal { get; set; }

        /// <summary>
        ///     Gets or sets the position.
        /// </summary>
        public int? Position { get; set; }

        /// <summary>
        ///     Gets or sets the regexp for validation.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", 
            Justification = "This is the name of the zendesk api key.")]
        public string RegexpForValidation { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether removable.
        /// </summary>
        public bool? Removable { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether required.
        /// </summary>
        public bool? Required { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether required in portal.
        /// </summary>
        public bool? RequiredInPortal { get; set; }

        /* for now, I'm commenting this out, as this object is not described in the documentation
         * http://developer.zendesk.com/documentation/rest_api/ticket_fields.html
        /// <summary>
        /// Gets or sets the system field options.
        /// </summary>
        [ReadOnly]
        public List<UNKNOWN> SystemFieldOptions { get; set; }
         */

        /// <summary>
        ///     Gets or sets the tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the title in portal.
        /// </summary>
        public string TitleInPortal { get; set; }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Gets or sets the updated at.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        ///     Gets or sets the url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether visible in portal.
        /// </summary>
        public bool? VisibleInPortal { get; set; }

        #endregion
    }
}