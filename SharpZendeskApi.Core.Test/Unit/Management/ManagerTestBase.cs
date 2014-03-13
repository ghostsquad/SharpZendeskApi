﻿namespace SharpZendeskApi.Core.Test.Unit.Management
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Moq;

    using Ploeh.AutoFixture;

    using RestSharp;

    using SharpZendeskApi.Core.Exceptions;
    using SharpZendeskApi.Core.Management;
    using SharpZendeskApi.Core.Models;

    using TinyIoC;

    using Xunit;
    using Xunit.Should;

    public abstract class ManagerTestBase<TModel, TInterface, TManager> : IUseFixture<ManagerFixture<TModel>>
        where TModel : TrackableZendeskThingBase, TInterface, new()
        where TInterface : class, IZendeskThing
        where TManager : class, IManager<TInterface>
    {
        #region Constants

        protected const int ExpectedId = 1;

        #endregion

        #region Constructors and Destructors

        protected ManagerTestBase()
        {
            this.ClientMock = new Mock<IZendeskClient>().SetupProperty(x => x.Container, new TinyIoCContainer());
            this.ResponseMock = new Mock<IRestResponse<TModel>>();
            this.Manager = (TManager)Activator.CreateInstance(typeof(TManager), this.ClientMock.Object);
        }

        #endregion

        #region Properties

        protected Mock<IZendeskClient> ClientMock { get; set; }

        protected IManager<TInterface> Manager { get; set; }

        protected ManagerFixture<TModel> ManagerFixture { get; set; }

        protected Mock<IRestResponse<TModel>> ResponseMock { get; set; }

        #endregion

        #region Public Methods and Operators

        [Fact]
        public void Exists_WhenNotInCacheAndNotFoundResponse_ExpectFalse()
        {
            // arrange
            var mockManager = new Mock<IManager<TInterface>> { CallBase = true };
            mockManager.Setup(x => x.Get(ExpectedId, true))
                .Throws(new NotFoundException(new RestRequest(), HttpStatusCode.NotFound));

            // act
            var actualResult = mockManager.Object.Exists(ExpectedId);

            // assert
            actualResult.Should().BeFalse();
        }

        [Fact]
        public void Exists_WhenNotInCacheAndOkResponse_ExpectTrue()
        {
            // arrange
            this.SetupOkResponse();
            this.SetupVerifiableClientExecute();
            var mockManager = this.GetMockManager();
            mockManager.Setup(x => x.Get(ExpectedId, false)).Returns(this.ManagerFixture.Model).Verifiable();

            // act
            var actualResult = mockManager.Object.Exists(ExpectedId);

            // assert
            actualResult.Should().BeTrue();
            mockManager.Verify(x => x.Get(ExpectedId, false), Times.Once);
        }

        [Fact]
        public void Exists_WithCachedModel_ExpectPulledFromCacheAndReturnTrue()
        {
            // arrange
            this.Manager.Cache.Add(ExpectedId, this.ManagerFixture.Model);

            // act
            var actualResult = this.Manager.Exists(ExpectedId);

            // assert
            actualResult.Should().BeTrue();
        }

        [Fact]
        public virtual void GetMany_WithValidRequestAndExistingObject_ShouldReturnWithObject()
        {
            // arrange
            var pageResponse = this.GetPageResponse(2);

            IRestRequest actualRequest = null;
            this.ClientMock.Setup(x => x.Execute<IPage<TInterface>>(It.IsAny<IRestRequest>()))
                .Returns(pageResponse)
                .Callback<IRestRequest>(r => actualRequest = r);

            var expectedResourceParameter = string.Format(
                "{0}/show_many.json?ids={1}", 
                typeof(TModel).GetTypeNameAsCPlusPlusStyle().Pluralize(), 
                string.Join(",", new[] { 1, 2 }));

            // act
            var actualObjects = this.Manager.GetMany(new[] { 1, 2 }).Take(2).ToList();

            // assert
            actualRequest.Should().NotBeNull();

            actualRequest.Resource.Should().Be(expectedResourceParameter);
            actualRequest.Method.Should().Be(Method.GET);

            actualObjects.Should().NotBeEmpty().And.HaveCount(2).And.ContainInOrder(pageResponse.Data.Collection);
        }

        [Fact]
        public void Get_WhenForced_ExpectRequestMade()
        {
            // arrange
            var responseMock = new Mock<IRestResponse<TModel>>();
            responseMock.SetupProperty(x => x.Data, this.ManagerFixture.Model)
                .SetupProperty(x => x.StatusCode, HttpStatusCode.OK)
                .SetupProperty(x => x.Request, new RestRequest { Method = Method.GET });

            IRestRequest actualRequest = null;
            this.ClientMock.Setup(x => x.Execute<TModel>(It.IsAny<IRestRequest>()))
                .Returns(responseMock.Object)
                .Callback<IRestRequest>(r => actualRequest = r);

            // act
            var actualItem = this.Manager.Get(ExpectedId, true);

            // assert
            this.ClientMock.Verify(x => x.Execute<TModel>(It.IsAny<IRestRequest>()), Times.Once);
            actualRequest.Should().NotBeNull();
            actualRequest.Method.Should().Be(Method.GET);
            var expectedResource = string.Format(
                "{0}/{1}.json", 
                typeof(TModel).GetTypeNameAsCPlusPlusStyle().Pluralize(), 
                ExpectedId);
            actualRequest.Resource.Should().Be(expectedResource);
            actualItem.ShouldBeSameAs(this.ManagerFixture.Model);
        }

        [Fact]
        public void Get_WhenNoCacheAndNotForced_ExpectRequestMade()
        {
            // arrange
            this.SetupOkResponse();
            this.SetupVerifiableClientExecute();
            this.Manager.Client = this.ClientMock.Object;

            // act
            var actualItem = this.Manager.Get(ExpectedId, false);

            // assert
            this.ClientMock.Verify(x => x.Execute<TModel>(It.IsAny<IRestRequest>()), Times.Once);
            actualItem.ShouldBeSameAs(this.ManagerFixture.Model);
        }

        [Fact]
        public void Get_WhenOkResponse_ExpectAddedToCache()
        {
            // arrange
            this.SetupOkResponse();
            this.SetupVerifiableClientExecute();

            // act
            this.Manager.Get(ExpectedId, true);

            // assert
            this.Manager.Cache.ContainsKey(ExpectedId).Should().BeTrue();
        }

        [Fact]
        public void Get_WithCachedModelNotForced_ExpectRetrievedFromCache()
        {
            // arrange
            this.Manager.Cache[ExpectedId] = this.ManagerFixture.Model;

            // act
            var actualItem = this.Manager.Get(ExpectedId, false);

            // assert
            actualItem.Should().BeSameAs(this.ManagerFixture.Model);
        }

        public void SetFixture(ManagerFixture<TModel> data)
        {
            this.ManagerFixture = data;
        }

        [Fact]
        public void SubmimtUpdatesFor_WhenNotSubmittedObject_ExpectSharpZendeskException()
        {
            // arrange
            var model = new TModel();

            // act & assert
            this.Manager.Invoking(x => x.SubmitUpdatesFor(model))
                .ShouldThrow<SharpZendeskException>()
                .WithMessage("Cannot perform this operation. The object has not yet been submitted to Zendesk!");
        }

        [Fact]
        public void SubmitNew_GivenObjectWithNullMandatoryProperties_ExpectMandatoryPropertyNullValueException()
        {
            // arrange
            var model = new TModel();

            // act & assert
            this.Manager.Invoking(x => x.SubmitNew(model)).ShouldThrow<MandatoryPropertyNullValueException>();
        }

        public abstract void SubmitNew_UsingParameterizedConstructor_ExpectSuccess();

        [Fact]
        public void SubmitNew_WithAlreadySubmittedObject_ExpectArgumentException()
        {
            // arrange
            var model = new TModel { WasSubmitted = true };

            // act & assert
            this.Manager.Invoking(x => x.SubmitNew(model))
                .ShouldThrow<SharpZendeskException>()
                .WithMessage("Cannot perform this operation. The object has already been submitted to Zendesk!");
        }

        [Fact]
        public void SubmitNew_WithNull_ExpectArgumentNullException()
        {
            // act & assert
            this.Manager.Invoking(x => x.SubmitNew(null)).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void SubmitUpdatesFor_WithNull_ExpectArgumentNullException()
        {
            // act & assert
            this.Manager.Invoking(x => x.SubmitUpdatesFor(null)).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void SubmitUpdatesFor_WithValidRequestAndExistingObject_ShouldReturnSuccessful()
        {
            // arrange
            var model = this.ManagerFixture.Fixture.Create<TModel>();
            var modelAsTrackable = model as TrackableZendeskThingBase;
            modelAsTrackable.Id = 1;
            modelAsTrackable.WasSubmitted = true;
            modelAsTrackable.ChangedProperties.Add("foo");

            this.SetupOkResponse();

            IRestRequest actualRequest = null;
            this.SetupVerifiableClientExecuteGetActualRequest(x => actualRequest = x);

            const string JsonBodyInput = "{\"test\"=1}";
            const string ExpectedJsonBody = "application/json=" + JsonBodyInput;

            TrackableZendeskThingBase actualSerializedObject = null;
            var fakeSerializerMock = new Mock<IZendeskSerializer>();
            fakeSerializerMock.Setup(x => x.Serialize(It.IsAny<TrackableZendeskThingBase>()))
                .Callback<TrackableZendeskThingBase>(x => actualSerializedObject = x)
                .Returns(ExpectedJsonBody)
                .Verifiable();

            this.ClientMock.Object.Container.Register(
                fakeSerializerMock.Object, 
                SerializationScenario.Create.ToString());

            const string ExpectedResource = "tickets/1.json";

            // act
            this.Manager.SubmitUpdatesFor(model);

            // assert
            actualRequest.Should().NotBeNull();
            actualRequest.Method.Should().Be(Method.PUT);
            actualRequest.Resource.Should().Be(ExpectedResource);
            actualRequest.Parameters.First(x => x.Type == ParameterType.RequestBody).Value.Should().Be(ExpectedJsonBody);
            actualSerializedObject.ShouldBeSameAs(model);
        }

        [Fact]
        public void TryGet_WhenExistsFalse_ExpectNullAndFalse()
        {
            // arrange
            var mockManager = this.GetMockManager();
            mockManager.Setup(x => x.Exists(ExpectedId)).Returns(false).Verifiable();

            // act
            TInterface actualModel;
            var actualResult = mockManager.Object.TryGet(ExpectedId, out actualModel);

            // assert
            actualResult.Should().BeFalse();
            actualModel.Should().BeNull();
            mockManager.Verify(x => x.Exists(ExpectedId), Times.Once());
        }

        [Fact]
        public void TryGet_WhenExistsTrue_ExpectModelRetrievedFromCacheAndReturnTrue()
        {
            // arrange
            var mockManager = this.GetMockManager();
            mockManager.Setup(x => x.Exists(It.IsAny<int>()))
                .Callback(() => Debug.Write("MOCKED Exists"))
                .Returns(true)
                .Verifiable();
            mockManager.Object.Cache[ExpectedId] = this.ManagerFixture.Model;

            // act
            TInterface actualModel;
            var actualResult = mockManager.Object.TryGet(ExpectedId, out actualModel);

            // assert
            mockManager.Verify(x => x.Exists(ExpectedId), Times.Once());
            actualResult.Should().BeTrue();
            actualModel.Should().BeSameAs(this.ManagerFixture.Model);
        }

        #endregion

        #region Methods

        internal IRestResponse<IPage<TInterface>> GetPageResponse(int numberOfObjects)
        {
            var objects = new List<TInterface>();
            for (var i = 1; i <= numberOfObjects; i++)
            {
                int temp = i;
                var objectToAdd = new TModel { Id = temp };
                objects.Add(objectToAdd);
            }

            return
                Mock.Of<IRestResponse<IPage<TInterface>>>(
                    pgRsp =>
                    pgRsp.StatusCode == HttpStatusCode.OK
                    && pgRsp.Data == Mock.Of<IPage<TInterface>>(pg => pg.Collection == objects)
                    && pgRsp.Request == new RestRequest(Method.GET));
        }

        protected Mock<TManager> GetMockManager(bool callBase = true)
        {
            return new Mock<TManager>(this.ClientMock.Object) { CallBase = callBase };
        }

        protected void SetupOkResponse(Method method = Method.GET)
        {
            this.ResponseMock.SetupProperty(x => x.Data, this.ManagerFixture.Model)
                .SetupProperty(x => x.StatusCode, HttpStatusCode.OK)
                .SetupProperty(x => x.Request, Mock.Of<IRestRequest>(x => x.Method == method))
                .SetupProperty(x => x.Data, this.ManagerFixture.Model);
        }

        protected void SetupVerifiableClientExecute()
        {
            this.SetupVerifiableClientExecuteGetActualRequest(x => { });
        }

        protected void SetupVerifiableClientExecuteGetActualRequest(Action<IRestRequest> callBackAction)
        {
            this.ClientMock.Setup(x => x.Execute<TModel>(It.IsAny<IRestRequest>()))
                .Returns(this.ResponseMock.Object)
                .Callback(callBackAction)
                .Verifiable();
        }

        #endregion
    }
}