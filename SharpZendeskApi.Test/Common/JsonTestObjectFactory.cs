namespace SharpZendeskApi.Test.Common
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Ploeh.AutoFixture;

    using RestSharp;

    using SharpZendeskApi.Models;
    using SharpZendeskApi.Test.Common.JsonObjects;

    /// <summary>
    /// The json test object factory.
    ///     Using the Singleton Pattern as described here:
    ///     http://msdn.microsoft.com/en-us/library/ff650316.aspx
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Urls will not always abide by this rule.")]
    public sealed class JsonTestObjectFactory : GenericFactory<JsonTestObjectBase>
    {
        #region Static Fields

        /// <summary>
        ///     The sync root.
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        ///     The instance.
        /// </summary>
        private static volatile JsonTestObjectFactory instance;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="JsonTestObjectFactory" /> class from being created.
        /// </summary>
        private JsonTestObjectFactory()
        {
            // easy way to populate this with your test classes in PowerShell
            // cd .\SharpZendeskApi\SharpZendeskApi.Core.Test\Common\JsonObjects
            // Get-ChildItem | ?{$_.Name -like "*Json.cs"} | %{Write-Host "this.Register<$($_.BaseName)>();"}
            this.Register<AttachmentJson>();
            this.Register<ConditionJson>();
            this.Register<ConditionsJson>();
            this.Register<CustomFieldJson>();
            this.Register<ExecutionJson>();
            this.Register<GroupSortJson>();
            this.Register<RestrictionJson>();
            this.Register<SatisfactionRatingJson>();
            this.Register<ThumbnailJson>();
            this.Register<TicketCommentJson>();
            this.Register<TicketFieldJson>();
            this.Register<TicketJson>();
            this.Register<UserJson>();
            this.Register<ViaJson>();
            this.Register<ViewJson>();
            this.Register<ViewOutputJson>();

            // pages
            this.Register<PageJson<TicketJson, Ticket>>();
            this.Register<PageJson<UserJson, User>>();
            this.Register<PageJson<ViewJson, View>>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static JsonTestObjectFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new JsonTestObjectFactory();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get test object.
        /// </summary>
        /// <param name="fixture">
        /// The fixture.
        /// </param>
        /// <typeparam name="T">
        /// The derived type to create.
        /// </typeparam>
        /// <returns>
        /// The <see cref="JsonObject"/>.
        /// </returns>
        public JsonObject Create<T>(IFixture fixture) where T : JsonTestObjectBase
        {
            return this.Create<T>().GetJsonObject(fixture);
        }

        /// <summary>
        /// The create many.
        /// </summary>
        /// <param name="fixture">
        /// The fixture.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <typeparam name="T">
        /// The derived type to create.
        /// </typeparam>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<JsonObject> CreateMany<T>(IFixture fixture, int count = 3) where T : JsonTestObjectBase
        {
            var theMany = new List<JsonObject>();
            for (int i = 0; i < count; i++)
            {
                theMany.Add(this.Create<T>().GetJsonObject(fixture));
            }

            return theMany;
        }

        #endregion
    }
}