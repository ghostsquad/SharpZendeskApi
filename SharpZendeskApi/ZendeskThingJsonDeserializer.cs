namespace SharpZendeskApi
{
    using System.Reflection;

    using Microsoft.Practices.Unity;

    using RestSharp.Deserializers;

    using SharpZendeskApi.Models;

    /// <summary>
    /// The zendesk thing json deserializer.
    /// </summary>
    internal class ZendeskThingJsonDeserializer : JsonDeserializer
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ZendeskThingJsonDeserializer"/> class.
        /// </summary>
        public ZendeskThingJsonDeserializer()
        {
            this.Container = new UnityContainer();

            // trackable objects
            this.Container.RegisterType<ITicket, Ticket>();
            this.Container.RegisterType<ISatisfactionRating, SatisfactionRating>();
            this.Container.RegisterType<IAttachment, Attachment>();
            this.Container.RegisterType<ISatisfactionRating, SatisfactionRating>();
            this.Container.RegisterType<IThumbnail, Thumbnail>();
            this.Container.RegisterType<ITicketComment, TicketComment>();
            this.Container.RegisterType<ITicketField, TicketField>();
            this.Container.RegisterType<IUser, User>();
            this.Container.RegisterType<IView, View>();

            // pages
            this.Container.RegisterType<IPage<ITicket>, TicketsPage>();
            this.Container.RegisterType<IPage<IUser>, UserPage>();
            this.Container.RegisterType<IPage<IView>, ViewPage>();

            this.DeserializationResolver = x => this.Container.Resolve(x);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the container.
        /// </summary>
        public IUnityContainer Container { get; private set; }

        #endregion
    }
}