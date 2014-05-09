namespace SharpZendeskApi
{
    using Microsoft.Practices.Unity;

    using RestSharp;    

    public interface IZendeskClient : IRestClient
    {
        IUnityContainer Container { get; set; }
    }
}