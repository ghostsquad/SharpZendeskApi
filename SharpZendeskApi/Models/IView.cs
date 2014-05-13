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

        IList<Condition> All { get; set; }

        IList<Condition> Any { get; set; }

        /// <summary>
        ///     Gets or sets the conditions.
        /// </summary>
        Conditions Conditions { get; set; }

        /// <summary>
        ///     Gets or sets the execution.
        /// </summary>
        Execution Execution { get; set; }

        /// <summary>
        ///     Gets or sets the restriction.
        /// </summary>
        Restriction Restriction { get; set; }

        /// <summary>
        ///     Gets or sets the sla id.
        /// </summary>
        int? SlaId { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        string Title { get; set; }

        [ReadOnly]
        DateTime? UpdatedAt { get; }

        [ReadOnly]
        DateTime? CreatedAt { get; }
    }
}