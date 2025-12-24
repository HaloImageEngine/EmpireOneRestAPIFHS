using EmpireOneRestAPIFHS.DataManager;
using EmpireOneRestAPIFHS.Security; // Encryption
using EmpireOneRestAPIFHS.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

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
    [RoutePrefix("api/RapidCMS/login")]
    /// <summary>
    /// Login & account endpoints for TechJump.
    /// </summary>
    public class CMSDemoLoginController : ApiController
    {
        private readonly DataAccess02 _data = new DataAccess02();

        private static string GetCs()
        {
            var cs = System.Configuration.ConfigurationManager
                .ConnectionStrings["CMSDemoProd"]?.ConnectionString;

            if (string.IsNullOrWhiteSpace(cs))
                throw new InvalidOperationException("Connection string 'CMSDemoProd' not found.");

            return cs;
        }

        // -------------------- CORS PREFLIGHT (OPTIONS) --------------------
        // These help when IIS/WebDAV/etc. interfere with OPTIONS routing.
        [HttpOptions, Route("")]
        public IHttpActionResult OptionsRoot() => Ok();

        [HttpOptions, Route("verify")]
        public IHttpActionResult OptionsVerify() => Ok();

        [HttpOptions, Route("verify-alias")]
        public IHttpActionResult OptionsVerifyAlias() => Ok();

        [HttpOptions, Route("create")]
        public IHttpActionResult OptionsCreate() => Ok();

        // ------------------------------------------------------------
        // 1) CreateLogin: creates login (Users) + profile (UsersInfo)
        // POST /api/RapidCMS/login/create
        // ------------------------------------------------------------
        /// <summary>Creates a new login and user profile. Password is hashed (PBKDF2).</summary>
        [HttpPost, Route("create")]
        public async Task<IHttpActionResult> CreateLogin([FromBody] CreateLoginRequest body)
        {
            if (body == null) return BadRequest("Body is required.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                // Hash password (v1$iterations$salt$hash)
                var passwordHash = Encryption.EncryptPassword(body.Password ?? string.Empty);

                using (var conn = new SqlConnection(GetCs()))
                using (var cmd = new SqlCommand("dbo.spInsert_UserInfo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Map proc params
                    cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value =
                        (object)body.FirstName.ToLower() ?? DBNull.Value;
                    cmd.Parameters.Add("@MiddleInitial", SqlDbType.NChar, 1).Value =
                        (object)body.MiddleInitial?.ToLower() ?? DBNull.Value;
                    cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value =
                        (object)body.LastName.ToLower() ?? DBNull.Value;

                    // Login email used for Users.Email and Users.UserName
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value =
                        (object)body.Email ?? DBNull.Value;

                    // 8-char alias (controller enforces length; DB has CHAR(8))
                    cmd.Parameters.Add("@UserAlias", SqlDbType.Char, 20).Value =
                        body.UserAlias ?? "USERAL01";

                    //cmd.Parameters.Add("@City", SqlDbType.NVarChar, 100).Value =
                    //    (object)body.City ?? DBNull.Value;
                    //cmd.Parameters.Add("@State", SqlDbType.NVarChar, 50).Value =
                    //    (object)body.State ?? DBNull.Value;
                    cmd.Parameters.Add("@Zip", SqlDbType.NVarChar, 15).Value =
                        (object)body.Zip ?? DBNull.Value;

                    //if (body.BirthMonth.HasValue)
                    //    cmd.Parameters.Add("@BirthMonth", SqlDbType.TinyInt).Value = body.BirthMonth.Value;
                    //else
                    //    cmd.Parameters.Add("@BirthMonth", SqlDbType.TinyInt).Value = DBNull.Value;

                    // Contact email for UsersInfo (can be same as login email)
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.NVarChar, 255).Value =
                        (object)body.Email ?? DBNull.Value;

                    cmd.Parameters.Add("@PasswordHash", SqlDbType.VarChar, 255).Value = passwordHash;

                    cmd.Parameters.Add("@PhoneNum", SqlDbType.NVarChar, 25).Value =
                        (object)body.PhoneNum ?? DBNull.Value;
                    cmd.Parameters.Add("@ReadPW", SqlDbType.NVarChar, 50).Value =
                        (object)body.Password ?? DBNull.Value;

                    var pUserId = cmd.Parameters.Add("@NewUserId", SqlDbType.Int);
                    pUserId.Direction = ParameterDirection.Output;

                    var pUserInfoId = cmd.Parameters.Add("@NewUserInfoId", SqlDbType.Int);
                    pUserInfoId.Direction = ParameterDirection.Output;

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    var newUserId = (pUserId.Value == DBNull.Value)
                        ? (int?)null
                        : Convert.ToInt32(pUserId.Value);

                    var newUserInfoId = (pUserInfoId.Value == DBNull.Value)
                        ? (int?)null
                        : Convert.ToInt32(pUserInfoId.Value);

                    EmailSendService esvc = new EmailSendService();

                    string returnemailmsg = esvc.SendEmail_SMTP_Register(body.Email.ToString(), pUserId.ToString(), body.UserAlias.ToString());

                    return Ok(new
                    {
                        ok = true,
                        userId = newUserId,
                        userInfoId = newUserInfoId
                    });
                }
            }
            catch (SqlException ex)
            {
                return Content(System.Net.HttpStatusCode.Conflict,
                    new { ok = false, error = $"SQL error {ex.Number}: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ------------------------------------------------------------
        // 2) VerifyLogin: checks password vs stored hash in dbo.Users
        // POST /api/RapidCMS/login/verify
        // ------------------------------------------------------------
        /// <summary>Verifies a user's login using email + password.</summary>
        [HttpPost, Route("verify")]
        public async Task<IHttpActionResult> VerifyLogin(
            [FromBody] VerifyLoginRequest body,
            CancellationToken ct)
        {
            if (body == null) return BadRequest("Body is required.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                int userId;
                string storedHash;

                using (var conn = new SqlConnection(GetCs()))
                {
                    await conn.OpenAsync(ct).ConfigureAwait(false);

                    // 1) Read user row
                    using (var cmd = new SqlCommand(@"
                        SELECT TOP (1) UserId, PasswordHash
                        FROM dbo.Users
                        WHERE Email = @Email;", conn))
                    {
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value =
                            body.Email ?? string.Empty;

                        using (var rdr = await cmd
                            .ExecuteReaderAsync(CommandBehavior.SingleRow, ct)
                            .ConfigureAwait(false))
                        {
                            if (!await rdr.ReadAsync(ct).ConfigureAwait(false))
                            {
                                return Content(System.Net.HttpStatusCode.Unauthorized,
                                    new { ok = false, error = "Invalid email or password." });
                            }

                            userId = rdr.GetInt32(0);
                            storedHash = rdr.IsDBNull(1) ? null : rdr.GetString(1);
                        }
                    }

                    // 2) Validate password
                    var password = body.Password ?? string.Empty;
                    if (string.IsNullOrWhiteSpace(storedHash) ||
                        !Encryption.PasswordMatch(password, storedHash))
                    {
                        return Content(System.Net.HttpStatusCode.Unauthorized,
                            new { ok = false, error = "Invalid email or password." });
                    }

                    // 3) Update last login
                    await UpdateLastLoginUtcAsync(conn, userId, ct).ConfigureAwait(false);

                    return Ok(new { ok = true, userId });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ------------------------------------------------------------
        // 2b) VerifyLoginAlias: checks password via UsersInfo.UserAlias
        // POST /api/RapidCMS/login/verify-alias
        // ------------------------------------------------------------
        /// <summary>Verifies a user's login using 8-char UserAlias + password.</summary>
        [HttpPost, Route("verify-alias")]
        public async Task<IHttpActionResult> VerifyLoginAlias(
            [FromBody] VerifyLoginAliasRequest body,
            CancellationToken ct)
        {
            if (body == null) return BadRequest("Body is required.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Normalize alias (DB column is CHAR(8); compare exact)
            var alias = (body.Alias ?? string.Empty).Trim().ToUpperInvariant();
            if (alias.Length < 6 || alias.Length > 20)
                return BadRequest("Alias must be between 6 and 20 characters.");

            try
            {
                int userId;
                string storedHash;

                using (var conn = new SqlConnection(GetCs()))
                {
                    await conn.OpenAsync(ct).ConfigureAwait(false);

                    // 1) Read user row by alias
                    using (var cmd = new SqlCommand(@"
                        SELECT TOP (1) u.UserId, u.PasswordHash
                        FROM dbo.Users u
                        INNER JOIN dbo.UsersInfo ui ON ui.UserId = u.UserId
                        WHERE ui.UserAlias = @Alias;", conn))
                    {
                        cmd.Parameters.Add("@Alias", SqlDbType.VarChar, 20).Value = alias;

                        using (var rdr = await cmd
                            .ExecuteReaderAsync(CommandBehavior.SingleRow, ct)
                            .ConfigureAwait(false))
                        {
                            if (!await rdr.ReadAsync(ct).ConfigureAwait(false))
                            {
                                // Prevent alias enumeration
                                return Content(System.Net.HttpStatusCode.Unauthorized,
                                    new { ok = false, error = "Invalid alias or password." });
                            }

                            userId = rdr.GetInt32(0);
                            storedHash = rdr.IsDBNull(1) ? null : rdr.GetString(1);
                        }
                    }

                    // 2) Password check
                    var password = body.Password ?? string.Empty;
                    if (string.IsNullOrWhiteSpace(storedHash) ||
                        !Encryption.PasswordMatch(password, storedHash))
                    {
                        return Content(System.Net.HttpStatusCode.Unauthorized,
                            new { ok = false, error = "Invalid alias or password." });
                    }

                    // 3) Update last login
                    await UpdateLastLoginUtcAsync(conn, userId, ct).ConfigureAwait(false);

                    return Ok(new { ok = true, userId });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private static async Task UpdateLastLoginUtcAsync(
            SqlConnection conn,
            int userId,
            CancellationToken ct)
        {
            using (var cmd = new SqlCommand(
                "UPDATE dbo.Users SET LastLoginUtc = SYSUTCDATETIME() WHERE UserId = @UserId;",
                conn))
            {
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
            }
        }

        // -------------------- Queries --------------------

        /// <summary>Get profile + cards by user alias.</summary>
        [HttpGet, Route("getprofilebyuseralias")]
        public IHttpActionResult Get_Profile_byUserAlias(
            [FromUri][Required] string useralias)
            => Ok(_data.Get_UserInfowithCards(useralias));

        // -------------------- DTOs --------------------

        public class CreateLoginRequest
        {
            [Required, StringLength(50)]
            public string FirstName { get; set; }

            [StringLength(1)]
            public string MiddleInitial { get; set; }

            [Required, StringLength(50)]
            public string LastName { get; set; }

            [Required, EmailAddress, StringLength(255)]
            public string Email { get; set; }        // Users.Email (+ UserName)

            [Required, StringLength(20, MinimumLength = 6,
                ErrorMessage = "UserAlias must be between 6 and 20 characters.")]
            public string UserAlias { get; set; }    // UsersInfo.UserAlias (CHAR(8))

            [Required, StringLength(200)]
            public string Password { get; set; }     // plaintext from client; hashed server-side

            //[StringLength(100)]
            //public string City { get; set; }

            //[StringLength(50)]
            //public string State { get; set; }

            [StringLength(15)]
            public string Zip { get; set; }

            //[Range(1, 12)]
            //public int? BirthMonth { get; set; }

            [StringLength(25)]
            public string PhoneNum { get; set; }     // e.g. 123-456-7890
        }

        public class VerifyLoginRequest
        {
            [Required, EmailAddress, StringLength(255)]
            public string Email { get; set; }

            [Required, StringLength(200)]
            public string Password { get; set; }
        }

        public class VerifyLoginAliasRequest
        {
            [Required, StringLength(20, MinimumLength = 6)]
            public string Alias { get; set; }

            [Required, StringLength(200)]
            public string Password { get; set; }
        }
    }
}
