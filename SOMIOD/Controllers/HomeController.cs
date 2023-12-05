using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace SOMIOD.Controllers
{
    public class HomeController : ApiController
    {
        public IHttpActionResult Index()
        {
            return Ok();
        }

        [Route("somiod")]
        [HttpPost]
        public IHttpActionResult PostData([FromBody] string data)
        {
            //gurdar na BD o estado
            return Ok();
        }
    }
}
