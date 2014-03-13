namespace SharpZendeskApi.Core.ContractResolution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class CustomizableSerializationContractResolver : DefaultContractResolver
    {
        #region Fields

        private Predicate<JsonProperty>[] jsonPropertyExclusions;

        private Predicate<JsonProperty>[] jsonPropertyInclusions;

        private Predicate<MemberInfo>[] memberInfoExclusions;

        private Predicate<MemberInfo>[] memberInfoInclusions;

        private Action<JsonProperty>[] modifications;

        #endregion

        #region Constructors and Destructors

        public CustomizableSerializationContractResolver()
        {
            this.Customizations = new List<IContractResolverCustomization>();
        }

        public CustomizableSerializationContractResolver(IContractResolverCustomization customization)
        {
            if (customization == null)
            {
                throw new ArgumentNullException("customization");
            }

            this.Setup(new[] { customization });
        }

        public CustomizableSerializationContractResolver(IEnumerable<IContractResolverCustomization> customizations)
        {
            if (customizations == null)
            {
                throw new ArgumentNullException("customizations");
            }

            this.Setup(customizations);
        }

        #endregion

        #region Public Properties

        public IList<IContractResolverCustomization> Customizations { get; set; }

        #endregion

        #region Methods

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            /* we are allowed to return null here if we want to exclude this property from being serialized
             * https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Serialization/DefaultContractResolver.cs
             * See protected virtual IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
             */
            if (this.memberInfoInclusions.Any(x => !x(member)))
            {
                return null;
            }

            if (this.memberInfoExclusions.Any(x => x(member)))
            {
                return null;
            }

            var property = base.CreateProperty(member, memberSerialization);

            if (this.jsonPropertyInclusions.Any(x => !x(property)))
            {
                return null;
            }

            if (this.jsonPropertyExclusions.Any(x => x(property)))
            {
                return null;
            }

            foreach (var modification in this.modifications)
            {
                modification(property);
            }

            return property;
        }

        private void Setup(IEnumerable<IContractResolverCustomization> customizations)
        {
            this.Customizations = customizations.ToList();

            // TODO make this suck less
            this.memberInfoInclusions =
                this.Customizations.Select(x => x.IncludeMemberInfoPredicate).Where(x => x != null).ToArray();
            this.memberInfoExclusions =
                this.Customizations.Select(x => x.ExcludeMemberInfoPredicate).Where(x => x != null).ToArray();
            this.jsonPropertyInclusions =
                this.Customizations.Select(x => x.IncludeJsonPropertyPredicate).Where(x => x != null).ToArray();
            this.jsonPropertyExclusions =
                this.Customizations.Select(x => x.ExcludeJsonPropertyPredicate).Where(x => x != null).ToArray();
            this.modifications = this.Customizations.Select(x => x.Modification).Where(x => x != null).ToArray();
        }

        #endregion
    }
}