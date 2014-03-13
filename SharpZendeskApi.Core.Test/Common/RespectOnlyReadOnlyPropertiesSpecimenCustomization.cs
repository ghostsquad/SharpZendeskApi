namespace SharpZendeskApi.Core.Test.Common
{
    using System.Linq;
    using System.Reflection;

    using Ploeh.AutoFixture.Kernel;

    using SharpZendeskApi.Core.Models.Attributes;

    public class RespectOnlyReadOnlyPropertiesSpecimenCustomization : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var requestAsPropertyInfo = request as PropertyInfo;
            if (requestAsPropertyInfo == null)
            {
                return new NoSpecimen(request);
            }

            var readOnlyAttributes = requestAsPropertyInfo.GetCustomAttributes(typeof(ReadOnlyAttribute), true);
            return readOnlyAttributes.Any() ? new NoSpecimen(request) : null;
        }
    }
}
