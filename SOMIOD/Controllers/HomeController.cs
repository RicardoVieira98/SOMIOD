using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Xml.Linq;

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
        public IHttpActionResult PostApplication([FromBody] XElement data)
        {
            if (data is null)
            {
                return BadRequest();
            }

            //gurdar na BD o estado

            return Ok();
        }

        [Route("somiod/{application}")]
        [HttpPost]
        public IHttpActionResult PostContainer(string application, [FromBody] XElement data)
        {
            if (data is null)
            {
                return BadRequest();
            }

            //gurdar na BD o estado

            return Ok();
        }

        [Route("somiod/{application}/{container}")]
        [HttpPost]
        public IHttpActionResult PostSubscription(string application,string container, [FromBody] XElement data)
        {
            if (data is null)
            {
                return BadRequest();
            }   

            //gurdar na BD o estado


            return Ok();
        }

        [Route("somiod/{application}/{container}")]
        [HttpPost]
        public IHttpActionResult PostData(string application, string container, [FromBody] XElement data)
        {
            if(data is null || string.IsNullOrEmpty(application) || string.IsNullOrEmpty(container))
            {
                return BadRequest();
            }

            //gurdar na BD o estado

            return Ok();
        }
    }
}
