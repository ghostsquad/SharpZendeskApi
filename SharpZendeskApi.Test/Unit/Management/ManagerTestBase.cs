namespace SharpZendeskApi.Test.Unit.Management
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

    using SharpZendeskApi.Exceptions;
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;    

    using Xunit;
    using Xunit.Should;

    public abstract class ManagerTestBase<TModel, TInterface, TManager> : IUseFixture<ManagerFixture<TModel>>
        where TModel : TrackableZendeskThingBase, TInterface, new()
        where TInterface : class, IZendeskThing, ITrackable
        where TManager : class, IManager<TInterface>
    {
        #region Constants

        protected const int ExpectedId = 1;

        #endregion

        #region Constructors and Destructors

        protected ManagerTestBase()
        {
            this.ClientMock = new Mock<IZendeskClient>();
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
        public virtual void GetMany_WithValidRequestAndExistingObject_ShouldReturnWithObject()
        {
            // arrange
            var pageResponse = this.GetPageResponse(2);

            IRestRequest actualRequest = null;
            this.ClientMock.Setup(x => x.Execute<IPage<TModel>>(It.IsAny<IRestRequest>()))
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
        public void Get_WhenValidRequest_ExpectRequestMade()
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
            var actualItem = this.Manager.Get(ExpectedId);

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
        public void SubmitUpdatesFor_WhenNoChangedProperties_ExpectNoChange()
        {
            // arrange
            var model = new TModel { WasSubmitted = true };
            this.ClientMock.Setup(x => x.Execute(It.IsAny<IRestRequest>())).Verifiable();

            // act
            this.Manager.SubmitUpdatesFor(model);

            // assert
            this.ClientMock.Verify(x => x.Execute<TModel>(It.IsAny<IRestRequest>()), Times.Never);
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

            this.ClientMock.Object.DeserializationResolver.Register(
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
        public void TryGet_WhenGetFails_ExpectNullAndFalse()
        {
            // arrange
            var mockManager = this.GetMockManager();
            mockManager.Setup(x => x.Get(ExpectedId)).Throws<SharpZendeskException>().Verifiable();

            // act
            TInterface actualModel;
            var actualResult = mockManager.Object.TryGet(ExpectedId, out actualModel);

            // assert
            actualResult.Should().BeFalse();
            actualModel.Should().BeNull();
            mockManager.Verify(x => x.Get(ExpectedId), Times.Once());
        }

        [Fact]
        public void TryGet_WhenGetSucceeds_ExpectOutAndReturnTrue()
        {
            // arrange
            var mockManager = this.GetMockManager();
            mockManager.Setup(x => x.Get(It.IsAny<int>()))
                .Callback(() => Debug.Write("MOCKED Exists"))
                .Returns(this.ManagerFixture.Model)
                .Verifiable();

            // act
            TInterface actualModel;
            var actualResult = mockManager.Object.TryGet(ExpectedId, out actualModel);

            // assert
            mockManager.Verify(x => x.Get(ExpectedId), Times.Once());
            actualResult.Should().BeTrue();
            actualModel.Should().BeSameAs(this.ManagerFixture.Model);
        }

        #endregion

        #region Methods

        internal IRestResponse<IPage<TModel>> GetPageResponse(int numberOfObjects)
        {
            var objects = new List<TModel>();
            for (var i = 1; i <= numberOfObjects; i++)
            {
                int temp = i;
                var objectToAdd = new TModel { Id = temp };
                objects.Add(objectToAdd);
            }

            return
                Mock.Of<IRestResponse<IPage<TModel>>>(
                    pgRsp =>
                    pgRsp.StatusCode == HttpStatusCode.OK
                    && pgRsp.Data == Mock.Of<IPage<TModel>>(pg => pg.Collection == objects)
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