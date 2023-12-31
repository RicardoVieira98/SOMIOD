﻿using SOMIOD.Data;
using SOMIOD.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Xml;
using Application = SOMIOD.Library.Models.Application;

namespace SOMIOD.Controllers
{
    public class ApplicationController : ApiController
    {
        private readonly SomiodDBContext _context;

        public ApplicationController()
        {
            _context = new SomiodDBContext();
        }

        [HttpGet]   
        [Route("somiod/{applicationName}")]
        public IHttpActionResult GetApplication(string applicationName)
        {
            try
            {
                if (Shared.AreArgsEmpty(new List<string> { applicationName }))
                {
                    return BadRequest("Requested resource is empty, please request a resource");
                }

                if(!Shared.DoesApplicationExist(_context,applicationName))
                {
                    return NotFound();
                }

                var apps = _context.Applications.Where(x => string.Equals(x.Name,applicationName)).ToList();

                if (apps is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error getting application of database");
                }

                if (apps.Count is 0)
                {
                    return NotFound();
                }

                return Ok(XmlHandler.OnlyApplicationsXml(apps));
            }
            catch(Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
        [HttpGet]
        [Route("somiod")]
        public IHttpActionResult GetApplications()
        {
            try
            {
                if (Request.Headers.Count() < 1 ||
                    Request.Headers.All(x => x.Key != "somiod-discover") ||
                    !string.Equals(Request.Headers.First(x => x.Key == "somiod-discover").Value.FirstOrDefault(), Headers.Application.ToString()))
                {
                    return BadRequest("Header was not found or incorrect, please make sure you are sending a correct header with the operation-type necessary");
                }

                var applications = _context.Applications.ToList();
                if (applications is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error getting applications of database");
                }

                if (applications.Count is 0)
                {
                    return NotFound();
                }

                return Ok(XmlHandler.OnlyApplicationsXml(applications));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("somiod")]
        public IHttpActionResult PostApplication([FromBody] XmlElement application)
        {
            try
            {
                if (application is null || application.Attributes?.Count < 1)
                {
                    return BadRequest("Form sent is empty");
                }

                if (string.IsNullOrEmpty(application.Attributes[1].Value) ||
                    !Shared.IsDateCreatedCorrect(DateTime.Parse(application.Attributes[2].Value)))
                {
                    return BadRequest("Input form is not correct, please make sure all fields are filled correctly before updating a application");
                }

                var newApp = new Application()
                {
                    Name = application.Attributes[1]?.Value,
                    CreatedDate = DateTime.Parse(application.Attributes[2]?.Value)
                };

                if(Shared.DoesApplicationExist(_context, newApp.Name))
                {
                    return Content(HttpStatusCode.Conflict, "Application already exists");
                }

                var entity = _context.Applications.Add(newApp);
                if (entity is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error Inserting new Application in our database");
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
        [Route("somiod")]
        public IHttpActionResult PutApplication([FromBody] XmlElement application)
        {
            try
            {
                if (application is null || application.Attributes?.Count < 1)
                {
                    return BadRequest("Form sent is empty");
                }

                if (string.IsNullOrEmpty(application.Attributes[1].Value) ||
                    !Shared.IsDateCreatedCorrect(DateTime.Parse(application.Attributes[2].Value)))
                {
                    return BadRequest("Input form is not correct, please make sure all fields are filled correctly before updating a application");
                }

                var app = new Application()
                {
                    Id = Int32.Parse(application.Attributes[0]?.Value),
                    Name = application.Attributes[1]?.Value,
                    CreatedDate = DateTime.Parse(application.Attributes[2]?.Value)
                };

                if (!Shared.DoesApplicationExist(_context, app.Name))
                {
                    return NotFound();
                }

                var dbApp = _context.Applications.SingleOrDefault(x => x.Id == app.Id);
                if (dbApp is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error updating application in our database");
                }

                dbApp.Name = app.Name;
                dbApp.CreatedDate = app.CreatedDate;

                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("somiod/{applicationName}")]
        public IHttpActionResult DeleteApplication(string applicationName)
        {
            try
            {
                if (Shared.AreArgsEmpty(new List<string> { applicationName})) 
                {
                    return BadRequest("Input form had empty field/s, please fill all mandatory fields");
                }

                var entity = _context.Applications.FirstOrDefault(x => string.Equals(x.Name, applicationName));

                if (entity is null)
                {
                    return NotFound();
                }

                var entityRemoved = _context.Applications.Remove(entity);
                if (entityRemoved is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error Deleting Application");
                }

                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}