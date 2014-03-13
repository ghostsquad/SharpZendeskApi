namespace SharpZendeskApi.Core
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using RestSharp;

    using SharpZendeskApi.Core.Models;

    public class Listing<T> : IListing<T>
        where T : IZendeskThing
    {
        // this is the default maximum items per page for the zendesk api
        // http://developer.zendesk.com/documentation/rest_api/introduction.html#collections
        #region Constants

        private const int DefaultItemsPerPage = 100;

        #endregion

        #region Constructors and Destructors

        internal Listing(IRestClient client, IRestRequest request)
        {
            this.Client = client;
            this.Request = request;
        }

        #endregion

        #region Public Properties

        public bool AtEndOfPage { get; internal set; }

        public int? CurrentPage { get; private set; }

        public int? NextPage { get; private set; }

        public int? PreviousPage { get; private set; }

        #endregion

        #region Properties

        private IRestClient Client { get; set; }

        private IRestRequest Request { get; set; }

        #endregion

        #region Public Methods and Operators

        public IEnumerator<T> GetEnumerator()
        {
            return this.GetEnumerator(100);
        }

        public IEnumerator<T> GetEnumerator(int itemsPerRequest, int maxItems = -1)
        {
            return new ListingEnumerator<T>(this, itemsPerRequest, maxItems);
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        private class ListingEnumerator<T1> : IEnumerator<T1>
            where T1 : IZendeskThing
        {
            #region Fields

            private int currentIndexWithinPage;

            private T1[] currentPageCollection;

            private int currentPageCollectionCount;

            private int currentPageNumber;

            private bool isNextPageAvailable = true;

            private int itemsPerRequest;

            private IRestRequest lastRequest;

            private Listing<T1> listing;

            private int maxItems;

            private int totalCount;

            #endregion

            #region Constructors and Destructors

            public ListingEnumerator(Listing<T1> listing, int itemsPerRequest, int maxItems)
            {
                this.listing = listing;
                this.maxItems = maxItems;
                this.itemsPerRequest = itemsPerRequest < 0 ? DefaultItemsPerPage : itemsPerRequest;
                this.maxItems = maxItems;
                this.Reset();
            }

            #endregion

            #region Public Properties

            public T1 Current
            {
                get
                {
                    return this.currentPageCollection[this.currentIndexWithinPage];
                }
            }

            #endregion

            #region Explicit Interface Properties

            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

            #endregion

            #region Public Methods and Operators

            public void Dispose()
            {
                // nothing to do here
            }

            public bool MoveNext()
            {
                this.SetAtEndOfPage();
                if (this.currentIndexWithinPage < 0 || this.listing.AtEndOfPage)
                {
                    // stop moving forward if we are at the last page and at the last item on the current page.
                    if (this.currentPageNumber > 0 && !this.isNextPageAvailable)
                    {
                        return false;
                    }

                    this.FetchNextPage();
                    return !(this.currentIndexWithinPage + 1 > this.currentPageCollectionCount);
                }

                this.currentIndexWithinPage++;
                this.SetAtEndOfPage();
                return true;
            }

            public void Reset()
            {
                this.listing.AtEndOfPage = false;
                this.lastRequest = null;
                this.currentIndexWithinPage = -1;
                this.currentPageNumber = 0;
                this.currentPageCollection = new T1[0];
            }

            #endregion

            #region Methods

            private void FetchNextPage()
            {
                if (this.lastRequest == null)
                {
                    this.lastRequest = this.listing.Request;
                }

                var nextRequest = new RestRequest(this.lastRequest.Resource, Method.GET);
                if (this.lastRequest.Parameters != null)
                {
                    foreach (var param in this.lastRequest.Parameters)
                    {
                        if (param.Name == "page")
                        {
                            param.Value = this.listing.CurrentPage = ++this.currentPageNumber;
                        }

                        nextRequest.AddParameter(param);
                    }
                }
                else
                {
                    nextRequest.AddParameter("page", this.listing.CurrentPage = ++this.currentPageNumber);
                }

                this.lastRequest = nextRequest;

                IRestResponse<IPage<T1>> response = this.listing.Client.Execute<IPage<T1>>(this.lastRequest);

                response.ThrowIfProblem();

                this.currentIndexWithinPage = 0;
                this.currentPageCollection = response.Data.Collection.ToArray();
                this.currentPageCollectionCount = this.currentPageCollection.Count();
                this.totalCount = response.Data.Count;
                this.isNextPageAvailable = response.Data.NextPage != null;
                this.listing.PreviousPage = this.currentPageNumber <= 1 ? null : new int?(this.currentPageNumber - 1);
                this.listing.NextPage = this.isNextPageAvailable ? new int?(this.currentPageNumber + 1) : null;
                this.SetAtEndOfPage();
            }

            private void SetAtEndOfPage()
            {
                this.listing.AtEndOfPage = this.currentIndexWithinPage + 1 >= this.currentPageCollectionCount;
            }

            #endregion
        }
    }
}