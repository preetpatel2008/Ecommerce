using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public  class LoginHistoryModel
    {
        public string UserName { get; set; }
        public string LoginDate { get; set; }
        public string LoginTime { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
