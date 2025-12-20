using EmpireOneRestAPIITJ.Models;
using global::EmpireOneRestAPIITJ.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace EmpireOneRestAPIITJ.Services
{


        public class UserSubscriptionDto
        {
            [Key]
            public int SubscriptionID { get; set; }    
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

    public class SubscriptionPlan
    {
        [Key]
        public string PlanCode { get; set; }
        public string PlanName { get; set; }
        public string BillingPeriod { get; set; }
        // ... other properties
    }

    public class SubscriptionService
    {
        public List<UserSubscriptionDto> Get_UserSubscriptions1(int userid, string plancode)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var query = (from us in db.UserSubscriptions
                              //   join u in db.Users on us.UserId equals u.UserId
                                 where us.PlanCode == plancode
                                  //   && us.UserId == userid
                                 select new UserSubscriptionDto
                                 {
                                     SubscriptionID = us.SubscriptionId,
                                     UserId = us.UserId,
                                     UserAlias = us.UserAlias,
                                     PlanCode = us.PlanCode,
                                     StartDate = us.StartUtc,
                                     EndDate = us.CurrentPeriodEndUtc
                                   //  DisplayName = u.DisplayName
                                 })
                                 .OrderBy(c => c.UserId)
                                 .Take(2)
                                 .ToList();
                    return query;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    

        public List<UserSubscriptionDto> Get_UserSubscriptions(int userid, string plancode)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    //var query = (from us in db.UserSubscriptions
                    //                 //   join u in db.Users on us.UserId equals u.UserId
                    //             where us.PlanCode == plancode
                    //             //   && us.UserId == userid
                    //             select new UserSubscriptionDto
                    //             {
                    //                 SubscriptionID = us.SubscriptionId,
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

                    var query = (from us in db.UserSubscriptions
                                 where us.PlanCode == "BASIC12"

                                 select new UserSubscriptionDto
                                 {
                                     SubscriptionID = us.SubscriptionId,
                                     PlanCode = us.PlanCode,

                                 }
                                 ).Take(5)
                         //        .OrderBy(PlanCode)
                                 .ToList();
                    return query;
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