namespace SharpZendeskApi.Core.ContractResolution
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Newtonsoft.Json.Serialization;

    using SharpZendeskApi.Core.Models.Attributes;

    public class ExcludeReadOnlyCustomization : IContractResolverCustomization
    {
        private static volatile ExcludeReadOnlyCustomization instance;
        private static readonly object SyncRoot = new Object();

        private ExcludeReadOnlyCustomization() { }

        public static ExcludeReadOnlyCustomization Default
        {
            get 
            {
                if (instance == null) 
                {
                lock (SyncRoot) 
                {
                    if (instance == null)
                    {
                        instance = new ExcludeReadOnlyCustomization();
                    }
                }
                }

                return instance;
            }
        }

        public Predicate<JsonProperty> IncludeJsonPropertyPredicate { get; private set; }

        public Predicate<MemberInfo> IncludeMemberInfoPredicate { get; private set; }

        public Predicate<JsonProperty> ExcludeJsonPropertyPredicate { get; private set; }

        public Predicate<MemberInfo> ExcludeMemberInfoPredicate
        {
            get
            {
                return m => m.GetCustomAttributes(typeof(ReadOnlyAttribute), true).Any();
            }
        }

        public Action<JsonProperty> Modification { get; private set; }
    }
}
