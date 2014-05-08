namespace SharpZendeskApi.Test.Common.JsonObjects
{
    using Ploeh.AutoFixture;

    using RestSharp;

    /// <summary>
    /// The thumbnail json.
    /// </summary>
    public class ThumbnailJson : JsonTestObjectBase
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
                           { "size", fixture.Create<int>() }
                       };
        }

        #endregion
    }
}