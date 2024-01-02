using SOMIOD.Data;
using SOMIOD.Library;
using SOMIOD.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Xml;
using Container = SOMIOD.Library.Models.Container;

namespace SOMIOD.Controllers
{
    public class ContainerController : ApiController
    {
        private readonly SomiodDBContext _context;

        public ContainerController()
        {
            _context = new SomiodDBContext();
        }

        [HttpGet]
        [Route("somiod/{applicationName}/{containerName}")]
        public IHttpActionResult GetContainer(string applicationName, string containerName)
        {
            try
            {
                if (Shared.AreArgsEmpty(new List<string> { applicationName, containerName }))
                {
                    return BadRequest("One or more fields are empty.");
                }

                if(!Shared.DoesApplicationExist(_context, applicationName))
                {
                    return BadRequest("Application requested does not exists.");
                }

                if (!Shared.DoesContainerExist(_context, containerName))
                {
                    return NotFound();
                }

                var containers = _context.Containers.Where(x => string.Equals(x.Name, containerName)).ToList();

                if (containers is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error getting container of database");
                }

                return Ok(XmlHandler.OnlyContainersXml(containers));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("somiod/application")]
        public IHttpActionResult GetContainers()
        {
            try
            {
                if (Request.Headers.Count() < 1 ||
                    Request.Headers.All(x => x.Key != "somiod-discover") ||
                    !string.Equals(Request.Headers.First(x => x.Key == "somiod-discover").Value.FirstOrDefault(), Headers.Container.ToString()))
                {
                    return BadRequest("Header was not found or incorrect, please make sure you are sending a correct header with the operation-type necessary");
                }

                var containers = _context.Containers.ToList();
                if (containers is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error getting applications of database");
                }

                if (containers.Count is 0)
                {
                    return NotFound();
                }

                return Ok(XmlHandler.OnlyContainersXml(containers));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("somiod/{applicationName}/con")]
        public IHttpActionResult GetContainersByApplication(string applicationName)
        {
            try
            {
                if (Shared.AreArgsEmpty(new List<string> { applicationName }))
                {
                    return BadRequest("Field was empty.");
                }

                if (!Shared.DoesApplicationExist(_context, applicationName))
                {
                    return BadRequest("Application or container sent do not exist on our database");
                }

                if (Request.Headers.Count() < 1 ||
                    Request.Headers.Any(x => x.Key == "somiod-discover") ||
                    !string.Equals(Request.Headers.First(x => x.Key == "somiod-discover").Value.FirstOrDefault(), Headers.Container.ToString().ToLower()))
                {
                    return BadRequest("Header was not found or incorrect, please make sure you are sending a correct header with the operation-type necessary");
                }

                var appId = _context.Applications.SingleOrDefault(x => string.Equals(x.Name, applicationName))?.Id;

                var cons = _context.Containers.Where(x => x.Parent == appId).ToList();

                if (cons is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error getting list of subcriptions of database");
                }

                if (cons.Count is 0)
                {
                    return NotFound();
                }

                _context.SaveChanges();
                return Ok(XmlHandler.OnlyContainersXml(cons));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("somiod/{applicationName}")]
        public IHttpActionResult PostContainer(string applicationName, [FromBody] XmlElement container)
        {
            try
            {
                if (container is null || container.Attributes?.Count < 1)
                {
                    return BadRequest("Form sent is empty");
                }

                if (string.IsNullOrEmpty(container.Attributes[1].Value) ||
                    !Shared.IsDateCreatedCorrect(DateTime.Parse(container.Attributes[2].Value)) ||
                    Int32.Parse(container.Attributes[3].Value) != 0)
                {
                    return BadRequest("Input form is not correct, please make sure all fields are filled correctly before inserting a container");
                }

                var newCon = new Container()
                {
                    Name = container.Attributes[1]?.Value,
                    CreatedDate = DateTime.Parse(container.Attributes[2]?.Value),
                    Parent = Int32.Parse(container.Attributes[3].Value)
                };

                if (Shared.DoesContainerExist(_context, newCon.Name))
                {
                    return Content(HttpStatusCode.Conflict, "Container already exists");
                }

                if(!IsContainerConnected(applicationName, newCon))
                {
                    return Content(HttpStatusCode.Conflict, "Container parent you are trying to insert does not exist, please insert a container with an existing parent associated");

                }

                var entity = _context.Containers.Add(newCon);
                if (entity is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error Inserting new container in our database");
                }

                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("somiod/{applicationName}")]
        public IHttpActionResult PutContainer(string applicationName, [FromBody] XmlElement container)
        {
            try
            {
                if (container is null || container.Attributes?.Count < 1)
                {
                    return BadRequest("Form sent is empty");
                }

                if (Int32.Parse(container.Attributes[0].Value) != 0 ||
                    string.IsNullOrEmpty(container.Attributes[1].Value) ||
                    !Shared.IsDateCreatedCorrect(DateTime.Parse(container.Attributes[2].Value)) ||
                    Int32.Parse(container.Attributes[3].Value) != 0)
                {
                    return BadRequest("Input form is not correct, please make sure all fields are filled correctly before updating a container");
                }

                var newCon = new Container()
                {
                    Id = Int32.Parse(container.Attributes[0].Value),
                    Name = container.Attributes[1]?.Value,
                    CreatedDate = DateTime.Parse(container.Attributes[2]?.Value),
                    Parent = Int32.Parse(container.Attributes[3].Value)
                };

                if (!Shared.DoesContainerExist(_context, newCon.Name))
                {
                    return Content(HttpStatusCode.Conflict, "Container does not exist");
                }

                if (!IsContainerConnected(applicationName, newCon))
                {
                    return Content(HttpStatusCode.Conflict, "Container parent you are trying to update does not exist, please update a container with an existing parent associated");

                }

                var entity = _context.Containers.Add(newCon);
                if (entity is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error updating new container in our database");
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
        [Route("somiod/{applicationName}/{containerName}")]
        public IHttpActionResult DeleteContainer(string applicationName, string containerName)
        {
            try
            {
                if (Shared.AreArgsEmpty(new List<string> { applicationName, containerName}))
                {
                    return BadRequest("Form had empty field/s, please fill all mandatory fields");
                }

                if (!Shared.DoesApplicationExist(_context, applicationName))
                {
                    return Content(HttpStatusCode.Conflict, "The container you are trying to insert does not have a existing parent, please delete a container with an existing application associated");
                }

                if (!Shared.DoesContainerExist(_context, containerName))
                {
                    return NotFound();
                }

                var dbSub = _context.Containers.SingleOrDefault(x => string.Equals(x.Name, containerName));

                var entity = _context.Containers.Remove(dbSub);
                if (entity is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error deleting container");
                }
                _context.SaveChanges();
                return Ok();

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private bool IsContainerConnected(string applicationName, Container container)
        {
            if (!Shared.DoesApplicationExist(_context, applicationName)) return false;

            int? appId = _context.Applications.FirstOrDefault(x => string.Equals(x.Name, applicationName))?.Id;
            return _context.Containers.Any(x => string.Equals(x.Name, container.Name) && x.Parent == appId);

        }
    }
}