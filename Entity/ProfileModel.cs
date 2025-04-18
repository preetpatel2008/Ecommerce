using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ProfileModel
    {
        public int LoginMasterId { get; set; }
        public string? FirstName { get; set; }
        public string?Email { get; set; }
        public string? MobileNumber { get; set; }
        public string? Address { get; set; }
    }
}
