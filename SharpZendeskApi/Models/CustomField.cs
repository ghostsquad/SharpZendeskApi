// http://developer.zendesk.com/documentation/rest_api/tickets.html
// custom field is a KeyValuePair<int,object>
namespace SharpZendeskApi.Models
{
    /// <summary>
    ///     The custom field.
    /// </summary>
    public class CustomField : TrackableZendeskThingBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        #endregion
    }
}