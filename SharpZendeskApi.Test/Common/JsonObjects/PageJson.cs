namespace SharpZendeskApi.Test.Common.JsonObjects
{
    using Ploeh.AutoFixture;

    using RestSharp;

    using SharpZendeskApi.Models;

    public class PageJson<T, TModel> : JsonTestObjectBase
        where T : JsonTestObjectBase where TModel : IZendeskThing
    {
        #region Public Methods and Operators

        public override JsonObject GetJsonObject(IFixture fixture)
        {
            return new JsonObject
                       {
                           {
                               typeof(TModel).GetTypeNameAsCPlusPlusStyle().Pluralize(), 
                               JsonTestObjectFactory.Instance.CreateMany<T>(fixture)
                           }, 
                           { "count", fixture.Create<int>() }, 
                           { "next_page", fixture.Create<string>() }, 
                           { "previous_page", fixture.Create<string>() }
                       };
        }

        #endregion
    }
}