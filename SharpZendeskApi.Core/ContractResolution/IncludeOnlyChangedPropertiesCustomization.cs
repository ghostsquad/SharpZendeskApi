namespace SharpZendeskApi.Core.ContractResolution
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Newtonsoft.Json.Serialization;

    internal class IncludeOnlyChangedPropertiesCustomization : IContractResolverCustomization
    {
        #region Constructors and Destructors

        public IncludeOnlyChangedPropertiesCustomization(HashSet<string> changedProperties)
        {
            if (changedProperties == null)
            {
                throw new ArgumentNullException("changedProperties");
            }

            this.ChangedProperties = changedProperties;
        }

        #endregion

        #region Public Properties

        public HashSet<string> ChangedProperties { get; private set; }

        public Predicate<JsonProperty> ExcludeJsonPropertyPredicate
        {
            get
            {
                return null;
            }
        }

        public Predicate<MemberInfo> ExcludeMemberInfoPredicate
        {
            get
            {
                return null;
            }
        }

        public Predicate<JsonProperty> IncludeJsonPropertyPredicate
        {
            get
            {
                return null;
            }
        }

        public Predicate<MemberInfo> IncludeMemberInfoPredicate
        {
            get
            {
                return x => this.ChangedProperties.Contains(x.Name);
            }
        }

        public Action<JsonProperty> Modification
        {
            get
            {
                return null;
            }
        }

        #endregion
    }
}