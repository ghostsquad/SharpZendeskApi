namespace SharpZendeskApi.Test.Fakes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    using Newtonsoft.Json.Serialization;

    using SharpZendeskApi.ContractResolution;

    public class ExcludeIfEndsIn1Customization : IContractResolverCustomization
    {
        #region Public Properties

        public Predicate<JsonProperty> ExcludeJsonPropertyPredicate
        {
            get
            {
                return p => p.PropertyName.EndsWith("1");
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