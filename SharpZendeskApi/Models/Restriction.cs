namespace SharpZendeskApi.Models
{
    /// <summary>
    /// The restriction.
    /// </summary>
    public class Restriction : IZendeskThing
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string Type { get; set; }

        #endregion
    }
}