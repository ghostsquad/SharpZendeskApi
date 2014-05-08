namespace SharpZendeskApi.Core.ContractResolution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    internal class CustomizableSerializationContractResolver : DefaultContractResolver
    {
        #region Fields

        private readonly List<Predicate<JsonProperty>> jsonPropertyExclusions;

        private readonly List<Predicate<JsonProperty>> jsonPropertyInclusions;

        private readonly List<Predicate<MemberInfo>> memberInfoExclusions;

        private readonly List<Predicate<MemberInfo>> memberInfoInclusions;

        private readonly List<Action<JsonProperty>> modifications;

        #endregion

        #region Constructors and Destructors

        public CustomizableSerializationContractResolver()
        {
            this.Customizations = new List<IContractResolverCustomization>();
            this.jsonPropertyExclusions = new List<Predicate<JsonProperty>>();
            this.jsonPropertyInclusions = new List<Predicate<JsonProperty>>();
            this.memberInfoExclusions = new List<Predicate<MemberInfo>>();
            this.memberInfoInclusions = new List<Predicate<MemberInfo>>();
            this.modifications = new List<Action<JsonProperty>>();
        }

        #endregion

        #region Properties

        internal IList<IContractResolverCustomization> Customizations { get; set; }

        #endregion

        #region Public Methods and Operators

        public CustomizableSerializationContractResolver Customize(IContractResolverCustomization customization)
        {
            if (customization == null)
            {
                throw new ArgumentNullException("customization");
            }

            this.Customizations.Add(customization);

            if (customization.ExcludeJsonPropertyPredicate != null)
            {
                this.jsonPropertyExclusions.Add(customization.ExcludeJsonPropertyPredicate);
            }

            if (customization.IncludeJsonPropertyPredicate != null)
            {
                this.jsonPropertyInclusions.Add(customization.IncludeJsonPropertyPredicate);
            }

            if (customization.ExcludeMemberInfoPredicate != null)
            {
                this.memberInfoExclusions.Add(customization.ExcludeMemberInfoPredicate);
            }

            if (customization.IncludeMemberInfoPredicate != null)
            {
                this.memberInfoInclusions.Add(customization.IncludeMemberInfoPredicate);
            }

            if (customization.Modification != null)
            {
                this.modifications.Add(customization.Modification);
            }

            return this;
        }

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

        #endregion
    }
}