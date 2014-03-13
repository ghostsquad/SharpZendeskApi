namespace SharpZendeskApi.Core.Test.Common.JsonObjects
{
    using Ploeh.AutoFixture;

    using RestSharp;

    /// <summary>
    /// The attachment json.
    /// </summary>
    public class AttachmentJson : JsonTestObjectBase
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
                           { "id", fixture.Create<int>() }, 
                           { "name", fixture.Create<string>() }, 
                           { "content_url", fixture.Create<string>() }, 
                           { "content_type", fixture.Create<string>() }, 
                           { "thumbnails", JsonTestObjectFactory.Instance.CreateMany<ThumbnailJson>(fixture) }, 
                           { "size", fixture.Create<int>() }
                       };
        }

        #endregion
    }
}