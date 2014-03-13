namespace SharpZendeskApi.Core.Exceptions
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;

    using RestSharp;

    [Serializable]
    public class ZendeskRequestException : Exception
    {
        #region Constructors and Destructors

        public ZendeskRequestException(
            string message, 
            Exception innerException, 
            IRestRequest originalRequest, 
            HttpStatusCode statusCode)
            : base(message, innerException)
        {
            this.OriginalRequest = originalRequest;
            this.StatusCode = statusCode;
        }

        public ZendeskRequestException(string message, IRestRequest originalRequest, HttpStatusCode statusCode)
            : base(message)
        {
            this.OriginalRequest = originalRequest;
            this.StatusCode = statusCode;
        }

        public ZendeskRequestException(IRestRequest originalRequest, HttpStatusCode statusCode)
        {
            this.OriginalRequest = originalRequest;
            this.StatusCode = statusCode;
        }

        public ZendeskRequestException(
            SerializationInfo info, 
            StreamingContext context, 
            IRestRequest originalRequest, 
            HttpStatusCode statusCode)
            : base(info, context)
        {
            this.OriginalRequest = originalRequest;
            this.StatusCode = statusCode;
        }

        #endregion

        #region Public Properties

        public IRestRequest OriginalRequest { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }

        #endregion
    }
}