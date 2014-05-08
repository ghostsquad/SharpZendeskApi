namespace SharpZendeskApi.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    using RestSharp;

    using SharpZendeskApi.Core.Exceptions;

    internal static class ResponseExtensions
    {
        public static void ThrowIfProblem(this IRestResponse response)
        {
            if (response == null)
            {
                throw new SharpZendeskException("The response was null.");
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException(response.Content);
            }            

            if (response.StatusCode == (HttpStatusCode)429)
            {
                throw new TooManyRequestsException(response.Content, response.Request, response.StatusCode);
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new ZendeskRequestException(response.Content, response.Request, response.StatusCode);
            }

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            if (response.Request == null)
            {
                throw new SharpZendeskException("The response object does not contain the original request.");
            }

            if (response.Request.Method == Method.GET)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new NotFoundException(response.Content, response.Request, response.StatusCode);
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ZendeskRequestException(response.Content, response.Request, response.StatusCode);
                }
            }

            if (response.Request.Method == Method.POST && response.StatusCode != HttpStatusCode.Created)
            {
                throw new ZendeskRequestException(response.Content, response.Request, response.StatusCode);
            }

            if (response.Request.Method == Method.PUT && response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NotModified)
            {
                throw new ZendeskRequestException(response.Content, response.Request, response.StatusCode);
            }

            if (response.Request.Method == Method.DELETE && response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new ZendeskRequestException(response.Content, response.Request, response.StatusCode);
            }            
        }

        public static void ThrowIfProblem<T>(this IRestResponse<T> response)
        {
            ThrowIfProblem(response as IRestResponse);

            // ReSharper disable once CompareNonConstrainedGenericWithNull
            // see https://github.com/restsharp/RestSharp/blob/18826dd66905df9e25c10e606363b4d39aa88c4c/RestSharp/RestClient.cs
            // Data property will be null if deserialization fails
            if (response.Data == null)
            {
                throw new SharpZendeskException("Response data was null or not in the correct format. Expected type: " + typeof(T));
            }            
        }
    }
}
