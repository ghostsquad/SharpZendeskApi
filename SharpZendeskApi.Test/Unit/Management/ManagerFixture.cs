namespace SharpZendeskApi.Test.Unit.Management
{
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using SharpZendeskApi.Models;

    public class ManagerFixture<TModel>
        where TModel : TrackableZendeskThingBase
    {
        #region Constructors and Destructors

        public ManagerFixture()
        {
            this.Fixture = new Fixture().Customize(new AutoMoqCustomization());
            this.Model = this.Fixture.Create<TModel>();
        }

        #endregion

        #region Public Properties

        public IFixture Fixture { get; private set; }

        public TModel Model { get; private set; }

        #endregion
    }
}