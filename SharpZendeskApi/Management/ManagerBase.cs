namespace SharpZendeskApi.Management
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.Unity;

    using RestSharp;

    using SharpZendeskApi.Models;

    public abstract class ManagerBase<TModel, TInterface> : IManager<TInterface>
        where TInterface : class, IZendeskThing, ITrackable
        where TModel : TrackableZendeskThingBase, TInterface
    {
        #region Constructors and Destructors               

        protected ManagerBase(ZendeskClientBase client)
        {
            this.Client = client;
            this.PluralizedModelName = typeof(TModel).GetTypeNameAsCPlusPlusStyle().Pluralize();
        }

        #endregion

        #region Public Properties

        public ZendeskClientBase Client { get; set; }

        #endregion

        #region Properties

        protected string PluralizedModelName { get; private set; }

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

            var obj = this.Client.RequestHandler.MakeRequest<TInterface>(request);
            (obj as TModel).WasSubmitted = true;

            return obj;
        }

        protected virtual IListing<TInterface> GetMany(string url)
        {
            // http://developer.zendesk.com/documentation/rest_api/tickets.html#show-multiple-tickets
            var request = new RestRequest(url, Method.GET)
                              {
                                  RootElement = typeof(TModel).GetTypeNameAsCPlusPlusStyle().Pluralize()
                              };

            return this.Client.GetListing<TModel, TInterface>(request);
        }

        protected TInterface SubmitNew(string url, TInterface obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            obj.ThrowIfSubmitted();
            obj.ThrowIfAnyMandatoryPropertyIsNull();

            var objAsModel = obj as TModel;

            var serializer = this.Client.GetSerializer(SerializationScenario.Create);
            var jsonBody = serializer.Serialize(objAsModel);

            var request = new RestRequest(url, Method.POST)
                              {
                                  RequestFormat = DataFormat.Json,
                                  RootElement = typeof(TModel).GetTypeNameAsCPlusPlusStyle()
                              };

            request.AddBody(jsonBody);

            var submittedObject = this.Client.RequestHandler.MakeRequest<TModel>(request);
            submittedObject.WasSubmitted = true;

            return submittedObject;
        }

        protected void SubmitUpdatesFor(string url, TInterface obj)
        {
            obj.ThrowIfNotSubmitted();

            if (!obj.ChangedProperties.Any())
            {
                return;
            }

            var objAsModel = obj as TModel;

            var serializer = this.Client.GetSerializer(SerializationScenario.Update);
            var jsonBody = serializer.Serialize(objAsModel);

            var request = new RestRequest(url, Method.PUT)
                              {
                                  RequestFormat = DataFormat.Json,
                                  RootElement = typeof(TModel).GetTypeNameAsCPlusPlusStyle()
                              };

            request.AddBody(jsonBody);
            this.Client.RequestHandler.MakeRequest<TInterface>(request);
        }

        #endregion
    }
}