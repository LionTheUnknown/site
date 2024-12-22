using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CouponsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Coupons
        public ActionResult Index()
        {
            return View(db.Coupons.ToList());
        }

        // GET: Coupons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coupon coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // GET: Coupons/Create
		public ActionResult Create()
		{
			ViewBag.Products = db.Products.ToList();
			var coupon = new Coupon
			{
				SelectedProductIds = new List<int>(),
				ProductMinQuantities = new Dictionary<int, int?>()
			};
			return View(coupon);
		}

		// POST: Coupons/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Coupon coupon, Dictionary<int, int?> ProductMinQuantities, List<int> SelectedProductIds)
		{
			ViewBag.Products = db.Products.ToList();
			
			if (!ModelState.IsValid)
				return View(coupon);
			
			try 
			{
				coupon.Sukurimo_data = DateTime.UtcNow;
				coupon.SelectedProductIds = SelectedProductIds ?? new List<int>();
				coupon.ProductMinQuantities = ProductMinQuantities ?? new Dictionary<int, int?>();
				
				var random = new Random();
				string code;
				do 
				{
					string timestamp = DateTime.UtcNow.ToString("yyMMddHHmm");
					string randomPart = new string(Enumerable.Range(0, 4)
						.Select(_ => "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"[random.Next(36)])
						.ToArray());
					code = $"{timestamp}{randomPart}";
				} 
				while (db.Coupons.Any(c => c.Kodas == code));
				
				coupon.Kodas = code;
				
				db.Coupons.Add(coupon);
				db.SaveChanges();
				
				if (coupon.SelectedProductIds != null && coupon.SelectedProductIds.Any())
				{
					foreach (var productId in coupon.SelectedProductIds)
					{
						int? minQuantity = null;
						if (coupon.ProductMinQuantities != null && 
							coupon.ProductMinQuantities.ContainsKey(productId))
						{
							minQuantity = coupon.ProductMinQuantities[productId];
						}
						
						db.Database.ExecuteSqlCommand(
							"INSERT INTO nuolaidoskodas_produktas (fk_produktas, fk_nuolaidoskodas, minkiekis) VALUES ({0}, {1}, {2})",
							productId, coupon.Id, minQuantity
						);
					}
				}
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View(coupon);
			}
		}

        // GET: Coupons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coupon coupon = db.Coupons.Include(c => c.CouponProducts).FirstOrDefault(c => c.Id == id);
            if (coupon == null)
            {
                return HttpNotFound();
            }

            ViewBag.Products = db.Products.ToList();

            coupon.SelectedProductIds = coupon.CouponProducts
                .Select(c => c.ProductId)
                .ToList();

            coupon.ProductMinQuantities = coupon.CouponProducts
                .ToDictionary(c => c.ProductId, c => (int?)c.MinQuantity);

            return View(coupon);
        }

        // POST: Coupons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Coupon coupon, Dictionary<int, int?> ProductMinQuantities, List<int> SelectedProductIds)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var existingCoupon = db.Coupons
						.Include(c => c.CouponProducts)
						.FirstOrDefault(c => c.Id == coupon.Id);

					if (existingCoupon == null)
					{
						return HttpNotFound();
					}

					db.Entry(existingCoupon).CurrentValues.SetValues(coupon);
					
					db.Database.ExecuteSqlCommand(
						"DELETE FROM nuolaidoskodas_produktas WHERE fk_nuolaidoskodas = {0}",
						coupon.Id
					);

					if (SelectedProductIds != null && SelectedProductIds.Any())
					{
						foreach (var productId in SelectedProductIds)
						{
							int? minQuantity = null;
							if (ProductMinQuantities != null && 
								ProductMinQuantities.ContainsKey(productId))
							{
								minQuantity = ProductMinQuantities[productId];
							}

							db.Database.ExecuteSqlCommand(
								"INSERT INTO nuolaidoskodas_produktas (fk_produktas, fk_nuolaidoskodas, minkiekis) VALUES ({0}, {1}, {2})",
								productId, coupon.Id, minQuantity
							);
						}
					}

					db.SaveChanges();
					return RedirectToAction("Index");
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
				}
			}

			ViewBag.Products = db.Products.ToList();
			return View(coupon);
		}
        // GET: Coupons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coupon coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // POST: Coupons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Coupon coupon = db.Coupons.Find(id);
            db.Coupons.Remove(coupon);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
