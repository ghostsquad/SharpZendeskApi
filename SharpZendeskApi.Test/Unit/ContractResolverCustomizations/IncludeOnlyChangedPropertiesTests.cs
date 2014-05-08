namespace SharpZendeskApi.Test.Unit.ContractResolverCustomizations
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using FluentAssertions;

    using SharpZendeskApi.ContractResolution;
    using SharpZendeskApi.Test.Fakes;

    using Xunit;

    public class IncludeOnlyChangedPropertiesTests
    {
        private const string PropertyName = "Read";
        private HashSet<string> changedProperties = new HashSet<string>();

        private PropertyInfo property = typeof(ReadWriteMandatory).GetProperty(PropertyName);

        private IContractResolverCustomization customization;

        public IncludeOnlyChangedPropertiesTests()
        {
            this.customization = new IncludeOnlyChangedPropertiesCustomization(this.changedProperties);
        }

        [Fact]
        public void GivenEmptyPropertyNamesHashSetExpectIncludeToBeFalse()
        {                        
            // arrange not needed

            // act
            var actualResult = this.customization.IncludeMemberInfoPredicate(this.property);

            // assert
            AssertionExtensions.Should((bool)actualResult).BeFalse("because the property is not part of the changed properties hashset");
        }

        [Fact]
        public void GivenChangedPropertyExpectIncludeToBeTrue()
        {
            // arrange                                
            this.changedProperties.Add(PropertyName);
            this.customization = new IncludeOnlyChangedPropertiesCustomization(this.changedProperties);
             
            // act
            var actualResult = this.customization.IncludeMemberInfoPredicate(this.property);

            // assert
            AssertionExtensions.Should((bool)actualResult).BeTrue("because the property is included in the changed properties hashset");
        }

        [Fact]
        public void GivenNullHashSetExpectNullArgumentException()
        {
            // act
            Assert.Throws<ArgumentNullException>(() => new IncludeOnlyChangedPropertiesCustomization(null));                        
        }
    }
}
