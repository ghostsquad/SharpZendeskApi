namespace SharpZendeskApi.Core.Test.Common.JsonObjects
{
    using Ploeh.AutoFixture;

    using RestSharp;

    public class RestrictionJson : JsonTestObjectBase
    {
        #region Public Methods and Operators

        public override JsonObject GetJsonObject(IFixture fixture)
        {
            return new JsonObject { { "id", fixture.Create<int>() }, { "type", fixture.Create<string>() } };
        }

        #endregion
    }
}