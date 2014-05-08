namespace SharpZendeskApi.Test.Common
{
    using Ploeh.AutoFixture;

    using RestSharp;

    /// <summary>
    /// The test json object base.
    /// </summary>
    public abstract class JsonTestObjectBase
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get json object.
        /// </summary>
        /// <param name="fixture">
        /// The fixture.
        /// </param>
        /// <returns>
        /// The <see cref="JsonObject"/>.
        /// </returns>
        public abstract JsonObject GetJsonObject(IFixture fixture);

        #endregion
    }
}