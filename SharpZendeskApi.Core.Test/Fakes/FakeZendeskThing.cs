using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpZendeskApi.Core.Test.Fakes
{
    using SharpZendeskApi.Core.Models;

    public class FakeZendeskThing : TrackableZendeskThingBase
    {
        public int Foo { get; set; }
    }
}
