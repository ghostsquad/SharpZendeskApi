namespace SharpZendeskApi.Models
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Newtonsoft.Json;

    /// <summary>
    /// The restriction.
    /// </summary>
    public class Restriction : IZendeskThing, INotifyPropertyChanged
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int? Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
                this.OnPropertyChanged();
            }
        }

        private int? id;

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;
                this.OnPropertyChanged();
            }
        }

        private string type;

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}