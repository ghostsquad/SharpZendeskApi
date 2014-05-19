// .\GetZendeskModelDataFromUrl.ps1 http://developer.zendesk.com/documentation/rest_api/views.html
// Views are represented as simple flat JSON objects which have the following keys.
// Name        Type       Description
// ----        ----       -----------
// active      boolean    Useful for determining if the view should be displayed
// conditions  Conditions An object describing how the view is constructed
// created_at  date       The time the view was created
// execution   Execute    An object describing how the view should be executed
// id          integer    Automatically assigned when created
// restriction object     Who may access this account. Will be null when everyone in the account can access it.
// sla_id      integer    If the view is for an SLA this is the id
// title       string     The title of the view
// updated_at  date       The time of the last update of the view
namespace SharpZendeskApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.Unity;

    using SharpZendeskApi.Models.Attributes;

    /// <summary>
    ///     The view.
    /// </summary>
    public class View : TrackableZendeskThingBase, IView
    {
        #region Fields

        private bool? active;

        private Condition[] all;

        private Condition[] any;

        private ViewOutput output;

        private Restriction restriction;

        private int? slaId;

        private string title;

        #endregion

        #region Constructors and Destructors

        [InjectionConstructor]
        public View()
        {
            this.Restriction = new Restriction();
            this.Output = new ViewOutput();
            this.ChangedPropertiesSet.Clear();
        }

        public View(Condition[] all, string title)
        {
            this.all = all;
            this.title = title;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether active.
        /// </summary>
        public bool? Active
        {
            get
            {
                return this.active;
            }

            set
            {
                this.active = value;
                this.NotifyPropertyChanged();
            }
        }

        [Mandatory]
        public IEnumerable<Condition> All
        {
            get
            {
                return this.all != null ? this.all.ToArray() : null;
            }

            set
            {
                this.all = value != null ? value.ToArray() : new Condition[0];
                this.NotifyPropertyChanged();
            }
        }

        public IEnumerable<Condition> Any
        {
            get
            {
                return this.any != null ? this.any.ToArray() : null;
            }

            set
            {
                this.any = value != null ? value.ToArray() : new Condition[0];
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the conditions.
        /// </summary>
        [ReadOnly]
        public Conditions Conditions { get; set; }

        /// <summary>
        ///     Gets or sets the created at.
        /// </summary>
        [ReadOnly]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        ///     Gets or sets the execution.
        /// </summary>
        [ReadOnly]
        public Execution Execution { get; set; }

        public ViewOutput Output
        {
            get
            {
                return this.output;
            }

            set
            {
                this.output = value;
                if (value != null)
                {
                    this.Output.PropertyChanged += this.OnPropertyChanged;
                }

                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the restriction.
        /// </summary>
        public Restriction Restriction
        {
            get
            {
                return this.restriction;
            }

            set
            {
                this.restriction = value;
                if (value != null)
                {
                    this.Restriction.PropertyChanged += this.OnPropertyChanged;
                }
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the sla id.
        /// </summary>
        public int? SlaId
        {
            get
            {
                return this.slaId;
            }

            set
            {
                this.slaId = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        [Mandatory]
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the updated at.
        /// </summary>
        [ReadOnly]
        public DateTime? UpdatedAt { get; set; }

        #endregion
    }
}