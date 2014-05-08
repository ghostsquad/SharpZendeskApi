namespace SharpZendeskApi.Management
{
    using System;
    using System.Collections.Generic;

    using RestSharp;

    using SharpZendeskApi.Models;

    public abstract class ManagerBase<TModel, TInterface> : IManager<TInterface>
        where TInterface : IZendeskThing, ITrackable
        where TModel : TrackableZendeskThingBase, TInterface
    {
        #region Constructors and Destructors

        protected ManagerBase(IZendeskClient client)
        {
            this.Client = client;
        }

        #endregion

        #region Public Properties

        public IZendeskClient Client { get; set; }

        #endregion

        #region Public Methods and Operators

        public abstract TInterface Get(int id);

        public abstract IListing<TInterface> GetMany(IEnumerable<int> ids);

        public abstract TInterface SubmitNew(TInterface obj);

        public abstract void SubmitUpdatesFor(TInterface obj);

        public virtual bool TryGet(int id, out TInterface value)
        {
            try
            {
                value = this.Get(id);
                return true;
            }
            catch (Exception)
            {
                value = default(TInterface);
                return false;
            }
        }

        #endregion

        #region Methods

        protected TInterface Get(string url, int id)
        {
            var request = new RestRequest(url, Method.GET)
                              {
                                  RootElement = typeof(TModel).GetTypeNameAsCPlusPlusStyle()
                              };

            var response = this.Client.Execute<TModel>(request);

            response.ThrowIfProblem();

            return response.Data;
        }

        protected virtual IListing<TInterface> GetMany(string url)
        {
            // http://developer.zendesk.com/documentation/rest_api/tickets.html#show-multiple-tickets
            var request = new RestRequest(url, Method.GET)
                              {
                                  RootElement =
                                      typeof(TModel).GetTypeNameAsCPlusPlusStyle()
                                      .Pluralize()
                              };

            return new Listing<TModel, TInterface>(this.Client, request);
        }

        protected TInterface SubmitNew(string url, TrackableZendeskThingBase obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            obj.ThrowIfSubmitted();
            obj.ThrowIfAnyMandatoryPropertyIsNull();

            var handler = this.Client.Container.Resolve<IZendeskSerializer>(SerializationScenario.Create.ToString());
            var jsonBody = handler.Serialize(obj);

            var request = new RestRequest(url, Method.POST)
                              {
                                  RequestFormat = DataFormat.Json,
                                  RootElement = typeof(TModel).GetTypeNameAsCPlusPlusStyle()
                              };

            request.AddBody(jsonBody);
            var response = this.Client.Execute<TModel>(request);

            response.ThrowIfProblem();

            obj.WasSubmitted = true;

            return response.Data;
        }

        protected void SubmitUpdatesFor(string url, TModel obj)
        {
            obj.ThrowIfNotSubmitted();

            if (obj.ChangedProperties.Count == 0)
            {
                return;
            }

            var handler = this.Client.Container.Resolve<IZendeskSerializer>(SerializationScenario.Create.ToString());
            var jsonBody = handler.Serialize(obj);

            var request = new RestRequest(url, Method.PUT)
                              {
                                  RequestFormat = DataFormat.Json,
                                  RootElement = typeof(TModel).GetTypeNameAsCPlusPlusStyle()
                              };

            request.AddBody(jsonBody);
            var response = this.Client.Execute<TModel>(request);

            response.ThrowIfProblem();
        }

        #endregion
    }
}