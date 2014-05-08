namespace SharpZendeskApi
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using RestSharp;

    using SharpZendeskApi.Models;

    public class Listing<TModel, TInterface> : IListing<TInterface>
        where TModel : TrackableZendeskThingBase, TInterface
        where TInterface : IZendeskThing, ITrackable
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

        public IEnumerator<TInterface> GetEnumerator()
        {
            return this.GetEnumerator(100);
        }

        public IEnumerator<TInterface> GetEnumerator(int itemsPerRequest, int maxItems = -1)
        {
            return new ListingEnumerator(this, itemsPerRequest, maxItems);
        }

        #endregion

        #region Explicit Interface Methods

        // this is to fulfill the IEnumerable contract
        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        private class ListingEnumerator : IEnumerator<TInterface>
        {
            #region Fields

            private readonly Listing<TModel, TInterface> listing;

            private int currentIndexWithinPage;

            private TInterface[] currentPageCollection;

            private int currentPageCollectionCount;

            private int currentPageNumber;

            private bool isNextPageAvailable = true;

            private int itemsPerRequest;

            private IRestRequest lastRequest;

            private int maxItems;

            private int totalCount;

            #endregion

            #region Constructors and Destructors

            public ListingEnumerator(Listing<TModel, TInterface> listing, int itemsPerRequest, int maxItems)
            {
                this.listing = listing;
                this.maxItems = maxItems;
                this.itemsPerRequest = itemsPerRequest < 0 ? DefaultItemsPerPage : itemsPerRequest;
                this.maxItems = maxItems;
                this.Reset();
            }

            #endregion

            #region Public Properties

            public TInterface Current
            {
                get
                {
                    return this.currentPageCollection[this.currentIndexWithinPage];
                }
            }

            #endregion

            #region Explicit Interface Properties

            // this is to fulfill the IEnumerator contract
            [ExcludeFromCodeCoverage]
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
                this.currentPageCollection = new TInterface[0];
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

                var response = this.listing.Client.Execute<IPage<TModel>>(this.lastRequest);

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