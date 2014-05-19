namespace SharpZendeskApi.Test.Common.JsonObjects
{
    using Ploeh.AutoFixture;

    using RestSharp;

    public class ViewColumnJson : JsonTestObjectBase
    {
        public override JsonObject GetJsonObject(IFixture fixture)
        {
            return new JsonObject
                       {
                           { "id", fixture.Create<string>() },
                           { "title", fixture.Create<string>() }
                       };
        }
    }
}
