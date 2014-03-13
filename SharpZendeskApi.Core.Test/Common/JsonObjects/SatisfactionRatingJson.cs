namespace SharpZendeskApi.Core.Test.Common.JsonObjects
{
    using System;

    using Ploeh.AutoFixture;

    using RestSharp;

    /// <summary>
    ///     The satisfaction rating json.
    /// </summary>
    public class SatisfactionRatingJson : JsonTestObjectBase
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
                           { "assignee_id", fixture.Create<int>() }, 
                           { "comment", fixture.Create<string>() }, 
                           { "created_at", fixture.Create<DateTime>().ToUtcIso8601() }, 
                           { "group_id", fixture.Create<int>() }, 
                           { "id", fixture.Create<int>() }, 
                           { "requester_id", fixture.Create<int>() }, 
                           { "score", fixture.Create<string>() }, 
                           { "ticket_id", fixture.Create<int>() }, 
                           { "updated_at", fixture.Create<DateTime>().ToUtcIso8601() }, 
                           { "url", fixture.Create<string>() }
                       };
        }

        #endregion
    }
}