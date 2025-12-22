using DocumentFormat.OpenXml.EMMA;
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

namespace EmpireOneRestAPIFHS.DataManager
.Controllers
{
    // CORS for TechJump frontends + localhost dev
    [EnableCors(
        origins:
            "http://localhost:4200," +
            "https://localhost:4200," +
            "https://CMSDemo.com," +
            "https://www.CMSDemo.com," +
            "https://techinterviewjump.com," +
            "https://www.techinterviewjump.com",
        headers: "*",
        methods: "*")]
    [RoutePrefix("api/profile")]
    public class CMSDemoProfileController : ApiController
    {
        private readonly DataAccess02 _data = new DataAccess02();
        private readonly string _connString =
            ConfigurationManager.ConnectionStrings["CMSDemoDB"]?.ConnectionString;

        // GET /api/CMSDemoImageLoad/db-ping
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

        // POST /api/CMSDemoinsertITechCards
        // Body: InsertITechCards
        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(CreateImageLoadRequest req)
        {
           
            try
            {
                using (var conn = new SqlConnection(_connString))
                using (var cmd = new SqlCommand("Create_UserImageLoads", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = req.UserId;
                    cmd.Parameters.Add("@UserAlias", SqlDbType.VarChar, 8).Value = req.UserAlias.ToUpper();
                    cmd.Parameters.Add("@PlanCode", SqlDbType.VarChar, 50).Value = req.PlanCode.ToUpper();
                  

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult))
                    {
                        if (!reader.HasRows)
                            return InternalServerError(new Exception("Create_UserImageLoads did not return a row."));

                        ImageLoadWithPlan created = null;
                        if (await reader.ReadAsync())
                            created = MapImageLoadWithPlan(reader);

                        if (created == null)
                            return InternalServerError(new Exception("Failed to map created ImageLoad."));

                        // 201 Created with Location header
                        var location = new Uri(Request.RequestUri, created.ImageLoadId.ToString());
                        return Created(location, new { ok = true, ImageLoad = created });
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
        /// <summary>
        /// -----------------------------------------------------------------  Get ------------
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        // POST /api/CMSDemo/support/contact
        [HttpPost, Route("support/contact")]
        public async Task<IHttpActionResult> Insert_ITechCard_Mega(ModelSupportTicket model)
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

            // Return simple payload; you can later map to a DTO if you like
            return Ok(new
            {
                TicketId = newTicketId,
                Status = "Open",
                Message = "Support ticket created successfully."
            });
        }





        //----------------------------------------------------------------------------------------

        // GET /api/CMSDemoImageLoad/{id}
        [HttpGet, Route("{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            if (string.IsNullOrWhiteSpace(_connString))
                return BadRequest("Connection string 'CMSDemoDB' not found.");

            const string sql = @"
                SELECT us.*, p.PlanName, p.Description, p.TicketSets
                FROM dbo.UserImageLoads us
                JOIN dbo.ImageLoadPlans p ON p.PlanCode = us.PlanCode
                WHERE us.ImageLoadId = @id;
                ";

            try
            {
                using (var conn = new SqlConnection(_connString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow))
                    {
                        if (!reader.HasRows) return NotFound();

                        ImageLoadWithPlan found = null;
                        if (await reader.ReadAsync())
                            found = MapImageLoadWithPlan(reader);

                        return Ok(new { ok = true, ImageLoad = found });
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


        /// <summary>Get BallCount Powerball winners by weeks</summary>
        [HttpGet, Route("profile/getprofilebyuseralias")]
        public IHttpActionResult Get_Profile_byUserAlias([FromUri][Required] string useralias)
            => Ok(_data.Get_UserInfowithCards(useralias));




      

        // --------------------------
        // Helpers / DTOs
        // --------------------------

        private static ImageLoadWithPlan MapImageLoadWithPlan(SqlDataReader r)
        {
            // Guard for DBNull on nullable fields
            DateTime? dt(object o) => o == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(o);
            int? ni(object o) => o == DBNull.Value ? (int?)null : Convert.ToInt32(o);
            string ns(object o) => o == DBNull.Value ? null : Convert.ToString(o);

            return new ImageLoadWithPlan
            {
                // UserImageLoads
                ImageLoadId = Convert.ToInt32(r["ImageLoadId"]),
                UserId = Convert.ToInt32(r["UserId"]),
                UserAlias = Convert.ToString(r["UserAlias"]),
                PlanCode = Convert.ToString(r["PlanCode"]),
                Status = Convert.ToString(r["Status"]),
                AutoRenew = Convert.ToBoolean(r["AutoRenew"]),
                Quantity = Convert.ToInt32(r["Quantity"]),
                StartUtc = Convert.ToDateTime(r["StartUtc"]),
                CurrentPeriodStartUtc = dt(r["CurrentPeriodStartUtc"]),
                CurrentPeriodEndUtc = dt(r["CurrentPeriodEndUtc"]),
                TrialEndUtc = dt(r["TrialEndUtc"]),
                CanceledAtUtc = dt(r["CanceledAtUtc"]),
                EndedAtUtc = dt(r["EndedAtUtc"]),
                PriceAtPurchase = Convert.ToDecimal(r["PriceAtPurchase"]),
                Currency = Convert.ToString(r["Currency"]),
                ExternalProvider = ns(r["ExternalProvider"]),
                ExternalImageLoadId = ns(r["ExternalImageLoadId"]),
                CreatedAt = Convert.ToDateTime(r["CreatedAt"]),
                UpdatedAt = Convert.ToDateTime(r["UpdatedAt"]),

                // From ImageLoadPlans join
                PlanName = Convert.ToString(r["PlanName"]),
                Description = ns(r["Description"]),
                TicketSets = ni(r["TicketSets"])
            };
        }
    }

    // --------- DTOs ---------

    public sealed class CreateImageLoadRequestProfile
    {
        public int UserId { get; set; }              // required
        public string UserAlias { get; set; } = "";      // exactly 8 chars
        public string PlanCode { get; set; } = "";      // required
        //public bool AutoRenew { get; set; } = true;    // optional, default true
        //public int Quantity { get; set; } = 1;       // optional, default 1
        //public string ExternalProvider { get; set; }     // optional
        //public string ExternalImageLoadId { get; set; } // optional
    }

    public sealed class ImageLoadWithPlanProfile
    {
        // From UserImageLoads
        public int ImageLoadId { get; set; }
        public int UserId { get; set; }
        public string UserAlias { get; set; }
        public string PlanCode { get; set; }
        public string Status { get; set; }
        public bool AutoRenew { get; set; }
        public int Quantity { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime? CurrentPeriodStartUtc { get; set; }
        public DateTime? CurrentPeriodEndUtc { get; set; }
        public DateTime? TrialEndUtc { get; set; }
        public DateTime? CanceledAtUtc { get; set; }
        public DateTime? EndedAtUtc { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public string Currency { get; set; }
        public string ExternalProvider { get; set; }
        public string ExternalImageLoadId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // From ImageLoadPlans join
        public string PlanName { get; set; }
        public string Description { get; set; }
        public int? TicketSets { get; set; }
    }
}

