using SOMIOD.Data;
using SOMIOD.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Xml;
using Container = SOMIOD.Library.Models.Container;

namespace SOMIOD.Controllers
{
    [RoutePrefix("api/somiod")]
    public class ContainerController : ApiController
    {
        private readonly SomiodDBContext _context;

        public ContainerController()
        {
            _context = new SomiodDBContext();
        }

        [HttpGet]
        [Route("{applicationName}/{containerName}")]
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
        [Route("application/container")]
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
        [Route("{applicationName}/con")]
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
                    !Request.Headers.Any(x => x.Key == "somiod-discover") ||
                    !string.Equals(Request.Headers.First(x => x.Key == "somiod-discover").Value.FirstOrDefault(), Headers.Container.ToString()))
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
        [Route("{applicationParentName}/insert")]
        public IHttpActionResult PostContainer(string applicationParentName, [FromBody] XmlElement container)
        {
            try
            {
                if (container is null || container.Attributes?.Count < 1)
                {
                    return BadRequest("Form sent is empty");
                }

                if (!Shared.DoesApplicationExist(_context, applicationParentName))
                {
                    return BadRequest("Application parent does not exist");
                }

                if (!Shared.IsDateCreatedCorrect(DateTime.Parse(container.Attributes["createddate"]?.Value)))
                {
                    return BadRequest("Input form is not correct, please make sure all fields are filled correctly before inserting a container");
                }

                var newCon = new Container()
                {
                    Name = Shared.FillResourceName(container),
                    CreatedDate = DateTime.Parse(container.Attributes["createddate"]?.Value),
                    Parent = _context.Applications.FirstOrDefault(x => x.Name == applicationParentName).Id,
                };

                if (Shared.DoesContainerExist(_context, newCon.Name))
                {
                    return Content(HttpStatusCode.Conflict, "Container already exists");
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
        [Route("{applicationParentName}/update")]
        public IHttpActionResult PutContainer(string applicationParentName, [FromBody] XmlElement container)
        {
            try
            {
                if (container is null || container.Attributes?.Count < 1)
                {
                    return BadRequest("Form sent is empty");
                }

                if (!Shared.DoesApplicationExist(_context, applicationParentName))
                {
                    return BadRequest("Application parent does not exist");
                }

                if (Int32.Parse(container.Attributes["id"]?.Value) == 0 ||
                    string.IsNullOrEmpty(container.Attributes["name"]?.Value) ||
                    !Shared.IsDateCreatedCorrect(DateTime.Parse(container.Attributes["createddate"]?.Value)))
                {
                    return BadRequest("Input form is not correct, please make sure all fields are filled correctly before updating a container");
                }

                var newCon = new Container()
                {
                    Id = Int32.Parse(container.Attributes["id"]?.Value),
                    Name = container.Attributes["name"]?.Value,
                    CreatedDate = DateTime.Parse(container.Attributes["createddate"]?.Value),
                    Parent = _context.Applications.FirstOrDefault(x => string.Equals(x.Name, applicationParentName)).Id
                };

                if (!Shared.DoesContainerExist(_context, newCon.Id))
                {
                    return Content(HttpStatusCode.Conflict, "Container does not exist");
                }

                if (!Shared.IsContainerConnected(_context,applicationParentName, newCon))
                {
                    return Content(HttpStatusCode.Conflict, "Container parent you are trying to update does not exist, please update a container with an existing parent associated");
                }

                var dbCon = _context.Containers.FirstOrDefault(x => x.Id == newCon.Id);
                if (dbCon is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error retrieving container in our database");
                }

                dbCon.Name = newCon.Name;
                dbCon.CreatedDate = newCon.CreatedDate;
                dbCon.Parent = newCon.Parent;

                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{applicationName}/{containerName}")]
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

                if(IsContainerAParent(dbSub))
                {
                    return Content(HttpStatusCode.Conflict, "Container you are trying to delete has datas or subscriptions as children, delete the children first");
                }

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

        private bool IsContainerAParent(Container container)
        {
            return _context.Subscriptions.Any(x => x.Parent == container.Id) &&
                    _context.Datas.Any(y => y.Parent == container.Id);
        }
    }
}