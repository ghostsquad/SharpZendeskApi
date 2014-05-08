namespace SharpZendeskApi.Test.Fakes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using Newtonsoft.Json.Serialization;

    using SharpZendeskApi.ContractResolution;
    using SharpZendeskApi.Models.Attributes;

    public class IncludeAttributeCustomization : IContractResolverCustomization
    {
        #region Public Properties

        [ExcludeFromCodeCoverage]
        public Predicate<JsonProperty> ExcludeJsonPropertyPredicate
        {
            get
            {
                return null;
            }
        }

        [ExcludeFromCodeCoverage]
        public Predicate<MemberInfo> ExcludeMemberInfoPredicate
        {
            get
            {
                return null;
            }
        }

        [ExcludeFromCodeCoverage]
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
                return m => m.GetCustomAttributes(typeof(ReadOnlyAttribute), true).Any();
            }
        }

        [ExcludeFromCodeCoverage]
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