
using System.ComponentModel.DataAnnotations;

namespace Entity
{
    public class UserModel
    {
        [Key]
        public long UserID { get; set; }
        public long LoginMasterID { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        public string UserPhotoTitle { get; set; } = string.Empty;
        public string UserPhotoName { get; set; } = string.Empty;
        public byte[] UserPhotoImage { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string ProfileSummary { get; set; } = string.Empty;
        //public IFormFile UserProfilePhoto { get; set; }
        public Boolean IsShowPicture { get; set; } = false;
        public Boolean IsChangeImage { get; set; } = false;
        //public String UserPhotoBase64String { get; set; } = string.Empty;
        public Int16 SubscriptionPlanTypeID { get; set; }
        public string SubscriptionPlanTypeName { get; set; } = string.Empty;
        public int SubscriptionPlanTypeAmount { get; set; }
        public string UserJSON { get; set; }
        public string StripePublishableKey { get; set; } = string.Empty;
        public bool Active { get; set; }


    }
}
