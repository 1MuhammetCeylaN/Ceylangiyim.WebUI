using Ceylangiyim.Core.Entities;

namespace Ceylangiyim.WebUI.Models
{
    public class CheckoutViewModel
    {
        public List<CartLine> CartProducts { get; set; }
        public decimal TotalPrice { get; set; }
        public List<Address>? Addresses { get; set; }
        public CartViewModel? CartViewModel { get; set; }
        public decimal? AppliedCouponDiscount { get; set; } // Kupon indirimi
        public decimal? CouponDiscount { get; set; } // Kupon indirimi
        public string? CouponCode { get; set; } // Kupon indirimi
        public List<decimal> ProductPrices { get; set; }

    }
}
