namespace SharpZendeskApi.Core
{
    using SharpZendeskApi.Core.Models;

    public interface IZendeskSerializer
    {
        #region Public Properties

        #endregion

        #region Public Methods and Operators

        string Serialize(TrackableZendeskThingBase zendeskThing);

        #endregion
    }
}