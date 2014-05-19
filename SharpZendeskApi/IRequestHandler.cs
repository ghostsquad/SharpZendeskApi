namespace SharpZendeskApi
{
    using RestSharp;

    using SharpZendeskApi.Models;

    internal interface IRequestHandler
    {
        T MakeRequest<T>(IRestRequest request) where T : class;

        void MakeRequest(IRestRequest request);
    }
}
