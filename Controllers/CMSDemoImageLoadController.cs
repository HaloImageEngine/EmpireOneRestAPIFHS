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
using System.Linq;
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
     // Process and save image if provided
   string savedImagePath = null;
  int? imageSize = null;
            if (!string.IsNullOrEmpty(req.ImageBase64))
     {
   var imageData = await SaveBase64Image(req);
       savedImagePath = imageData.Path;
      imageSize = imageData.Size;
     }

       // Insert image info into database using DataAccess02
           int newImageId = await _data.InsertImageInfo(
  imageType: req.ImageMimeType ?? "image/jpeg",
  imageLocation: savedImagePath,
 userAlias: req.UserAlias,
  userId: req.UserId,
  size: imageSize,
        dimWidth: null, // Will be populated if you extract from image
     dimHeight: null, // Will be populated if you extract from image
    imageOrientation: null); // Will be populated if you extract from image

      // Build response with image info
       var imageLoadInfo = new ImageLoadInfo
     {
           ImageId = newImageId,
  ImageType = req.ImageMimeType ?? "image/jpeg",
         ImageLocation = savedImagePath,
      UserAlias = req.UserAlias,
  UserId = req.UserId,
    Size = imageSize,
      DimWidth = null,
        DimHeight = null,
     ImageOrientation = null
    };

    var location = new Uri(Request.RequestUri, imageLoadInfo.ImageId.ToString());
  return Created(location, new { ok = true, Image = imageLoadInfo, ImagePath = savedImagePath });
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
     /// Saves a base64-encoded image from mobile device to server storage.
  /// Returns image path and size.
      /// </summary>
 private async Task<(string Path, int Size)> SaveBase64Image(CreateImageLoadRequest req)
        {
 try
       {
      // Parse base64 string
       string base64Data = req.ImageBase64;
string mimeType = req.ImageMimeType;

     // Check if it's a data URI (e.g., "data:image/jpeg;base64,/9j/4AAQ...")
       if (base64Data.StartsWith("data:"))
         {
 var parts = base64Data.Split(',');
   if (parts.Length == 2)
      {
 // Extract MIME type from data URI
         var headerParts = parts[0].Split(new[] { ':', ';' }, StringSplitOptions.RemoveEmptyEntries);
  if (headerParts.Length >= 2)
     {
    mimeType = headerParts[1]; // e.g., "image/jpeg"
        }
base64Data = parts[1]; // Get actual base64 data
    }
     }

     // Convert base64 to bytes
    byte[] imageBytes = Convert.FromBase64String(base64Data);

   // Determine file extension
         string fileExtension = GetFileExtensionFromMimeType(mimeType, req.ImageFileName);

   // Get upload path from config or use default
    var uploadPathConfig = System.Configuration.ConfigurationManager.AppSettings["ImageUploadPath"] ?? "~/App_Data/UploadedImages";
    var uploadPath = uploadPathConfig.StartsWith("~")
 ? System.Web.HttpContext.Current.Server.MapPath(uploadPathConfig)
    : uploadPathConfig;

    // Create user-specific folder
    var userFolderPath = System.IO.Path.Combine(uploadPath, req.UserId.ToString());
      if (!System.IO.Directory.Exists(userFolderPath))
        {
     System.IO.Directory.CreateDirectory(userFolderPath);
  }

     // Generate filename based on imageFileName or use GUID
        string uniqueFileName;
        if (!string.IsNullOrWhiteSpace(req.ImageFileName))
{
            // Sanitize the filename to remove invalid characters
            string sanitizedName = SanitizeFileName(System.IO.Path.GetFileNameWithoutExtension(req.ImageFileName));
        
     // Add timestamp to prevent collisions while keeping original name recognizable
     string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
     uniqueFileName = $"{sanitizedName}_{timestamp}{fileExtension}";
        }
        else
        {
  // Fallback to GUID if no filename provided
      uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        }

      var filePath = System.IO.Path.Combine(userFolderPath, uniqueFileName);

// Validate file size (max 10MB for mobile uploads)
   const int maxSizeBytes = 10 * 1024 * 1024; // 10MB
 if (imageBytes.Length > maxSizeBytes)
     {
   throw new Exception($"Image size ({imageBytes.Length / (1024 * 1024)}MB) exceeds maximum allowed size of {maxSizeBytes / (1024 * 1024)}MB.");
     }

       // Save file asynchronously
await Task.Run(() => System.IO.File.WriteAllBytes(filePath, imageBytes));

    // Return relative path and size for database storage
 return ($"{req.UserId}/{uniqueFileName}", imageBytes.Length);
    }
  catch (FormatException)
            {
  throw new Exception("Invalid base64 image data provided.");
   }
        }

      /// <summary>
        /// Sanitizes a filename by removing invalid characters and limiting length.
   /// </summary>
        private string SanitizeFileName(string fileName)
        {
       if (string.IsNullOrWhiteSpace(fileName))
    return "image";

            // Remove invalid file path characters
   var invalidChars = System.IO.Path.GetInvalidFileNameChars();
            var sanitized = new string(fileName
    .Where(ch => !invalidChars.Contains(ch))
   .ToArray());

    // Replace spaces with underscores
            sanitized = sanitized.Replace(" ", "_");

            // Limit length to 50 characters
            if (sanitized.Length > 50)
    sanitized = sanitized.Substring(0, 50);

// If sanitization removed everything, use default
     if (string.IsNullOrWhiteSpace(sanitized))
                sanitized = "image";

            return sanitized;
        }

        /// <summary>
        /// Gets file extension from MIME type or filename.
        /// </summary>
        private string GetFileExtensionFromMimeType(string mimeType, string fileName)
        {
            // Try MIME type first
            if (!string.IsNullOrEmpty(mimeType))
            {
                switch (mimeType.ToLowerInvariant())
                {
                    case "image/jpeg":
                    case "image/jpg":
                        return ".jpg";
                    case "image/png":
                        return ".png";
                    case "image/gif":
                        return ".gif";
                    case "image/webp":
                        return ".webp";
                    case "image/heic":
                        return ".heic";
                }
            }

            // Fall back to filename extension
            if (!string.IsNullOrEmpty(fileName))
            {
                var ext = System.IO.Path.GetExtension(fileName);
                if (!string.IsNullOrEmpty(ext))
                {
                    return ext.ToLowerInvariant();
                }
            }

            // Default to .jpg
            return ".jpg";
        }

        //----------------------------------------------------------------------------------------
        // Helper Methods
        //----------------------------------------------------------------------------------------

        private static ImageLoadInfo MapImageLoadInfo(SqlDataReader r)
        {
    int? ni(object o) => o == DBNull.Value ? (int?)null : Convert.ToInt32(o);
            string ns(object o) => o == DBNull.Value ? null : Convert.ToString(o);

            return new ImageLoadInfo
{
         ImageId = Convert.ToInt32(r["ImageId"]),
   ImageType = ns(r["ImageType"]),
       ImageLocation = ns(r["ImageLocation"]),
     UserAlias = Convert.ToString(r["UserAlias"]),
    UserId = Convert.ToInt32(r["UserId"]),
       Size = ni(r["Size"]),
     DimWidth = ni(r["DimWidth"]),
           DimHeight = ni(r["DimHeight"]),
          ImageOrientation = ns(r["ImageOrientation"])
            };
        }

   // --------------------------
        // DTOs (Data Transfer Objects)
        // --------------------------

        public class CreateImageLoadRequest
        {
   public int UserId { get; set; }      // required
    public string UserAlias { get; set; } = "";  // exactly 8 chars
         
      /// <summary>
 /// Base64-encoded image string from mobile device.
   /// Example: "data:image/jpeg;base64,/9j/4AAQSkZJRg..." or just the base64 data without prefix
   /// </summary>
 public string ImageBase64 { get; set; }

            /// <summary>
   /// Original filename from mobile device (e.g., "photo.jpg")
       /// Used to determine file extension if not in base64 data URI
       /// </summary>
            public string ImageFileName { get; set; }

        /// <summary>
  /// Image MIME type (e.g., "image/jpeg", "image/png")
        /// Optional - can be inferred from ImageBase64 data URI or ImageFileName
    /// </summary>
  public string ImageMimeType { get; set; }
        }

        public class ImageLoadInfo
     {
            public int ImageId { get; set; }
        public string ImageType { get; set; }
       public string ImageLocation { get; set; }
         public string UserAlias { get; set; }
            public int UserId { get; set; }
 public int? Size { get; set; }
public int? DimWidth { get; set; }
            public int? DimHeight { get; set; }
          public string ImageOrientation { get; set; }
        }
 }
}
