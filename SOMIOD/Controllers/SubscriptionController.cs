using SOMIOD.Data;
using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace SOMIOD.Controllers
{
    public class SubscriptionController : ApiController
    {
        private readonly SomiodDBContext _context;

        public SubscriptionController(SomiodDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("somiod/{applicationName}/{containerName}/")]
        public IHttpActionResult GetSubscription(string applicationName, string containerName, string subscriptionName)
        {
            if (!DoesApplicationExist(applicationName) || !DoesContainerExist(containerName))
            {
                return BadRequest();
            }

            if(!DoesSubscriptionExist(subscriptionName)){
                return NotFound();
            }

            var sub = _context.Subscriptions.FirstOrDefault(x => String.Equals(x.Name,subscriptionName));
            return Ok(sub);
        }

        [HttpPost]
        [Route("somiod/{applicationName]/{containerName}/")]
        public IHttpActionResult PostSubscription(string applicationName, string containerName, [FromBody]XmlElement subscription)
        {
            if (!DoesApplicationExist(applicationName) || !DoesContainerExist(containerName))
            {
                return BadRequest();
            }
            var sub = new Subscription()
            {
                Name = subscription.SelectSingleNode($"/application/container/subscription/name")?.InnerText,
                CreatedDate = DateTime.Parse(subscription.SelectSingleNode($"/application/container/subscription/createddate")?.InnerText),
                Event = subscription.SelectSingleNode($"/application/container/subscription/event")?.InnerText,
                Endpoint = subscription.SelectSingleNode($"/application/container/subscription/endpoint")?.InnerText,
                Parent = Int32.Parse(subscription.SelectSingleNode($"/application/container/subscription/parent")?.InnerText),
            };

            if (DoesSubscriptionExist(sub.Name))
            {
                //already exists! return correct response
                return BadRequest();
            }

            //check to see if createddate is correct, as event and endpoint as parent!

            _context.Subscriptions.Add(sub);
            _context.SaveChanges();
            return Ok();
        }

        private bool DoesApplicationExist(string applicationName)
        {
            return _context.Applications.Any(x => String.Equals(x.Name, applicationName));
        }

        private bool DoesContainerExist(string containerName)
        {
            return _context.Containers.Any(x => String.Equals(x.Name, containerName));
        }

        private bool DoesSubscriptionExist(string subscriptionName)
        {
            return _context.Subscriptions.Any(x => String.Equals(x.Name, subscriptionName));
        }
    }
}