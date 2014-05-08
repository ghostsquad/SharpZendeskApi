namespace SharpZendeskApi.Test.Unit
{
    using System.Linq;

    using FluentAssertions;

    using Newtonsoft.Json;

    using SharpZendeskApi.ContractResolution;
    using SharpZendeskApi.Test.Fakes;

    using Xunit;

    public class CreationSerializerTests
    {
        #region Fields

        private readonly CreationSerializer serializer = new CreationSerializer();

        #endregion

        #region Public Methods and Operators

        [Fact]
        public void ExpectCStyleNamingCustomizationUsed()
        {
            this.serializer.Resolver.Customizations.Any(x => x is CStyleNamingCustomization).Should().BeTrue();
        }

        [Fact]
        public void ExpectExcludeReadOnlyUnlessMandatoryCustomizationUsed()
        {
            this.serializer.Resolver.Customizations.Any(x => x is ExcludeReadOnlyUnlessMandatoryCustomization)
                .Should()
                .BeTrue();
        }

        [Fact]
        public void ExpectIgnoreNulls()
        {
            this.serializer.Settings.NullValueHandling.Should().Be(NullValueHandling.Ignore);
        }

        [Fact]
        public void ExpectIsoDateFormat()
        {
            this.serializer.Settings.DateFormatHandling.Should().Be(DateFormatHandling.IsoDateFormat);
        }

        [Fact]
        public void ExpectSettingsToIncludeResolver()
        {
            this.serializer.Settings.ContractResolver.Should().Be(this.serializer.Resolver);
        }

        [Fact]
        public void Serialize_ExpectOutputWrappedInRootElement()
        {
            // arrange
            const string ExpectedJson = "{\"fake_zendesk_thing\":{\"foo\":0}}";

            // act
            var json = this.serializer.Serialize(new FakeZendeskThing());

            // assert
            json.Should().Be(ExpectedJson);
        }

        #endregion
    }
}