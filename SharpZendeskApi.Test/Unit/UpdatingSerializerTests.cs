namespace SharpZendeskApi.Test.Unit
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Moq;

    using Newtonsoft.Json;

    using SharpZendeskApi.ContractResolution;
    using SharpZendeskApi.Models;
    using SharpZendeskApi.Test.Fakes;

    using Xunit;

    public class UpdatingSerializerTests
    {
        #region Fields

        private readonly UpdatingSerializer serializer = new UpdatingSerializer();

        #endregion

        #region Public Methods and Operators

        [Fact]
        public void ExpectCStyleNamingCustomizationUsed()
        {
            // assert
            this.serializer.Resolver.Customizations.Should().HaveCount(2);
            this.serializer.Resolver.Customizations.Any(x => x is CStyleNamingCustomization).Should().BeTrue();
        }

        [Fact]
        public void ExpectDateFormatHandlingToIsoDateFormat()
        {
            this.serializer.Settings.DateFormatHandling.Should().Be(DateFormatHandling.IsoDateFormat);
        }

        [Fact]
        public void ExpectExcludeReadOnlyCustomizationUsed()
        {
            // assert
            this.serializer.Resolver.Customizations.Should().HaveCount(2);
            this.serializer.Resolver.Customizations.Any(x => x is ExcludeReadOnlyCustomization).Should().BeTrue();
        }

        [Fact]
        public void ExpectNullValueHandlingSetToIgnore()
        {
            this.serializer.Settings.NullValueHandling.Should().Be(NullValueHandling.Ignore);
        }

        [Fact]
        public void ExpectSettingsToIncludeResolver()
        {
            this.serializer.Settings.ContractResolver.Should().Be(this.serializer.Resolver);
        }

        [Fact]
        public void Serialize_ExpectChangedPropertiesCustomizationAdded()
        {
            // arrange
            const string ExpectedChangedProperty = "foo";
            var thing =
                Mock.Of<TrackableZendeskThingBase>(
                    x => x.ChangedPropertiesSet == new HashSet<string> { ExpectedChangedProperty });

            // act
            this.serializer.Serialize(thing);

            // assert
            var customizations = this.serializer.Resolver.Customizations;

            customizations.Should().HaveCount(3);
            var customization =
                customizations.FirstOrDefault(x => x is IncludeOnlyChangedPropertiesCustomization) as
                IncludeOnlyChangedPropertiesCustomization;

            AssertionExtensions.Should((object)customization).NotBeNull();
            AssertionExtensions.Should((IEnumerable<string>)customization.ChangedProperties).HaveCount(1);
            AssertionExtensions.Should((bool)customization.ChangedProperties.Contains(ExpectedChangedProperty)).BeTrue();
        }

        [Fact]
        public void Serialize_ExpectOutputWrappedInRootElement()
        {
            // arrange
            const string ExpectedJson = "{\"fake_zendesk_thing\":{\"foo\":0}}";

            // act
            var json = this.serializer.Serialize(new FakeZendeskThing
                                                     {
                                                         WasSubmitted = true,
                                                         ChangedPropertiesSet = new HashSet<string> { "Foo" }
                                                     });

            // assert
            json.Should().Be(ExpectedJson);
        }

        #endregion
    }
}