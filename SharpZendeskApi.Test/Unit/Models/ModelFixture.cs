namespace SharpZendeskApi.Test.Unit.Models
{
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using Newtonsoft.Json;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using RestSharp;

    using SharpZendeskApi.Models;
    using SharpZendeskApi.Test.Common;
    using SharpZendeskApi.Test.Common.JsonObjects;

    using JsonSerializer = RestSharp.Serializers.JsonSerializer;

    public class ModelFixture<TJson, TModel>
        where TJson : JsonTestObjectBase where TModel : IZendeskThing, new()
    {
        #region Constructors and Destructors
        
        public ModelFixture()
        {
            this.Properties =
                typeof(TModel).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(m => !m.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Any())
                    .OrderBy(x => x.Name);
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