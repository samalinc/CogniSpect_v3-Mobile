using System.Net.Http;

namespace CSMobile.Infrastructure.Services.WebClient
{
    public class WebApiRequestOptions
    {
        public HttpMethod Method { get; set; }

        public string Endpoint { get; set; }
        
        public string SecurityToken { get; set; }

        public object Body { get; set; }
    }
}