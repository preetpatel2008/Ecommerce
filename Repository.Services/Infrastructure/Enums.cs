﻿using System;
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
            SuperAdmin = 1,
            Admin = 2,
            User = 3
        }
    }
}
