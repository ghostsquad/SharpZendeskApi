namespace SharpZendeskApi.Core.ContractResolution
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Newtonsoft.Json.Serialization;

    using SharpZendeskApi.Core.Models.Attributes;

    internal class ExcludeReadOnlyCustomization : IContractResolverCustomization
    {
        #region Static Fields

        private static readonly object SyncRoot = new object();

        private static volatile ExcludeReadOnlyCustomization Instance;

        #endregion

        #region Constructors and Destructors

        private ExcludeReadOnlyCustomization()
        {
        }

        #endregion

        #region Public Properties

        public static ExcludeReadOnlyCustomization Default
        {
            get
            {
                if (Instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (Instance == null)
                        {
                            Instance = new ExcludeReadOnlyCustomization();
                        }
                    }
                }

                return Instance;
            }
        }

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