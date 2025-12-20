using DocumentFormat.OpenXml.EMMA;
using EmpireOneRestAPIITJ.Controllers;
using EmpireOneRestAPIITJ.DataManager;
using EmpireOneRestAPIITJ.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EmpireOneRestAPIITJ.DataManager
.Controllers
{
    // CORS for TechJump frontends + localhost dev
    [EnableCors(
        origins:
            "http://localhost:4200," +
            "https://localhost:4200," +
            "https://itechjump.com," +
            "https://www.itechjump.com," +
            "https://techinterviewjump.com," +
            "https://www.techinterviewjump.com",
        headers: "*",
        methods: "*")]
    [RoutePrefix("api/profile")]
    public class ITechJumpProfileController : ApiController
    {
        private readonly DataAccess02 _data = new DataAccess02();
        private readonly string _connString =
            ConfigurationManager.ConnectionStrings["ITechJumpDB"]?.ConnectionString;

        // GET /api/ITechjumpsubscription/db-ping
        [HttpGet, Route("db-ping")]
        public async Task<IHttpActionResult> DbPing()
        {
            if (string.IsNullOrWhiteSpace(_connString))
                return BadRequest("Connection string 'ITechJumpDB' not found.");

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

        // POST /api/ITechjumpinsertITechCards
        // Body: InsertITechCards
        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(CreateSubscriptionRequest req)
        {
           
            try
            {
                using (var conn = new SqlConnection(_connString))
                using (var cmd = new SqlCommand("Create_UserSubscriptions", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = req.UserId;
                    cmd.Parameters.Add("@UserAlias", SqlDbType.VarChar, 8).Value = req.UserAlias.ToUpper();
                    cmd.Parameters.Add("@PlanCode", SqlDbType.VarChar, 50).Value = req.PlanCode.ToUpper();
                  

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult))
                    {
                        if (!reader.HasRows)
                            return InternalServerError(new Exception("Create_UserSubscriptions did not return a row."));

                        SubscriptionWithPlan created = null;
                        if (await reader.ReadAsync())
                            created = MapSubscriptionWithPlan(reader);

                        if (created == null)
                            return InternalServerError(new Exception("Failed to map created subscription."));

                        // 201 Created with Location header
                        var location = new Uri(Request.RequestUri, created.SubscriptionId.ToString());
                        return Created(location, new { ok = true, subscription = created });
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
        // POST /api/ITechjump/support/contact
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

        // GET /api/ITechjumpsubscription/{id}
        [HttpGet, Route("{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            if (string.IsNullOrWhiteSpace(_connString))
                return BadRequest("Connection string 'ITechJumpDB' not found.");

            const string sql = @"
                SELECT us.*, p.PlanName, p.Description, p.TicketSets
                FROM dbo.UserSubscriptions us
                JOIN dbo.SubscriptionPlans p ON p.PlanCode = us.PlanCode
                WHERE us.SubscriptionId = @id;
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

                        SubscriptionWithPlan found = null;
                        if (await reader.ReadAsync())
                            found = MapSubscriptionWithPlan(reader);

                        return Ok(new { ok = true, subscription = found });
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




        // GET /api/ITechjumpsubscription/{plancode}
        [HttpGet, Route("{plancode:maxlength(50)}")]   // ← valid; or just "{plancode}"
        public async Task<IHttpActionResult> Get_PlanDetail_ByPlanCode(string plancode)
        {
            if (string.IsNullOrWhiteSpace(_connString))
                return BadRequest("Connection string 'ITechJumpDB' not found.");

            if (string.IsNullOrWhiteSpace(plancode))
                return BadRequest("PlanCode is required.");

            try
            {
                using (var conn = new SqlConnection(_connString))
                using (var cmd = new SqlCommand("dbo.Get_Subscription_byPlanCode", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PlanCode", SqlDbType.VarChar, 50).Value = plancode.Trim();

                    await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow))
                    {
                        if (!reader.HasRows)
                            return NotFound();

                        ModelSubscriptionPlans found = null;

                        if (await reader.ReadAsync())
                        {
                            // Ordinals (faster + avoids typos)
                            int oPlanCode = reader.GetOrdinal("PlanCode");
                            int oPlanName = reader.GetOrdinal("PlanName");
                            int oDescription = reader.GetOrdinal("Description");
                            int oPrice = reader.GetOrdinal("Price");
                            // int oBillingPeriod = reader.GetOrdinal("BillingPeriod"); // returned by SP but not in this model
                            int oMinNumbers = reader.GetOrdinal("MinNumbers");
                            int oMaxNumbers = reader.GetOrdinal("MaxNumbers");
                            int oTicketSets = reader.GetOrdinal("TicketSets");
                            int oIsActive = reader.GetOrdinal("IsActive");

                            found = new ModelSubscriptionPlans
                            {
                                PlanCode = reader.IsDBNull(oPlanCode) ? null : reader.GetString(oPlanCode),
                                PlanName = reader.IsDBNull(oPlanName) ? null : reader.GetString(oPlanName),
                                Description = reader.IsDBNull(oDescription) ? null : reader.GetString(oDescription),
                                Price = reader.IsDBNull(oPrice) ? 0m : reader.GetDecimal(oPrice),
                                //MinNumbers = reader.IsDBNull(oMinNumbers) ? (int?)null : reader.GetInt32(oMinNumbers),
                                //MaxNumbers = reader.IsDBNull(oMaxNumbers) ? (int?)null : reader.GetInt32(oMaxNumbers),
                                //TicketSets = reader.IsDBNull(oTicketSets) ? (int?)null : reader.GetInt32(oTicketSets),
                                IsActive = !reader.IsDBNull(oIsActive) && reader.GetBoolean(oIsActive)
                            };
                        }

                        return Ok(new { ok = true, subscription = found });
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


        // --------------------------
        // Helpers / DTOs
        // --------------------------

        private static SubscriptionWithPlan MapSubscriptionWithPlan(SqlDataReader r)
        {
            // Guard for DBNull on nullable fields
            DateTime? dt(object o) => o == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(o);
            int? ni(object o) => o == DBNull.Value ? (int?)null : Convert.ToInt32(o);
            string ns(object o) => o == DBNull.Value ? null : Convert.ToString(o);

            return new SubscriptionWithPlan
            {
                // UserSubscriptions
                SubscriptionId = Convert.ToInt32(r["SubscriptionId"]),
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
                ExternalSubscriptionId = ns(r["ExternalSubscriptionId"]),
                CreatedAt = Convert.ToDateTime(r["CreatedAt"]),
                UpdatedAt = Convert.ToDateTime(r["UpdatedAt"]),

                // From SubscriptionPlans join
                PlanName = Convert.ToString(r["PlanName"]),
                Description = ns(r["Description"]),
                TicketSets = ni(r["TicketSets"])
            };
        }
    }

    // --------- DTOs ---------

    public sealed class CreateSubscriptionRequestProfile
    {
        public int UserId { get; set; }              // required
        public string UserAlias { get; set; } = "";      // exactly 8 chars
        public string PlanCode { get; set; } = "";      // required
        //public bool AutoRenew { get; set; } = true;    // optional, default true
        //public int Quantity { get; set; } = 1;       // optional, default 1
        //public string ExternalProvider { get; set; }     // optional
        //public string ExternalSubscriptionId { get; set; } // optional
    }

    public sealed class SubscriptionWithPlanProfile
    {
        // From UserSubscriptions
        public int SubscriptionId { get; set; }
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
        public string ExternalSubscriptionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // From SubscriptionPlans join
        public string PlanName { get; set; }
        public string Description { get; set; }
        public int? TicketSets { get; set; }
    }
}

