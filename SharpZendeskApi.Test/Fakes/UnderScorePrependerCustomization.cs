namespace SharpZendeskApi.Test.Fakes
{
    using System;
    using System.Reflection;

    using Newtonsoft.Json.Serialization;

    using SharpZendeskApi.ContractResolution;

    internal class UnderScorePrependerCustomization : IContractResolverCustomization
    {
        #region Public Properties

        public Predicate<JsonProperty> ExcludeJsonPropertyPredicate { get; private set; }

        public Predicate<MemberInfo> ExcludeMemberInfoPredicate { get; private set; }

        public Predicate<JsonProperty> IncludeJsonPropertyPredicate { get; private set; }

        public Predicate<MemberInfo> IncludeMemberInfoPredicate { get; private set; }

        public Action<JsonProperty> Modification
        {
            get
            {
                return PrependUnderscore;
            }
        }

        #endregion

        #region Methods

        private static void PrependUnderscore(JsonProperty property)
        {
            property.PropertyName = "_" + property.PropertyName;
        }

        #endregion
    }
}