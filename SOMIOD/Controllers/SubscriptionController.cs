using SOMIOD.Data;
using SOMIOD.Library;
using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        [Route("somiod/{applicationName}/{containerName}/sub")]
        public IHttpActionResult GetSubscription(string applicationName, string containerName, string subscriptionName)
        {
            try
            {
                if (!DoesApplicationExist(applicationName) || !DoesContainerExist(containerName))
                {
                    return BadRequest();
                }

                if (!DoesSubscriptionExist(subscriptionName))
                {
                    return NotFound();
                }

                var sub = _context.Subscriptions.FirstOrDefault(x => String.Equals(x.Name, subscriptionName));
                return Ok(sub);

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("somiod/{applicationName}/{containerName}")] //RICARDO TEST IF LIST OF SUBSCRIPTIONS RETURN AS GOOD XML
        public IHttpActionResult GetSubscriptionsByContainer(string applicationName,string containerName)
        {
            try
            {
                if (!DoesApplicationExist(applicationName) || !DoesContainerExist(containerName))
                {
                    return BadRequest();
                }

                if (Request.Headers.Count() < 1 ||
                    Request.Headers.Any(x => x.Key == "somiod-discover") ||
                    !string.Equals(Request.Headers.First(x => x.Key == "somiod-discover").Value.FirstOrDefault(), Headers.Subscription.ToString().ToLower()))
                {
                    return BadRequest();
                }

                var conId = _context.Containers.SingleOrDefault(x => string.Equals(x.Name,containerName)).Id;

                var subs = _context.Subscriptions.Where(x => x.Parent == conId);

                if(subs is null || subs.Count() == 0)
                {
                    return Content(HttpStatusCode.InternalServerError, $"Error getting list of subcriptions of container: {containerName}");
                }

                _context.SaveChanges();
                return Ok(subs);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("somiod/{applicationName}/container")]
        public IHttpActionResult GetSubscriptions(string applicationName)
        {
            try
            {
                if (!DoesApplicationExist(applicationName))
                {
                    return BadRequest();
                }

                if (Request.Headers.Count() < 1 ||
                    Request.Headers.Any(x => x.Key == "somiod-discover") ||
                    !string.Equals(Request.Headers.First(x => x.Key == "somiod-discover").Value.FirstOrDefault(), Headers.Subscription.ToString().ToLower()))
                {
                    return BadRequest();
                }

                var subs = _context.Subscriptions.ToList();

                if (subs is null)
                {
                    return Content(HttpStatusCode.InternalServerError, $"Error getting list of subcriptions");
                }

                _context.SaveChanges();
                return Ok(subs);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("somiod/{applicationName}/{containerName}/")]
        public IHttpActionResult PostSubscription(string applicationName, string containerName, [FromBody]XmlElement subscription)
        {
            try
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
                    return Content(HttpStatusCode.Conflict, "Resource already exists");
                }

                //RICARDO check to see if event and endpoint!
                if (!IsDateCreatedCorrect(sub.CreatedDate) ||
                    !IsSubscriptionConnected(applicationName, containerName, sub))
                {
                    return BadRequest();
                }

                var entity = _context.Subscriptions.Add(sub);
                if (entity is null) return Content(HttpStatusCode.InternalServerError, "Error Inserting new Subscription");
                _context.SaveChanges();
                return Ok();

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("somiod/{applicationName}/{containerName}")]
        public IHttpActionResult PutSubscription(string applicationName, string containerName, [FromBody] XmlElement subscription)
        {
            try
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

                //RICARDO check to see if event and endpoint!
                if (!IsDateCreatedCorrect(sub.CreatedDate) ||
                    !IsSubscriptionConnected(applicationName, containerName, sub))
                {
                    return BadRequest();
                }

                var dbSub = _context.Subscriptions.SingleOrDefault(x => x.Id == sub.Id);
                if(dbSub != null) { return NotFound(); }

                dbSub.Name = sub.Name;
                dbSub.CreatedDate = sub.CreatedDate;
                dbSub.Event = sub.Event;
                dbSub.Endpoint = sub.Endpoint;
                dbSub.Parent = sub.Parent;

                _context.SaveChanges();
                return Ok();

            }
            catch(Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("somiod/{applicationName}/{containerName}/sub")]
        public IHttpActionResult DeleteSubscription(string applicationName,string containerName, string subscriptionName)
        {
            try
            {
                if (!DoesApplicationExist(applicationName) || !DoesContainerExist(containerName))
                {
                    return BadRequest();
                }

                if(!DoesSubscriptionExist(subscriptionName))
                {
                    return NotFound();
                }

                var dbSub = _context.Subscriptions.SingleOrDefault(x => string.Equals(x.Name, subscriptionName));

                var entity = _context.Subscriptions.Remove(dbSub);
                if (entity is null) return Content(HttpStatusCode.InternalServerError, "Error deleting subscription");
                
                _context.SaveChanges();
                return Ok();

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
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

        private bool IsDateCreatedCorrect(DateTime date)
        {
            return date < DateTime.Now;
        }

        private bool IsSubscriptionConnected(string applicationName, string containerName, Subscription sub)
        {
            int appId = _context.Applications.FirstOrDefault(x => x.Name == applicationName).Id;
            int conId = _context.Containers.FirstOrDefault(x => x.Name == containerName).Id;
            return _context.Subscriptions.Any(x => x.Id == sub.Id && x.Parent == conId) && _context.Containers.Any(x => x.Id == sub.Parent && x.Parent == appId); 
        }
    }
}