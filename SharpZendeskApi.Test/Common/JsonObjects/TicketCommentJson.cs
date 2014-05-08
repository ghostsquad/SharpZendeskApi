namespace SharpZendeskApi.Test.Common.JsonObjects
{
    using Ploeh.AutoFixture;

    using RestSharp;

    /// <summary>
    ///     The ticket comment json.
    /// </summary>
    public class TicketCommentJson : JsonTestObjectBase
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get json object.
        /// </summary>
        /// <param name="fixture">
        /// The fixture.
        /// </param>
        /// <returns>
        /// The <see cref="JsonObject"/>.
        /// </returns>
        public override JsonObject GetJsonObject(IFixture fixture)
        {
            return new JsonObject
                       {
                           {
                               "attachments", JsonTestObjectFactory.Instance.CreateMany<AttachmentJson>(fixture)
                           },
                           { "author_id", fixture.Create<int>() }, 
                           { "body", fixture.Create<string>() }, 
                           { "html_body", fixture.Create<string>() }, 
                           { "id", fixture.Create<int>() }, 
                           { "public", fixture.Create<bool>() },
                           { "type", fixture.Create<string>() }
                       };
        }

        #endregion
    }
}