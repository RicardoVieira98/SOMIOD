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

namespace SOMIOD.Controllers
{
    public class ApplicationController : ApiController
    {
        private readonly string LocalBDConnectionString = WebConfigurationManager.ConnectionStrings["LocalInstanceBD"].ConnectionString;

        public ApplicationController()
        {
            LocalBDConnectionString = !string.IsNullOrEmpty(WebConfigurationManager.ConnectionStrings["LocalInstanceBD"].ConnectionString) ?
                WebConfigurationManager.ConnectionStrings["LocalInstanceBD"].ConnectionString : "";
        }

        [HttpGet]
        [Route("somiod")]
        public IHttpActionResult GetApplication(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            Application dbApp = new Application();

            #region  retrieve Application using ADO.NET
            //using (SqlConnection sqlConnection = new SqlConnection())
            //{
            //    string cmdText = $"SELECT * FROM Applications WHERE Name = @name;";
            //    sqlConnection.ConnectionString = LocalBDConnectionString;
            //    sqlConnection.Open();
            //    SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
            //    cmd.Parameters.Add(new SqlParameter("name", name));

            //    using (SqlDataReader reader = cmd.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            dbApp.CreatedDate = (DateTime)reader[0];
            //            dbApp.Id = (int)reader[1];
            //            dbApp.Name = (string)reader[2];
            //            //etc...
            //        }
            //    }

            //    sqlConnection.Dispose();
            //    sqlConnection.Close();

            //}
            #endregion

            #region retrieve Application using Entity Framework
            SomiodDBContext context = new SomiodDBContext(LocalBDConnectionString);
            var result = context.Applications.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            if (result is null)
            {
                return NotFound();
            }
            dbApp = result;
            #endregion

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

            #region  insert Application using ADO.NET
            //using (SqlConnection sqlConnection = new SqlConnection())
            //{
            //    string cmdText = $"INSERT INTO Applications (Name, CreatedDate) VALUES (@name,@createddate);";
            //    sqlConnection.ConnectionString = LocalBDConnectionString;
            //    sqlConnection.Open();
            //    SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
            //    cmd.Parameters.Add(new SqlParameter("name", dbApp.Name));
            //    cmd.Parameters.Add(new SqlParameter("createddate", dbApp.CreatedDate));
            //    var rowModified = cmd.ExecuteNonQuery();

            //    if (rowModified < 1) return Content(HttpStatusCode.InternalServerError, "Error Inserting New Application");

            //    sqlConnection.Dispose();
            //    sqlConnection.Close();

            //}
            #endregion

            #region insert Application using Entity Framework
            SomiodDBContext context = new SomiodDBContext(LocalBDConnectionString);
            var entity = context.Applications.Add(dbApp);
            if(entity is null) return Content(HttpStatusCode.InternalServerError, "Error Inserting new Application");
            #endregion

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

            #region  updating Application using ADO.NET
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                string cmdText = $"UPDATE Applications SET Name = @name,CreatedDate = @createddate WHERE Id = @id;";
                sqlConnection.ConnectionString = LocalBDConnectionString;
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("id", dbApp.Id));
                cmd.Parameters.Add(new SqlParameter("name", dbApp.Name));
                cmd.Parameters.Add(new SqlParameter("createddate", dbApp.CreatedDate));
                var rowModified = cmd.ExecuteNonQuery();

                if (rowModified < 1) return Content(HttpStatusCode.InternalServerError, "Error Updating Application");

                sqlConnection.Dispose();
                sqlConnection.Close();
            }
            #endregion

            #region updating Application using Entity Framework
            SomiodDBContext context = new SomiodDBContext(LocalBDConnectionString);
            var entity = context.Applications.Add(dbApp);
            if (entity is null) return Content(HttpStatusCode.InternalServerError, "Error Updating Application");
            #endregion

            return Ok();
        }

        [HttpDelete]
        [Route("somiod")]
        public IHttpActionResult DeleteApplication(string id)
        {
            if (String.IsNullOrEmpty(id)) { return BadRequest(); }

            #region  deleting Application using ADO.NET
            //using (SqlConnection sqlConnection = new SqlConnection())
            //{
            //    string cmdText = $"DELETE FROM Application WHERE Id = @id;";
            //    sqlConnection.ConnectionString = LocalBDConnectionString;
            //    sqlConnection.Open();
            //    SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
            //    cmd.Parameters.Add(new SqlParameter("id", id));
            //    var rowModified = cmd.ExecuteNonQuery();

            //    if (rowModified < 1) return NotFound();

            //    sqlConnection.Dispose();
            //    sqlConnection.Close();
            //}
            #endregion

            #region deleting Application using Entity Framework
            SomiodDBContext context = new SomiodDBContext(LocalBDConnectionString);
            var entity = context.Applications.FirstOrDefault(x => x.Id == Int32.Parse(id));
            
            if(entity is null)
            {
                return NotFound();
            }

            var entityRemoved = context.Applications.Remove(entity);
            if (entityRemoved is null) return Content(HttpStatusCode.InternalServerError, "Error Deleting Application");
            #endregion

            return Ok();
        }
    }
}