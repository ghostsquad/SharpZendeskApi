﻿namespace SharpZendeskApi.Core.Test.Unit
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Moq;

    using Newtonsoft.Json;

    using SharpZendeskApi.Core.ContractResolution;
    using SharpZendeskApi.Core.Models;
    using SharpZendeskApi.Core.Test.Fakes;

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
                    x => x.ChangedProperties == new HashSet<string> { ExpectedChangedProperty });

            // act
            this.serializer.Serialize(thing);

            // assert
            var customizations = this.serializer.Resolver.Customizations;

            customizations.Should().HaveCount(3);
            var customization =
                customizations.FirstOrDefault(x => x is IncludeOnlyChangedPropertiesCustomization) as
                IncludeOnlyChangedPropertiesCustomization;

            customization.Should().NotBeNull();
            customization.ChangedProperties.Should().HaveCount(1);
            customization.ChangedProperties.Contains(ExpectedChangedProperty).Should().BeTrue();
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
                                                         ChangedProperties = new HashSet<string> { "Foo" }
                                                     });

            // assert
            json.Should().Be(ExpectedJson);
        }

        #endregion
    }
}