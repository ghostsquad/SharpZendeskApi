namespace SharpZendeskApi.Test.Fakes
{
    using System;
    using System.Reflection;

    using Newtonsoft.Json.Serialization;

    using SharpZendeskApi.ContractResolution;

    internal class NullCustomization : IContractResolverCustomization
    {
        #region Public Properties

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
                return null;
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