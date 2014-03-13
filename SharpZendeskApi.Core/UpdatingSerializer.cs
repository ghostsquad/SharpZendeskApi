namespace SharpZendeskApi.Core
{
    using Newtonsoft.Json;

    using SharpZendeskApi.Core.ContractResolution;
    using SharpZendeskApi.Core.Models;

    public class UpdatingSerializer : IZendeskSerializer
    {
        public string Serialize(TrackableZendeskThingBase zendeskThing)
        {
            var settings = new JsonSerializerSettings();

            var updateContractResolver =
                new CustomizableSerializationContractResolver(
                    new IContractResolverCustomization[]
                        {
                            CStyleNamingCustomization.Default,
                            ExcludeReadOnlyCustomization.Default,
                            new IncludeOnlyChangedPropertiesCustomization(zendeskThing.ChangedProperties)
                        });

            settings.ContractResolver = updateContractResolver;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;

            var jsonBody = JsonConvert.SerializeObject(zendeskThing, settings);
            var jsonBodyWrapped = jsonBody.WrapSerializedStringInTypeRoot(zendeskThing.GetType());

            return jsonBodyWrapped;
        }
    }
}
