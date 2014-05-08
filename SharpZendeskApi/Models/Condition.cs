// http://developer.zendesk.com/documentation/rest_api/views.html#conditions
// Each condition in an array has the following properties:
// Name     Type   Description
// ----     ----   -----------
// field    string The name of a ticket field
// operator string A comparison operator
// value    string The value of a ticket field
namespace SharpZendeskApi.Models
{
    /// <summary>
    /// The condition.
    /// </summary>
    public class Condition : IZendeskThing
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        #endregion      
    }
}