/* Updating Serializer should perform the following
 * 1. Ignore changed properties
 * 2. Ignore nulls (unset properties)
 * 3. Json properties should use the CStyle naming convention which is
 *    http://google-styleguide.googlecode.com/svn/trunk/cppguide.xml#General_Naming_Rules
 *      a. all lowercase
 *      b. separate words with underscore
 * 4. ReadOnly properties should not be included (as those will not be accepted for updates)
 * 5. Json.Net does not natively wrap the object with a RootName
 *    (e.g. Foo should like { "foo": { ... } }
 *    So we will do that manually
 */
namespace SharpZendeskApi
{
    using Newtonsoft.Json;

    using SharpZendeskApi.ContractResolution;
    using SharpZendeskApi.Models;

    internal class UpdatingSerializer : IZendeskSerializer
    {
        #region Constructors and Destructors

        public UpdatingSerializer()
        {
            this.Resolver = new CustomizableSerializationContractResolver();
            this.Settings = new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore,
                                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                                    ContractResolver = this.Resolver
                                };

            this.Resolver.Customize(CStyleNamingCustomization.Default).Customize(ExcludeReadOnlyCustomization.Default);
        }

        #endregion

        #region Properties

        internal CustomizableSerializationContractResolver Resolver { get; private set; }

        internal JsonSerializerSettings Settings { get; private set; }

        #endregion

        #region Public Methods and Operators

        public string Serialize(TrackableZendeskThingBase zendeskThing)
        {
            this.Resolver.Customize(new IncludeOnlyChangedPropertiesCustomization(zendeskThing.ChangedProperties));

            var jsonBody = JsonConvert.SerializeObject(zendeskThing, this.Settings);
            var jsonBodyWrapped = jsonBody.WrapSerializedStringInTypeRoot(zendeskThing.GetType());

            return jsonBodyWrapped;
        }

        #endregion
    }
}