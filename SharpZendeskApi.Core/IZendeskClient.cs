namespace SharpZendeskApi.Core
{
    using RestSharp;

    using TinyIoC;

    public interface IZendeskClient : IRestClient
    {
        TinyIoCContainer Container { get; set; }
    }
}