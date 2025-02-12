using Ceylangiyim.Core.Entities;
using Ceylangiyim.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceylangiyim.Service.Concrete
{
    public class CartService : ICartService
    {
        public List<CartLine> CartLines = new();
        public void AddProduct(Product product, int quantity)
        {
            // Ürünü ID + beden bilgisine göre kontrol et
            var urun = CartLines.FirstOrDefault(p => p.UniqueKey == $"{product.Id}-{product.Name}");

            if (urun != null) // Eğer aynı ID ve aynı beden varsa quantity artır
            {
                urun.Quantity += quantity;
            }
            else
            {
                // Yeni beden seçildiyse yeni bir ürün olarak sepete ekle
                CartLines.Add(new CartLine
                {
                    Quantity = quantity,
                    Product = product,
                    ProductId = product.Id,
                    ProductName = product.Name,
                });
            }
        }



        public void ClearAll()
        {
            CartLines.Clear();
        }

        public void RemoveProduct(Product product)
        {
            CartLines.RemoveAll(p => p.Product.Id == product.Id);
        }

        public decimal TotalPrice()
        {
            return CartLines.Sum(p => (p.Product.DiscountedPrice ?? p.Product.Price) * p.Quantity);
        }


        public void UpdateProduct(Product product, int quantity)
        {
            var urun = CartLines.FirstOrDefault(p => p.Product.Id == product.Id);

            if (urun != null)
            {
                urun.Quantity = quantity;

            }
            else
            {
                CartLines.Add(new CartLine
                {
                    Quantity = quantity,
                    Product = product
                });
            }
        }
    }
}
