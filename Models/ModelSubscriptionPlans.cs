using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpireOneRestAPIFHS.Models
{
    [Table("SubscriptionPlans")]
    public class ModelSubscriptionPlansFull
    {
        // PK
        [Key]
        [Required]
        [StringLength(50)]
        public string PlanCode { get; set; }

        [Required]
        [StringLength(100)]
        public string PlanName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        [Required]
        [StringLength(20)]
        public string BillingPeriod { get; set; }

        public int? TrialDays { get; set; }

        public int? MaxUsers { get; set; }

        public int? MinNumbers { get; set; }

        public int? MaxNumbers { get; set; }

        public int? TicketSets { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // Note: datetime2(0) precision is enforced at the DB level; 
        // [Column(TypeName = "datetime2")] is included for clarity.
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }
    }

    public class ModelSubscriptionPlans
    {
        // PK
        [Key]
        [Required]
        [StringLength(50)]
        public string PlanCode { get; set; }

        [Required]
        [StringLength(100)]
        public string PlanName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int SubscriptionId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

       
        public DateTime? PlanStartDate { get; set; }

        public DateTime? PlanEndDate { get; set; }

        public string BillingPeriod { get; set; }

        [Required]
        public bool IsActive { get; set; }

        
    }

    public class ModelSubscriptionPlansShort
    {
        // PK
        [Key]
        [Required]
        [StringLength(50)]
        public string PlanCode { get; set; }

        [Required]
        [StringLength(100)]
        public string PlanName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int? TicketSets { get; set; }

        [Required]
        public bool IsActive { get; set; }


    }

    public class UserSubscriptionDto
    {
        [Key]
        public int UserId { get; set; }
        public string UserAlias { get; set; }
        public string PlanCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PlanName { get; set; }
        public string BillingPeriod { get; set; }
        public string DisplayName { get; set; }
    }
}
