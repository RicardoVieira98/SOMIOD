using System.Data.SqlClient;
using System.Net;
using System.Web.Configuration;
using System.Web.Http;
using System.Xml;
using System;
using SOMIOD.Models;

namespace SOMIOD.Controllers
{
    public class ContainerController : ApiController
    {
        private readonly string LocalBDConnectionString;

        public ContainerController()
        {
            LocalBDConnectionString = !string.IsNullOrEmpty(WebConfigurationManager.ConnectionStrings["LocalInstanceBD"].ConnectionString) ?
                WebConfigurationManager.ConnectionStrings["LocalInstanceBD"].ConnectionString : "";
        }

        #region GET

        [HttpGet]
        [Route("somiod/{applicationName}/{containerId}")]
        public IHttpActionResult GetContainer(string applicationName, int containerId)
        {
            Container dbContainer = new Container();

            // Retrieve a specific container using ADO.NET
            using (SqlConnection sqlConnection = new SqlConnection(LocalBDConnectionString))
            {
                string cmdText = $"SELECT Id AS ContainerId, Name, CreatedDate, ParentId FROM Container WHERE ApplicationName = @appName AND Id = @containerId;";
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("appName", applicationName));
                cmd.Parameters.Add(new SqlParameter("containerId", containerId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dbContainer.Id = (int)reader["ContainerId"];
                        dbContainer.Name = (string)reader["Name"];
                        dbContainer.CreatedDate = (DateTime)reader["CreatedDate"];
                        dbContainer.ParentId = (int)reader["ParentId"];
                    }
                }
            }

            // Return container details
            return Ok(dbContainer);
        }

        #endregion GET

        #region POST

        [HttpPost]
        [Route("somiod/{applicationName}")]
        public IHttpActionResult PostContainer(string applicationName, [FromBody] XmlElement data)
        {
            if (data == null) { return BadRequest(); }

            var container = new Container()
            {
                Name = data.SelectSingleNode("/container/@NAME")?.InnerText,
                CreatedDate = DateTime.Parse(data.SelectSingleNode("/container/@CREATEDDATE")?.InnerText),
                ParentId = int.Parse(data.SelectSingleNode("/container/@PARENT")?.InnerText)
            };

            // Insert container using ADO.NET
            using (SqlConnection sqlConnection = new SqlConnection(LocalBDConnectionString))
            {
                string cmdText = $"INSERT INTO Container (Name, CreatedDate, ApplicationName, ParentId) VALUES (@name, @createddate, @appName, @parentId);";
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("name", container.Name));
                cmd.Parameters.Add(new SqlParameter("createddate", container.CreatedDate));
                cmd.Parameters.Add(new SqlParameter("appName", applicationName));
                cmd.Parameters.Add(new SqlParameter("parentId", container.ParentId));
                var rowModified = cmd.ExecuteNonQuery();

                if (rowModified < 1) return Content(HttpStatusCode.InternalServerError, "Error Inserting New Container");
            }

            return Ok();
        }

        #endregion POST

        #region PUT

        [HttpPut]
        [Route("somiod/{applicationName}/{containerId}")]
        public IHttpActionResult PutContainer(string applicationName, int containerId, [FromBody] XmlElement data)
        {
            if (data == null) { return BadRequest(); }

            var container = new Container()
            {
                Id = containerId,
                Name = data.SelectSingleNode("/container/@NAME")?.InnerText,
                CreatedDate = DateTime.Parse(data.SelectSingleNode("/container/@CREATEDDATE")?.InnerText),
                ParentId = int.Parse(data.SelectSingleNode("/container/@PARENT")?.InnerText)
            };

            // Update container using ADO.NET
            using (SqlConnection sqlConnection = new SqlConnection(LocalBDConnectionString))
            {
                string cmdText = $"UPDATE Container SET Name = @name, CreatedDate = @createddate, ParentId = @parentId WHERE Id = @id AND ApplicationName = @appName;";
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("id", container.Id));
                cmd.Parameters.Add(new SqlParameter("name", container.Name));
                cmd.Parameters.Add(new SqlParameter("createddate", container.CreatedDate));
                cmd.Parameters.Add(new SqlParameter("parentId", container.ParentId));
                cmd.Parameters.Add(new SqlParameter("appName", applicationName));
                var rowModified = cmd.ExecuteNonQuery();

                if (rowModified < 1) return Content(HttpStatusCode.InternalServerError, "Error Updating Container");
            }

            return Ok();
        }

        #endregion PUT

        #region DELETE

        [HttpDelete]
        [Route("somiod/{applicationName}/{containerId}")]
        public IHttpActionResult DeleteContainer(string applicationName, int containerId)
        {
            // Delete container using ADO.NET
            using (SqlConnection sqlConnection = new SqlConnection(LocalBDConnectionString))
            {
                string cmdText = $"DELETE FROM Container WHERE Id = @id AND ApplicationName = @appName;";
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("id", containerId));
                cmd.Parameters.Add(new SqlParameter("appName", applicationName));
                var rowModified = cmd.ExecuteNonQuery();

                if (rowModified < 1) return NotFound();
            }

            return Ok();
        }

        #endregion DELETE
    }
}