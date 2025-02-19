using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceylangiyim.Core.Entities
{
    public class Coupon : IEntity
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Kupon Kodu")]
        public string Code { get; set; }  // Kupon Kodu
        [Range(1, 100 ,ErrorMessage = "İndirim oranı 1 ile 100 arasında olmalıdır!") ]
        
        public decimal? Discount { get; set; }  // İndirim Oranı (Örneğin: %10 indirim)
        public decimal? DiscountAmount { get; set; }

        public DateTime ExpirationDate { get; set; }  // Kuponun Son Geçerlilik Tarihi

        public bool IsActive { get; set; }  // Kupon Aktif mi?
    }
}
