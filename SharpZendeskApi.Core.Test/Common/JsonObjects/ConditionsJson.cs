namespace SharpZendeskApi.Core.Test.Common.JsonObjects
{
    using Ploeh.AutoFixture;

    using RestSharp;

    using SharpZendeskApi.Core.Models;

    /// <summary>
    /// The conditions json.
    /// </summary>
    public class ConditionsJson : JsonTestObjectBase
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
                           { "all", JsonTestObjectFactory.Instance.CreateMany<ConditionJson>(fixture) }, 
                           { "any", JsonTestObjectFactory.Instance.CreateMany<ConditionJson>(fixture) }
                       };
        }

        #endregion
    }
}