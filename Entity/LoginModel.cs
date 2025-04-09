using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Entity
{
    public class LoginModel
    {
        [Key]
        public long LoginMasterID { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        [StringLength(16, ErrorMessage = "Password must be between 6 and 16 characters", MinimumLength = 6)]
        public string Password { get; set; }
        //public bool? IsVerified { get; set; }
        public long EntityTypeID { get; set; }
        public long EntityID { get; set; }
        public string VerificationCode { get; set; } = string.Empty;

        public string token { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public bool IsActive { get; set; }
        public string? Phone { get; set; }
        public string? EntityTypeName { get; set; }
       // public string DepartmentName { get; set; }
        public long CreatedByID { get; set; }
        public long ModifiedByID { get; set; }

        public bool IsDeleted { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

    }

    public class AuthModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public long EntityTypeID { get; set; }
        public bool RememberMe { get; set; } = true;
    }
}
