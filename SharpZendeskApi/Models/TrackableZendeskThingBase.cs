namespace SharpZendeskApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using Newtonsoft.Json;

    public abstract class TrackableZendeskThingBase : IZendeskThing, ITrackable
    {
        #region Constructors and Destructors

        protected TrackableZendeskThingBase()
        {
            this.ChangedPropertiesSet = new HashSet<string>();
        }

        #endregion

        #region Public Properties

        [JsonIgnore]
        public IEnumerable<string> ChangedProperties
        {
            get
            {
                return this.ChangedPropertiesSet.ToList();
            }
        }

        [Attributes.ReadOnly]
        public int? Id { get; set; }

        [JsonIgnore]
        public bool WasSubmitted { get; internal set; }

        #endregion

        #region Properties

        internal HashSet<string> ChangedPropertiesSet { get; set; }

        #endregion

        #region Methods

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.WasSubmitted)
            {
                if (propertyName != null && !this.ChangedProperties.Contains(propertyName))
                {
                    this.ChangedPropertiesSet.Add(propertyName);
                }
            }
        }

        protected void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            this.NotifyPropertyChanged(propertyChangedEventArgs.PropertyName);
        }

        #endregion
    }
}