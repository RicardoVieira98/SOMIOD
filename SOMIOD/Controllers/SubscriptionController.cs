using SOMIOD.Data;
using SOMIOD.Library;
using SOMIOD.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Xml;
using uPLibrary.Networking.M2Mqtt;

namespace SOMIOD.Controllers
{
    [RoutePrefix("api/somiod")]
    public class SubscriptionController : ApiController
    {
        private readonly SomiodDBContext _context;

        public SubscriptionController()
        {
            _context = new SomiodDBContext();
        }

        [HttpGet]
        [Route("{applicationName}/{containerName}/sub/{subscriptionName}")]
        public IHttpActionResult GetSubscription(string applicationName, string containerName, string subscriptionName)
        {
            try
            {
                if (Shared.AreArgsEmpty(new List<string> { applicationName, containerName, subscriptionName }))
                {
                    return BadRequest("Input form had empty field/s, please fill all mandatory fields");
                }

                if (!Shared.DoesApplicationExist(_context, applicationName) ||
                    !Shared.DoesContainerExist(_context, containerName))
                {
                    return BadRequest("Application or container requested do not exist on our database");
                }

                if (!DoesSubscriptionExist(subscriptionName))
                {
                    return NotFound();
                }

                var sub = _context.Subscriptions.Where(x => String.Equals(x.Name, subscriptionName)).ToList();

                if (sub is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error getting list of subcriptions of database");
                }

                return Ok(XmlHandler.OnlySubscriptionsXml(sub));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("{applicationName}/{containerName}/sub")]
        public IHttpActionResult GetSubscriptionsByContainer(string applicationName,string containerName)
        {
            try
            {
                if (Shared.AreArgsEmpty(new List<string> { applicationName, containerName }))
                {
                    return BadRequest("Input form had empty field/s, please fill all mandatory fields");
                }

                if (!Shared.DoesApplicationExist(_context,applicationName) || 
                    !Shared.DoesContainerExist(_context, containerName))
                {
                    return BadRequest("Application or container sent do not exist on our database");
                }

                if (Request.Headers.Count() < 1 ||
                    !Request.Headers.Any(x => x.Key == "somiod-discover") ||
                    !string.Equals(Request.Headers.First(x => x.Key == "somiod-discover").Value.FirstOrDefault(), Headers.Subscription.ToString()))
                {
                    return BadRequest("Header was not found or incorrect, please make sure you are sending a correct header with the operation-type necessary");
                }

                var conId = _context.Containers.SingleOrDefault(x => string.Equals(x.Name,containerName))?.Id;

                var subs = _context.Subscriptions.Where(x => x.Parent == conId).ToList();

                if (subs is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error getting list of subcriptions of database");
                }

                if (subs.Count is 0)
                {
                    return NotFound();
                }

                _context.SaveChanges();
                return Ok(XmlHandler.OnlySubscriptionsXml(subs));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("application/container/sub")]
        public IHttpActionResult GetSubscriptions()
        {
            try
            {
                if (Request.Headers.Count() < 1 ||
                    !Request.Headers.Any(x => x.Key == "somiod-discover") ||
                    !string.Equals(Request.Headers.First(x => x.Key == "somiod-discover").Value.FirstOrDefault(), Headers.Subscription.ToString()))
                {
                    return BadRequest("Header was not found or incorrect, please make sure you are sending a correct header with the operation-type necessary");
                }

                var subs = _context.Subscriptions.ToList();

                if (subs is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error getting list of subcriptions of database");
                }

                if (subs.Count is 0)
                {
                    return NotFound();
                }

                _context.SaveChanges();
                return Ok(XmlHandler.OnlySubscriptionsXml(subs));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("{applicationName}/{containerName}/sub")]
        public IHttpActionResult PostSubscription(string applicationName, string containerName, [FromBody]XmlElement subscription)  
        {
            try
            {
                if (Shared.AreArgsEmpty(new List<string> { applicationName, containerName }))
                {
                    return BadRequest("Input form had empty field/s, please fill all mandatory fields");
                }

                if (subscription is null || subscription.Attributes?.Count < 1)
                {
                    return BadRequest("Form sent is incomplete");
                }

                if (!Shared.DoesApplicationExist(_context, applicationName))
                {
                    return BadRequest("Application grand parent does not exist");
                }

                if (!Shared.DoesContainerExist(_context, containerName))
                {
                    return BadRequest("Container parent does not exist");
                }

                if(!Shared.IsContainerConnected(_context,applicationName,_context.Containers.FirstOrDefault(x => string.Equals(x.Name, containerName))))
                {
                    return Content(HttpStatusCode.Conflict, "Container parent does not exist or is not associated");
                }

                if (string.IsNullOrEmpty(subscription.Attributes["name"]?.Value) ||
                    !Shared.IsDateCreatedCorrect(DateTime.Parse(subscription.Attributes["createddate"]?.Value)) ||
                    !Enum.IsDefined(typeof(Events), subscription.Attributes["event"]?.Value) ||
                    string.IsNullOrEmpty(subscription.Attributes["endpoint"]?.Value))
                {
                    return BadRequest("Input form is not correct, please make sure all fields are correct before creating a new subscription");
                }

                var sub = new Subscription()
                {
                    Name = subscription.Attributes["name"]?.Value,
                    CreatedDate = DateTime.Parse(subscription.Attributes["createddate"]?.Value),
                    Event = subscription.Attributes["event"]?.Value,
                    Endpoint = subscription.Attributes["endpoint"]?.Value,
                    Parent = _context.Containers.FirstOrDefault(x => string.Equals(x.Name,containerName)).Id,
                };

                if (DoesSubscriptionExist(sub.Name))
                {
                    return Content(HttpStatusCode.Conflict, "Subscription already exists");
                }

                var entity = _context.Subscriptions.Add(sub);
                if (entity is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error Inserting new subscription into database");
                }
                _context.SaveChanges();
                return Ok();

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{applicationName}/{containerName}/sub/{subscriptionName}")]
        public IHttpActionResult DeleteSubscription(string applicationName,string containerName, string subscriptionName)
        {
            try
            {
                if (Shared.AreArgsEmpty(new List<string> { applicationName, containerName, subscriptionName }))
                {
                    return BadRequest("Input form had empty field/s, please fill all mandatory fields");
                }

                if(!Shared.DoesApplicationExist(_context, applicationName) ||
                    !Shared.DoesContainerExist(_context, containerName))
                {
                    return Content(HttpStatusCode.Conflict, "Subscription parent you are trying to delete does not exists, or the containers parent's does not exist, please insert a subscription with an existing container associated and the container associated with an existing application");
                }

                if (!DoesSubscriptionExist(subscriptionName))
                {
                    return NotFound();
                }

                var dbSub = _context.Subscriptions.SingleOrDefault(x => string.Equals(x.Name, subscriptionName));

                var entity = _context.Subscriptions.Remove(dbSub);
                if (entity is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error deleting subscription");
                }
                _context.SaveChanges();
                return Ok();

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private bool DoesSubscriptionExist(string subscriptionName)
        {
            return _context.Subscriptions.Any(x => String.Equals(x.Name, subscriptionName));
        }

        [Obsolete]
        private void CallMessageBroker()
        {
//            MqttClient mClient = new MqttClient(IPAddress.Parse("127.0.0.1"));

//            string[] mStrTopicsInfo = { "news", "complaints" };

//            mClient.Connect(Guid.NewGuid().ToString());

//            if (!mClient.IsConnected)
//            {
//                MessageBox.Show("Error connecting to message broker...");
//                return;
//            }
//            mClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
//            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
//MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE};//QoS
//            mClient.Subscribe(mStrTopicsInfo, qosLevels);



            MqttClient mcClient = new MqttClient(IPAddress.Parse("1883"));
            string[] mStrTopicsInfo = { "news", "complaints" };
            mcClient.Connect(Guid.NewGuid().ToString());
            if (!mcClient.IsConnected)
            {

            }

            mcClient.Publish("news", Encoding.UTF8.GetBytes("Hello World!"));
            if (mcClient.IsConnected)
            {
                mcClient.Unsubscribe(mStrTopicsInfo); //Put this in a button to see notify!
                mcClient.Disconnect(); //Free process and process's resources
            }

        }
    }
}