namespace SharpZendeskApi.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class ViewOutput : INotifyPropertyChanged
    {
        #region Fields

        private string[] columns;

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public IEnumerable<string> Columns
        {
            get
            {
                return this.columns != null ? this.columns.ToArray() : null;
            }

            set
            {
                this.columns = value != null ? value.ToArray() : new string[0];
                this.OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}