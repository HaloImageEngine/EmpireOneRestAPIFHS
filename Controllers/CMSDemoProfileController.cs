using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Presentation;
using EmpireOneRestAPIFHS.Controllers;
using EmpireOneRestAPIFHS.DataManager;
using EmpireOneRestAPIFHS.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using EmpireOneRestAPIFHS.Services;

namespace EmpireOneRestAPIFHS.DataManager.Controllers
{
    // CORS for TechJump frontends + localhost dev
    [EnableCors(
     origins:
            "http://localhost:4200," +
          "https://localhost:4200," +
       "https://firehorseusa.com," +
   "https://www.firehorseusa.com," +
            "https://firehorseusa.com," +
     "https://www.firehorseusa.com",
headers: "*",
        methods: "*")]
    [RoutePrefix("api/profile")]
    public class CMSDemoProfileController : ApiController
    {
      private readonly DataAccess02 _data = new DataAccess02();
        private readonly string _connString =
            ConfigurationManager.ConnectionStrings["CMSDemoDB"]?.ConnectionString;

        // GET /api/profile/db-ping
        [HttpGet, Route("db-ping")]
        public async Task<IHttpActionResult> DbPing()
        {
       if (string.IsNullOrWhiteSpace(_connString))
return BadRequest("Connection string 'CMSDemoDB' not found.");

            try
          {
                using (var conn = new SqlConnection(_connString))
  {
         await conn.OpenAsync();
              using (var cmd = new SqlCommand("SELECT SYSUTCDATETIME()", conn))
             {
   var serverUtc = (DateTime)await cmd.ExecuteScalarAsync();
              return Ok(new
 {
             ok = true,
          serverTimeUtc = serverUtc,
   dataSource = conn.DataSource,
           database = conn.Database
         });
             }
          }
   }
          catch (SqlException ex)
            {
      return InternalServerError(new Exception($"SQL error {ex.Number}: {ex.Message}"));
            }
      catch (Exception ex)
            {
          return InternalServerError(ex);
   }
        }

        // POST /api/profile/support/contact
[HttpPost, Route("support/contact")]
    public async Task<IHttpActionResult> CreateSupportTicket(ModelSupportTicket model)
        {
  if (model == null)
return BadRequest("Request body cannot be null.");

        if (!ModelState.IsValid)
         return BadRequest(ModelState);

      int newTicketId;

            using (var conn = new SqlConnection(_connString))
            using (var cmd = new SqlCommand("dbo.spInsertSupportTicket", conn))
       {
         cmd.CommandType = CommandType.StoredProcedure;

       // Required params
       cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserID;
         cmd.Parameters.Add("@Subject", SqlDbType.VarChar, 800).Value = model.Subject;
          cmd.Parameters.Add("@Comment", SqlDbType.VarChar, 800).Value = model.Comment;

                // Optional params
          cmd.Parameters.Add("@UserAlias", SqlDbType.VarChar, 50)
               .Value = (object)model.UserAlias ?? DBNull.Value;

           cmd.Parameters.Add("@Email", SqlDbType.VarChar, 255)
                    .Value = (object)model.Email ?? DBNull.Value;

                // DTID: let DB default if not set
        var dtidParam = cmd.Parameters.Add("@DTID", SqlDbType.DateTime2);
  if (model.DTID == default(DateTime))
   dtidParam.Value = DBNull.Value;
 else
 dtidParam.Value = model.DTID;

   // Output param
       var outputIdParam = cmd.Parameters.Add("@NewTicketId", SqlDbType.Int);
   outputIdParam.Direction = ParameterDirection.Output;

         await conn.OpenAsync().ConfigureAwait(false);
          await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

                newTicketId = (int)outputIdParam.Value;
 }

         return Ok(new
      {
          TicketId = newTicketId,
                Status = "Open",
        Message = "Support ticket created successfully."
         });
     }

        /// <summary>Get profile by user alias</summary>
     [HttpGet, Route("getprofilebyuseralias")]
        public IHttpActionResult Get_Profile_byUserAlias([FromUri][Required] string useralias)
      => Ok(_data.Get_UserInfowithCards(useralias));
    }
}

