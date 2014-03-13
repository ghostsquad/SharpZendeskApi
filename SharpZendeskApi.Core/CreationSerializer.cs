namespace SharpZendeskApi.Core
{
    using Newtonsoft.Json;

    using SharpZendeskApi.Core.ContractResolution;
    using SharpZendeskApi.Core.Models;

    public class CreationSerializer : IZendeskSerializer
    {
        public string Serialize(TrackableZendeskThingBase zendeskThing)
        {
            var settings = new JsonSerializerSettings();

            var createContractResolver =
                new CustomizableSerializationContractResolver(
                    new IContractResolverCustomization[]
                        {
                            CStyleNamingCustomization.Default,
                            ExcludeReadOnlyUnlessMandatoryCustomization.Default
                        });

            settings.ContractResolver = createContractResolver;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;

            var jsonBody = JsonConvert.SerializeObject(zendeskThing, settings);
            var jsonBodyWrapped = jsonBody.WrapSerializedStringInTypeRoot(zendeskThing.GetType());

            return jsonBodyWrapped;
        }
    }
}
