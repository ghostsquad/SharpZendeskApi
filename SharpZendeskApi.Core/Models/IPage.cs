// http://developer.zendesk.com/documentation/rest_api/introduction.html#collections
// {
//  "users": [ ... ],
//  "count": 1234,
//  "next_page": "https://account.zendesk.com/api/v2/users.json?page=2",
//  "previous_page": null
// }

namespace SharpZendeskApi.Core.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The PagedResponse interface.
    /// </summary>
    /// <typeparam name="T">
    /// Zendesk Api Model type.
    /// </typeparam>
    internal interface IPage<T> where T : IZendeskThing
    {
        #region Public Properties

        List<T> Collection { get; set; }

        string NextPage { get; set; }

        int Count { get; set; }

        string PreviousPage { get; set; }

        #endregion
    }
}