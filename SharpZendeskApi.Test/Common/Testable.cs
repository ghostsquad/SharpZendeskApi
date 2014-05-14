namespace SharpZendeskApi.Test.Common
{
    using System;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    public class Testable<TClassUnderTest>
        where TClassUnderTest : class
    {
        #region Constructors and Destructors

        public Testable()
        {
            this.Fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        public Testable(Action<Testable<TClassUnderTest>> setup)
            : this()
        {
            setup(this);
        }

        #endregion

        #region Public Properties

        public TClassUnderTest ClassUnderTest
        {
            get
            {
                return this.Fixture.Create<TClassUnderTest>();
            }
        }

        public IFixture Fixture { get; private set; }

        #endregion

        #region Public Methods and Operators

        public Mock<TDependencyToMock> InjectMock<TDependencyToMock>() where TDependencyToMock : class
        {
            var a = new Mock<TDependencyToMock>();                        
            this.Fixture.Inject(a.Object);
            return a;
        }

        #endregion
    }
}