namespace SharpZendeskApi.Test.Common.JsonObjects
{
    using Ploeh.AutoFixture;

    using RestSharp;

    /// <summary>
    /// The execution json.
    /// </summary>
    public class ExecutionJson : JsonTestObjectBase
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
                           { "columns", JsonTestObjectFactory.Instance.CreateMany<ViewColumnJson>(fixture) },
                           { "group", JsonTestObjectFactory.Instance.Create<GroupSortJson>(fixture) },
                           { "sort", JsonTestObjectFactory.Instance.Create<GroupSortJson>(fixture) }
                       };
        }

        #endregion
    }
}