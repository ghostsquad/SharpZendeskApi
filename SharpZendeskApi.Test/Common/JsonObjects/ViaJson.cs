namespace SharpZendeskApi.Test.Common.JsonObjects
{
    using Ploeh.AutoFixture;

    using RestSharp;

    public class ViaJson : JsonTestObjectBase
    {
        public override JsonObject GetJsonObject(IFixture fixture)
        {
            return new JsonObject();
        }
    }
}
