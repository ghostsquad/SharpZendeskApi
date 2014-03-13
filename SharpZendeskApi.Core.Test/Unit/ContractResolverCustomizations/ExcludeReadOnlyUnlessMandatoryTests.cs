namespace SharpZendeskApi.Core.Test.Unit.ContractResolverCustomizations
{
    using FluentAssertions;

    using SharpZendeskApi.Core.ContractResolution;
    using SharpZendeskApi.Core.Test.Fakes;

    using Xunit;

    public class ExcludeReadOnlyUnlessMandatoryTests
    {
        [Fact]
        public void GivenReadOnlyMandatoryPropertyExpectFalse()
        {
            // arrange
            var property = typeof(ReadWriteMandatory).GetProperty("ReadMandatory");

            // act
            var actualResult = ExcludeReadOnlyUnlessMandatoryCustomization.Default.ExcludeMemberInfoPredicate(property);

            // assert
            actualResult.Should().BeFalse("because the property has ReadOnly and Mandatory attributes");
        }

        [Fact]
        public void GivenReadOnlyPropertyExpectTrue()
        {
            // arrange
            var property = typeof(ReadWriteMandatory).GetProperty("Read");

            // act
            var actualResult = ExcludeReadOnlyUnlessMandatoryCustomization.Default.ExcludeMemberInfoPredicate(property);

            // assert
            actualResult.Should().BeTrue("because the property only has the ReadOnly attribute");
        }

        [Fact]
        public void GivenNoAttributesExpectFalse()
        {
            // arrange
            var property = typeof(ReadWriteMandatory).GetProperty("ReadWrite");

            // act
            var actualResult = ExcludeReadOnlyUnlessMandatoryCustomization.Default.ExcludeMemberInfoPredicate(property);

            // assert
            actualResult.Should().BeFalse("because the property has no attributes");
        }

        [Fact]
        public void GivenOnlyMandatoryPropertyExpectFalse()
        {
            // arrange
            var property = typeof(ReadWriteMandatory).GetProperty("Mandatory");

            // act
            var actualResult = ExcludeReadOnlyUnlessMandatoryCustomization.Default.ExcludeMemberInfoPredicate(property);

            // assert
            actualResult.Should().BeFalse("because the property only has the Mandatory attribute");
        }
    }
}
