using SOMIOD.Data;
using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using WebGrease.Css.Extensions;

namespace SOMIOD.Controllers
{
    public class ApplicationController : ApiController
    {
        private readonly SomiodDBContext _context;

        public ApplicationController(SomiodDBContext context)
        {
            _context = context;
        }

        [HttpGet]   
        [Route("somiod/{applicationName}")]
        public IHttpActionResult GetApplication(string applicationName)
        {
            if (String.IsNullOrEmpty(applicationName))
            {
                return BadRequest();
            }

            Application dbApp = new Application();

            var result = _context.Applications.FirstOrDefault(x => x.Name.ToLower() == applicationName.ToLower());
            if (result is null)
            {
                return NotFound();
            }
            dbApp = result;
            _context.SaveChanges();

            return Ok(dbApp);
        }

        [HttpPost]
        [Route("somiod")]
        public IHttpActionResult PostApplication([FromBody] XmlElement data)
        {
            if (data == null) { return BadRequest(); }

            var dbApp = new Application()
            {
                Name = data.SelectSingleNode("/application/@NAME")?.InnerText,
                CreatedDate = DateTime.Parse(data.SelectSingleNode("/application/@CREATEDDATE")?.InnerText)
            };

            var entity = _context.Applications.Add(dbApp);
            if(entity is null) return Content(HttpStatusCode.InternalServerError, "Error Inserting new Application");
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut]
        [Route("somiod")]
        public IHttpActionResult PutApplication([FromBody] XmlElement data)
        {
            if (data == null) { return BadRequest(); }

            var dbApp = new Application()
            {
                Id = int.Parse(data.SelectSingleNode("/application/@ID")?.InnerText),
                Name = data.SelectSingleNode("/application/@NAME")?.InnerText,
                CreatedDate = DateTime.Parse(data.SelectSingleNode("/application/@CREATEDDATE")?.InnerText)
            };

            var entity = _context.Applications.Add(dbApp);
            if (entity is null) return Content(HttpStatusCode.InternalServerError, "Error Updating Application");
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        [Route("somiod/{applicationId}")]
        public IHttpActionResult DeleteApplication(string applicationId)
        {
            if (String.IsNullOrEmpty(applicationId)) { return BadRequest(); }

            var entity = _context.Applications.FirstOrDefault(x => x.Id == Int32.Parse(applicationId));
            
            if(entity is null)
            {
                return NotFound();
            }

            var entityRemoved = _context.Applications.Remove(entity);
            if (entityRemoved is null) return Content(HttpStatusCode.InternalServerError, "Error Deleting Application");
            _context.SaveChanges();

            return Ok();
        }
    }
}