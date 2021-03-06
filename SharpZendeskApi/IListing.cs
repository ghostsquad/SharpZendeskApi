﻿namespace SharpZendeskApi
{
    using System.Collections.Generic;

    using SharpZendeskApi.Models;

    public interface IListing<out TInterface> : IEnumerable<TInterface>
        where TInterface : IZendeskThing, ITrackable
    {
        #region Public Properties

        bool AtEndOfPage { get; }

        int? CurrentPage { get; }

        int Count { get; }

        int? NextPage { get; }

        int? PreviousPage { get; }

        #endregion

        #region Public Methods and Operators

        IEnumerator<TInterface> GetEnumerator(int itemsPerRequest, int maxItems = -1);

        #endregion
    }
}