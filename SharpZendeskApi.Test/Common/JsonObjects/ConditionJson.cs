namespace SharpZendeskApi.Test.Common.JsonObjects
{
    using Ploeh.AutoFixture;

    using RestSharp;

    /// <summary>
    /// The condition json.
    /// </summary>
    public class ConditionJson : JsonTestObjectBase
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
                           { "field", fixture.Create<string>() },
                           { "operator", fixture.Create<string>() },
                           { "value", fixture.Create<string>() }
                       };
        }

        #endregion
    }
}