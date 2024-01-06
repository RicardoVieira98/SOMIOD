using SOMIOD.Data;
using SOMIOD.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SOMIOD
{
    public class Shared
    {
        public static bool DoesApplicationExist(SomiodDBContext context, int applicationId)
        {
            return context.Applications.Any(x => x.Id == applicationId);
        }

        public static bool DoesApplicationExist(SomiodDBContext context, string applicationName)
        {
            return context.Applications.Any(x => string.Equals(x.Name, applicationName));
        }

        public static bool DoesContainerExist(SomiodDBContext context, int containerId)
        {
            return context.Containers.Any(x => x.Id == containerId);
        }

        public static bool DoesContainerExist(SomiodDBContext context, string containerName)
        {
            return context.Containers.Any(x => string.Equals(x.Name, containerName));
        }

        public static bool IsDateCreatedCorrect(DateTime date)
        {
            return date < DateTime.Now;
        }

        public static bool IsNameEmpty(string resourceName) 
        {
            return string.IsNullOrEmpty(resourceName);
        }

        public static string FillResourceName(XmlElement resource)
        {
            string name = "";
            if (IsNameEmpty(resource.Attributes["name"]?.Value))
            {
                name = Guid.NewGuid().ToString();
            }
            else
            {
                name = resource.Attributes["name"]?.Value;
            }
            return name;
        }

        public static bool AreArgsEmpty(List<string> args)
        {
            return args.Any(x => string.IsNullOrEmpty(x));
        }

        public static bool IsContainerConnected(SomiodDBContext context, string applicationName, Container container)
        {
            int? appId = context.Applications.FirstOrDefault(x => string.Equals(x.Name, applicationName))?.Id;
            return context.Containers.Any(x => x.Id == container.Id && x.Parent == appId.Value);
        }
    }
}