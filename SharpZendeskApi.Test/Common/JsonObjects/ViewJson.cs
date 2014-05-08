namespace SharpZendeskApi.Test.Common.JsonObjects
{
    using System;

    using Ploeh.AutoFixture;

    using RestSharp;

    /// <summary>
    /// The view json.
    /// </summary>
    public class ViewJson : JsonTestObjectBase
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
                           { "active", fixture.Create<bool>() }, 
                           { "conditions", JsonTestObjectFactory.Instance.Create<ConditionsJson>(fixture) }, 
                           { "created_at", fixture.Create<DateTime>().ToUtcIso8601() }, 
                           { "execution", JsonTestObjectFactory.Instance.Create<ExecutionJson>(fixture) }, 
                           { "id", fixture.Create<int>() }, 
                           { "restriction", JsonTestObjectFactory.Instance.Create<RestrictionJson>(fixture) }, 
                           { "sla_id", fixture.Create<int>() }, 
                           { "title", fixture.Create<string>() }, 
                           { "updated_at", fixture.Create<DateTime>().ToUtcIso8601() }
                       };
        }

        #endregion
    }
}