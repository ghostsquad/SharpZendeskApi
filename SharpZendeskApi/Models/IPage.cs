// http://developer.zendesk.com/documentation/rest_api/introduction.html#collections
// {
//  "users": [ ... ],
//  "count": 1234,
//  "next_page": "https://account.zendesk.com/api/v2/users.json?page=2",
//  "previous_page": null
// }

namespace SharpZendeskApi.Models
{
    using System.Collections.Generic;

    internal interface IPage<TInterface> where TInterface : class, IZendeskThing, ITrackable
    {
        #region Public Properties

        IList<TInterface> Collection { get; set; }

        string NextPage { get; set; }

        int Count { get; set; }

        string PreviousPage { get; set; }

        #endregion
    }
}