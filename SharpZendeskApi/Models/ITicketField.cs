namespace SharpZendeskApi.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface ITicketField : IZendeskThing, ITrackable
    {
        /// <summary>
        ///     Gets or sets a value indicating whether active.
        /// </summary>
        bool? Active { get; set; }

        /// <summary>
        ///     Gets or sets the collapsed for agents.
        /// </summary>
        string CollapsedForAgents { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether editable in portal.
        /// </summary>
        bool? EditableInPortal { get; set; }

        /// <summary>
        ///     Gets or sets the position.
        /// </summary>
        int? Position { get; set; }

        /// <summary>
        ///     Gets or sets the regexp for validation.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "This is the name of the zendesk api key.")]
        string RegexpForValidation { get; set; }

        /// <summary>
        ///     Gets a value indicating whether removable.
        /// </summary>
        bool? Removable { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether required.
        /// </summary>
        bool? Required { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether required in portal.
        /// </summary>
        bool? RequiredInPortal { get; set; }

        /// <summary>
        ///     Gets or sets the tag.
        /// </summary>
        string Tag { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        ///     Gets or sets the title in portal.
        /// </summary>
        string TitleInPortal { get; set; }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        string Type { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether visible in portal.
        /// </summary>
        bool? VisibleInPortal { get; set; }
    }
}