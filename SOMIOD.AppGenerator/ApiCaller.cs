using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SOMIOD.Library.Models;

namespace SOMIOD.AppGenerator
{
    public class ApiCaller
    {
        //APPLICATIONS
        public static Application GetApplication(HttpClient client, string applicationName)
        {
            var response = client.GetAsync(client.BaseAddress + applicationName);

            if (response.Result.StatusCode != HttpStatusCode.OK)
            {
                Shared.ShowError(response.Result);
                return null;
            }

            Application app = new Application();
            return Shared.GetObject(response, app) as Application;
        }

        public static List<string> GetAllApplicationNames(HttpClient client)
        {
            WebClient.AddOperationTypeHeader(client, Library.Headers.Application);
            var response = client.GetAsync(client.BaseAddress);
            if (!(response.Result.StatusCode == HttpStatusCode.OK))
            {
                Shared.ShowError(response.Result);
                return null;
            }

            return Shared.SetObjectNamesList(response, "//applications");
        }

        public static bool DeleteApplication(HttpClient client, string applicationName)
        {
            var response = client.DeleteAsync(client.BaseAddress + applicationName);

            if (response.Result.StatusCode != HttpStatusCode.OK)
            {
                Shared.ShowError(response.Result);
                return false;
            }
            return true;
        }

        //CONTAINERS
        public static Library.Models.Container GetContainer(HttpClient client, string applicationName, string containerName)
        {
            var response = client.GetAsync(client.BaseAddress + applicationName + "/" + containerName);

            if (response.Result.StatusCode != HttpStatusCode.OK)
            {
                Shared.ShowError(response.Result);
                return null;
            }

            Library.Models.Container app = new Library.Models.Container();
            return Shared.GetObject(response, app) as Library.Models.Container;
        }

        public static List<string> GetAllContainersNamesFromApplication(HttpClient client, string applicationName)
        {
            WebClient.AddOperationTypeHeader(client, Library.Headers.Container);
            var response = client.GetAsync(client.BaseAddress + applicationName + "/con");
            if (!(response.Result.StatusCode == HttpStatusCode.OK))
            {
                Shared.ShowError(response.Result);
                return null;
            }

            return Shared.SetObjectNamesList(response, "//containers");
        }

        public static List<string> GetAllContainersNames(HttpClient client)
        {
            WebClient.AddOperationTypeHeader(client, Library.Headers.Container);
            var response = client.GetAsync(client.BaseAddress + "application/container");
            if (!(response.Result.StatusCode == HttpStatusCode.OK))
            {
                Shared.ShowError(response.Result);
                return null;
            }

            return Shared.SetObjectNamesList(response, "//containers");
        }

        public static bool DeleteContainer(HttpClient client, string applicationName, string containername)
        {
            var response = client.DeleteAsync(client.BaseAddress + applicationName + "/" + containername);

            if (response.Result.StatusCode != HttpStatusCode.OK)
            {
                Shared.ShowError(response.Result);
                return false;
            }
            return true;
        }

        //SUBSCRIPTION
        public static List<string> GetAllSubscriptionNames(HttpClient client)
        {
            WebClient.AddOperationTypeHeader(client, Library.Headers.Subscription);
            var response = client.GetAsync(client.BaseAddress + "application/container/sub");
            if (!(response.Result.StatusCode == HttpStatusCode.OK))
            {
                Shared.ShowError(response.Result);
                return null;
            }

            return Shared.SetObjectNamesList(response, "//subscriptions");
        }

        public static List<string> GetAllSubscriptionNamesFromContainer(HttpClient client, string applicationName, string containerName)
        {
            WebClient.AddOperationTypeHeader(client, Library.Headers.Subscription);
            var response = client.GetAsync(client.BaseAddress + applicationName + "/" + containerName + "/sub");
            if (!(response.Result.StatusCode == HttpStatusCode.OK))
            {
                Shared.ShowError(response.Result);
                return null;
            }

            return Shared.SetObjectNamesList(response, "//subscriptions");
        }

        public static bool DeleteSubscription(HttpClient client, string applicationName, string containerName, string subscriptionName)
        {
            var response = client.DeleteAsync(client.BaseAddress + applicationName + "/" + containerName + "/sub/" + subscriptionName);
            if (response.Result.StatusCode != HttpStatusCode.OK)
            {
                Shared.ShowError(response.Result);
                return false;
            }
            return true;
        }
    }
}
