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
            return this.InjectMock<TDependencyToMock>(MockBehavior.Default);
        }

        public Mock<TDependencyToMock> InjectMock<TDependencyToMock>(params object[] args)
            where TDependencyToMock : class
        {
            return this.InjectMock<TDependencyToMock>(MockBehavior.Default, args);
        }

        public Mock<TDependencyToMock> InjectMock<TDependencyToMock>(MockBehavior behavior, params object[] args)
            where TDependencyToMock : class
        {
            var a = new Mock<TDependencyToMock>(behavior, args);
            this.Fixture.Inject(a.Object);
            return a;
        }

        #endregion
    }
}