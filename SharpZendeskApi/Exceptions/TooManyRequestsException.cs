namespace SharpZendeskApi.Exceptions
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;

    using RestSharp;

    [Serializable]
    public class TooManyRequestsException : ZendeskRequestException
    {
        #region Constructors and Destructors

        public TooManyRequestsException(
            string message, 
            Exception innerException, 
            IRestRequest originalRequest, 
            HttpStatusCode statusCode)
            : base(message, innerException, originalRequest, statusCode)
        {
        }

        public TooManyRequestsException(string message, IRestRequest originalRequest, HttpStatusCode statusCode)
            : base(message, originalRequest, statusCode)
        {
        }

        public TooManyRequestsException(IRestRequest originalRequest, HttpStatusCode statusCode)
            : base(originalRequest, statusCode)
        {
        }

        public TooManyRequestsException(
            SerializationInfo info, 
            StreamingContext context, 
            IRestRequest originalRequest, 
            HttpStatusCode statusCode)
            : base(info, context, originalRequest, statusCode)
        {
        }

        #endregion
    }
}