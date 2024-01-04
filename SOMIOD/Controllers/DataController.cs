using Microsoft.Ajax.Utilities;
using SOMIOD.Data;
using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace SOMIOD.Controllers
{
    public class DataController : ApiController
    {
        //private readonly string LocalBDConnectionString;
        private readonly SomiodDBContext _context;
        public DataController(SomiodDBContext context)
        {
            _context = context;
        }
        //public IHttpActionResult Verify(int? application, int? container, int? id)
        //{
        //    if (id is null || application is null || container is null)
        //    {
        //        return BadRequest();
        //    }
        //    //application
        //    var App = _context.Applications.FirstOrDefault(x => x.Id == application);
        //    if (App == null) { return NotFound(); }
        //    if (_context == null) { return NotFound(); }
        //    //container
        //    var result = _context.Containers.Where(x => x.Id == container && x.Parent == _context.Applications.FirstOrDefault(y => y.Id == application).Id);
        //    if (result == null) { return NotFound(); }
        //    if (_context == null) { return NotFound(); }

        //    //data
        //    var result2 = _context.Datas.Where(x => x.Id == id
        //        && x.Parent == _context.Containers.FirstOrDefault(y => y.Id == container
        //            && y.Parent == _context.Applications.FirstOrDefault(z => z.Id == application).Id).Id).FirstOrDefault();
        //    if (result2 == null) { return NotFound(); }
        //    if (_context == null) { return NotFound(); }
        //    return Ok();
        //}

        [HttpGet]
        [Route("somiod/{application}/{container}/data/{int:id}")]

        public IHttpActionResult GetData(int? application, int? container, int? id)
        {
            if (id is null || application is null || container is null)
            {
                return BadRequest();
            }

            Models.Data dbData = new Models.Data();


            //// SomiodDBContext _context = new SomiodDBContext(LocalBDConnectionString);
            //application
            var App = _context.Applications.FirstOrDefault(x => x.Id == application);
            if (App == null) { return NotFound(); }
            if (_context == null) { return NotFound(); }
            //container
            var result = _context.Containers.Where(x => x.Id == container && x.Parent == _context.Applications.FirstOrDefault(y => y.Id == application).Id);
            if (result == null) { return NotFound(); }
            if (_context == null) { return NotFound(); }

            //data
            var result2 = _context.Datas.Where(x => x.Id == id
                && x.Parent == _context.Containers.FirstOrDefault(y => y.Id == container
                    && y.Parent == _context.Applications.FirstOrDefault(z => z.Id == application).Id).Id).FirstOrDefault();
            if (result2 == null) { return NotFound(); }
            if (_context == null) { return NotFound(); }
            // Verify(application, container, id);
            dbData = result2;
            _context.SaveChanges();

            return Ok(dbData);
        }

        [HttpDelete]
        [Route("somiod/{application}/{container}/data/{int:id}")]
        public IHttpActionResult DeleteData(int? application, int? container, int? id)
        {
            if (id is null || application is null || container is null)
            {
                return BadRequest();
            }

            //application
            var App = _context.Applications.FirstOrDefault(x => x.Id == application);
            if (App == null) { return NotFound(); }
            if (_context == null) { return NotFound(); }
            //container
            var result = _context.Containers.Where(x => x.Id == container && x.Parent == _context.Applications.FirstOrDefault(y => y.Id == application).Id);
            if (result == null) { return NotFound(); }
            if (_context == null) { return NotFound(); }

            //data
            var result2 = _context.Datas.Where(x => x.Id == id
                && x.Parent == _context.Containers.FirstOrDefault(y => y.Id == container
                    && y.Parent == _context.Applications.FirstOrDefault(z => z.Id == application).Id).Id).FirstOrDefault();
            if (result2 == null) { return NotFound(); }
            if (_context == null) { return NotFound(); }

            var entityRemoved = _context.Datas.Remove(result2);
            if (entityRemoved is null) return Content(HttpStatusCode.InternalServerError, "Error Deleting Data");
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("somiod/{application}/{container}/data/{int:id}")]
        public IHttpActionResult PostData(int? application, int? container, int? id)
        {
            if (id is null || application is null || container is null)
            {
                return BadRequest();
            }
            Models.Data dbData = new Models.Data();

            //application
            var App = _context.Applications.FirstOrDefault(x => x.Id == application);
            if (App == null) { return NotFound(); }
            if (_context == null) { return NotFound(); }
            //container
            var result = _context.Containers.Where(x => x.Id == container && x.Parent == _context.Applications.FirstOrDefault(y => y.Id == application).Id);
            if (result == null) { return NotFound(); }
            if (_context == null) { return NotFound(); }

            //data
            var result2 = _context.Datas.Where(x => x.Id == id
                && x.Parent == _context.Containers.FirstOrDefault(y => y.Id == container
                    && y.Parent == _context.Applications.FirstOrDefault(z => z.Id == application).Id).Id).FirstOrDefault();
            if (result2 == null) { return NotFound(); }
            if (_context == null) { return NotFound(); }

            var entity = _context.Datas.Add(dbData);
            if (entity is null) return Content(HttpStatusCode.InternalServerError, "Error Inserting new Data");
            _context.SaveChanges();


            return Ok();
        }
    }
}