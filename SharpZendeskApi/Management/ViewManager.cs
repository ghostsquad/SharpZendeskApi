using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpZendeskApi.Management
{
    using RestSharp;

    using SharpZendeskApi.Models;

    public class ViewManager : ManagerBase<View, IView>
    {
        private const string SingleEndpoint = "views/{0}.json";

        private const string CreateListEndpoint = "views.json";

        private const string ActiveEndpoint = "views/active.json";

        private const string CompactListEndpoint = "views/compact.json";

        public ViewManager(ZendeskClientBase client)
            : base(client)
        {
        }

        public override IView Get(int id)
        {
            var url = string.Format(SingleEndpoint, id);
            return this.Get(url, id);
        }

        public override void SubmitUpdatesFor(IView obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            var url = string.Format(SingleEndpoint, obj.Id.Value);
            this.SubmitUpdatesFor(url, obj);
        }

        public IListing<IView> GetAvailableViews(bool full = false)
        {
            var url = full ? CreateListEndpoint : CompactListEndpoint;

            var request = new RestRequest(url, Method.GET) { RootElement = this.PluralizedModelName };

            return new Listing<View, IView>(this.Client, request);
        }

        public IListing<IView> GetActiveViews()
        {
            var request = new RestRequest(ActiveEndpoint, Method.GET) { RootElement = this.PluralizedModelName };

            return new Listing<View, IView>(this.Client, request);
        }        

        public override IListing<IView> GetMany(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public override IView SubmitNew(IView obj)
        {
            return this.SubmitNew(CreateListEndpoint, obj);
        }
    }
}
