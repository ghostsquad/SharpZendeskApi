namespace SharpZendeskApi.Core.Management
{
    using System;
    using System.Collections.Generic;

    using RestSharp;

    using SharpZendeskApi.Core.Exceptions;
    using SharpZendeskApi.Core.Models;

    public abstract class ManagerBase<TModel, TInterface> : IManager<TInterface>
        where TInterface : IZendeskThing
        where TModel : TrackableZendeskThingBase, TInterface
    {
        #region Constructors and Destructors

        protected ManagerBase(IZendeskClient client)
        {
            this.Client = client;
            this.Cache = new Dictionary<int, TInterface>();
        }

        #endregion

        #region Public Properties

        public Dictionary<int, TInterface> Cache { get; set; }

        public IZendeskClient Client { get; set; }

        #endregion

        #region Public Methods and Operators

        public virtual bool Exists(int id)
        {
            try
            {
                return this.Get(id) != null;
            }
            catch (NotFoundException)
            {
                return false;
            }
        }

        public abstract TInterface Get(int id, bool force = false);

        public abstract IListing<TInterface> GetMany(IEnumerable<int> ids);

        public virtual void RefreshCache()
        {
            var items = this.GetMany(this.Cache.Keys);
            foreach (var item in items)
            {
                var itemAsT = item as TModel;

                // ReSharper disable once PossibleNullReferenceException
                // T will always implement TInferface due to generic constraints
                if (itemAsT.Id != null)
                {
                    this.Cache[itemAsT.Id.Value] = item;
                }
            }
        }

        public abstract TInterface SubmitNew(TInterface obj);

        public abstract void SubmitUpdatesFor(TInterface obj);

        public virtual bool TryGet(int id, out TInterface value)
        {
            var exists = this.Exists(id);

            value = exists ? this.Cache[id] : default(TInterface);

            return exists;
        }

        #endregion

        #region Methods

        protected TInterface Get(string url, int id, bool force)
        {
            if (!force && this.Cache.ContainsKey(id))
            {
                return this.Cache[id];
            }

            var request = new RestRequest(url, Method.GET)
                              {
                                  RootElement = typeof(TModel).GetTypeNameAsCPlusPlusStyle()
                              };

            var response = this.Client.Execute<TModel>(request);

            response.ThrowIfProblem();

            this.Cache[id] = response.Data;

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

            return new Listing<TInterface>(this.Client, request);
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

            if (response.Data.Id != null)
            {
                this.Cache[response.Data.Id.Value] = response.Data;
            }
            else
            {
                throw new SharpZendeskException("Response had no ID!");
            }

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

            this.Cache[obj.Id.Value] = response.Data;
        }

        #endregion
    }
}