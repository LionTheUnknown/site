using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";
        private const string CouponSessionKey = "AppliedCoupon";

        public ActionResult Index()
        {
            var cart = Session[CartSessionKey] as List<CartItem> ?? new List<CartItem>();
            var couponCode = Session[CouponSessionKey] as string;

            // Reset prices to original before applying discount
            foreach (var item in cart)
            {
                item.Price = item.OriginalPrice;
            }

            if (!string.IsNullOrEmpty(couponCode))
            {
                using (var db = new DatabaseContext())
                {
                    var coupon = db.Coupons
                        .Include("CouponProducts")
                        .FirstOrDefault(c => c.Kodas == couponCode &&
                                           (!c.Galiojimo_pabaigos_data.HasValue || c.Galiojimo_pabaigos_data >= DateTime.Now) &&
                                           (!c.Veikimo_pradzios_data.HasValue || c.Veikimo_pradzios_data <= DateTime.Now) &&
                                           (!c.Yra_ribotas || (c.Yra_ribotas && c.Panaudojimu_sk > 0)));

                    if (coupon != null)
                    {
                        bool anyItemDiscounted = false;
                        foreach (var item in cart)
                        {
                            var couponProduct = coupon.CouponProducts
                                .FirstOrDefault(cp => cp.ProductId == item.ProductId);

                            if (couponProduct != null &&
                                (!couponProduct.MinQuantity.HasValue || item.Quantity >= couponProduct.MinQuantity.Value))
                            {
                                item.Price = item.OriginalPrice * (1 - (float)(coupon.Verte / 100));
                                anyItemDiscounted = true;
                            }
                        }
                        if (anyItemDiscounted)
							ViewBag.CouponMessage = "Nuolaida pritaikyta";
                        else
						{
                            ViewBag.CouponMessage = "Šis nuolaidos kodas netinka nė vienai iš jūsų krepšelyje esančių prekių";
                            Session[CouponSessionKey] = null;
						}
                    }
                    else
                    {
                        ViewBag.CouponMessage = "Neteisingas nuolaidos kodas arba jis nebegalioja";
                        Session[CouponSessionKey] = null;
                    }
                }
            }

            return View(cart);
        }

        [HttpPost]
        public ActionResult ApplyCoupon(string couponCode)
        {
            Session[CouponSessionKey] = couponCode;
            return RedirectToAction("Index");
        }


        // POST: AddToCart
        [HttpPost]
        public ActionResult AddToCart(int id, string name, float price)
        {
            var cart = Session[CartSessionKey] as List<CartItem> ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(c => c.ProductId == id);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = id,
                    Name = name,
                    Price = price,
                    OriginalPrice = price,
                    Quantity = 1
                });
            }

            Session[CartSessionKey] = cart;

            return RedirectToAction("Index");
        }

        // POST: Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(string paymentType, string promoCode, int storeCredit)
        {
            var cart = Session[CartSessionKey] as List<CartItem>;
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Orders");
            }

            var userId = (int)Session["UserId"];
            var totalCost = cart.Sum(c => c.Price * c.Quantity);

            // Apply store credit
            var finalStoreCredit = Math.Min(storeCredit, (int)totalCost);
            var finalCost = totalCost - finalStoreCredit;

            using (var db = new DatabaseContext())
            {
                // Get payment type record from PaymentTypes table
                var paymentTypeRecord = db.PaymentTypes.FirstOrDefault(pt => pt.Name == paymentType);
                if (paymentTypeRecord == null)
                {
                    TempData["ErrorMessage"] = "Invalid payment type selected.";
                    return RedirectToAction("Cart");
                }

                // Create the order
                var order = new Order
                {
                    location = "Customer's Address", // Replace with user's actual address
                    itemCount = cart.Sum(c => c.Quantity),
                    cost = (float)cart.Sum(c => c.Price * c.Quantity),
                    start = DateTime.Now,
                    end = DateTime.Now.AddDays(3),
                    status = "Pateiktas",
                    buyerId = userId,
                    OrderProduct = cart.Select(c => new OrderProduct
                    {
                        ProductId = c.ProductId,
                        cost = (float)(c.Price * c.Quantity)
                    }).ToList()
                };

                db.Orders.Add(order);
                db.SaveChanges(); // Save to generate the Order ID

                // Now create the payment object with the generated Order ID
                var payment = new Payment
                {
                    cost = (float)finalCost,
                    FkPirkejas = userId,
                    FkMokejimoTipas = paymentTypeRecord.Id,
                    FkUzsakymas = order.id // Use the generated Order ID
                };

                if (paymentType.Contains("parduotuves_kreditas"))
                {
                    // Deduct store credit if it's part of the payment type
                    var buyer = db.Buyers.Find(userId);
                    if (buyer != null)
                    {
                        buyer.Credit -= finalStoreCredit;
                        db.SaveChanges();
                    }
                }

                // Handle remaining payment methods
                if (paymentType.Contains("kortele"))
                {
                    // Process credit card payment
                }
                else if (paymentType.Contains("paypal"))
                {
                    // Process PayPal payment
                }

                db.Payments.Add(payment);
                db.SaveChanges();

                // Optionally create a new coupon for the user
                try
                {
                    var newCoupon = new Coupon
                    {
                        Sukurimo_data = DateTime.UtcNow,
                        Veikimo_pradzios_data = DateTime.UtcNow,
                        Galiojimo_pabaigos_data = DateTime.UtcNow.AddMonths(1),
                        Panaudojimu_sk = 1,
                        Yra_ribotas = true,
                        Verte = 10, // 10% discount
                        Pavadinimas = "Lojalumo nuolaida",
                        Aprasymas = "Ačiū už pirkinį! Štai jūsų nuolaida kitam apsipirkimui."
                    };

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

                    newCoupon.Kodas = code;
                    db.Coupons.Add(newCoupon);
                    db.SaveChanges();

                    foreach (var item in cart.Where(x => x.Quantity >= 2))
                    {
                        db.Database.ExecuteSqlCommand(
                            "INSERT INTO nuolaidoskodas_produktas (fk_produktas, fk_nuolaidoskodas, minkiekis) VALUES ({0}, {1}, {2})",
                            item.ProductId, newCoupon.Id, 1
                        );
                    }

                    var user = db.Users.Find(userId);
                    if (user != null)
                    {
                        db.Database.ExecuteSqlCommand(
                            "UPDATE vartotojas SET fk_nuolaidoskodas = {0} WHERE id = {1}",
                            newCoupon.Id, userId
                        );
                    }

                    TempData["CouponCode"] = code;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to create coupon: {ex.Message}");
                }

                // Clear the cart session
                Session[CartSessionKey] = null;

                // Redirect to the orders page
                return RedirectToAction("Index", "Orders");
            }
        }


    }
}
