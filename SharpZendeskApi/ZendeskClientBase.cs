namespace SharpZendeskApi
{
    using System;

    using Microsoft.Practices.Unity;

    using RestSharp;

    using SharpZendeskApi.Models;

    public abstract class ZendeskClientBase : RestClient
    {
        protected ZendeskClientBase()
        {
            this.RequestHandler = new RequestHandler(this);
        }

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

        internal virtual IRequestHandler RequestHandler { get; private set; }
    }
}