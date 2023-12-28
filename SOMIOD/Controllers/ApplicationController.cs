﻿using SOMIOD.Data;
using SOMIOD.Library;
using SOMIOD.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Xml;

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
                    //RICARDO SEND NICER MESSAGES
                    return BadRequest();
                }

                var apps = _context.Applications.Where(x => x.Name.ToLower() == applicationName.ToLower()).ToList();
                if (apps is null)
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
                    !Request.Headers.Any(x => x.Key == "somiod-discover") ||
                    !string.Equals(Request.Headers.First(x => x.Key == "somiod-discover").Value.FirstOrDefault(), Headers.Application.ToString()))
                {
                    return BadRequest();
                }

                var applications = _context.Applications.ToList();
                return Ok(XmlHandler.OnlyApplicationsXml(applications));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("somiod")]
        public IHttpActionResult PostApplication([FromBody] XmlElement data)
        {
            try
            {
                if (Shared.AreArgsEmpty(new List<string> { data.InnerText })) 
                { 
                    return BadRequest(); 
                }

                var dbApp = new Application()
                {
                    Name = data.SelectSingleNode("/application/[@NAME]")?.InnerText,
                    CreatedDate = DateTime.Parse(data.SelectSingleNode("/application/[@CREATEDDATE]")?.InnerText)
                };

                var entity = _context.Applications.Add(dbApp);
                if (entity is null)
                {
                    return Content(HttpStatusCode.InternalServerError, "Error Inserting new Application");
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
        public IHttpActionResult PutApplication([FromBody] XmlElement data)
        {
            try
            {
                if (Shared.AreArgsEmpty(new List<string> { data.InnerText })) 
                { 
                    return BadRequest(); 
                }

                var app = new Application()
                {
                    Id = int.Parse(data.SelectSingleNode("/application/[@ID]")?.InnerText),
                    Name = data.SelectSingleNode("/application/[@NAME]")?.InnerText,
                    CreatedDate = DateTime.Parse(data.SelectSingleNode("/application/[@CREATEDDATE]")?.InnerText)
                };

                var dbApp = _context.Applications.SingleOrDefault(x => x.Id == app.Id);
                if (dbApp is null)
                {
                    return BadRequest();
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
        [Route("somiod/{applicationId}")]
        public IHttpActionResult DeleteApplication(string applicationName)
        {
            try
            {
                if (Shared.AreArgsEmpty(new List<string> { applicationName})) 
                { 
                    return BadRequest(); 
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