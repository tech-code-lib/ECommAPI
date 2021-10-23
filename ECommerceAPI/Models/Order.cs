﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderPlacedDate { get; set; }
        public decimal Total { get; set; }

    }
}
