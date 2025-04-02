using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
namespace Entity
{
    [Table("LoginMaster")]
    public class RegisterModel
    {
        [Key]
        public long LoginMasterID { get; set; }
        public long EntityTypeID { get; set; }
        public long EntityID { get; set; }


        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name can't exceed 50 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [Range(1000000000, 9999999999, ErrorMessage = "Invalid mobile number")]
        // [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be 10 digits.")]
        public string? Mobile { get; set; }
        public long CreatedByID { get; set; }
        public string? VerificationCode { get; set; }
        public long ModifiedByID { get; set; }

        public string? Phone { get; set; }
        public string? LastName { get; set; }
    }
}
