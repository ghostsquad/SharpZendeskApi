namespace SharpZendeskApi.Core.Exceptions
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;

    using RestSharp;

    [Serializable]
    public class NotFoundException : ZendeskRequestException
    {
        #region Constructors and Destructors

        public NotFoundException(
            string message, 
            Exception innerException, 
            IRestRequest originalRequest, 
            HttpStatusCode statusCode)
            : base(message, innerException, originalRequest, statusCode)
        {
        }

        public NotFoundException(string message, IRestRequest originalRequest, HttpStatusCode statusCode)
            : base(message, originalRequest, statusCode)
        {
        }

        public NotFoundException(IRestRequest originalRequest, HttpStatusCode statusCode)
            : base(originalRequest, statusCode)
        {
        }

        public NotFoundException(
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