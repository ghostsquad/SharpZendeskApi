namespace SharpZendeskApi.Core.ContractResolution
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Newtonsoft.Json.Serialization;

    using SharpZendeskApi.Core.Models.Attributes;

    internal class ExcludeReadOnlyUnlessMandatoryCustomization : IContractResolverCustomization
    {
        #region Static Fields

        private static readonly object SyncRoot = new object();

        private static volatile ExcludeReadOnlyUnlessMandatoryCustomization Instance;

        #endregion

        #region Constructors and Destructors

        private ExcludeReadOnlyUnlessMandatoryCustomization()
        {
        }

        #endregion

        #region Public Properties

        public static ExcludeReadOnlyUnlessMandatoryCustomization Default
        {
            get
            {
                if (Instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (Instance == null)
                        {
                            Instance = new ExcludeReadOnlyUnlessMandatoryCustomization();
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
                return m =>
                    {
                        var customAttributes = m.GetCustomAttributes(true);
                        return customAttributes.All(x => x.GetType() != typeof(MandatoryAttribute))
                               && customAttributes.Any(x => x.GetType() == typeof(ReadOnlyAttribute));
                    };
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