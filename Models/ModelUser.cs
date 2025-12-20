using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace EmpireOneRestAPIITJ.Models
{
    public class ModelUser
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(1)]
        public string MiddleInitial { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress, StringLength(255)]
        public string Email { get; set; }        // Users.Email (+ UserName)

        [Required, StringLength(8, MinimumLength = 8, ErrorMessage = "UserAlias must be exactly 8 characters.")]
        public string UserAlias { get; set; }    // UsersInfo.UserAlias (CHAR(8))

        [Required, StringLength(200)]
        public string Password { get; set; }     // plaintext from client; hashed server-side

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        [Range(1, 12)]
        public int? BirthMonth { get; set; }
        [StringLength(25)]
        public string PhoneNum { get; set; }

        public DateTime? Date { get; set; }

        public string SubscriptionID {get; set;}
        public string SubscriptionType { get; set; }

        public string Status { get; set; }
        public int TicketSets { get; set; }
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
    }

    public class ModelUserResponse
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(1)]
        public string MiddleInitial { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress, StringLength(255)]
        public string Email { get; set; }        // Users.Email (+ UserName)

        [Required, StringLength(8, MinimumLength = 8, ErrorMessage = "UserAlias must be exactly 8 characters.")]
        public string UserAlias { get; set; }    // UsersInfo.UserAlias (CHAR(8))
        public int UserID { get; set; }    // UsersInfo.UserAlias (CHAR(8))
        [Required, StringLength(200)]
        public string Password { get; set; }     // plaintext from client; hashed server-side

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        [Range(1, 12)]
        public int? BirthMonth { get; set; }
        [StringLength(25)]
        public string PhoneNum { get; set; }

        public DateTime? Date { get; set; }

        public string SubscriptionID { get; set; }
        public string SubscriptionType { get; set; }


        public string Status { get; set; }
        public int TicketSets { get; set; }
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime SubscriptionEndDate { get; set; }

    }

    public class ModelUserResponseCards
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(1)]
        public string MiddleInitial { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress, StringLength(255)]
        public string Email { get; set; }        // Users.Email (+ UserName)

        [Required, StringLength(8, MinimumLength = 8, ErrorMessage = "UserAlias must be exactly 8 characters.")]
        public string UserAlias { get; set; }    // UsersInfo.UserAlias (CHAR(8))
        public int UserID { get; set; }    // UsersInfo.UserAlias (CHAR(8))
        [Required, StringLength(200)]
        public string Password { get; set; }     // plaintext from client; hashed server-side

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        [Range(1, 12)]
        public int? BirthMonth { get; set; }
        [StringLength(25)]
        public string PhoneNum { get; set; }

        public DateTime? Date { get; set; }

        public string SubscriptionID { get; set; }
        public string SubscriptionType { get; set; }


        public string Status { get; set; }
        public int TicketSets { get; set; }
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime SubscriptionEndDate { get; set; }

  

    }
}
