using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using EmpireOneRestAPIFHS.Models;
using EmpireOneRestAPIFHS.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace EmpireOneRestAPIFHS.DataManager
{
    //This was updated by Chat
    public class DataAccess02
    {
        public string dbconn { get; private set; }
        public string ApiDbConnectionString { get; private set; }

        public DataAccess02()
        {

            dbconn = ConfigurationManager.ConnectionStrings["CMSDemoDB"]?.ConnectionString;

        }

        public List<ModelDropNames> Get_DropName(string userid)
        {
            SqlConnection conn = null;
            SqlDataReader rdr = null;

            string connDB = dbconn;
            List<ModelDropNames> cnames = new List<ModelDropNames>();
            DateTime sdt = DateTime.Now.AddDays(-30);
            DateTime edt = DateTime.Now;

            try
            {
                SqlConnection sqlCon = new SqlConnection(dbconn);
                sqlCon.Open();

                SqlCommand sql_cmnd = new SqlCommand("Get_iTechCards_Names", sqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql_cmnd.CommandType = CommandType.StoredProcedure;
                sql_cmnd.Parameters.AddWithValue("@UserId", SqlDbType.VarChar).Value = userid;

                rdr = sql_cmnd.ExecuteReader();
                int i = 0;
                while (rdr.Read())
                {
                    ModelDropNames cname = new ModelDropNames();
                    cname.ID = i;
                    cname.CardName = rdr[0].ToString();
                    cnames.Add(cname);
                    i++;

                }
                sqlCon.Close();

            }
            catch (SqlException ex)
            {
                string error = ex.Message;
            }
            finally
            {
                conn?.Close();
                rdr?.Close();
            }

            return cnames;
        }

     
      

        
       

        public int Update_Power_Prize(string userid, int wid, string fullprize, string cashprize)
        {

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(dbconn))
                {
                    sqlCon.Open();
                    SqlCommand sql_cmnd = new SqlCommand("Update_PrizePower", sqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    string datevalue = DateTime.Now.ToString();
                    DateTime dt = DateTime.Parse(datevalue);

                    sql_cmnd.Parameters.AddWithValue("@WID", SqlDbType.Int).Value = wid;

                    sql_cmnd.Parameters.AddWithValue("@FullPrize", SqlDbType.Int).Value = fullprize;
                    sql_cmnd.Parameters.AddWithValue("@CashPrize", SqlDbType.Int).Value = cashprize;
                    sql_cmnd.Parameters.AddWithValue("@WIN", SqlDbType.Int).Value = 0;

                    sql_cmnd.ExecuteNonQuery();

                    sqlCon.Close();
                }
            }
            catch (SqlException ex)
            {
                string error = ex.Message;
            }
            return 1;

        }
        public int Update_Mega_Prize(string userid, int wid, string fullprize, string cashprize)
        {

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(dbconn))
                {
                    sqlCon.Open();
                    SqlCommand sql_cmnd = new SqlCommand("Update_PrizeMega", sqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    string datevalue = DateTime.Now.ToString();
                    DateTime dt = DateTime.Parse(datevalue);

                    sql_cmnd.Parameters.AddWithValue("@WID", SqlDbType.Int).Value = wid;

                    sql_cmnd.Parameters.AddWithValue("@FullPrize", SqlDbType.Int).Value = fullprize;
                    sql_cmnd.Parameters.AddWithValue("@CashPrize", SqlDbType.Int).Value = cashprize;
                    sql_cmnd.Parameters.AddWithValue("@WIN", SqlDbType.Int).Value = 0;



                    sql_cmnd.ExecuteNonQuery();

                    sqlCon.Close();
                }
            }
            catch (SqlException ex)
            {
                string error = ex.Message;
            }
            return 1;

        }


        public int Update_Texas_Prize(string userid, int wid, string fullprize, string cashprize)
        {

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(dbconn))
                {
                    sqlCon.Open();
                    SqlCommand sql_cmnd = new SqlCommand("Update_PrizeTexas", sqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    string datevalue = DateTime.Now.ToString();
                    DateTime dt = DateTime.Parse(datevalue);

                    sql_cmnd.Parameters.AddWithValue("@WID", SqlDbType.Int).Value = wid;

                    sql_cmnd.Parameters.AddWithValue("@FullPrize", SqlDbType.Int).Value = fullprize;
                    sql_cmnd.Parameters.AddWithValue("@CashPrize", SqlDbType.Int).Value = cashprize;
                    sql_cmnd.Parameters.AddWithValue("@WIN", SqlDbType.Int).Value = 0;

                    sql_cmnd.ExecuteNonQuery();

                    sqlCon.Close();
                }
            }
            catch (SqlException ex)
            {
                string error = ex.Message;
            }
            return 1;

        }
        public int Update_TwoStep_Prize(string userid, int wid, string fullprize, string cashprize)
        {

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(dbconn))
                {
                    sqlCon.Open();
                    SqlCommand sql_cmnd = new SqlCommand("Update_PrizeTwoStep", sqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    string datevalue = DateTime.Now.ToString();
                    DateTime dt = DateTime.Parse(datevalue);

                    sql_cmnd.Parameters.AddWithValue("@WID", SqlDbType.Int).Value = wid;

                    sql_cmnd.Parameters.AddWithValue("@FullPrize", SqlDbType.Int).Value = fullprize;
                    sql_cmnd.Parameters.AddWithValue("@CashPrize", SqlDbType.Int).Value = cashprize;
                    sql_cmnd.Parameters.AddWithValue("@WIN", SqlDbType.Int).Value = 0;



                    sql_cmnd.ExecuteNonQuery();

                    sqlCon.Close();
                }
            }
            catch (SqlException ex)
            {
                string error = ex.Message;
            }
            return 1;

        }



        //public int Insert_iTech_Card(string userid, string cardname, string cardgame, int cardgroup, int cardslot, int active, int b1, int b2, int b3, int b4, int b5, int p1, int x1)
        //{

        //    try
        //    {
        //        using (SqlConnection sqlCon = new SqlConnection(dbconn))
        //        {
        //            sqlCon.Open();
        //            SqlCommand sql_cmnd = new SqlCommand("Insert_iTechCard", sqlCon)
        //            {
        //                CommandType = CommandType.StoredProcedure
        //            };

        //            string datevalue = DateTime.Now.ToString();
        //            DateTime dt = DateTime.Parse(datevalue);

        //            sql_cmnd.Parameters.AddWithValue("@userid", SqlDbType.NVarChar).Value = userid;
        //            sql_cmnd.Parameters.AddWithValue("@CDate", SqlDbType.NVarChar).Value = datevalue;
        //            sql_cmnd.Parameters.AddWithValue("@CardName", SqlDbType.NVarChar).Value = cardname;
        //            sql_cmnd.Parameters.AddWithValue("@CardGame", SqlDbType.NVarChar).Value = cardgame;
        //            sql_cmnd.Parameters.AddWithValue("@CardGroup", SqlDbType.Int).Value = cardgroup;
        //            sql_cmnd.Parameters.AddWithValue("@CardSlot", SqlDbType.Int).Value = cardslot;
        //            sql_cmnd.Parameters.AddWithValue("@Active", SqlDbType.Int).Value = active;
        //            sql_cmnd.Parameters.AddWithValue("@Num1", SqlDbType.Int).Value = b1;
        //            sql_cmnd.Parameters.AddWithValue("@Num2", SqlDbType.Int).Value = b2;
        //            sql_cmnd.Parameters.AddWithValue("@Num3", SqlDbType.Int).Value = b3;
        //            sql_cmnd.Parameters.AddWithValue("@Num4", SqlDbType.Int).Value = b4;
        //            sql_cmnd.Parameters.AddWithValue("@Num5", SqlDbType.Int).Value = b5;
        //            sql_cmnd.Parameters.AddWithValue("@PNum", SqlDbType.Int).Value = p1;
        //            sql_cmnd.Parameters.AddWithValue("@Multi", SqlDbType.NVarChar).Value = x1;


        //            sql_cmnd.ExecuteNonQuery();

        //            sqlCon.Close();
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        string error = ex.Message;
        //    }
        //    return 1;

        //}

        public int Insert_iTech_Card_Texas(string userid, string cardname, string cardgame, int cardgroup, int cardslot, int active, int b1, int b2, int b3, int b4, int b5, int b6, int x1)
        {

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(dbconn))
                {
                    sqlCon.Open();
                    SqlCommand sql_cmnd = new SqlCommand("Insert_iTechCard_Texas", sqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    string datevalue = DateTime.Now.ToString();
                    DateTime dt = DateTime.Parse(datevalue);

                    sql_cmnd.Parameters.AddWithValue("@userid", SqlDbType.NVarChar).Value = userid;
                    sql_cmnd.Parameters.AddWithValue("@CDate", SqlDbType.NVarChar).Value = datevalue;
                    sql_cmnd.Parameters.AddWithValue("@CardName", SqlDbType.NVarChar).Value = cardname;
                    sql_cmnd.Parameters.AddWithValue("@CardGame", SqlDbType.NVarChar).Value = cardgame;
                    sql_cmnd.Parameters.AddWithValue("@CardGroup", SqlDbType.Int).Value = cardgroup;
                    sql_cmnd.Parameters.AddWithValue("@CardSlot", SqlDbType.Int).Value = cardslot;
                    sql_cmnd.Parameters.AddWithValue("@Active", SqlDbType.Int).Value = active;
                    sql_cmnd.Parameters.AddWithValue("@Num1", SqlDbType.Int).Value = b1;
                    sql_cmnd.Parameters.AddWithValue("@Num2", SqlDbType.Int).Value = b2;
                    sql_cmnd.Parameters.AddWithValue("@Num3", SqlDbType.Int).Value = b3;
                    sql_cmnd.Parameters.AddWithValue("@Num4", SqlDbType.Int).Value = b4;
                    sql_cmnd.Parameters.AddWithValue("@Num5", SqlDbType.Int).Value = b5;
                    sql_cmnd.Parameters.AddWithValue("@Num6", SqlDbType.Int).Value = b6;

                    sql_cmnd.Parameters.AddWithValue("@Multi", SqlDbType.NVarChar).Value = x1;


                    sql_cmnd.ExecuteNonQuery();

                    sqlCon.Close();
                }
            }
            catch (SqlException ex)
            {
                string error = ex.Message;
            }
            return 1;

        }


        public int Clear_iTech_Card_Texas(string userid, string cardname, string cardgame, int cardgroup, int cardslot, int active, int b1, int b2, int b3, int b4, int b5, int x1)
        {

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(dbconn))
                {
                    sqlCon.Open();
                    SqlCommand sql_cmnd = new SqlCommand("Clear_iTechCard_Texas", sqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    string datevalue = DateTime.Now.ToString();
                    DateTime dt = DateTime.Parse(datevalue);

                    sql_cmnd.Parameters.AddWithValue("@userid", SqlDbType.NVarChar).Value = userid;
                    sql_cmnd.Parameters.AddWithValue("@CDate", SqlDbType.NVarChar).Value = datevalue;
                    sql_cmnd.Parameters.AddWithValue("@CardName", SqlDbType.NVarChar).Value = cardname;
                    sql_cmnd.Parameters.AddWithValue("@CardGame", SqlDbType.NVarChar).Value = cardgame;
                    sql_cmnd.Parameters.AddWithValue("@CardGroup", SqlDbType.Int).Value = cardgroup;
                    sql_cmnd.Parameters.AddWithValue("@CardSlot", SqlDbType.Int).Value = cardslot;
                    sql_cmnd.Parameters.AddWithValue("@Active", SqlDbType.Int).Value = active;
                    sql_cmnd.Parameters.AddWithValue("@Num1", SqlDbType.Int).Value = b1;
                    sql_cmnd.Parameters.AddWithValue("@Num2", SqlDbType.Int).Value = b2;
                    sql_cmnd.Parameters.AddWithValue("@Num3", SqlDbType.Int).Value = b3;
                    sql_cmnd.Parameters.AddWithValue("@Num4", SqlDbType.Int).Value = b4;
                    sql_cmnd.Parameters.AddWithValue("@Num5", SqlDbType.Int).Value = b5;

                    sql_cmnd.Parameters.AddWithValue("@Multi", SqlDbType.NVarChar).Value = x1;


                    sql_cmnd.ExecuteNonQuery();

                    sqlCon.Close();
                }
            }
            catch (SqlException ex)
            {
                string error = ex.Message;
            }
            return 1;

        }

        /// <summary>
        /// Inserts image information and returns the new ImageId.
        /// </summary>
        public async Task<int> InsertImageInfo(
   string imageType,
 string imageLocation,
    string userAlias,
    int userId,
            int? size = null,
            int? dimWidth = null,
            int? dimHeight = null,
       string imageOrientation = null)
        {
 int newImageId = 0;

   try
         {
        using (SqlConnection sqlCon = new SqlConnection(dbconn))
     {
       await sqlCon.OpenAsync().ConfigureAwait(false);

     using (SqlCommand sql_cmnd = new SqlCommand("spInsert_ImageInfo", sqlCon))
                    {
      sql_cmnd.CommandType = CommandType.StoredProcedure;

     // Input Parameters
    sql_cmnd.Parameters.AddWithValue("@ImageType",
     (object)imageType ?? DBNull.Value);

         sql_cmnd.Parameters.AddWithValue("@ImageLocation",
      (object)imageLocation ?? DBNull.Value);

             sql_cmnd.Parameters.AddWithValue("@UserAlias",
          (object)userAlias ?? DBNull.Value);

          sql_cmnd.Parameters.AddWithValue("@UserId", userId);

     sql_cmnd.Parameters.AddWithValue("@Size",
            size.HasValue ? (object)size.Value : DBNull.Value);

  sql_cmnd.Parameters.AddWithValue("@DimWidth",
      dimWidth.HasValue ? (object)dimWidth.Value : DBNull.Value);

    sql_cmnd.Parameters.AddWithValue("@DimHeight",
   dimHeight.HasValue ? (object)dimHeight.Value : DBNull.Value);

         sql_cmnd.Parameters.AddWithValue("@ImageOrientation",
                (object)imageOrientation ?? DBNull.Value);

     // OUTPUT Parameter
        SqlParameter outputIdParam = new SqlParameter
              {
          ParameterName = "@ImageID",
         SqlDbType = SqlDbType.Int,
           Direction = ParameterDirection.Output
      };
          sql_cmnd.Parameters.Add(outputIdParam);

          // Execute async
     await sql_cmnd.ExecuteNonQueryAsync().ConfigureAwait(false);

     // Read OUTPUT Value
   if (outputIdParam.Value != null && outputIdParam.Value != DBNull.Value)
              {
newImageId = Convert.ToInt32(outputIdParam.Value);
   }
       }
   }
    }
      catch (SqlException ex)
      {
        string logWriterErr = ex.Message;
      // TODO: log properly if needed
    throw; // Re-throw to let caller handle the error
 }

            return newImageId;
        }

        public int Insert_UserInfo(ModelUser body)
        {

            try
            {
                // Hash password (v1$iterations$salt$hash)
                var passwordHash = Encryption.EncryptPassword(body.Password ?? string.Empty);

                using (var conn = new SqlConnection(dbconn))
                using (var cmd = new SqlCommand("dbo.Insert_UserInfo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Map proc params
                    cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = (object)body.FirstName ?? DBNull.Value;
                    cmd.Parameters.Add("@MiddleInitial", SqlDbType.NChar, 1).Value = (object)body.MiddleInitial ?? DBNull.Value;
                    cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = (object)body.LastName ?? DBNull.Value;

                    // Login email used for Users.Email and Users.UserName
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value = (object)body.Email ?? DBNull.Value;

                    // 8-char alias (controller enforces length; DB has CHAR(8))
                    cmd.Parameters.Add("@UserAlias", SqlDbType.Char, 8).Value = body.UserAlias ?? "USERAL01";

                    cmd.Parameters.Add("@City", SqlDbType.NVarChar, 100).Value = (object)body.City ?? DBNull.Value;
                    cmd.Parameters.Add("@State", SqlDbType.NVarChar, 50).Value = (object)body.State ?? DBNull.Value;
                    cmd.Parameters.Add("@Zip", SqlDbType.NVarChar, 15).Value = (object)body.Zip ?? DBNull.Value;

                    if (body.BirthMonth.HasValue)
                        cmd.Parameters.Add("@BirthMonth", SqlDbType.TinyInt).Value = body.BirthMonth.Value;
                    else
                        cmd.Parameters.Add("@BirthMonth", SqlDbType.TinyInt).Value = DBNull.Value;

                    // Contact email for UsersInfo (can be same as login email)
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.NVarChar, 255).Value = (object)body.Email ?? DBNull.Value;

                    cmd.Parameters.Add("@PasswordHash", SqlDbType.VarChar, 255).Value = passwordHash;

                    cmd.Parameters.Add("@PhoneNum", SqlDbType.NVarChar, 25).Value = (object)body.PhoneNum ?? DBNull.Value;
                    cmd.Parameters.Add("@ReadPW", SqlDbType.NVarChar, 50).Value = (object)body.Password ?? DBNull.Value;

                    var pUserId = cmd.Parameters.Add("@NewUserId", SqlDbType.Int);
                    pUserId.Direction = ParameterDirection.Output;

                    var pUserInfoId = cmd.Parameters.Add("@NewUserInfoId", SqlDbType.Int);
                    pUserInfoId.Direction = ParameterDirection.Output;

                    conn.OpenAsync();
                    cmd.ExecuteNonQueryAsync();

                    var newUserId = (pUserId.Value == DBNull.Value) ? (int?)null : Convert.ToInt32(pUserId.Value);
                    var newUserInfoId = (pUserInfoId.Value == DBNull.Value) ? (int?)null : Convert.ToInt32(pUserInfoId.Value);

                    //return Ok(new
                    //{
                    //    ok = true,
                    //    userId = newUserId,
                    //    userInfoId = newUserInfoId
                    //});

                    return 0;
                }
            }
            catch (SqlException ex)
            {
                return 2;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }


        public ModelUserResponse Get_UserInfo(string useralias )
        {
            if (string.IsNullOrWhiteSpace(useralias)) return null;

            try
            {
                using (var conn = new SqlConnection(dbconn))
                using (var cmd = new SqlCommand("dbo.Get_UserInformation_byUserAlias", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Proc expects NVARCHAR(50)
                    var p = cmd.Parameters.Add("@UserAlias", SqlDbType.NVarChar, 50);
                    p.Value = useralias.ToUpperInvariant();

                    conn.Open();
                    using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (!rdr.Read())
                        {
                            return null;
                        }

                        // ----- User / profile ordinals -----
                        int oFirstName = rdr.GetOrdinal("FirstName");
                        int oMiddleInitial = rdr.GetOrdinal("MiddleInitial");
                        int oLastName = rdr.GetOrdinal("LastName");

                        int oEmail = rdr.GetOrdinal("Email");          // dbo.Users.Email
                        int oEmailAddress = rdr.GetOrdinal("EmailAddress");   // dbo.UsersInfo.EmailAddress

                        int oUserAlias = rdr.GetOrdinal("UserAlias");
                        int oUserID = rdr.GetOrdinal("UserID");

                        int oCity = rdr.GetOrdinal("City");
                        int oState = rdr.GetOrdinal("State");
                        int oZip = rdr.GetOrdinal("Zip");
                        int oBirthMonth = rdr.GetOrdinal("BirthMonth");     // TINYINT
                        int oPhoneNum = rdr.GetOrdinal("PhoneNum");
                        int oReadPW = rdr.GetOrdinal("ReadPW");

                        // ----- Subscription ordinals (columns exist but row values may be NULL) -----
                        int oSubId = rdr.GetOrdinal("SubscriptionId");
                        int oPlanCode = rdr.GetOrdinal("PlanCode");
                        int oStatus = rdr.GetOrdinal("Status");
                        int oQuantity = rdr.GetOrdinal("Quantity");
                        int oCurStartUtc = rdr.GetOrdinal("CurrentPeriodStartUtc");
                        int oCurEndUtc = rdr.GetOrdinal("CurrentPeriodEndUtc");
                        int oStartUtc = rdr.GetOrdinal("StartUtc");
                        int oEndedAtUtc = rdr.GetOrdinal("EndedAtUtc");

                        // ----- Build model -----
                        var model = new ModelUserResponse();

                        // User / profile
                        model.FirstName = rdr.IsDBNull(oFirstName) ? null : rdr.GetString(oFirstName);
                        model.MiddleInitial = rdr.IsDBNull(oMiddleInitial) ? null : rdr.GetString(oMiddleInitial).Trim();
                        model.LastName = rdr.IsDBNull(oLastName) ? null : rdr.GetString(oLastName);

                        // Prefer Users.Email; fall back to UsersInfo.EmailAddress
                        if (!rdr.IsDBNull(oEmail))
                            model.Email = rdr.GetString(oEmail);
                        else if (!rdr.IsDBNull(oEmailAddress))
                            model.Email = rdr.GetString(oEmailAddress);
                        else
                            model.Email = null; // NOTE: model has [Required]; validate upstream if necessary

                        model.UserAlias = rdr.IsDBNull(oUserAlias) ? null : rdr.GetString(oUserAlias);
                        model.UserID = rdr.GetInt32(oUserID);
                        model.Password = rdr.IsDBNull(oReadPW) ? null : rdr.GetString(oReadPW); // consider removing in prod

                        model.City = rdr.IsDBNull(oCity) ? null : rdr.GetString(oCity);
                        model.State = rdr.IsDBNull(oState) ? null : rdr.GetString(oState);
                        model.Zip = rdr.IsDBNull(oZip) ? null : rdr.GetString(oZip);
                        model.BirthMonth = rdr.IsDBNull(oBirthMonth) ? (int?)null : Convert.ToInt32(rdr.GetByte(oBirthMonth));
                        model.PhoneNum = rdr.IsDBNull(oPhoneNum) ? null : rdr.GetString(oPhoneNum);

                        // Not supplied by proc
                        model.Date = null;

                        // Subscription (may be NULL per column)
                        model.SubscriptionID = rdr.IsDBNull(oSubId) ? null : rdr.GetInt32(oSubId).ToString();
                        // Using PlanCode as SubscriptionType; swap to PlanName if you add it to the proc and prefer friendly text
                        model.SubscriptionType = rdr.IsDBNull(oPlanCode) ? null : rdr.GetString(oPlanCode);
                        model.Status = rdr.IsDBNull(oStatus) ? null : rdr.GetString(oStatus);
                        model.TicketSets = rdr.IsDBNull(oQuantity) ? 0 : rdr.GetInt32(oQuantity);

                        // Start = CurrentPeriodStartUtc ?? StartUtc (or MinValue when none)
                        DateTime startDate = DateTime.MinValue;
                        if (!rdr.IsDBNull(oCurStartUtc)) startDate = rdr.GetDateTime(oCurStartUtc);
                        else if (!rdr.IsDBNull(oStartUtc)) startDate = rdr.GetDateTime(oStartUtc);
                        model.SubscriptionStartDate = startDate;

                        // End = CurrentPeriodEndUtc ?? EndedAtUtc (or MinValue when none)
                        DateTime endDate = DateTime.MinValue;
                        if (!rdr.IsDBNull(oCurEndUtc)) endDate = rdr.GetDateTime(oCurEndUtc);
                        else if (!rdr.IsDBNull(oEndedAtUtc)) endDate = rdr.GetDateTime(oEndedAtUtc);
                        model.SubscriptionEndDate = endDate;

                        return model;
                    }
                }
            }
            catch (SqlException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public ModelUserResponseCards Get_UserInfowithCards(string useralias)
        {
            if (string.IsNullOrWhiteSpace(useralias)) return null;

            var model = new ModelUserResponseCards();

            try
            {
                using (var conn = new SqlConnection(dbconn))
                {
                    conn.Open(); // Open connection once at the beginning

                    // First query - Get user information
                    if (!PopulateUserInfo(conn, useralias, model))
                    {
                        return null; // User not found
                    }

                    // Second query - Get card counts
                  //  PopulateCardCounts(conn, useralias, model);

                    return model;
                }
            }
            catch (SqlException sqlex)
            {
                // Log the exception properly
                // Logger.LogError($"SQL Error in Get_UserInfowithCards: {sqlex.Message}", sqlex);
                return null;
            }
            catch (Exception ex)
            {
                // Log the exception properly
                // Logger.LogError($"Error in Get_UserInfowithCards: {ex.Message}", ex);
                return null;
            }
        }

        private bool PopulateUserInfo(SqlConnection conn, string useralias, ModelUserResponseCards model)
        {
            using (var cmd = new SqlCommand("dbo.Get_UserInformation_byUserAlias", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var p = cmd.Parameters.Add("@UserAlias", SqlDbType.NVarChar, 50);
                p.Value = useralias.ToUpperInvariant();

                using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (!rdr.Read())
                    {
                        return false; // User not found
                    }

                    // Get ordinals
                    var ordinals = GetUserInfoOrdinals(rdr);

                    // Populate user/profile data
                    PopulateUserProfileData(rdr, ordinals, model);

                    // Populate subscription data
                    PopulateSubscriptionData(rdr, ordinals, model);

                    return true; // Successfully populated
                }
            }
        }

        //private void PopulateCardCounts(SqlConnection conn, string useralias, ModelUserResponseCards model)
        //{
        //    model.CardCount = new List<ModelCardCount>();

        //    using (var cmd = new SqlCommand("dbo.GetCount_iTechCard_ByAlias_All", conn))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        var p1 = cmd.Parameters.Add("@UserID", SqlDbType.Int);
        //        p1.Value = model.UserID;
        //        var p2 = cmd.Parameters.Add("@UserAlias", SqlDbType.VarChar, 10);
        //        p2.Value = useralias.ToUpperInvariant();

        //        using (var rdr = cmd.ExecuteReader())
        //        {
        //            while (rdr.Read())
        //            {
        //                var cardCount = new ModelCardCount
        //                {
        //                    UserID = model.UserID,
        //                    UserAlias = useralias.ToUpperInvariant(),
        //                    CDate = DateTime.Now,
        //                    CardGame = rdr["TableGroup"].ToString(),
        //                    CardGameCount = Convert.ToInt32(rdr["Card_Count"])
        //                };
        //                model.CardCount.Add(cardCount);
        //            }
        //        }
        //    }
        //}

        private dynamic GetUserInfoOrdinals(SqlDataReader rdr)
        {
            return new
            {
                FirstName = rdr.GetOrdinal("FirstName"),
                MiddleInitial = rdr.GetOrdinal("MiddleInitial"),
                LastName = rdr.GetOrdinal("LastName"),
                Email = rdr.GetOrdinal("Email"),
                EmailAddress = rdr.GetOrdinal("EmailAddress"),
                UserAlias = rdr.GetOrdinal("UserAlias"),
                UserID = rdr.GetOrdinal("UserID"),
                City = rdr.GetOrdinal("City"),
                State = rdr.GetOrdinal("State"),
                Zip = rdr.GetOrdinal("Zip"),
                BirthMonth = rdr.GetOrdinal("BirthMonth"),
                PhoneNum = rdr.GetOrdinal("PhoneNum"),
                ReadPW = rdr.GetOrdinal("ReadPW"),
                SubscriptionId = rdr.GetOrdinal("SubscriptionId"),
                PlanCode = rdr.GetOrdinal("PlanCode"),
                Status = rdr.GetOrdinal("Status"),
                Quantity = rdr.GetOrdinal("Quantity"),
                CurrentPeriodStartUtc = rdr.GetOrdinal("CurrentPeriodStartUtc"),
                CurrentPeriodEndUtc = rdr.GetOrdinal("CurrentPeriodEndUtc"),
                StartUtc = rdr.GetOrdinal("StartUtc"),
                EndedAtUtc = rdr.GetOrdinal("EndedAtUtc")
            };
        }

        private void PopulateUserProfileData(SqlDataReader rdr, dynamic ordinals, ModelUserResponseCards model)
        {
            model.FirstName = rdr.IsDBNull(ordinals.FirstName) ? null : rdr.GetString(ordinals.FirstName);
            model.MiddleInitial = rdr.IsDBNull(ordinals.MiddleInitial) ? null : rdr.GetString(ordinals.MiddleInitial).Trim();
            model.LastName = rdr.IsDBNull(ordinals.LastName) ? null : rdr.GetString(ordinals.LastName);

            // Prefer Users.Email; fall back to UsersInfo.EmailAddress
            if (!rdr.IsDBNull(ordinals.Email))
                model.Email = rdr.GetString(ordinals.Email);
            else if (!rdr.IsDBNull(ordinals.EmailAddress))
                model.Email = rdr.GetString(ordinals.EmailAddress);
            else
                model.Email = null;

            model.UserAlias = rdr.IsDBNull(ordinals.UserAlias) ? null : rdr.GetString(ordinals.UserAlias);
            model.UserID = rdr.GetInt32(ordinals.UserID);
            model.Password = rdr.IsDBNull(ordinals.ReadPW) ? null : rdr.GetString(ordinals.ReadPW);

            model.City = rdr.IsDBNull(ordinals.City) ? null : rdr.GetString(ordinals.City);
            model.State = rdr.IsDBNull(ordinals.State) ? null : rdr.GetString(ordinals.State);
            model.Zip = rdr.IsDBNull(ordinals.Zip) ? null : rdr.GetString(ordinals.Zip);
            model.BirthMonth = rdr.IsDBNull(ordinals.BirthMonth) ? (int?)null : Convert.ToInt32(rdr.GetByte(ordinals.BirthMonth));
            model.PhoneNum = rdr.IsDBNull(ordinals.PhoneNum) ? null : rdr.GetString(ordinals.PhoneNum);

            model.Date = null; // Not supplied by proc
        }

        private void PopulateSubscriptionData(SqlDataReader rdr, dynamic ordinals, ModelUserResponseCards model)
        {
            model.SubscriptionID = rdr.IsDBNull(ordinals.SubscriptionId) ? null : rdr.GetInt32(ordinals.SubscriptionId).ToString();
            model.SubscriptionType = rdr.IsDBNull(ordinals.PlanCode) ? null : rdr.GetString(ordinals.PlanCode);
            model.Status = rdr.IsDBNull(ordinals.Status) ? null : rdr.GetString(ordinals.Status);
            model.TicketSets = rdr.IsDBNull(ordinals.Quantity) ? 0 : rdr.GetInt32(ordinals.Quantity);

            // Start = CurrentPeriodStartUtc ?? StartUtc (or MinValue when none)
            DateTime startDate = DateTime.MinValue;
            if (!rdr.IsDBNull(ordinals.CurrentPeriodStartUtc))
                startDate = rdr.GetDateTime(ordinals.CurrentPeriodStartUtc);
            else if (!rdr.IsDBNull(ordinals.StartUtc))
                startDate = rdr.GetDateTime(ordinals.StartUtc);
            model.SubscriptionStartDate = startDate;

            // End = CurrentPeriodEndUtc ?? EndedAtUtc (or MinValue when none)
            DateTime endDate = DateTime.MinValue;
            if (!rdr.IsDBNull(ordinals.CurrentPeriodEndUtc))
                endDate = rdr.GetDateTime(ordinals.CurrentPeriodEndUtc);
            else if (!rdr.IsDBNull(ordinals.EndedAtUtc))
                endDate = rdr.GetDateTime(ordinals.EndedAtUtc);
            model.SubscriptionEndDate = endDate;
        }


        //public int Insert_iTech_Card_TwoStep(string userid, string cardname, string cardgame, int cardgroup, int cardslot, int active, int b1, int b2, int b3, int b4, int b5, int x1)
        //{

        //    try
        //    {
        //        using (SqlConnection sqlCon = new SqlConnection(dbconn))
        //        {
        //            sqlCon.Open();
        //            SqlCommand sql_cmnd = new SqlCommand("Insert_iTechCard_TwoStep", sqlCon)
        //            {
        //                CommandType = CommandType.StoredProcedure
        //            };

        //            string datevalue = DateTime.Now.ToString();
        //            DateTime dt = DateTime.Parse(datevalue);

        //            sql_cmnd.Parameters.AddWithValue("@userid", SqlDbType.NVarChar).Value = userid;
        //            sql_cmnd.Parameters.AddWithValue("@CDate", SqlDbType.NVarChar).Value = datevalue;
        //            sql_cmnd.Parameters.AddWithValue("@CardName", SqlDbType.NVarChar).Value = cardname;
        //            sql_cmnd.Parameters.AddWithValue("@CardGame", SqlDbType.NVarChar).Value = cardgame;
        //            sql_cmnd.Parameters.AddWithValue("@CardGroup", SqlDbType.Int).Value = cardgroup;
        //            sql_cmnd.Parameters.AddWithValue("@CardSlot", SqlDbType.Int).Value = cardslot;
        //            sql_cmnd.Parameters.AddWithValue("@Active", SqlDbType.Int).Value = active;
        //            sql_cmnd.Parameters.AddWithValue("@Num1", SqlDbType.Int).Value = b1;
        //            sql_cmnd.Parameters.AddWithValue("@Num2", SqlDbType.Int).Value = b2;
        //            sql_cmnd.Parameters.AddWithValue("@Num3", SqlDbType.Int).Value = b3;
        //            sql_cmnd.Parameters.AddWithValue("@Num4", SqlDbType.Int).Value = b4;
        //            sql_cmnd.Parameters.AddWithValue("@Num5", SqlDbType.Int).Value = b5;

        //            sql_cmnd.Parameters.AddWithValue("@Multi", SqlDbType.NVarChar).Value = x1;


        //            sql_cmnd.ExecuteNonQuery();

        //            sqlCon.Close();
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        string error = ex.Message;
        //    }
        //    return 1;

        //}



    }
}