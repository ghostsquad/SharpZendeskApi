// http://developer.zendesk.com/documentation/rest_api/views.html#execution
// View Execution is a read-only object that describes how to display a collection of tickets in a View.
// Name    Type   Description
// ----    ----   -----------
// columns Array  The ticket fields to display. Custom fields have an id, title, type and url referencing the Ticket Field
// group   Object When present, the structure indicating how the tickets are grouped
// sort    Object The column structure of the field used for sorting.
namespace SharpZendeskApi.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// The execution.
    /// </summary>
    public class Execution : IZendeskThing
    {
        #region Public Properties

        public IList<ViewColumn> Columns { get; set; }

        public IList<ViewColumn> Fields { get; set; }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        public IList<TicketField> CustomFields { get; set; }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        public GroupSort Group { get; set; }

        /// <summary>
        /// Gets or sets the sort.
        /// </summary>
        public GroupSort Sort { get; set; }

        public string GroupBy { get; set; }

        public string GroupOrder { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }

        #endregion
    }
}