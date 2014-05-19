namespace SharpZendeskApi
{
    using System;

    using RestSharp;

    using SharpZendeskApi.Models;

    public abstract class ZendeskClientBase
    {
        #region Constructors and Destructors

        protected ZendeskClientBase()
        {
            this.RestClient = new RestClient();
        }

        #endregion

        #region Properties

        internal virtual IRequestHandler RequestHandler { get; set; }

        protected RestClient RestClient { get; private set; }

        #endregion

        #region Methods

        internal virtual IZendeskSerializer GetSerializer(SerializationScenario scenario)
        {
            switch (scenario)
            {
                case SerializationScenario.Create:
                    {
                        return new CreationSerializer();
                    }

                case SerializationScenario.Update:
                    {
                        return new UpdatingSerializer();
                    }

                default:
                    {
                        throw new ArgumentException("Unknown Scenario: " + scenario);
                    }
            }
        }

        internal virtual IListing<TInterface> GetListing<TModel, TInterface>(IRestRequest request)
            where TInterface : class, ITrackable
            where TModel : TrackableZendeskThingBase, TInterface
        {
            return new Listing<TModel, TInterface>(this, request);
        }

        #endregion
    }
}