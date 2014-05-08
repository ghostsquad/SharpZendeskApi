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

    using RestSharp;

    using SharpZendeskApi.Core.Exceptions;
    using SharpZendeskApi.Core.Models;
    using SharpZendeskApi.Core.Models.Attributes;
    using SharpZendeskApi.Core.Test.Common;

    [TestFixture]
    public class TicketManagementTests : ManagementTestBase<Ticket>
    {
        [SetUp]
        public void Setup()
        {
            this.ZendeskThing = new Ticket { ModelFacade = this.ManagerMock.Object };
        }

        [Test]
        public void WhenCreatingTicketExpectReadOnlyFieldsToBePopulatedByResponse()
        {
            // arrange
            this.Fixture.Customizations.Add(new RespectOnlyReadOnlyPropertiesSpecimenCustomization());

            this.ZendeskThing.RequesterId = 1;
            this.ZendeskThing.Description = this.Fixture.Create<string>();

            var responseTicket = this.Fixture.Create<Ticket>();

            var response = new Mock<IRestResponse<Ticket>>()
                .SetupProperty(x => x.Data, responseTicket)
                .SetupProperty(x => x.StatusCode, HttpStatusCode.Created)
                .SetupProperty(x => x.Request.Method, Method.POST);

            this.ClientMock.Setup(x => x.Execute<Ticket>(It.IsAny<IRestRequest>())).Returns(response.Object);

            // act
            this.ZendeskThing.Create(this.ManagerMock.Object);

            // assert
            var ticketProperties = typeof(Ticket).GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(x => x.Name);            
            foreach (var property in ticketProperties)
            {
                var customAttributes = property.GetCustomAttributes(true);           

                if (customAttributes.Any(x => x.GetType() == typeof(ReadOnlyAttribute)))
                {
                    var responseTicketProperty = typeof(Ticket).GetProperty(property.Name);
                    var responseTicketPropertyValue = responseTicketProperty.GetValue(responseTicket);
                    responseTicketPropertyValue.Should().NotBeNull();
                    property.GetValue(this.ZendeskThing).Should().Be(responseTicketPropertyValue);
                }
                else if (customAttributes.All(x => x.GetType() != typeof(MandatoryAttribute)))
                {
                    property.GetValue(this.ZendeskThing).Should().BeNull();
                }                
            }
        }

        [Test]
        public void GivenAllMandatoryValuesAndNonNullReadOnlyValuesExpectOnlyMandatoryPropertiesInRequestBody()
        {
            // arrange

            // Id is ReadOnly
            // RequesterId is Mandatory (the only mandatory property)            
            this.ZendeskThing.Id = 1;
            this.ZendeskThing.RequesterId = 2;
            this.ZendeskThing.Description = "test";
            var response =
                Mock.Of<IRestResponse<Ticket>>(x =>
                    x.Data == this.ZendeskThing 
                    && x.StatusCode == HttpStatusCode.Created
                    && x.Request == new RestRequest(Method.POST));
            IRestRequest actualRequest = null;
            this.ClientMock.Setup(x => x.Execute<Ticket>(It.IsAny<IRestRequest>()))
                .Returns(response)
                .Callback<IRestRequest>(x => actualRequest = x);

            const string ExpectedResource = "tickets.json";
            const string ExpectedJsonBody = "{\"ticket\":{\"description\":\"test\",\"requester_id\":2}}";

            // act
            this.ZendeskThing.Create(this.ManagerMock.Object);

            // assert
            // http://developer.zendesk.com/documentation/rest_api/tickets.html#creating-tickets
            actualRequest.Resource.Should().Be(ExpectedResource);
            actualRequest.Method.Should().Be(Method.POST);
            actualRequest.Parameters.Where(p => p.Type == ParameterType.RequestBody)
                .Should().NotBeNull()
                .And.HaveCount(1);
            actualRequest.Parameters[0].Value.Should().Be(ExpectedJsonBody);
        }      

        [Test]
        public void WhenNoMandatoryFieldsNullWhenCreateExpectMandatoryPropertyNullValueException()
        {
            // arrange not needed                                    

            // act & assert
            this.ZendeskThing.Invoking(x => x.Create(this.ManagerMock.Object))
                .ShouldThrow<MandatoryPropertyNullValueException>(
                    "because none of the mandatory properties were provided.");
        }

        [Test]
        public void WhenTicketRefreshedExpectSameIdProvidedToManager()
        {
            // arrange
            const int ExpectedId = 1;

            var responseTicket = this.Fixture.Create<Ticket>();
            responseTicket.Id = 1;

            int? actualRequestedId = null;
            this.ManagerMock.Setup(x => x.Get(It.IsAny<int>()))
                    .Returns(responseTicket)
                    .Callback<int>(x => actualRequestedId = x);

            this.ZendeskThing.Id = ExpectedId;
            this.ZendeskThing.ModelFacade = this.ManagerMock.Object;
            this.ZendeskThing.IsCreated = true;            

            // act
            this.ZendeskThing.Refresh();

            // assert
            actualRequestedId.Should().Be(ExpectedId);
        }

        [Test]
        public void CanRefreshExistingTicket()
        {
            // arrange
            var responseTicket = this.Fixture.Create<Ticket>();           
            responseTicket.Id = 1;
                        
            this.ManagerMock.Setup(x => x.Get(It.IsAny<int>())).Returns(responseTicket);
            this.ZendeskThing.Id = 1;
            this.ZendeskThing.IsCreated = true;
            this.ZendeskThing.ModelFacade = ManagerMock.Object;

            // act
            this.ZendeskThing.Refresh();

            // assert
            var ticketProperties = typeof(Ticket).GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(x => x.Name);
            foreach (var property in ticketProperties)
            {
                property.GetValue(this.ZendeskThing).Should().NotBeNull();
            }
        }

        [Test]
        public void WhenTicketUpdateExpectNonNullPropertiesSentTicket()
        {
            this.ZendeskThing.Id = 1;
            this.ZendeskThing.RequesterId = 2;            
            this.ZendeskThing.As<SelfManagedEntityBase<Ticket>>().IsCreated = true;

            var response =
                Mock.Of<IRestResponse<Ticket>>(x =>
                    x.Data == this.ZendeskThing
                    && x.StatusCode == HttpStatusCode.Created
                    && x.Request == new RestRequest(Method.POST));
            IRestRequest actualRequest = null;
            this.ClientMock.Setup(x => x.Execute<Ticket>(It.IsAny<IRestRequest>()))
                .Returns(response)
                .Callback<IRestRequest>(x => actualRequest = x);

            const string ExpectedResource = "tickets/1.json";
            const string ExpectedJsonBody = "{\"ticket\":{\"requester_id\":2}}";

            // act
            this.ZendeskThing.Update();

            // assert
            // http://developer.zendesk.com/documentation/rest_api/tickets.html#creating-tickets
            actualRequest.Resource.Should().Be(ExpectedResource);
            actualRequest.Method.Should().Be(Method.PUT);
            actualRequest.Parameters.Where(p => p.Type == ParameterType.RequestBody)
                .Should().NotBeNull()
                .And.HaveCount(1);
            actualRequest.Parameters[0].Value.Should().Be(ExpectedJsonBody);
        }
    }
}
