using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceylangiyim.Core.Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ürün Adı Alanı Zorunludur.")]
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        [Display(Name = "Resim")]
        public string? Image { get; set; }
        [Display(Name = "1. Yardımcı Resim")]
        public string? HelperImage1 { get; set; }
        [Display(Name = "2. Yardımcı Resim")]
        public string? HelperImage2 { get; set; }
        [Display(Name = "3. Yardımcı Resim")]
        public string? HelperImage3 { get; set; }
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }
        [Display(Name = "Ürün Kodu")]
        public string? ProductCode { get; set; }
        [Display(Name = "Stok Adeti")]
        public int Stock { get; set; }
        [Display(Name = "Aktif?")]
        public bool IsActive { get; set; }
        [Display(Name = "Ana Sayfada Göster?")]
        public bool IsHome { get; set; }
        [Display(Name = "Kategory")]
        public int? CategoryId { get; set; }
        [Display(Name = "Kategory")]
        public Category? Category { get; set; }
        [Display(Name = "Marka")]
        public int? BrandId { get; set; }
        [Display(Name = "Marka")]
        public Brand? Brand { get; set; }
        [Display(Name = "Sıra Numarası")]
        public int OrderNo { get; set; }
        [Display(Name = "Kayıt Tarihi")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Display(Name = "İndirim Oranı")]
        public int? DiscountRate { get; set; }
        [Display(Name = "İndirimli Fiyat")]
        public decimal? DiscountedPrice { get; set; }
        [Display(Name = "Yeni Eklenen?")]
        public bool NewlyAdded { get; set; }
        [Display(Name = "XS")]
        public bool size1 { get; set; }
        [Display(Name = "S")]
        public bool size2 { get; set; }
        [Display(Name = "M")]
        public bool size3 { get; set; }
        [Display(Name = "L")]
        public bool size4 { get; set; }
        [Display(Name = "XL")]
        public bool size5 { get; set; }
        [Display(Name = "XXL")]
        public bool size6 { get; set; }
        public List<ProductColor>? ProductColors { get; set; } // Yeni eklendi
        [Display(Name="Renk")]
        public string? Color { get; set; }

    }
}
