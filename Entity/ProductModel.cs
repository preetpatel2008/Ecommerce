using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Forms;

namespace Entity
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public double Price { get; set; }
        
        public string? ImageUrl { get; set; }


        [Required]
        public IBrowserFile? ImageFile { get; set; }

    }
}
