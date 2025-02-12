using Ceylangiyim.Core.Entities;
using Ceylangiyim.Data;
using Ceylangiyim.Service.Abstract;
using Ceylangiyim.Service.Concrete;
using Ceylangiyim.WebUI.ExtensionMethods;
using Ceylangiyim.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ceylangiyim.WebUI.ViewComponents
{
    public class Header : ViewComponent
    {
        private readonly IService<Product> _productService;
        private readonly DatabaseContext _databaseContext;

        public Header(IService<Product> productService, DatabaseContext databaseContext)
        {
            _productService = productService;
            _databaseContext = databaseContext;
        }

        private CartService GetCart()
        {
            return HttpContext.Session.GetJson<CartService>("Cart") ?? new CartService();
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = GetCart();

            // Sadece veritabanında bulunan aktif ürünleri filtreleyelim
            var filteredCartLines = cart.CartLines
                .Where(c => _databaseContext.Products.Any(p => p.Id == c.ProductId))
                .ToList();

            var model = new CartViewModel()
            {
                CartLines = filteredCartLines,
                TotalPrice = filteredCartLines.Sum(c => c.Quantity *
                    (_databaseContext.Products
                        .Where(p => p.Id == c.ProductId)
                        .Select(p => (p.DiscountedPrice.HasValue ? p.DiscountedPrice.Value : p.Price))
                        .FirstOrDefault()))
            };

            return View(model);
        }
    }
}
