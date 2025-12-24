using EmpireOneRestAPIFHS.DataManager;
using EmpireOneRestAPIFHS.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EmpireOneRestAPIFHS.Controllers
{
    // Allow calls from your frontend + (optionally) same host
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
    [RoutePrefix("api/CMSDemo")]
    public class CMSDemoController : ApiController
    {
        private readonly DataAccess _data1 = new DataAccess();

        // -------------------- Infra / DB check --------------------

        /// <summary>Ping the DB and return server info (sanity check).</summary>
        [HttpGet, Route("db-ping")]
        public async Task<IHttpActionResult> DbPing(CancellationToken ct)
        {
            var cs = System.Configuration.ConfigurationManager
                .ConnectionStrings["CMSDemoDB"]?.ConnectionString;

            if (string.IsNullOrWhiteSpace(cs))
                return BadRequest("Connection string 'CMSDemoDB' not found.");

            try
            {
                using (var conn = new SqlConnection(cs))
                using (var cmd = new SqlCommand("SELECT SYSUTCDATETIME()", conn))
                {
                    await conn.OpenAsync(ct);
                    var serverUtc = (DateTime)await cmd.ExecuteScalarAsync(ct);

                    return Ok(new
                    {
                        ok = true,
                        serverTimeUtc = serverUtc,
                        dataSource = conn.DataSource,
                        database = conn.Database
                    });
                }
            }
            catch (SqlException ex)
            {
                // Wrap with extra context but keep original as InnerException
                return InternalServerError(
                    new Exception($"SQL error {ex.Number}: {ex.Message}", ex));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // -------------------- Reads (GET) --------------------

        /// <summary>Get all questions.</summary>
        [HttpGet, Route("Tech/GetAllQuestion")]
        public async Task<IHttpActionResult> Get_AllQuestions(CancellationToken ct)
        {
            var data = await _data1.Get_AllQuestionsAsync(ct);
            return Ok(data);
        }

        /// <summary>Get all questions by category.</summary>
        /// <remarks>Route kept as-is for compatibility: Tech/GetGetQuestionsbyCat</remarks>
        [HttpGet, Route("Tech/GetGetQuestionsbyCat")]
        public async Task<IHttpActionResult> Get_QuestionbyCat(
            [FromUri][Required] string cat,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _data1.Get_QuestionsbyCatAsync(cat, ct);
            return Ok(data);
        }

        /// <summary>Get all questions by category.</summary>
        /// <remarks>Route kept as-is for compatibility: Tech/GetGetQuestionsbyCat</remarks>
        [HttpGet, Route("Tech/GetKeywordbyQID")]
        public async Task<IHttpActionResult> Get_KeywordbyQID(
            [FromUri][Required] string qid,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _data1.Get_KeyWordByQIDAsync(qid, ct);
            return Ok(data);
        }

        /// <summary>Get dropdown categories by category key.</summary>
        [HttpGet, Route("Tech/GetTestResultsbyUserId")]
        public IHttpActionResult Get_TestResultsbyUserId(
            [FromUri][Required] string userid,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = _data1.Get_TestResultbyUserIdList(userid);
            return Ok(data);
        }

        /// <summary>Get dropdown categories by category key.</summary>
        [HttpGet, Route("Tech/GetTestResultsbyTestId")]
        public IHttpActionResult Get_TestResultsbyTestId(
            [FromUri][Required] string testid,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = _data1.Get_TestResultbyTestId(testid);
            return Ok(data);
        }

        /// <summary>Get dropdown categories by category key.</summary>
        [HttpGet, Route("Tech/GetDropDownCat")]
        public async Task<IHttpActionResult> Get_DropDownCat(
            [FromUri][Required] string cat,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _data1.Get_DropDownCatAsync(cat, ct);
            return Ok(data);
        }

        /// <summary>Get dropdown categories by category key.</summary>
        [HttpGet, Route("Tech/GetSearchKeyword")]
        public async Task<IHttpActionResult> Get_SearchbyKeyword(
            [FromUri][Required] string keyword,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _data1.Get_SearchByKeyword_Async(keyword, ct);
            return Ok(data);
        }

        // Optional: if you later want a POST endpoint to insert questions,
        // you can uncomment and adapt this:
        [HttpPost, Route("Tech/InsertAnswer")]
        public async Task<IHttpActionResult> InsertAnswer(
            [FromBody][Required] InsertAnswerKeywordDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Fix: Call InsertAnswerAsync instead of InsertAnswer (which is sync and returns ModelGradeReturn)
            var result = await _data1.InsertAnswerAsync(dto.QuestionID.ToString(), dto.Answer);

            return Ok(result);
        }


        // Optional: if you later want a POST endpoint to insert questions,
        // you can uncomment and adapt this:
        [HttpPost, Route("Tech/InsertAnswerScore")]
        public async Task<IHttpActionResult> InsertAnswerScore(
            [FromBody][Required] InsertAnswerKeywordDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Fix: Call InsertAnswerAsync instead of InsertAnswer (which is sync and returns ModelGradeReturn)
            var result = await _data1.InsertAnswerScoreAsync(dto.QuestionID.ToString(), dto.Answer, dto.UserID, dto.Category);

            return Ok(result);
        }

        // Optional: if you later want a POST endpoint to insert questions,
        // you can uncomment and adapt this:
        [HttpPost, Route("Tech/InsertQuestion")]
        public async Task<IHttpActionResult> InsertQuestion(
            [FromBody][Required] InsertQuestionDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rows = await _data1.InsertQuestion(dto.Category, dto.Question, dto.Answer);

            return Ok(new { rowsAffected = rows });
        }

        // -------------------- Image Upload --------------------

        /// <summary>Upload a profile image for a user.</summary>
        /// <remarks>
        /// POST /api/CMSDemo/upload-image
        /// Form data: userId (int or string), image (file)
        /// </remarks>
        [HttpPost, Route("upload-image")]
        public async Task<IHttpActionResult> UploadImage()
        {
            // Check if the request contains multipart/form-data
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Content(HttpStatusCode.UnsupportedMediaType,
                    new { ok = false, error = "Request must be multipart/form-data." });
            }

            try
            {
                // Get upload path from config
                var uploadPathConfig = System.Configuration.ConfigurationManager.AppSettings["ImageUploadPath"];
                if (string.IsNullOrWhiteSpace(uploadPathConfig))
                {
                    return InternalServerError(new Exception("ImageUploadPath not configured in Web.config"));
                }

                // Resolve ~ to physical path
                var uploadPath = uploadPathConfig.StartsWith("~")
                    ? HttpContext.Current.Server.MapPath(uploadPathConfig)
                    : uploadPathConfig;

                // Read multipart data
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                // Extract userId from form data
                string userId = null;
                var userIdContent = provider.Contents.FirstOrDefault(c =>
                    c.Headers.ContentDisposition?.Name?.Trim('"') == "userId");

                if (userIdContent != null)
                {
                    userId = await userIdContent.ReadAsStringAsync();
                }

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest("userId is required.");
                }

                // Extract image file
                var imageContent = provider.Contents.FirstOrDefault(c =>
                    c.Headers.ContentDisposition?.Name?.Trim('"') == "image");

                if (imageContent == null)
                {
                    return BadRequest("No image file uploaded.");
                }

                var fileName = imageContent.Headers.ContentDisposition.FileName?.Trim('"');
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return BadRequest("Image file name is missing.");
                }

                // Validate content type
                var contentType = imageContent.Headers.ContentType?.MediaType;
                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
                if (string.IsNullOrWhiteSpace(contentType) || !allowedTypes.Contains(contentType.ToLowerInvariant()))
                {
                    return BadRequest($"Invalid image type. Allowed: {string.Join(", ", allowedTypes)}");
                }

                // Get file extension
                var fileExtension = Path.GetExtension(fileName);
                if (string.IsNullOrWhiteSpace(fileExtension))
                {
                    // Fallback based on content type
                    if (contentType.ToLowerInvariant() == "image/jpeg" || contentType.ToLowerInvariant() == "image/jpg")
                        fileExtension = ".jpg";
                    else if (contentType.ToLowerInvariant() == "image/png")
                        fileExtension = ".png";
                    else if (contentType.ToLowerInvariant() == "image/gif")
                        fileExtension = ".gif";
                    else
                        fileExtension = ".jpg";
                }

                // Create user-specific folder
                var userFolderPath = Path.Combine(uploadPath, userId);
                if (!Directory.Exists(userFolderPath))
                {
                    Directory.CreateDirectory(userFolderPath);
                }

                // Generate unique file name
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(userFolderPath, uniqueFileName);

                // Save file
                var imageBytes = await imageContent.ReadAsByteArrayAsync();

                // Optional: validate file size (e.g., max 5MB)
                const int maxSizeBytes = 5 * 1024 * 1024; // 5MB
                if (imageBytes.Length > maxSizeBytes)
                {
                    return BadRequest($"Image size exceeds maximum allowed size of {maxSizeBytes / (1024 * 1024)}MB.");
                }

                await Task.Run(() => File.WriteAllBytes(filePath, imageBytes));

                // Return success with file info
                return Ok(new
                {
                    ok = true,
                    message = "Image uploaded successfully.",
                    userId = userId,
                    fileName = uniqueFileName,
                    filePath = Path.Combine(userId, uniqueFileName).Replace("\\", "/"),
                    fileSize = imageBytes.Length,
                    contentType = contentType
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Error uploading image: {ex.Message}", ex));
            }
        }

        // -------------------- DTOs --------------------

        public class InsertQuestionDto
        {
            [Required]
            public string Category { get; set; }

            [Required, MaxLength(100)]
            public string Question { get; set; }

            [Required, MaxLength(2000)]
            public string Answer { get; set; }
        }

        public class InsertAnswerDto
        {
            [Required]
            public string Category { get; set; }

            public int QuestionID { get; set; }

            [Required, MaxLength(2000)]
            public string Answer { get; set; }
        }

        public class InsertAnswerKeywordDto
        {
            [Required]
            public int QuestionID { get; set; }

            [Required]
            public string UserID { get; set; }

            [Required]
            public string Category { get; set; }

            [Required, MaxLength(2000)]
            public string Answer { get; set; }
        }
    }
}
