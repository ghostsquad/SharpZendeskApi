namespace SharpZendeskApi.Core.ContractResolution
{
    using System;
    using System.Reflection;

    using Newtonsoft.Json.Serialization;

    public class CStyleNamingCustomization : IContractResolverCustomization
    {
        #region Static Fields

        private static readonly object SyncRoot = new object();

        private static volatile CStyleNamingCustomization Instance;

        #endregion

        #region Constructors and Destructors

        private CStyleNamingCustomization()
        {
        }

        #endregion

        #region Public Properties

        public static CStyleNamingCustomization Default
        {
            get
            {
                if (Instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (Instance == null)
                        {
                            Instance = new CStyleNamingCustomization();
                        }
                    }
                }

                return Instance;
            }
        }

        public Predicate<JsonProperty> ExcludeJsonPropertyPredicate { get; private set; }

        public Predicate<MemberInfo> ExcludeMemberInfoPredicate { get; private set; }

        public Predicate<JsonProperty> IncludeJsonPropertyPredicate { get; private set; }

        public Predicate<MemberInfo> IncludeMemberInfoPredicate { get; private set; }

        public Action<JsonProperty> Modification
        {
            get
            {
                return ConvertNameToCStyle;
            }
        }

        #endregion

        #region Methods

        private static void ConvertNameToCStyle(JsonProperty property)
        {
            property.PropertyName = property.PropertyName.ToCPlusPlusNamingStyle();
        }

        #endregion
    }
}