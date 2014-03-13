namespace SharpZendeskApi.Core.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    [Serializable]
    public class MandatoryPropertyNullValueException : Exception
    {
        #region Constructors and Destructors

        public MandatoryPropertyNullValueException(
            string message, 
            Exception innerException, 
            IEnumerable<PropertyInfo> mandatoryProperties)
            : base(message, innerException)
        {
            this.MandatoryProperties = mandatoryProperties.ToArray();
        }

        public MandatoryPropertyNullValueException(string message, IEnumerable<PropertyInfo> mandatoryProperties)
            : base(message)
        {
            this.MandatoryProperties = mandatoryProperties.ToArray();
        }

        public MandatoryPropertyNullValueException(IEnumerable<PropertyInfo> mandatoryProperties)
        {
            this.MandatoryProperties = mandatoryProperties.ToArray();
        }

        public MandatoryPropertyNullValueException(
            SerializationInfo info, 
            StreamingContext context, 
            IEnumerable<PropertyInfo> mandatoryProperties)
            : base(info, context)
        {
            this.MandatoryProperties = mandatoryProperties.ToArray();
        }

        #endregion

        #region Public Properties

        public PropertyInfo[] MandatoryProperties { get; private set; }

        #endregion
    }
}