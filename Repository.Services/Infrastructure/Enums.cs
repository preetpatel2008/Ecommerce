using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Infrastructure
{
    public class Enums
    {
        public enum EntityType : long
        {
            SuperAdmin = 2,
            Admin = 1,
            User = 3
        }
    }
}
