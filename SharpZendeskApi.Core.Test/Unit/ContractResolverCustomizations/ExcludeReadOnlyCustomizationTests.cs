namespace SharpZendeskApi.Core.Test.Unit.ContractResolverCustomizations
{
    using FluentAssertions;

    using SharpZendeskApi.Core.ContractResolution;
    using SharpZendeskApi.Core.Test.Fakes;

    using Xunit;

    public class ExcludeReadOnlyCustomizationTests
    {
        [Fact]
        public void GivenPropertyWithOutReadOnlyAttributeExpectFalse()
        {
            // arrange
            var property = typeof(ReadWriteMandatory).GetProperty("ReadWrite");

            // act
            var actualResult = ExcludeReadOnlyCustomization.Default.ExcludeMemberInfoPredicate(property);

            // assert
            actualResult.Should().BeFalse("because the property does not have the ReadOnly attribute");
        }

        [Fact]
        public void GivenPropertyWithReadOnlyAttributeExpectTrue()
        {
            // arrange
            var property = typeof(ReadWriteMandatory).GetProperty("Read");

            // act
            var actualResult = ExcludeReadOnlyCustomization.Default.ExcludeMemberInfoPredicate(property);

            // assert
            actualResult.Should().BeTrue("because the property has the ReadOnly attribute");
        }            
    }
}
