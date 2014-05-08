namespace SharpZendeskApi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using SharpZendeskApi.Exceptions;
    using SharpZendeskApi.Models;
    using SharpZendeskApi.Models.Attributes;

    internal static class ZendeskThingExtensions
    {
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<T>(this IZendeskThing zendeskThing)
            where T : IZendeskSpecialAttribute
        {
            return zendeskThing.GetType().GetProperties()
                    .Where(p => p.GetCustomAttributes(typeof(T), true).Length > 0)
                    .ToList();
        }

        public static void ThrowIfAnyMandatoryPropertyIsNull(this IZendeskThing zendeskThing)
        {
            var nullValuedProperties =
                zendeskThing.GetPropertiesWithAttribute<MandatoryAttribute>()
                    .Where(x => x.GetValue(zendeskThing) == null).ToArray();

            if (nullValuedProperties.Any())
            {
                throw new MandatoryPropertyNullValueException(nullValuedProperties);
            }
        }

        public static void ThrowIfNotSubmitted(this TrackableZendeskThingBase zendeskThing)
        {
            if (!zendeskThing.WasSubmitted)
            {
                throw new SharpZendeskException("Cannot perform this operation. The object has not yet been submitted to Zendesk!");
            }
        }

        public static void ThrowIfSubmitted(this TrackableZendeskThingBase zendeskThing)
        {
            if (zendeskThing.WasSubmitted)
            {
                throw new SharpZendeskException("Cannot perform this operation. The object has already been submitted to Zendesk!");
            }
        }
    }
}