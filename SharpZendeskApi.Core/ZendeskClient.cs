namespace SharpZendeskApi.Core
{
    using System;

    using RestSharp;

    using TinyIoC;

    /// <summary>
    /// The sharp zendesk api client.
    /// </summary>
    public sealed class ZendeskClient : RestClient, IZendeskClient
    {        
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ZendeskClient"/> class.
        /// </summary>
        /// <param name="domain">
        /// The base url.
        /// </param>
        /// <param name="emailAddress">
        /// The email address.
        /// </param>
        /// <param name="passwordOrToken">
        /// The password or token.
        /// </param>
        /// <param name="authenticationMethod">
        /// The authentication method.
        /// </param>
        public ZendeskClient(
            string domain,
            string emailAddress,
            string passwordOrToken,
            ZendeskAuthenticationMethod authenticationMethod)
        {            
            if (authenticationMethod == ZendeskAuthenticationMethod.Token)
            {
                // per http://developer.zendesk.com/documentation/rest_api/introduction.html
                // If using the API token, use the following authentication format:
                // {email_address}/token:{api_token}
                emailAddress += "/token";
            }

            var domainUri = new Uri(domain);
            var apiUri = new Uri(domainUri, "api/v2");

            this.UserAgent = "SharpZendeskApi";
            this.BaseUrl = apiUri.AbsoluteUri;
            this.Authenticator = new HttpBasicAuthenticator(emailAddress, passwordOrToken);

            this.Container = new TinyIoCContainer();

            // register default serializers
            this.Container.Register<IZendeskSerializer, CreationSerializer>(SerializationScenario.Create.ToString());
            this.Container.Register<IZendeskSerializer, UpdatingSerializer>(SerializationScenario.Update.ToString());
        }

        #endregion

        #region Public Properties

        public TinyIoCContainer Container { get; set; }

        #endregion
    }
}