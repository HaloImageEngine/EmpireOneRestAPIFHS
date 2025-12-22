using EmpireOneRestAPIFHS.Models;
using global::EmpireOneRestAPIFHS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace EmpireOneRestAPIFHS.Services
{


        public class UserImageDto
        {
            [Key]
            public int ImageID { get; set; }    
            public int UserId { get; set; }
            public string UserAlias { get; set; }
            public string PlanCode { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string PlanName { get; set; }
            public string BillingPeriod { get; set; }
            public string DisplayName { get; set; }
        }

    public class UsersInfo
    {
        [Key]
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        // ... other properties
    }

    public class ImagePlan
    {
        [Key]
        public string PlanCode { get; set; }
        public string PlanName { get; set; }
        public string BillingPeriod { get; set; }
        // ... other properties
    }

    public class ImageService
    {
        //public List<UserImageDto> Get_UserImages1(int userid, string plancode)
        //{
        //    try
        //    {
        //        using (var db = new ApplicationDbContext())
        //        {
        //            var query = (from us in db.UserImages
        //                      //   join u in db.Users on us.UserId equals u.UserId
        //                         where us.PlanCode == plancode
        //                          //   && us.UserId == userid
        //                         select new UserImageDto
        //                         {
        //                             ImageID = us.ImageId,
        //                             UserId = us.UserId,
        //                             UserAlias = us.UserAlias,
        //                             PlanCode = us.PlanCode,
        //                             StartDate = us.StartUtc,
        //                             EndDate = us.CurrentPeriodEndUtc
        //                           //  DisplayName = u.DisplayName
        //                         })
        //                         .OrderBy(c => c.UserId)
        //                         .Take(2)
        //                         .ToList();
        //            return query;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;
        //        return null;
        //    }
        //}
    

        public List<UserImageDto> Get_UserImages(int userid, string plancode)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    //var query = (from us in db.UserImages
                    //                 //   join u in db.Users on us.UserId equals u.UserId
                    //             where us.PlanCode == plancode
                    //             //   && us.UserId == userid
                    //             select new UserImageDto
                    //             {
                    //                 ImageID = us.ImageId,
                    //                 UserId = us.UserId,
                    //                 UserAlias = us.UserAlias,
                    //                 PlanCode = us.PlanCode,
                    //                 StartDate = us.StartUtc,
                    //                 EndDate = us.CurrentPeriodEndUtc
                    //                 //  DisplayName = u.DisplayName
                    //             })
                    //             .OrderBy(c => c.UserId)
                    //             .Take(2)
                    //             .ToList();
                    //return query;

                    //var query = (from us in db.FH_Images

                    //             where us.PlanCode == "BASIC12"

                    //             select new UserImageDto
                    //             {
                    //                 ImageID = us.ImageId,
                    //                 PlanCode = us.PlanCode,

                    //             }
                    //             ).Take(5)
                    //     //        .OrderBy(PlanCode)
                    //             .ToList();
                   // return query;
                    return null;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }

}