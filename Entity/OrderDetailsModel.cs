using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class OrderDetailsModel
    {
        public string ProductDetails { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
