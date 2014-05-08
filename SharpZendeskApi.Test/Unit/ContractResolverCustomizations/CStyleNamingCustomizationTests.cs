namespace SharpZendeskApi.Test.Unit.ContractResolverCustomizations
{
    using FluentAssertions;

    using Newtonsoft.Json.Serialization;

    using SharpZendeskApi.ContractResolution;

    using Xunit.Extensions;

    public class CStyleNamingCustomizationTests
    {
        [Theory]
        [InlineData("a", "a")]
        [InlineData("aB", "a_b")]
        [InlineData("AB", "ab")]
        [InlineData("ab", "ab")]
        [InlineData("Ab", "ab")]
        [InlineData("a_b", "a_b")]
        [InlineData("A_B", "a_b")]
        [InlineData("a_B", "a_b")]
        [InlineData("A_b", "a_b")]
        [InlineData("aBdC", "a_bd_c")]
        public void CanConvertToCStyleNaming(string actualName, string expectedString)
        {
            // arrange
            var jsonProperty = new JsonProperty { PropertyName = actualName };

            // act
            CStyleNamingCustomization.Default.Modification(jsonProperty);

            // assert
            jsonProperty.PropertyName.Should().Be(expectedString);
        }        
    }
}
