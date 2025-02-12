using Ceylangiyim.Core.Entities;
using Ceylangiyim.Data;
using Ceylangiyim.Service.Abstract;
using Ceylangiyim.Service.Concrete;
using Ceylangiyim.WebUI.ExtensionMethods;
using Ceylangiyim.WebUI.Models;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ceylangiyim.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IService<Product> _productService;
        private readonly IService<Core.Entities.Address> _addressService;
        private readonly IService<AppUser> _appUserService;
        private readonly IService<Order> _orderService;
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _databaseContext;


        public CartController(IService<Product> productService, IService<Core.Entities.Address> addressService, IService<AppUser> appUserService, IService<Order> orderService, IConfiguration configuration, DatabaseContext databaseContext)
        {
            _productService = productService;
            _addressService = addressService;
            _appUserService = appUserService;
            _orderService = orderService;
            _configuration = configuration;
            _databaseContext = databaseContext;
        }

        public IActionResult Index()
        {
            var cart = GetCart();

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



        public IActionResult Add(int ProductId, int quantity = 1, string selectedSize = "")
        {
            var product = _productService.Find(ProductId);
            if (product != null && product.IsActive)
            {
                var cart = GetCart();

                // Aynı ürün ve aynı beden var mı kontrol et
                var existingCartItem = cart.CartLines.FirstOrDefault(p => p.Product.Id == ProductId && p.Product.Name.EndsWith($"- {selectedSize}"));

                if (existingCartItem != null)
                {
                    // Eğer aynı ürün ve beden varsa, quantity artır
                    existingCartItem.Quantity += quantity;
                }
                else
                {
                    // Yeni bir ürün nesnesi oluştur ve farklı beden olarak ekle
                    var newProduct = new Product
                    {
                        Id = product.Id, // ID aynı ama farklı referans olacak
                        Name = $"{product.Name} - {selectedSize}", // Beden bilgisini ekleyerek yeni isim oluştur
                        Price = product.Price,
                        Image = product.Image,
                        DiscountedPrice = product.DiscountedPrice // İndirimli fiyatı da taşıyoruz
                    };

                    // Yeni ürünü sepete ekle
                    cart.AddProduct(newProduct, quantity);
                }

                // Sepetteki her ürün için fiyatı kontrol et
                foreach (var item in cart.CartLines)
                {
                    // Eğer indirimli fiyat varsa, onu kullan
                    var displayPrice = item.Product.DiscountedPrice ?? item.Product.Price;
                    item.SizeName = selectedSize;
                    // İndirimli fiyatı kullanarak ürün fiyatını güncelleyebiliriz (görselde göstermek için, örneğin)
                    item.Product.Price = displayPrice;
                }

                // Güncellenmiş sepeti Session'a kaydet
                HttpContext.Session.SetJson("Cart", cart);

                return Redirect(Request.Headers["Referer"].ToString());
            }

            return RedirectToAction("Index");
        }



        public IActionResult UpdateQuantity(int ProductId, string selectedSize, int quantity = 1)
        {
            var product = _productService.Find(ProductId);

            if (product != null)
            {
                var cart = GetCart();

                // Ürün adı + beden bilgisini benzersiz anahtar olarak kullan
                var uniqueKey = $"{product.Id}-{selectedSize}";


                // Eğer ürün zaten varsa, quantity'yi arttır
                var existingCartLine = cart.CartLines.FirstOrDefault(p => p.UniqueKey == uniqueKey);

                if (existingCartLine != null)
                {
                    existingCartLine.Quantity = quantity;
                }
                else
                {
                    // Yeni bir ürün olarak sepete ekle
                    cart.AddProduct(product, quantity);
                }

                // Sepeti session'a kaydet
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }




        public IActionResult Update(int ProductId, int quantity = 1)
        {
            var product = _productService.Find(ProductId);
            if (product != null)
            {
                var cart = GetCart();
                cart.UpdateProduct(product, quantity);
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int ProductId, string selectedSize)
        {
            // Sepetten ürün silme işlemi
            var cart = GetCart();

            // UniqueKey'i oluşturup sepetteki ürünü buluyoruz
            var urun = cart.CartLines.FirstOrDefault(p => p.UniqueKey == $"{ProductId}-{selectedSize}");

            if (urun != null)
            {
                // Ürünü sepetteki listeden çıkarıyoruz
                cart.CartLines.Remove(urun);
            }

            // Sepeti güncelle
            HttpContext.Session.SetJson("Cart", cart);

            return RedirectToAction("Index"); // Veya sepete yönlendirme
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();

            var appUser = await _appUserService.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null) { return RedirectToAction("SignIn", "Account"); }

            // Sepetteki ürünlerin aktif olup olmadığını kontrol etmek için, sadece aktif ürünleri filtreleyelim
            var filteredCartLines = new List<CartLine>();

            foreach (var cartLine in cart.CartLines)
            {
                // Her ürün için veritabanını kontrol ediyoruz ve ürün aktifse listeye ekliyoruz
                var product = await _databaseContext.Products
                    .FirstOrDefaultAsync(p => p.Id == cartLine.ProductId && p.IsActive);

                if (product != null)
                {
                    // Eğer ürün aktifse, sepetteki ürünü ekliyoruz
                    cartLine.Product = product; // Ürün bilgilerini güncelliyoruz
                    filteredCartLines.Add(cartLine); // Aktif ürünü listeye ekliyoruz
                }
            }

            // Aktif ürünleri ve güncel fiyatları kullanarak toplam fiyatı hesaplıyoruz
            var model = new CheckoutViewModel()
            {
                CartProducts = filteredCartLines, // Yalnızca aktif ürünler
                TotalPrice = filteredCartLines.Sum(c => c.Quantity *
                    (c.Product.DiscountedPrice.HasValue ? c.Product.DiscountedPrice.Value : c.Product.Price)), // Güncel fiyat hesaplama
                Addresses = await _addressService.GetAllAsync(a => a.AppUserId == appUser.Id && a.IsActive)
            };

            return View(model);
        }




        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(string CardNumber, string CardNameSurname, string CardMonth, string CardYear, string CVV, string DeliveryAddress, string BillingAddress)
        {
            var cart = GetCart();

            var appUser = await _appUserService.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null) { return RedirectToAction("SignIn", "Account"); }

            var addresses = await _addressService.GetAllAsync(a => a.AppUserId == appUser.Id && a.IsActive);
            var model = new CheckoutViewModel()
            {
                CartProducts = cart.CartLines,
                TotalPrice = cart.CartLines.Sum(p => (p.Product.DiscountedPrice ?? p.Product.Price) * p.Quantity), // Güncellendi!
                Addresses = addresses
            };

            if (string.IsNullOrEmpty(CardNumber) || string.IsNullOrEmpty(CardNameSurname) || string.IsNullOrEmpty(CardMonth) || string.IsNullOrEmpty(CardYear) || string.IsNullOrEmpty(CVV) || string.IsNullOrEmpty(DeliveryAddress))
            {
                return View(model);
            }


            var faturaAdresi = addresses.FirstOrDefault(a => a.AddressGuid.ToString() == BillingAddress);
            var teslimatAdresi = addresses.FirstOrDefault(a => a.AddressGuid.ToString() == DeliveryAddress);

            // Ödeme İşlemi

            var siparis = new Order
            {
                AppUserId = appUser.Id,
                BillingAddress = $"{faturaAdresi.OpenAddress} {faturaAdresi.District} - {faturaAdresi.City} ", // BillingAddress,
                DeliveryAddress = $"{teslimatAdresi.OpenAddress} {teslimatAdresi.District} - {teslimatAdresi.City} ", // DeliveryAddress,

                CustomerId = appUser.UserGuid.ToString(),
                OrderDate = DateTime.Now,
                TotalPrice = cart.CartLines.Sum(p => (p.Product.DiscountedPrice ?? p.Product.Price) * p.Quantity), // Güncellendi!
                OrderNumber = Guid.NewGuid().ToString(),
                OrderState = 0,
                ProductName = string.Join(", ", cart.CartLines.Select(p => p.Product.Name)),
                OrderCount = cart.CartLines.Sum(p => p.Quantity),
                OrderLines = [],
            };



            // #region OdemeIslemi

            Options options = new Options();
            options.ApiKey = "sandbox-qVrJZDJLAOvSrf6opchDFezVBJtO7jyt"; // _configuration["IyzicOptions:ApiKey"];
            options.SecretKey = "sandbox-cPCNHmDRPqyMgUFxvKY2AjV6f8Eq2Fi1";//  _configuration["IyzicOptions:SecretKey"];
            options.BaseUrl = "https://sandbox-api.iyzipay.com";// _configuration["IyzicOptions:BaseUrl"];

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = HttpContext.Session.Id;
            request.Price = siparis.TotalPrice.ToString().Replace(",", ".");
            request.PaidPrice = siparis.TotalPrice.ToString().Replace(",", ".");
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B" + HttpContext.Session.Id;
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = CardNameSurname; // "John Doe";
            paymentCard.CardNumber = CardNumber; // "5528790000000008";
            paymentCard.ExpireMonth = CardMonth; // "12";
            paymentCard.ExpireYear = CardYear; // "2030";
            paymentCard.Cvc = CVV; // "123";
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY" + appUser.Id;
            buyer.Name = appUser.Name; // "John";
            buyer.Surname = appUser.SurName; // "Doe";
            buyer.GsmNumber = appUser.Phone; // "+905350000000";
            buyer.Email = appUser.Email; // "email@email.com";
            buyer.IdentityNumber = "11111111111";
            buyer.LastLoginDate = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss");
            buyer.RegistrationDate = appUser.CreateDate.ToString("yyyy-mm-dd hh:mm:ss"); //"2013-04-21 15:12:09";
            buyer.RegistrationAddress = siparis.DeliveryAddress; // "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = HttpContext.Connection.RemoteIpAddress?.ToString(); // "85.34.78.112";
            buyer.City = teslimatAdresi.City; // "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "";
            request.Buyer = buyer;

            var shippingAddress = new Iyzipay.Model.Address();
            shippingAddress.ContactName = appUser.Name + " " + appUser.SurName;
            shippingAddress.City = teslimatAdresi.City;
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = teslimatAdresi.OpenAddress; // "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "";
            request.ShippingAddress = shippingAddress;

            var billingAddress = new Iyzipay.Model.Address();
            billingAddress.ContactName = appUser.Name + " " + appUser.SurName;
            billingAddress.City = teslimatAdresi.City;
            billingAddress.Country = "Turkey";
            billingAddress.Description = faturaAdresi.OpenAddress; // "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();

            //BasketItem firstBasketItem = new BasketItem();
            //firstBasketItem.Id = "BI101";
            //firstBasketItem.Name = "Binocular";
            //firstBasketItem.Category1 = "Collectibles";
            //firstBasketItem.Category2 = "Accessories";
            //firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            //firstBasketItem.Price = "0.3";
            //basketItems.Add(firstBasketItem);


            foreach (var item in cart.CartLines)
            {
                decimal unitPrice = item.Product.DiscountedPrice ?? item.Product.Price; // İndirimli fiyat varsa onu al, yoksa normal fiyatı al

                siparis.OrderLines.Add(new OrderLine
                {
                    ProductId = item.Product.Id,
                    OrderId = siparis.Id,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    SizeName = item.SizeName,
                    ProductName = item.ProductName


                });

                basketItems.Add(new BasketItem
                {
                    Id = item.Product.Id.ToString(),
                    Name = item.Product.Name,
                    Category1 = "Kategori",
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = (unitPrice * item.Quantity).ToString().Replace(",", ".")
                });
            }


            request.BasketItems = basketItems;

            Payment payment = await Payment.Create(request, options);



            //  #endregion

            try
            {
                if (payment.Status == "success")
                {
                    // Ödeme başarılı , sipariş oluştur.

                    await _orderService.AddAsync(siparis);
                    var sonuuc = await _orderService.SaveChangesAsync();
                    if (sonuuc > 0)
                    {
                        HttpContext.Session.Remove("Cart");
                        return RedirectToAction("Thanks");
                    }
                }
                else
                {
                    TempData["Message"] = $"<div class='alert alert-danger' >Bir hata oluştu! ödeme işlemi başarısız!</div> ({payment.ErrorMessage})";
                }
            }
            catch (Exception)
            {

                TempData["Message"] = "<div class='alert alert-danger' >Bir Hata Oluştu!</div>";
            }

            return View(model);
        }

        public IActionResult Thanks()
        {
            return View();
        }

        private CartService GetCart()
        {
            return HttpContext.Session.GetJson<CartService>("Cart") ?? new CartService();
        }



    }
}
