namespace SharpZendeskApi.Core.Test.Unit.ContractResolverCustomizations
{
    using System;

    using FluentAssertions;

    using Newtonsoft.Json;

    using SharpZendeskApi.Core.ContractResolution;
    using SharpZendeskApi.Core.Models.Attributes;
    using SharpZendeskApi.Core.Test.Fakes;

    using Xunit;

    public class CustomizableSerializationContractResolverTests
    {
        private readonly JsonSerializerSettings settings = new JsonSerializerSettings();
        private readonly PascalTestClass testObject = new PascalTestClass();

        [Fact]
        public void GivenNullCustomizationExpectArgumentNullException()
        {
            // act and assert
            Assert.Throws<ArgumentNullException>(() => new CustomizableSerializationContractResolver().Customize(null));
        }

        [Fact]
        public void GivenNullPredicateAndModificationExpectNoChange()
        {
            // arrange
            var contractResolver = new CustomizableSerializationContractResolver().Customize(new NullCustomization());
            this.settings.ContractResolver = contractResolver;
            const string ExpectedJson = "{\"P1\":0,\"P2\":0,\"P3\":0}";

            // act
            var actualJson = JsonConvert.SerializeObject(this.testObject, this.settings);

            // assert
            actualJson.Should().Be(ExpectedJson);
        }        

        [Fact]
        public void Include1sPredicate_ExpectOnlyP1Serialized()
        {
            // arrange
            var contractResolver = new CustomizableSerializationContractResolver().Customize(new IncludeOnly1sCustomization());
            this.settings.ContractResolver = contractResolver;
            const string ExpectedJson = "{\"P1\":0}";

            // act
            var actualJson = JsonConvert.SerializeObject(this.testObject, this.settings);

            // assert
            actualJson.Should().Be(ExpectedJson, "because only P1 ends in 1.");
        }

        [Fact]
        public void Exclude1sPredicate_ExpectP1NotSerialized()
        {
            // arrange
            var contractResolver = new CustomizableSerializationContractResolver().Customize(new IncludeOnly1sCustomization());
            this.settings.ContractResolver = contractResolver;
            const string ExpectedJson = "{\"P1\":0}";

            // act
            var actualJson = JsonConvert.SerializeObject(this.testObject, this.settings);

            // assert
            actualJson.Should().Be(ExpectedJson, "because only P1 ends in 1.");
        }

        [Fact]
        public void UnderscorePrepender_ExpectUnderscorePrended()
        {
            // arrange
            var contractResolver = new CustomizableSerializationContractResolver().Customize(new UnderScorePrependerCustomization());
            this.settings.ContractResolver = contractResolver;
            const string ExpectedJson = "{\"_P1\":0,\"_P2\":0,\"_P3\":0}";

            // act
            var actualJson = JsonConvert.SerializeObject(this.testObject, this.settings);

            // assert
            actualJson.Should().Be(ExpectedJson, "because the modification is to prepend an underscore to each property name.");
        }

        [Fact]
        public void ExcludePropertyWithReadOnlyAttribute_WhenP3WithReadOnlyAttribute_ExpectP3NotSerialized()
        {
            // arrange
            var contractResolver = new CustomizableSerializationContractResolver().Customize(new ExcludeAttributeCustomization());
            this.settings.ContractResolver = contractResolver;
            const string ExpectedJson = "{\"P1\":0,\"P2\":0}";

            // act
            var actualJson = JsonConvert.SerializeObject(this.testObject, this.settings);

            // assert
            actualJson.Should().Be(ExpectedJson, "because P3 is the only property with the ReadOnly attribute.");
        }

        [Fact]
        public void IncludePropertyWithReadOnlyAttribute_WhenOnlyOnlyP3WithReadOnlyAttribute_ExpectOnlyP3Serialized()
        {
            // arrange
            var contractResolver = new CustomizableSerializationContractResolver().Customize(new IncludeAttributeCustomization());
            this.settings.ContractResolver = contractResolver;
            const string ExpectedJson = "{\"P3\":0}";

            // act
            var actualJson = JsonConvert.SerializeObject(this.testObject, this.settings);

            // assert
            actualJson.Should().Be(ExpectedJson, "because P3 is the only property with the ReadOnly attribute.");
        }

        [Fact]
        public void ExcludeIfEndsIn1Customization_WhenP1_ExpectP1NotSerialized()
        {
            // arrange
            var contractResolver = new CustomizableSerializationContractResolver().Customize(new ExcludeIfEndsIn1Customization());
            this.settings.ContractResolver = contractResolver;
            const string ExpectedJson = "{\"P2\":0,\"P3\":0}";

            // act
            var actualJson = JsonConvert.SerializeObject(this.testObject, this.settings);

            // assert
            actualJson.Should().Be(ExpectedJson, "because only P1 ends in 1.");
        }

        private class PascalTestClass
        {
            public int P1 { get; set; }

            public int P2 { get; set; }

            [ReadOnly]
            public int P3 { get; set; }
        }        
    }
}
