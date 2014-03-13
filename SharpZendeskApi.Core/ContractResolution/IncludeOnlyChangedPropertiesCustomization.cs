namespace SharpZendeskApi.Core.ContractResolution
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Newtonsoft.Json.Serialization;

    public class IncludeOnlyChangedPropertiesCustomization : IContractResolverCustomization
    {       
        private HashSet<string> changedProperties;

        public IncludeOnlyChangedPropertiesCustomization(HashSet<string> changedProperties)
        {
            if (changedProperties == null)
            {
                throw new ArgumentNullException("changedProperties");
            }

            this.changedProperties = changedProperties;
        }

        public Predicate<JsonProperty> IncludeJsonPropertyPredicate { get; private set; }

        public Predicate<MemberInfo> IncludeMemberInfoPredicate
        {
            get
            {
                return x => this.changedProperties.Contains(x.Name);
            }
        }

        public Predicate<JsonProperty> ExcludeJsonPropertyPredicate { get; private set; }

        public Predicate<MemberInfo> ExcludeMemberInfoPredicate { get; private set; }

        public Action<JsonProperty> Modification { get; private set; }
    }
}