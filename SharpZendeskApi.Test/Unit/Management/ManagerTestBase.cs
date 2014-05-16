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

    public abstract class ManagerTestBase<TModel, TInterface, TManager>
        where TModel : TrackableZendeskThingBase, TInterface, new()
        where TInterface : class, IZendeskThing, ITrackable
        where TManager : class, IManager<TInterface>
    {
        #region Constants

        protected const int ExpectedId = 1;

        #endregion

        protected Testable<TManager> testable = new Testable<TManager>();

        #region Constructors and Destructors

        protected ManagerTestBase()
        {
            this.RequestHandlerMock = new Mock<IRequestHandler>();
            this.ClientMock = new Mock<ZendeskClientBase>()
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
            var expectedPage = this.SetupPageResponse(x => actualRequest = x, 2);

            var expectedResourceParameter = string.Format(
                "{0}/show_many.json?ids={1}",
                typeof(TModel).GetTypeNameAsCPlusPlusStyle().Pluralize(),
                string.Join(",", new[] { 1, 2 }));

            // act
            var actualObjects = this.testable.ClassUnderTest.GetMany(new[] { 1, 2 }).Take(2).ToList();

            // assert
            actualRequest.Should().NotBeNull();

            actualRequest.Resource.Should().Be(expectedResourceParameter);
            actualRequest.Method.Should().Be(Method.GET);

            actualObjects.Should().NotBeEmpty().And.HaveCount(2).And.ContainInOrder(expectedPage.Collection);
        }       

        [Fact]
        public void Get_WhenValidRequest_ExpectRequestMade()
        {
            IRestRequest actualRequest = null;
            var model = this.SetupSingleResponse(x => actualRequest = x);

            // act
            var actualItem = this.testable.ClassUnderTest.Get(ExpectedId);

            // assert
            this.RequestHandlerMock.Verify(x => x.MakeRequest<TModel>(It.IsAny<IRestRequest>()), Times.Once);
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
            this.testable.ClassUnderTest.Invoking(x => x.SubmitUpdatesFor(model))
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
            this.testable.ClassUnderTest.SubmitUpdatesFor(model);

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
            this.testable.ClassUnderTest.Invoking(x => x.SubmitNew(model)).ShouldThrow<MandatoryPropertyNullValueException>();
        }

        public abstract void SubmitNew_UsingParameterizedConstructor_ExpectSuccess();

        [Fact]
        public void SubmitNew_WithAlreadySubmittedObject_ExpectArgumentException()
        {
            // arrange
            var model = new TModel { WasSubmitted = true };

            // act & assert
            this.testable.ClassUnderTest.Invoking(x => x.SubmitNew(model))
                .ShouldThrow<SharpZendeskException>()
                .WithMessage("Cannot perform this operation. The object has already been submitted to Zendesk!");
        }

        [Fact]
        public void SubmitNew_WithNull_ExpectArgumentNullException()
        {
            // act & assert
            this.testable.ClassUnderTest.Invoking(x => x.SubmitNew(null)).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void SubmitUpdatesFor_WithNull_ExpectArgumentNullException()
        {
            // act & assert
            this.testable.ClassUnderTest.Invoking(x => x.SubmitUpdatesFor(null)).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void SubmitUpdatesFor_AssertRequestCreation()
        {
            // arrange
            var modelMock = new Mock<TModel>();
            modelMock.SetupProperty(x => x.Id, 1)
                .SetupProperty(x => x.WasSubmitted, true)
                .SetupProperty(x => x.ChangedPropertiesSet, new HashSet<string> { "foo" });

            IRestRequest actualRequest = null;
            this.SetupSingleResponse(x => actualRequest = x);

            const string JsonBodyInput = "{\"test\"=1}";
            const string ExpectedJsonBody = "application/json=" + JsonBodyInput;

            TrackableZendeskThingBase actualSerializedObject = null;
            this.testable.InjectMock<IZendeskSerializer>()
                .Setup(x => x.Serialize(It.IsAny<TrackableZendeskThingBase>()))
                .Callback<TrackableZendeskThingBase>(x => actualSerializedObject = x)
                .Returns(ExpectedJsonBody)
                .Verifiable();

            var pluralizedModel = (typeof(TModel).Name + "s").ToCPlusPlusNamingStyle();
            var expectedResource = pluralizedModel + "/1.json";

            // act
            this.testable.ClassUnderTest.SubmitUpdatesFor(modelMock.Object);

            // assert
            actualRequest.Should().NotBeNull();
            actualRequest.Method.Should().Be(Method.PUT);
            actualRequest.Resource.Should().Be(expectedResource);
            actualRequest.Parameters.First(x => x.Type == ParameterType.RequestBody).Value.Should().Be(ExpectedJsonBody);
            actualSerializedObject.ShouldBeSameAs(modelMock.Object);
        }

        [Fact]
        public void TryGet_WhenGetFails_ExpectNullAndFalse()
        {
            var managerMock = this.testable.InjectMock<TManager>();
            managerMock.Setup(x => x.Get(ExpectedId))
                .Throws<SharpZendeskException>()
                .Verifiable();
            managerMock.CallBase = true;

            // act
            TInterface actualModel;
            var actualResult = testable.ClassUnderTest.TryGet(ExpectedId, out actualModel);

            // assert
            actualResult.Should().BeFalse();
            actualModel.Should().BeNull();
            managerMock.Verify(x => x.Get(ExpectedId), Times.Once());
        }

        [Fact]
        public void TryGet_WhenGetSucceeds_ExpectOutAndReturnTrue()
        {
            var model = Mock.Of<TModel>();

            var managerMock = this.testable.InjectMock<TManager>();
            managerMock.Setup(x => x.Get(It.IsAny<int>()))
                .Returns(model)
                .Verifiable();

            managerMock.CallBase = true;

            // act
            TInterface actualModel;
            var actualResult = this.testable.ClassUnderTest.TryGet(ExpectedId, out actualModel);

            // assert
            managerMock.Verify(x => x.Get(ExpectedId), Times.Once());
            actualResult.Should().BeTrue();
            actualModel.Should().BeSameAs(model);
        }

        #endregion

        #region Methods

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

            var page = this.testable.Fixture.Create<Mock<IPage<TModel>>>().SetupProperty(pg => pg.Collection, objects).Object;

            this.SetupPageResponse(callBackAction, page);

            return page;
        }

        protected TModel SetupSingleResponse(Action<IRestRequest> callBackAction)
        {
            var model = this.testable.Fixture.Freeze<TModel>();
            this.SetupSingleResponse(callBackAction, model);
            return model;
        }

        protected void SetupSingleResponse(Action<IRestRequest> callBackAction, TModel model)
        {
            this.RequestHandlerMock.Setup(x => x.MakeRequest<TModel>(It.IsAny<IRestRequest>()))
                .Returns(model)
                .Callback(callBackAction)
                .Verifiable();
        }

        #endregion
    }
}