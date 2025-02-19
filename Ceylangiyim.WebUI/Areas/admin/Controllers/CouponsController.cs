using Ceylangiyim.Core.Entities;
using Ceylangiyim.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ceylangiyim.WebUI.Areas.admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]

    public class CouponsController : Controller
    {
        private readonly DatabaseContext _context;

        public CouponsController(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var coupons = await _context.Coupons.ToListAsync();
            return View(coupons);
        }

        public IActionResult Create()
        {
            var model = new Coupon();
            return View(model);
        }

        // Kupon Ekleme İşlemi
        [HttpPost]
        public async Task<IActionResult> Create(Coupon coupon)
        {
            // Kupon Kodu arada boşluk varmı kontorlü
            if (coupon.Code != null && coupon.Code.Contains(" "))
            {
                ModelState.AddModelError("Code", "Kupon kodu arasında boşluk olamaz.");
            }

            // Discount ve DiscountAmount kontrolü
            if (coupon.Discount.HasValue && coupon.DiscountAmount.HasValue)
            {
                ModelState.AddModelError("Discount", "İndirim Oranı veya indirim tutarından yalnızca 1ini giriniz!");
                ModelState.AddModelError("DiscountAmount", "İndirim Oranı veya indirim tutarından yalnızca 1ini giriniz!");
            }

            // İndirim oranı kontrolü
            if (coupon.Discount.HasValue && (coupon.Discount < 1 || coupon.Discount > 100))
            {
                ModelState.AddModelError("Discount", "İndirim oranı 1 ile 100 arasında olmalıdır.");
            }

            // Eğer model geçerli değilse formu tekrar göster
            if (!ModelState.IsValid)
            {
                return View(coupon);
            }

            coupon.IsActive = true; // Kupon aktif olarak eklenir
            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            var coupon = _context.Coupons.FirstOrDefault(c => c.Id == id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }

            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
