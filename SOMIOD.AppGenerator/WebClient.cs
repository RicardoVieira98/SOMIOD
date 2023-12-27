using SOMIOD.Library;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SOMIOD.AppGenerator
{
    public class WebClient
    {
        static HttpClient client = new HttpClient();
        public WebClient() 
        {
        }
        public static HttpClient CreateHttpClient()
        {
            var APIPath = ConfigurationManager.AppSettings["localhost"];
            client.DefaultRequestHeaders.Add("Accept", "application/xml");
            client.BaseAddress = new Uri(APIPath);
            return client;
        }

        public static void AddOperationTypeHeader(HttpClient client, Headers operationType) 
        {
            client.DefaultRequestHeaders.Add("somiod-discover", operationType.ToString());
        }
    }
}
