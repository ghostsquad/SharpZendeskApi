namespace SharpZendeskApi
{
    using RestSharp;

    using TinyIoC;

    public interface IZendeskClient : IRestClient
    {
        TinyIoCContainer Container { get; set; }
    }
}