using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class OrdersModel
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [Range(1000000000, 9999999999, ErrorMessage = "Invalid mobile number")]
        public string? MobileNumber { get; set; }
        [Required(ErrorMessage = "Payment method is required.")]
        public string? PaymentMethod { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }
        public int TotalProducts { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PlacedOn { get; set; }
        public string? PaymentStatus { get; set; }
    }
}
