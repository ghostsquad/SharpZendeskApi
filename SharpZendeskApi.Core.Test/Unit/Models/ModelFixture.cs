namespace SharpZendeskApi.Core.Test.Unit.Models
{
    using System.Linq;
    using System.Reflection;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using RestSharp;
    using RestSharp.Serializers;

    using SharpZendeskApi.Core.Models;
    using SharpZendeskApi.Core.Test.Common;
    using SharpZendeskApi.Core.Test.Common.JsonObjects;

    public class ModelFixture<TJson, TModel>
        where TJson : JsonTestObjectBase where TModel : IZendeskThing, new()
    {
        #region Constructors and Destructors

        public ModelFixture()
        {
            this.Properties =
                typeof(TModel).GetProperties(BindingFlags.Instance | BindingFlags.Public).OrderBy(x => x.Name);
            this.Fixture = new Fixture().Customize(new AutoMoqCustomization());
            var jsonSerializer = new JsonSerializer();
            this.JsonTestObject = JsonTestObjectFactory.Instance.Create<TJson>(this.Fixture);
            this.SerializedJsonObject = jsonSerializer.Serialize(this.JsonTestObject);

            this.JsonTestPage = JsonTestObjectFactory.Instance.Create<PageJson<TJson, TModel>>(this.Fixture);
            this.SerializedPage = jsonSerializer.Serialize(this.JsonTestPage);
        }

        #endregion

        #region Public Properties

        public IFixture Fixture { get; set; }

        public JsonObject JsonTestObject { get; set; }

        public JsonObject JsonTestPage { get; set; }

        public IOrderedEnumerable<PropertyInfo> Properties { get; set; }

        public string SerializedJsonObject { get; set; }

        public string SerializedPage { get; set; }

        #endregion
    }
}