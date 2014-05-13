using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpZendeskApi.Test.Common.JsonObjects
{
    using Ploeh.AutoFixture;

    using RestSharp;

    public class ViewOutputJson : JsonTestObjectBase
    {
        public override JsonObject GetJsonObject(IFixture fixture)
        {
            return new JsonObject { { "columns", fixture.CreateMany<string>() } };
        }
    }
}
