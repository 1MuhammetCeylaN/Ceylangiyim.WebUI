using Ceylangiyim.Core.Entities;

namespace Ceylangiyim.WebUI.Models
{
    public class CartViewModel
    {
        public List<CartLine>? CartLines { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscountedPrice { get; set; }
        public decimal TotalOriginalPrice { get; set; }
        public bool? IsCoupon { get; set; }
        public decimal? CouponPrice { get; set; }
    }
}
