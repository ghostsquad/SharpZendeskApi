namespace SharpZendeskApi.Test.Unit.Models
{
    using System.Linq;
    using System.Reflection;

    using FluentAssertions;

    using Microsoft.Practices.Unity;

    using Ploeh.AutoFixture;

    using RestSharp;
    using RestSharp.Deserializers;

    using SharpZendeskApi.Models;
    using SharpZendeskApi.Models.Attributes;
    using SharpZendeskApi.Test.Common;

    using Xunit;
    using Xunit.Should;

    // ReSharper disable once InconsistentNaming
    public abstract class ModelTestBase<TJson, TModel, TInterface> : IUseFixture<ModelFixture<TJson, TModel>>
        where TJson : JsonTestObjectBase
        where TInterface : class, IZendeskThing
        where TModel : TrackableZendeskThingBase, TInterface, new()
    {
        protected ModelFixture<TJson, TModel> ModelFixture { get; private set; }

        [Fact]
        public void AllPropertiesShouldBeNullable()
        {
            this.ModelFixture.Properties.All(p => p.PropertyType.IsTypeNullable())
                .Should().BeTrue("because not all properties will have a value at all times, and default values are not acceptable.");
        }

        public abstract void CanCreateWithFilledMandatoryPropertiesUsingConstructor();

        [Fact]
        public void CanDeserialize()
        {
            // arrange
            var deserializer = new JsonDeserializer();            
            var response = new RestResponse { Content = this.ModelFixture.SerializedJsonObject };

            // act
            var actualModel = deserializer.Deserialize<TModel>(response);

            // assert
            foreach (var property in this.ModelFixture.Properties)
            {
                var value = property.GetValue(actualModel);
                value.Should().NotBeNull();
            }

            this.ModelFixture.JsonTestObject.Keys
                .ShouldBeEquivalentTo(this.ModelFixture.Properties.Select(x => x.Name.ToCPlusPlusNamingStyle()));
        }

        [Fact]
        public void CanDeserializePage()
        {
            // arrange            
            var deserializer = new JsonDeserializer();
            var container = new UnityContainer();
            container.RegisterType(typeof(IPage<TModel>), typeof(TicketsPage));

            deserializer.DeserializationResolver = x => container.Resolve(x);
            var response = new RestResponse { Content = this.ModelFixture.SerializedPage };

            var actualModel = deserializer.Deserialize<IPage<TModel>>(response);

            actualModel.Collection.Should().HaveCount(3);
            actualModel.Count.ShouldBeGreaterThan(0);
            actualModel.NextPage.ShouldNotBeNull();
            actualModel.PreviousPage.ShouldNotBeNull();
        }

        [Fact]
        public void NonReadOnlyPublicPropertiesShouldBeTrackedWhenChanged()
        {
            // arrange
            var randomizedData = this.ModelFixture.Fixture.Create<TModel>();
            var nonReadOnlyProperties =
                this.ModelFixture.Properties.Where(
                    m => !m.GetCustomAttributes(typeof(ReadOnlyAttribute), true).Any());

            // act
            foreach (var property in nonReadOnlyProperties)
            {
                var randomData = property.GetValue(randomizedData);
                var model = new TModel { WasSubmitted = true };
                property.SetValue(model, randomData);

                // assert
                model.ChangedProperties.Should().Contain(property.Name);
            }
        }

        [Fact]
        public void ReadOnlyPropertiesShouldNotHaveSetterOnInterface()
        {
            // arrange
            TInterface modelAsInterface = new TModel();
            var readOnlyProperties = modelAsInterface.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.GetCustomAttributes(typeof(ReadOnlyAttribute), true).Any());

            // act
            foreach (var property in readOnlyProperties)
            {
                // assert
                property.GetSetMethod().Should().BeNull();
            }
        }

        public void SetFixture(ModelFixture<TJson, TModel> data)
        {
            this.ModelFixture = data;
        }
    }
}