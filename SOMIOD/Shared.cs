using SOMIOD.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace SOMIOD
{
    public class Shared
    {
        public static bool DoesApplicationExist(SomiodDBContext context, string applicationName)
        {
            return context.Applications.Any(x => String.Equals(x.Name, applicationName));
        }

        public static bool DoesContainerExist(SomiodDBContext context, string containerName)
        {
            return context.Containers.Any(x => String.Equals(x.Name, containerName));
        }

        public static bool IsDateCreatedCorrect(DateTime date)
        {
            return date < DateTime.Now;
        }

        public static bool AreArgsEmpty(List<string> args)
        {
            return args.Any(x => string.IsNullOrEmpty(x));
        }
    }
}