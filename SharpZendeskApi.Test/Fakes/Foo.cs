namespace SharpZendeskApi.Test.Fakes
{   
    using SharpZendeskApi.Models;

    public interface IFoo : ITrackable
    {
    }

    public class Foo : TrackableZendeskThingBase, IFoo
    {
        public Foo()
        {
        }
    }
}