// .\GetZendeskModelDataFromUrl.ps1 http://developer.zendesk.com/documentation/rest_api/views.html
// Views are represented as simple flat JSON objects which have the following keys.
// Name        Type       Description
// ----        ----       -----------
// active      boolean    Useful for determining if the view should be displayed
// conditions  Conditions An object describing how the view is constructed
// created_at  date       The time the view was created
// execution   Execute    An object describing how the view should be executed
// id          integer    Automatically assigned when created
// restriction object     Who may access this account. Will be null when everyone in the account can access it.
// sla_id      integer    If the view is for an SLA this is the id
// title       string     The title of the view
// updated_at  date       The time of the last update of the view
namespace SharpZendeskApi.Core.Models
{
    using System;
    using System.Collections.ObjectModel;

    using RestSharp;

    /// <summary>
    ///     The view.
    /// </summary>
    public class View : TrackableZendeskThingBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether active.
        /// </summary>
        public bool? Active { get; set; }

        /// <summary>
        ///     Gets or sets the conditions.
        /// </summary>
        public Conditions Conditions { get; set; }

        /// <summary>
        ///     Gets or sets the created at.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        ///     Gets or sets the execution.
        /// </summary>
        public Execution Execution { get; set; }

        /// <summary>
        ///     Gets or sets the restriction.
        /// </summary>
        public Restriction Restriction { get; set; }

        /// <summary>
        ///     Gets or sets the sla id.
        /// </summary>
        public int? SlaId { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the updated at.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        #endregion
    }
}