namespace SharpZendeskApi.Core.ContractResolution
{
    using System;
    using System.Reflection;

    using Newtonsoft.Json.Serialization;

    internal interface IContractResolverCustomization
    {
        #region Public Properties

        Predicate<JsonProperty> ExcludeJsonPropertyPredicate { get; }

        Predicate<MemberInfo> ExcludeMemberInfoPredicate { get; }

        Predicate<JsonProperty> IncludeJsonPropertyPredicate { get; }

        Predicate<MemberInfo> IncludeMemberInfoPredicate { get; }

        Action<JsonProperty> Modification { get; }

        #endregion
    }
}