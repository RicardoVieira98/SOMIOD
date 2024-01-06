using SOMIOD.Data;
using SOMIOD.Library;
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
    public class DataController : ApiController
    {
        private readonly SomiodDBContext _context;

        public DataController()
        {
            _context = new SomiodDBContext();
        }

        [HttpGet]
        [Route("somiod/{applicationName}/{containerName}/data/{dataName}")]
        public IHttpActionResult GetData(string applicationName, string containerName, string dataName)
        {
            try
            {
                if (Shared.AreArgsEmpty(new List<string> { applicationName, containerName, dataName }))
                {
                    return BadRequest("Input form had empty field/s, please fill all mandatory fields");
                }

                if (!Shared.DoesApplicationExist(_context, applicationName) ||
                    !Shared.DoesContainerExist(_context, containerName))
                {
                    return BadRequest("Application or container requested do not exist on our database");
                }

                if (!DoesDataExist(dataName))
                {
                    return NotFound();
                }

                var data = _context.Datas.Where(x => String.Equals(x.Name, dataName)).ToList();

                if (data is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error getting list of subcriptions of database");
                }

                return Ok(XmlHandler.OnlyDataXml(data));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("somiod/{applicationName}/{containerName}/data")]
        public IHttpActionResult GetDatasByContainer(string applicationName, string containerName)
        {
            try
            {
                if (Shared.AreArgsEmpty(new List<string> { applicationName, containerName }))
                {
                    return BadRequest("Input form had empty field/s, please fill all mandatory fields");
                }

                if (!Shared.DoesApplicationExist(_context, applicationName) ||
                    !Shared.DoesContainerExist(_context, containerName))
                {
                    return BadRequest("Application or container sent do not exist on our database");
                }

                if (Request.Headers.Count() < 1 ||
                    Request.Headers.Any(x => x.Key == "somiod-discover") ||
                    !string.Equals(Request.Headers.First(x => x.Key == "somiod-discover").Value.FirstOrDefault(), Headers.Data.ToString().ToLower()))
                {
                    return BadRequest("Header was not found or incorrect, please make sure you are sending a correct header with the operation-type necessary");
                }

                var conId = _context.Containers.SingleOrDefault(x => string.Equals(x.Name, containerName))?.Id;

                var datas = _context.Datas.Where(x => x.Parent == conId).ToList();

                if(datas.Any(x => !IsDatasConnected(applicationName,containerName,x))) 
                {
                    return Content(HttpStatusCode.Conflict, "Data parent you are trying to insert does not exists, or the containers parent's does not exist, please insert a data with an existing container associated and the container associated with an existing application");
                }

                if (datas is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error getting list of datas of database");
                }

                if (datas.Count is 0)
                {
                    return NotFound();
                }

                _context.SaveChanges();
                return Ok(XmlHandler.OnlyDataXml(datas));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("somiod/application/container/data")]
        public IHttpActionResult GetDatas()
        {
            try
            {
                if (Request.Headers.Count() < 1 ||
                    !Request.Headers.Any(x => x.Key == "somiod-discover") ||
                    !string.Equals(Request.Headers.First(x => x.Key == "somiod-discover").Value.FirstOrDefault(), Headers.Data.ToString()))
                {
                    return BadRequest("Header was not found or incorrect, please make sure you are sending a correct header with the operation-type necessary");
                }

                var datas = _context.Datas.ToList();

                if (datas is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error getting list of subcriptions of database");
                }

                if (datas.Count is 0)
                {
                    return NotFound();
                }

                _context.SaveChanges();
                return Ok(XmlHandler.OnlyDataXml(datas));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("somiod/{applicationName}/{containerName}/data")]
        public IHttpActionResult PostData(string applicationName, string containerName, [FromBody] XmlElement data)
        {
            try
            {
                if (Shared.AreArgsEmpty(new List<string> { applicationName, containerName }))
                {
                    return BadRequest("Input form had empty field/s, please fill all mandatory fields");
                }

                if (data is null || data.Attributes?.Count < 1)
                {
                    return BadRequest("Form sent is incomplete");
                }

                if (string.IsNullOrEmpty(data.Attributes["name"]?.Value) ||
                    !Shared.IsDateCreatedCorrect(DateTime.Parse(data.Attributes["createddate"]?.Value)) ||
                    Int32.Parse(data.Attributes["parent"]?.Value) == 0 ||
                    string.IsNullOrEmpty(data.Attributes["content"]?.Value))
                {
                    return BadRequest("Input form is not correct, please make sure all fields are correct before creating a new subscription");
                }

                var newData = new Library.Models.Data()
                {
                    Name = data.Attributes["name"]?.Value,
                    CreatedDate = DateTime.Parse(data.Attributes["createddate"]?.Value),
                    Parent = Int32.Parse(data.Attributes["parent"]?.Value),
                    Content = data.Attributes["content"]?.Value
                };

                if (DoesDataExist(newData.Name))
                {
                    return Content(HttpStatusCode.Conflict, "Data already exists");
                }

                if (!IsDatasConnected(applicationName, containerName, newData))
                {
                    return Content(HttpStatusCode.Conflict, "Data parent you are trying to insert does not exists, or the containers parent's does not exist, please insert a data with an existing container associated and the container associated with an existing application");
                }

                var entity = _context.Datas.Add(newData);
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
        [Route("somiod/{applicationName}/{containerName}/data/{dataName}")]
        public IHttpActionResult DeleteData(string applicationName, string containerName, string dataName)
        {
            try
            {
                if (Shared.AreArgsEmpty(new List<string> { applicationName, containerName, dataName }))
                {
                    return BadRequest("Input form had empty field/s, please fill all mandatory fields");
                }

                if (!Shared.DoesApplicationExist(_context, applicationName) ||
                    !Shared.DoesContainerExist(_context, containerName))
                {
                    return Content(HttpStatusCode.Conflict, "Data parent you are trying to delete does not exists, or the containers parent's does not exist, please delete a data with an existing container associated and the container associated with an existing application");
                }

                if (!DoesDataExist(dataName))
                {
                    return NotFound();
                }

                var dbSub = _context.Datas.SingleOrDefault(x => string.Equals(x.Name, dataName));

                var entity = _context.Datas.Remove(dbSub);
                if (entity is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error deleting data");
                }

                _context.SaveChanges();
                return Ok();

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private bool DoesDataExist(string dataName)
        {
            return _context.Datas.Any(x => String.Equals(x.Name, dataName));
        }

        private bool IsDatasConnected(string applicationName, string containerName, Library.Models.Data data)
        {
            if (!Shared.DoesApplicationExist(_context, applicationName) || !Shared.DoesContainerExist(_context, containerName)) return false;

            int? appId = _context.Applications.FirstOrDefault(x => string.Equals(x.Name, applicationName))?.Id;
            int? conId = _context.Containers.FirstOrDefault(x => string.Equals(x.Name, containerName))?.Id;
            return _context.Datas.Any(x => string.Equals(x.Name, data.Name) && x.Parent == conId) && _context.Containers.Any(x => x.Id == data.Parent && x.Parent == appId);
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