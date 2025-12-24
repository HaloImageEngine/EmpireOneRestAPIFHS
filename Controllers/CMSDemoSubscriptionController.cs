using DocumentFormat.OpenXml.Spreadsheet;
using EmpireOneRestAPIFHS.DataManager;
using EmpireOneRestAPIFHS.Models;
using EmpireOneRestAPIFHS.Services;
using Stripe;
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
//using ImageLoadService = EmpireOneRestAPIFHS.Services.ImageLoadService;

namespace EmpireOneRestAPIFHS.Controllers
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
    [RoutePrefix("api/CMSDemoImageLoad")]
    public class CMSDemoImageLoadController : ApiController
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

        // POST /api/CMSDemoImageLoad
        // Body: CreateImageLoadRequest
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
                    cmd.Parameters.Add("@StripePaymentIntentId", SqlDbType.VarChar, 50).Value = req.StripePaymentIntentId;
                    cmd.Parameters.Add("@StripeChargeId", SqlDbType.VarChar, 50).Value = req.StripeChargeId;
                    cmd.Parameters.Add("@StripeCustomerId", SqlDbType.VarChar, 50).Value = req.StripeCustomerId;
                    cmd.Parameters.Add("@StripePaymentMethodId", SqlDbType.VarChar, 50).Value = req.StripePaymentMethodId;

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

    



        // --------------------------
        // Helpers / DTOs
        // --------------------------

        private static ImageLoadWithPlan MapImageLoadWithPlan(SqlDataReader r)
        {
            DateTime? dt(object o) => o == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(o);
            int? ni(object o) => o == DBNull.Value ? (int?)null : Convert.ToInt32(o);
            string ns(object o) => o == DBNull.Value ? null : Convert.ToString(o);

            return new ImageLoadWithPlan
            {
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
                PlanName = Convert.ToString(r["PlanName"]),
                Description = ns(r["Description"]),
                TicketSets = ni(r["TicketSets"])
            };
        }

        [HttpPost, Route("CallStripePayment")]
        public async Task<IHttpActionResult> CallStripePayment(CreateImageLoadRequest req)
        {
            var secretKey = ConfigurationManager.AppSettings["StripeSecretKey"];
            StripeConfiguration.ApiKey = secretKey;

            var paymentIntentService = new PaymentIntentService();
            var createOptions = new PaymentIntentCreateOptions
            {
                Amount = 999, // e.g., $9.99 in cents
                Currency = "usd",
                PaymentMethod = req.StripePaymentMethodId, // from frontend
                Confirm = true
            };

            PaymentIntent intent = await paymentIntentService.CreateAsync(createOptions);

            if (intent.Status == "succeeded")
            {
                // populate StripePaymentIntentId / StripeChargeId / StripeCustomerId
                req.StripePaymentIntentId = intent.Id;
                req.StripeChargeId = intent.LatestChargeId;
                req.StripeCustomerId = intent.CustomerId;

                // now persist ImageLoad using existing Create logic
                return await Create(req);
            }

            return Ok(new { ok = false, status = intent.Status, clientSecret = intent.ClientSecret });
        }
    }

    // --------- DTOs ---------

    public class CreateImageLoadRequest
    {
        public int UserId { get; set; }                 // required
        public string UserAlias { get; set; } = "";     // exactly 8 chars
        public string PlanCode { get; set; } = "";      // required
        public string StripePaymentIntentId { get; set; } = "";
        public string StripeChargeId { get; set; } = "";            // ch_xxx
        public string StripeCustomerId { get; set; } = "";         // cus_xxx(optional)

        // New: ID generated by Stripe on the frontend
        public string StripePaymentMethodId { get; set; } = "";

        
    }

    public class ImageLoadWithPlan
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
