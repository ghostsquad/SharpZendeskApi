namespace SharpZendeskApi.Core.Test.Fakes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using Newtonsoft.Json.Serialization;

    using SharpZendeskApi.Core.ContractResolution;
    using SharpZendeskApi.Core.Models.Attributes;

    public class ExcludeAttributeCustomization : IContractResolverCustomization
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

        public Predicate<MemberInfo> ExcludeMemberInfoPredicate
        {
            get
            {
                return m => m.GetCustomAttributes(typeof(ReadOnlyAttribute), true).Any();
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

        [ExcludeFromCodeCoverage]
        public Predicate<MemberInfo> IncludeMemberInfoPredicate
        {
            get
            {
                return null;
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