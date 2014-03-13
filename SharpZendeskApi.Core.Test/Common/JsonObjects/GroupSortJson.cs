namespace SharpZendeskApi.Core.Test.Common.JsonObjects
{
    using Ploeh.AutoFixture;

    using RestSharp;

    /// <summary>
    /// The group sort json.
    /// </summary>
    public class GroupSortJson : JsonTestObjectBase
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
                           { "id", fixture.Create<string>() }, 
                           { "order", fixture.Create<string>() }, 
                           { "title", fixture.Create<string>() }
                       };
        }

        #endregion
    }
}