﻿using Ceylangiyim.Core.Entities;

namespace Ceylangiyim.WebUI.Models
{
    public class CheckoutViewModel
    {
        public List<CartLine> CartProducts { get; set; }
        public decimal TotalPrice { get; set; }
        public List<Address>? Addresses { get; set; }
    }
}
