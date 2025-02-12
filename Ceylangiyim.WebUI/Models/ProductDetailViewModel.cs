using Ceylangiyim.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ceylangiyim.WebUI.Models
{
    public class ProductDetailViewModel
    {
        public Product? Product { get; set; }
        public IEnumerable<Product>? RelatedProducts { get; set; }
        public string? SelectedSize { get; set; }
        public List<SelectListItem>? SizeSelectList { get; set; }

        public void PopulateSizeSelectList(Product product)
        {
            SizeSelectList = new List<SelectListItem>();

            if (product.size1) SizeSelectList.Add(new SelectListItem { Value = "XS", Text = "XS" });
            if (product.size2) SizeSelectList.Add(new SelectListItem { Value = "S", Text = "S" });
            if (product.size3) SizeSelectList.Add(new SelectListItem { Value = "M", Text = "M" });
            if (product.size4) SizeSelectList.Add(new SelectListItem { Value = "L", Text = "L" });
            if (product.size5) SizeSelectList.Add(new SelectListItem { Value = "XL", Text = "XL" });
            if (product.size6) SizeSelectList.Add(new SelectListItem { Value = "XXL", Text = "XXL" });
        }
    }
}
