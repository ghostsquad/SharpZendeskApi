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
        private JsonSerializerSettings settings = new JsonSerializerSettings();
        private PascalTestClass testObject = new PascalTestClass();

        [Fact]
        public void GivenNullCustomizationExpectArgumentNullException()
        {
            // arrange
            IContractResolverCustomization nullCustomization = null;            

            // act and assert
            Assert.Throws<ArgumentNullException>(() => new CustomizableSerializationContractResolver(nullCustomization));
        }

        [Fact]
        public void GivenNullPredicateAndModificationExpectNoChange()
        {
            // arrange                       
            var contractResolver = new CustomizableSerializationContractResolver(new NullCustomization());
            this.settings.ContractResolver = contractResolver;            
            const string ExpectedJson = "{\"P1\":0,\"P2\":0,\"P3\":0}";

            // act
            var actualJson = JsonConvert.SerializeObject(this.testObject, this.settings);

            // assert
            actualJson.Should().Be(ExpectedJson);
        }

        [Fact]
        public void GivenInclude1sPredicateExpectOnlyP1Serialized()
        {
            // arrange            
            var contractResolver = new CustomizableSerializationContractResolver(new IncludeOnly1sCustomization());
            this.settings.ContractResolver = contractResolver;            
            const string ExpectedJson = "{\"P1\":0}";

            // act
            var actualJson = JsonConvert.SerializeObject(this.testObject, this.settings);

            // assert
            actualJson.Should().Be(ExpectedJson, "because only P1 ends in 1.");
        }

        [Fact]
        public void GivenExclude1sPredicateExpectP1NotSerialized()
        {
            // arrange            
            var contractResolver = new CustomizableSerializationContractResolver(new IncludeOnly1sCustomization());
            this.settings.ContractResolver = contractResolver;            
            const string ExpectedJson = "{\"P1\":0}";

            // act
            var actualJson = JsonConvert.SerializeObject(this.testObject, this.settings);

            // assert
            actualJson.Should().Be(ExpectedJson, "because only P1 ends in 1.");
        }

        [Fact]
        public void GivenUnderscorePrependerExpectUnderscorePrended()
        {
            // arrange            
            var contractResolver = new CustomizableSerializationContractResolver(new UnderScorePrependerCustomization());
            this.settings.ContractResolver = contractResolver;            
            const string ExpectedJson = "{\"_P1\":0,\"_P2\":0,\"_P3\":0}";

            // act
            var actualJson = JsonConvert.SerializeObject(this.testObject, this.settings);

            // assert
            actualJson.Should().Be(ExpectedJson, "because the modification is to prepend an underscore to each property name.");
        }

        [Fact]
        public void GivenExcludePropertyWithReadOnlyAttributeExpectP3NotSerialized()
        {
            // arrange
            var contractResolver = new CustomizableSerializationContractResolver(new ExcludeAttributeCustomization());
            this.settings.ContractResolver = contractResolver;
            const string ExpectedJson = "{\"P1\":0,\"P2\":0}";

            // act
            var actualJson = JsonConvert.SerializeObject(this.testObject, this.settings);

            // assert
            actualJson.Should().Be(ExpectedJson, "because P3 is the only property with the ReadOnly attribute.");
        }

        [Fact]
        public void GivenIncludePropertyWithReadOnlyAttributeExpectOnlyP3Serialized()
        {
            // arrange
            var contractResolver = new CustomizableSerializationContractResolver(new IncludeAttributeCustomization());
            this.settings.ContractResolver = contractResolver;
            const string ExpectedJson = "{\"P3\":0}";

            // act
            var actualJson = JsonConvert.SerializeObject(this.testObject, this.settings);

            // assert
            actualJson.Should().Be(ExpectedJson, "because P3 is the only property with the ReadOnly attribute.");
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
