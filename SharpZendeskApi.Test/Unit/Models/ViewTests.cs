namespace SharpZendeskApi.Test.Unit.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SharpZendeskApi.Models;
    using SharpZendeskApi.Test.Common.JsonObjects;

    public class ViewTests : ModelTestBase<ViewJson, View, IView>
    {
        public override void CanCreateWithFilledMandatoryPropertiesUsingConstructor()
        {
            throw new NotImplementedException();
        }
    }
}
