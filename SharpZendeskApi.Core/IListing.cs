namespace SharpZendeskApi.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SharpZendeskApi.Core.Models;

    public interface IListing<T>: IEnumerable<T> where T : IZendeskThing
    {
        IEnumerator<T> GetEnumerator(int itemsPerRequest, int maxItems = -1);

        bool AtEndOfPage { get; }

        int? CurrentPage { get; }

        int? NextPage { get; }

        int? PreviousPage { get; }
    }
}
