namespace SharpZendeskApi
{
    using System;
    using System.Net;

    using RestSharp;

    using SharpZendeskApi.Exceptions;
    using SharpZendeskApi.Models;

    internal class RequestHandler : IRequestHandler
    {
        private readonly IRestClient client;

        public RequestHandler(IRestClient client)
        {
            this.client = client;
        }

        public T MakeRequest<T>(IRestRequest request) where T : class
        {
            var response = this.client.Execute<T>(request);
            ThrowIfNoDataOrProblem(response);
            return response.Data;
        }        

        public void MakeRequest(IRestRequest request)
        {
            var response = this.client.Execute(request);
            ThrowIfProblem(response);
        }

        private static void ThrowIfProblem(IRestResponse response)
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

        private static void ThrowIfNoDataOrProblem<T>(IRestResponse<T> response) where T : class
        {
            ThrowIfProblem(response);

            // see https://github.com/restsharp/RestSharp/blob/18826dd66905df9e25c10e606363b4d39aa88c4c/RestSharp/RestClient.cs
            // Data property will be null if deserialization fails
            if (response.Data == null)
            {
                throw new SharpZendeskException("Response data was null or not in the correct format. Expected type: " + typeof(T));
            }
        }
    }
}