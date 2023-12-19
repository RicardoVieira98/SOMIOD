using SOMIOD.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace SOMIOD.Controllers
{
    public class DataController : ApiController
    {
        private readonly string LocalBDConnectionString;

        public DataController()
        {
            LocalBDConnectionString = !string.IsNullOrEmpty(WebConfigurationManager.ConnectionStrings["LocalInstanceBD"].ConnectionString) ?
                WebConfigurationManager.ConnectionStrings["LocalInstanceBD"].ConnectionString : "";
        }

        [HttpGet]
        [Route ("somiod/{application}/{container}/data/{int:id}")]

        public IHttpActionResult GetData(int? application, int? container, int? id)
        {
            if (id is null || application is null || container is n)
            {
                return BadRequest();
            }

            Models.Data dbData = new Models.Data();


            SomiodDBContext context = new SomiodDBContext(LocalBDConnectionString);
            //application
            var App = context.Applications.FirstOrDefault(x => x.Id == application);
            if(App == null) { return NotFound(); }
            if (context == null) { return NotFound(); }
            //container
            var result = context.Containers.Where(x => x.Id == container && x.Parent == context.Applications.FirstOrDefault(y =>y.Id == application).Id);
            if (result == null) { return NotFound(); }
            if (context == null) { return NotFound(); }

            //data
            var result2 = context.Datas.Where(x => x.Id == id 
                && x.Parent == context.Containers.FirstOrDefault(y => y.Id == container 
                    && y.Parent == context.Applications.FirstOrDefault(z => z.Id == application).Id).Id).FirstOrDefault();
            if (result2 == null) { return NotFound(); }
            if (context == null) { return NotFound(); }

            dbData = result2;
            context.SaveChanges();

            return Ok(dbData);
        }
    }
}