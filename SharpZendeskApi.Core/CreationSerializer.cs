namespace SharpZendeskApi.Core
{
    using Newtonsoft.Json;

    using SharpZendeskApi.Core.ContractResolution;
    using SharpZendeskApi.Core.Models;

    internal class CreationSerializer : IZendeskSerializer
    {
        #region Constructors and Destructors

        public CreationSerializer()
        {
            this.Resolver =
                new CustomizableSerializationContractResolver().Customize(CStyleNamingCustomization.Default)
                    .Customize(ExcludeReadOnlyUnlessMandatoryCustomization.Default);

            this.Settings = new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore,
                                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                                    ContractResolver = this.Resolver
                                };
        }

        #endregion

        #region Properties

        internal CustomizableSerializationContractResolver Resolver { get; private set; }

        internal JsonSerializerSettings Settings { get; private set; }

        #endregion

        #region Public Methods and Operators

        public string Serialize(TrackableZendeskThingBase zendeskThing)
        {
            var jsonBody = JsonConvert.SerializeObject(zendeskThing, this.Settings);
            var jsonBodyWrapped = jsonBody.WrapSerializedStringInTypeRoot(zendeskThing.GetType());

            return jsonBodyWrapped;
        }

        #endregion
    }
}