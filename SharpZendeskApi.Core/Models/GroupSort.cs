// http://developer.zendesk.com/documentation/rest_api/views.html#execution
// {
//    "execution" : {
//       "columns" : [{
//             "id" : "status",
//             "title" : "Status"
//          }, {
//             "id" : "updated",
//             "title" : "Updated"
//          }, {
//             "id" : 5,
//             "title" : "Account",
//             "type" : "text",
//             "url" : "https://example.zendesk.com/api/v2/ticket_fields/5.json"
//          },
//          ...
//       ]
//       "group" : {
//          "id" : "status",
//          "title" : "Status",
//          "order" : "desc"
//       },
//       "sort" : {
//          "id" : "updated",
//          "title" : "Updated",
//          "order" : "desc"
//       }
//    }
// }
namespace SharpZendeskApi.Core.Models
{
    using RestSharp;

    /// <summary>
    /// The group sort.
    /// </summary>
    public class GroupSort : IZendeskThing
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        #endregion
    }
}