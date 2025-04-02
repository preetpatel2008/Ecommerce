using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
        
namespace Entity
{
    public class UserAuthentication
    {
        public long LoginMasterID { get; set; }
        public long FirmID { get; set; }
        public long EntityTypeID { get; set; }
        public long EntityID { get; set; }
        public string EntityTypeName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Area { get; set; }
        public long BranchID { get; set; }
    }
}
