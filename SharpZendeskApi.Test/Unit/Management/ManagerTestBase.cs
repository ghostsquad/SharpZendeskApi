namespace SharpZendeskApi.Test.Unit.Management
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Microsoft.Practices.Unity;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using RestSharp;

    using SharpZendeskApi.Exceptions;
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;
    using SharpZendeskApi.Test.Common;

    using Xunit;
    using Xunit.Should;

    public abstract class ManagerTestBase<TModel, TInterface, TManager, TManagerInterface>
        where TModel : TrackableZendeskThingBase, TInterface, new()
        where TInterface : class, IZendeskThing, ITrackable
        where TManagerInterface : class, IManager<TInterface>
        where TManager : ManagerBase<TModel, TInterface>, TManagerInterface
    {
        #region Constants

        protected const int ExpectedId = 1;

        #endregion

        protected Testable<TManager> Testable { get; private set; }

        protected TManagerInterface TestableInterface
        {
            get
            {
                return this.Testable.ClassUnderTest;
            }
        }

        #region Constructors and Destructors

        protected ManagerTestBase()
        {
            this.Testable = new Testable<TManager>();
            this.RequestHandlerMock = new Mock<IRequestHandler>();
            this.ClientMock = this.Testable.InjectMock<ZendeskClientBase>()
                .SetupProperty(x => x.RequestHandler, this.RequestHandlerMock.Object);
        }

        #endregion

        #region Properties

        protected Mock<ZendeskClientBase> ClientMock { get; set; }

        internal Mock<IRequestHandler> RequestHandlerMock { get; set; }               

        #endregion

        #region Public Methods and Operators

        [Fact]
        public virtual void GetMany_WithValidRequestAndExistingObject_ShouldReturnWithObject()
        {
            IRestRequest actualRequest = null;
            var expectedListing = this.SetupListingResponse(x => actualRequest = x, 2);
            var expectedCollection = expectedListing.ToList();

            var expectedResourceParameter = string.Format(
                "{0}/show_many.json?ids={1}",
                typeof(TModel).GetTypeNameAsCPlusPlusStyle().Pluralize(),
                string.Join(",", new[] { 1, 2 }));

            // act
            var actualObjects = this.TestableInterface.GetMany(new[] { 1, 2 }).Take(2).ToList();

            // assert
            actualRequest.Should().NotBeNull();

            actualRequest.Resource.Should().Be(expectedResourceParameter);
            actualRequest.Method.Should().Be(Method.GET);

            actualObjects.Should().NotBeEmpty().And.HaveCount(2).And.ContainInOrder(expectedCollection);
        }       

        [Fact]
        public void Get_AssertRequest()
        {
            IRestRequest actualRequest = null;
            var model = this.SetupSingleResponse(x => actualRequest = x);

            // act
            var actualItem = this.TestableInterface.Get(ExpectedId);

            // assert
            this.RequestHandlerMock.Verify(x => x.MakeRequest<TInterface>(It.IsAny<IRestRequest>()), Times.Once);
            actualRequest.Should().NotBeNull();
            actualRequest.Method.Should().Be(Method.GET);
            var expectedResource = string.Format(
                "{0}/{1}.json",
                typeof(TModel).GetTypeNameAsCPlusPlusStyle().Pluralize(),
                ExpectedId);
            actualRequest.Resource.Should().Be(expectedResource);
            actualItem.ShouldBeSameAs(model);
        }

        [Fact]
        public void SubmitUpdatesFor_WhenNotSubmittedObject_ExpectSharpZendeskException()
        {
            // arrange
            var model = new TModel { Id = 1 };

            // act & assert
            this.TestableInterface.Invoking(x => x.SubmitUpdatesFor(model))
                .ShouldThrow<SharpZendeskException>()
                .WithMessage("Cannot perform this operation. The object has not yet been submitted to Zendesk!");
        }

        [Fact]
        public void SubmitUpdatesFor_WhenNoChangedProperties_ExpectNoChange()
        {
            // arrange
            var model = new TModel { WasSubmitted = true, Id = 1 };
            this.RequestHandlerMock.Setup(x => x.MakeRequest(It.IsAny<IRestRequest>())).Verifiable();

            // act
            this.TestableInterface.SubmitUpdatesFor(model);

            // assert
            this.RequestHandlerMock.Verify(x => x.MakeRequest(It.IsAny<IRestRequest>()), Times.Never);
            this.RequestHandlerMock.Verify(x => x.MakeRequest<TModel>(It.IsAny<IRestRequest>()), Times.Never);
        }

        [Fact]
        public virtual void SubmitNew_GivenObjectWithNullMandatoryProperties_ExpectMandatoryPropertyNullValueException()
        {
            // arrange
            var model = new TModel();

            // act & assert
            this.TestableInterface.Invoking(x => x.SubmitNew(model)).ShouldThrow<MandatoryPropertyNullValueException>();
        }

        [Fact]
        public void SubmitNew_ExpectObjectFromMakeRequestReturned()
        {
            var modelMock = Mock.Of<TModel>();
            this.RequestHandlerMock.Setup(x => x.MakeRequest<TModel>(It.IsAny<IRestRequest>())).Returns(modelMock);

            var serializerMock = new Mock<IZendeskSerializer>();
            serializerMock.Setup(x => x.Serialize(It.IsAny<TrackableZendeskThingBase>()))
                .Returns(string.Empty);

            this.ClientMock.Setup(x => x.GetSerializer(It.IsAny<SerializationScenario>()))
                .Returns(serializerMock.Object);

            var passInModel = this.Testable.Fixture.Create<TModel>();
            passInModel.WasSubmitted = false;

            // act
            var result = this.TestableInterface.SubmitNew(passInModel);

            result.ShouldBeSameAs(modelMock);
        }

        [Fact]
        public void SubmitNew_AssertRequest()
        {
            // arrange
            var model = this.Testable.Fixture.Create<TModel>();

            IRestRequest actualRequest = null;
            this.RequestHandlerMock.Setup(x => x.MakeRequest<TModel>(It.IsAny<IRestRequest>()))
                .Returns(model)
                .Callback<IRestRequest>(x => actualRequest = x)
                .Verifiable();

            const string JsonBodyInput = "{\"test\"=1}";
            const string ExpectedJsonBody = "application/json=" + JsonBodyInput;

            TrackableZendeskThingBase actualSerializedObject = null;
            var serializerMock = new Mock<IZendeskSerializer>();
            serializerMock.Setup(x => x.Serialize(It.IsAny<TrackableZendeskThingBase>()))
                .Callback<TrackableZendeskThingBase>(x => actualSerializedObject = x)
                .Returns(ExpectedJsonBody)
                .Verifiable();

            SerializationScenario? actualScenario = null;
            this.ClientMock.Setup(x => x.GetSerializer(It.IsAny<SerializationScenario>()))
                .Callback<SerializationScenario>(x => actualScenario = x)
                .Returns(serializerMock.Object)
                .Verifiable();

            string expectedResource = typeof(TModel).Name.Pluralize().ToLower() + ".json";

            // act
            this.TestableInterface.SubmitNew(model);

            // assert
            this.RequestHandlerMock.Verify(x => x.MakeRequest<TModel>(It.IsAny<IRestRequest>()), Times.Once);
            actualRequest.Should().NotBeNull();
            actualRequest.Method.Should().Be(Method.POST);
            actualRequest.Resource.Should().Be(expectedResource);
            actualRequest.Parameters.First(x => x.Type == ParameterType.RequestBody).Value.Should().Be(ExpectedJsonBody);

            actualSerializedObject.ShouldBeSameAs(model);

            actualScenario.ShouldNotBeNull();
            actualScenario.Should().Be(SerializationScenario.Create);
        }

        [Fact]
        public void SubmitNew_WithAlreadySubmittedObject_ExpectArgumentException()
        {
            // arrange
            var model = new TModel { WasSubmitted = true };

            // act & assert
            this.TestableInterface.Invoking(x => x.SubmitNew(model))
                .ShouldThrow<SharpZendeskException>()
                .WithMessage("Cannot perform this operation. The object has already been submitted to Zendesk!");
        }

        [Fact]
        public void SubmitNew_WithNull_ExpectArgumentNullException()
        {
            // act & assert
            this.TestableInterface.Invoking(x => x.SubmitNew(null)).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void SubmitUpdatesFor_WithNull_ExpectArgumentNullException()
        {
            // act & assert
            this.TestableInterface.Invoking(x => x.SubmitUpdatesFor(null)).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void SubmitUpdatesFor_AssertRequest()
        {
            // arrange
            var modelMock = new Mock<TModel>();
            modelMock.SetupProperty(x => x.Id, 1).SetupProperty(x => x.WasSubmitted, true);
            modelMock.Object.ChangedPropertiesSet.Add("foo");

            IRestRequest actualRequest = null;
            this.SetupSingleResponse(x => actualRequest = x);

            const string JsonBodyInput = "{\"test\"=1}";
            const string ExpectedJsonBody = "application/json=" + JsonBodyInput;

            TrackableZendeskThingBase actualSerializedObject = null;
            var serializer = new Mock<IZendeskSerializer>();
            serializer.Setup(x => x.Serialize(It.IsAny<TrackableZendeskThingBase>()))
                .Callback<TrackableZendeskThingBase>(x => actualSerializedObject = x)
                .Returns(ExpectedJsonBody)
                .Verifiable();

            SerializationScenario? actualScenario = null;
            this.ClientMock.Setup(x => x.GetSerializer(It.IsAny<SerializationScenario>()))
                .Callback<SerializationScenario>(x => actualScenario = x)
                .Returns(serializer.Object)
                .Verifiable();

            var pluralizedModel = (typeof(TModel).Name + "s").ToCPlusPlusNamingStyle();
            var expectedResource = pluralizedModel + "/1.json";

            // act
            this.TestableInterface.SubmitUpdatesFor(modelMock.Object);

            // assert
            this.RequestHandlerMock.Verify(x => x.MakeRequest<TInterface>(It.IsAny<IRestRequest>()), Times.Once);
            this.ClientMock.Verify(x => x.GetSerializer(It.IsAny<SerializationScenario>()), Times.Once);
            actualRequest.Should().NotBeNull();
            actualRequest.Method.Should().Be(Method.PUT);
            actualRequest.Resource.Should().Be(expectedResource);
            actualRequest.Parameters.First(x => x.Type == ParameterType.RequestBody).Value.Should().Be(ExpectedJsonBody);

            actualSerializedObject.ShouldBeSameAs(modelMock.Object);

            actualScenario.Should().NotBeNull();
            actualScenario.Should().Be(SerializationScenario.Update);
        }

        [Fact]
        public void TryGet_WhenGetFails_ExpectNullAndFalse()
        {
            this.RequestHandlerMock.Setup(x => x.MakeRequest<TInterface>(It.IsAny<IRestRequest>()))
                .Throws<Exception>();

            // act
            TInterface actualModel;
            var actualResult = this.TestableInterface.TryGet(ExpectedId, out actualModel);

            // assert
            actualResult.Should().BeFalse();
            actualModel.Should().BeNull();
        }

        [Fact]
        public void TryGet_WhenGetSucceeds_ExpectOutAndReturnTrue()
        {
            var model = Mock.Of<TInterface>();

            this.RequestHandlerMock.Setup(x => x.MakeRequest<TInterface>(It.IsAny<IRestRequest>()))
                .Returns(model);

            // act
            TInterface actualModel;
            var actualResult = this.TestableInterface.TryGet(ExpectedId, out actualModel);

            // assert
            actualResult.Should().BeTrue();
            actualModel.Should().BeSameAs(model);
        }

        #endregion

        #region Methods

        internal void SetupListingResponse(Action<IRestRequest> callBackAction, IListing<TInterface> listing)
        {
            this.ClientMock.Setup(x => x.GetListing<TModel, TInterface>(It.IsAny<IRestRequest>()))
                .Returns(listing)
                .Callback(callBackAction)
                .Verifiable();
        }

        internal IListing<TInterface> SetupListingResponse(Action<IRestRequest> callBackAction, int numberOfItemsInPage)
        {
            var objects = new List<TInterface>();
            for (var i = 1; i <= numberOfItemsInPage; i++)
            {
                int temp = i;
                var objectToAdd = new TModel { Id = temp };
                objects.Add(objectToAdd);
            }

            var listing = this.Testable.Fixture.Create<Mock<IListing<TInterface>>>();

            listing.Setup(x => x.GetEnumerator())
                .Returns(() => objects.GetEnumerator());

            this.SetupListingResponse(callBackAction, listing.Object);

            return listing.Object;
        }

        internal void SetupPageResponse(Action<IRestRequest> callBackAction, IPage<TModel> page)
        {
            this.RequestHandlerMock.Setup(x => x.MakeRequest<IPage<TModel>>(It.IsAny<IRestRequest>()))
                .Returns(page)
                .Callback(callBackAction)
                .Verifiable();
        }

        internal IPage<TModel> SetupPageResponse(Action<IRestRequest> callBackAction, int numberOfItemsInPage)
        {
            var objects = new List<TModel>();
            for (var i = 1; i <= numberOfItemsInPage; i++)
            {
                int temp = i;
                var objectToAdd = new TModel { Id = temp };
                objects.Add(objectToAdd);
            }

            var page = this.Testable.Fixture.Create<Mock<IPage<TModel>>>().SetupProperty(pg => pg.Collection, objects).Object;

            this.SetupPageResponse(callBackAction, page);

            return page;
        }

        protected TInterface SetupSingleResponse(Action<IRestRequest> callBackAction)
        {
            var obj = this.Testable.Fixture.Freeze<TInterface>();
            this.SetupSingleResponse(callBackAction, obj);
            return obj;
        }

        protected void SetupSingleResponse(Action<IRestRequest> callBackAction, TInterface obj)
        {
            this.RequestHandlerMock.Setup(x => x.MakeRequest<TInterface>(It.IsAny<IRestRequest>()))
                .Returns(obj)
                .Callback(callBackAction)
                .Verifiable();
        }

        #endregion
    }
}