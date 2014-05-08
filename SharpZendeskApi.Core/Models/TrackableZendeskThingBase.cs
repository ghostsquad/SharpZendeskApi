namespace SharpZendeskApi.Core.Models
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    using SharpZendeskApi.Core.Models.Attributes;

    public abstract class TrackableZendeskThingBase : IZendeskThing
    {
        #region Constructors and Destructors

        public TrackableZendeskThingBase()
        {
            this.ChangedProperties = new HashSet<string>();
        }

        #endregion

        #region Public Properties

        [ReadOnly]
        public int? Id { get; set; }

        #endregion

        #region Properties

        internal HashSet<string> ChangedProperties { get; set; }

        internal bool WasSubmitted { get; set; }

        #endregion

        #region Methods

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.WasSubmitted)
            {
                if (propertyName != null && !this.ChangedProperties.Contains(propertyName))
                {
                    this.ChangedProperties.Add(propertyName);
                }
            }
        }

        #endregion
    }
}