namespace SharpZendeskApi.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SharpZendeskException : Exception
    {
        #region Constructors and Destructors

        public SharpZendeskException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public SharpZendeskException(string message)
            : base(message)
        {
        }

        public SharpZendeskException()
        {
        }

        public SharpZendeskException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}