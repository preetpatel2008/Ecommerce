﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
  [Table("Cart")]
    public class CartModel
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }

    }
}
