namespace SharpZendeskApi.Models
{
    using System;
    using System.Collections.Generic;

    using SharpZendeskApi.Models.Attributes;

    public interface IView : IZendeskThing, ITrackable
    {
        /// <summary>
        ///     Gets or sets a value indicating whether active.
        /// </summary>
        bool? Active { get; set; }

        IEnumerable<Condition> All { get; set; }

        IEnumerable<Condition> Any { get; set; }

        /// <summary>
        ///     Gets or sets the conditions.
        /// </summary>
        Conditions Conditions { get; }

        /// <summary>
        ///     Gets or sets the execution.
        /// </summary>
        Execution Execution { get; }

        /// <summary>
        ///     Gets or sets the restriction.
        /// </summary>
        Restriction Restriction { get; set; }

        ViewOutput Output { get; set; }

        /// <summary>
        ///     Gets or sets the sla id.
        /// </summary>
        int? SlaId { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        string Title { get; set; }

        DateTime? UpdatedAt { get; }
        
        DateTime? CreatedAt { get; }
    }
}