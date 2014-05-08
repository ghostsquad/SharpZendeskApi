namespace SharpZendeskApi.Core.Test.Unit.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Reflection;

    using FluentAssertions;

    using Moq;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Kernel;

    using RestSharp;

    using SharpZendeskApi.Core.Exceptions;
    using SharpZendeskApi.Core.Facades;
    using SharpZendeskApi.Core.Models;
    using SharpZendeskApi.Core.Models.Attributes;
    using SharpZendeskApi.Core.Test.Common;

    public abstract class ManagementTestBase<T> where T : IZendeskThing
    {
        protected Mock<IRestClient> ClientMock { get; set; }

        protected IFixture Fixture { get; set; }

        protected Mock<IModelFacade<T>> ManagerMock { get; set; }

        protected T ZendeskThing { get; set; }

        [SetUp]
        public void Setup()
        {
            this.Fixture = new Fixture().Customize(new AutoMoqCustomization());
            this.ClientMock = new Mock<IRestClient>();
            this.ManagerMock = new Mock<IModelFacade<T>>().SetupProperty(x => x.Client, this.ClientMock.Object);
        }

        [Test]
        public void GivenNullManagerWhenCreateExpectArgumentNullException()
        {
            this.ZendeskThing.Invoking(x => x.Create(null)).ShouldThrow<ArgumentNullException>("because parameter is null");
        }

        [Test]
        public void GivenAlreadyCreatedZendeskThingWhenCreateExpectSharpZendeskException()
        {
            // arrange            
            this.ZendeskThing.As<SelfManagedEntityBase<T>>().IsCreated = true;            

            // act & assert
            this.ZendeskThing.Invoking(x => x.Create(this.ManagerMock.Object))
                .ShouldThrow<SharpZendeskException>("because create() was already called");
        }

        [Test]
        public void GivenUnauthorizedUserWhenCreateExpectUnauthorizedException()
        {
            // arrange
            this.ZendeskThing = this.Fixture.Create<T>();
            this.ZendeskThing.As<SelfManagedEntityBase<T>>().IsCreated = false;

            var response =
                Mock.Of<IRestResponse<Ticket>>(x => x.StatusCode == HttpStatusCode.Unauthorized);

            this.ClientMock.Setup(x => x.Execute<Ticket>(It.IsAny<IRestRequest>())).Returns(response);

            // act & assert
            this.ZendeskThing.Invoking(x => x.Create(this.ManagerMock.Object)).ShouldThrow<UnauthorizedAccessException>();
        }

        [Test]
        public void GivenIsCreatedFalseWhenUpdateExpectSharpZendeskException()
        {
            // arrange            
            this.ZendeskThing.As<SelfManagedEntityBase<T>>().IsCreated = false;

            // act & assert
            this.ZendeskThing.Invoking(x => x.Update())
                .ShouldThrow<SharpZendeskException>("because IsCreated is false");
        }

        [Test]
        public void GivenIsCreatedFalseWhenRefreshExpectSharpZendeskException()
        {
            // arrange            
            this.ZendeskThing.As<SelfManagedEntityBase<T>>().IsCreated = false;

            // act & assert
            this.ZendeskThing.Invoking(x => x.Refresh())
                .ShouldThrow<SharpZendeskException>("because IsCreated is false");
        }

        // TODO
        // Implement Delete (Admin only, so not a high priority)

        //[Test]
        //public void GivenIsCreatedFalseWhenDeleteExpectSharpZendeskException()
        //{
        //    // arrange            
        //    this.ZendeskThing.As<SelfManagedEntityBase<T>>().IsCreated = false;

        //    // act & assert
        //    this.ZendeskThing.Invoking(x => x.Delete())
        //        .ShouldThrow<SharpZendeskException>("because IsCreated is false");
        //}

        [Test]
        public void WhenPropertyChangedExpectLocalChangesUpdated()
        {
            // arrange
            var context = new SpecimenContext(this.Fixture);

            var zendeskThingAsBase = this.ZendeskThing.As<SelfManagedEntityBase<T>>();

            // act & assert
            // get properties not marked with ReadOnly attribute
            var properties =
                typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .OrderBy(x => x.Name)
                    .Where(x => x.GetCustomAttributes(true).All(y => y.GetType() != typeof(ReadOnlyAttribute)));

            foreach (var property in properties)
            {
                var value = context.Resolve(new SeededRequest(property.PropertyType, null));
                property.SetValue(this.ZendeskThing, value);

                zendeskThingAsBase.ChangedProperties.Should().Contain(property.Name, "because the value for this property was set");                
            }
        }

        [Test]
        public void GivenZendeskThingWithChangedPropertiesWhenUpdateExpectOnlyChangedPropertiesInRequest()
        {
            // arrange
            var context = new SpecimenContext(this.Fixture);

            var zendeskThing = new T();
            zendeskThing.As<SelfManagedEntityBase<Ticket>>().IsCreated = true;

            var managerMock = new Mock<IModelFacade<T>>()
                .SetupProperty(x => x.Client, this.ClientMock.Object);            

            var response = new Mock<IRestResponse<T>>()
                .SetupProperty(x => x.StatusCode, HttpStatusCode.OK)
                .SetupProperty(x => x.Request, new RestRequest(Method.PUT))
                .SetupProperty(x => x.Data, zendeskThing);

            IRestRequest actualRequest = null;
            this.ClientMock.Setup(x => x.Execute<T>(It.IsAny<IRestRequest>()))
                .Returns(response.Object)
                .Callback<IRestRequest>(x => actualRequest = x);

            zendeskThing.As<SelfManagedEntityBase<T>>().ModelFacade = managerMock.Object;

            // get the first property that is not readonly that is a string or int 
            // (for making the expectedjson creation easier)
            var property =
                typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .OrderBy(x => x.Name)
                    .FirstOrDefault(x => x.GetCustomAttributes(true).All(y => y.GetType() != typeof(ReadOnlyAttribute)) 
                        && (x.PropertyType == typeof(int?) || x.PropertyType == typeof(string)));            

            var expectedValue = context.Resolve(new SeededRequest(property.PropertyType, null));
            property.SetValue(zendeskThing, expectedValue);

            string jsonValue = property.PropertyType == typeof(string)
                                   ? "\"" + expectedValue + "\""
                                   : expectedValue.ToString();
            var expectedJsonString = string.Format("{{\"{0}\":{{\"{1}\":{2}}}}}", typeof(T).Name.ToCPlusPlusNamingStyle(), property.Name.ToCPlusPlusNamingStyle(), jsonValue);

            // act            
            zendeskThing.Update();

            // assert
            actualRequest.Parameters.Where(p => p.Type == ParameterType.RequestBody)
                .Should().NotBeNull()
                .And.HaveCount(1);            
            actualRequest.Parameters[0].Value.Should().Be(expectedJsonString);
        }

        [Test]
        public void GivenZendeskThingWithNoChangedPropertiesWhenUpdateExpectNoActionPerformed()
        {
            // arrange
            var zendeskThing = new T();
            var zendeskThingAsBase = zendeskThing.As<SelfManagedEntityBase<T>>();
            zendeskThingAsBase.IsCreated = true;

            // using strict behavior, and not setting anything up well ensure that an exception is thrown
            // if it is accessed (for instance to get the RestClient) by the method.
            var managerMock = new Mock<IModelFacade<T>>(MockBehavior.Strict);
            zendeskThing.As<SelfManagedEntityBase<T>>().ModelFacade = managerMock.Object;

            // act
            zendeskThing.Update();

            // assert
            zendeskThingAsBase.ChangedProperties.Should().BeEmpty("because no properties have changed");
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(x => x.Name);
            foreach (var property in properties)
            {
                property.GetValue(this.ZendeskThing).Should().BeNull();
            }
        }       
    }
}
