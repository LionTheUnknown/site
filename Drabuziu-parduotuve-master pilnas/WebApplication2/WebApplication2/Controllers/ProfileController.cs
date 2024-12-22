using System;
using System.Linq;
using System.Web.Mvc;
using WebApplication2.Models;
using WebApplication2.Data;
using System.Diagnostics;

namespace WebApplication2.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DatabaseContext db;

        public ProfileController()
        {
            db = new DatabaseContext();
        }

        // GET: Profile/View
        public ActionResult View()
        {
            var userId = Session["UserId"];

            if (userId == null)
            {
                return RedirectToAction("Error");
            }

            int parsedUserId;
            if (!int.TryParse(userId.ToString(), out parsedUserId))
            {
                return RedirectToAction("Error"); // Invalid session data
            }

            var user = db.Users.FirstOrDefault(u => u.Id == parsedUserId);
            if (user == null)
            {
                return RedirectToAction("Error");
            }

            // Check if the user is already in the Buyers or Sellers table
            var existingBuyer = db.Buyers.FirstOrDefault(b => b.Id == parsedUserId);
            var existingSeller = db.Sellers.FirstOrDefault(s => s.Id == parsedUserId);

            ViewBag.IsBuyer = existingBuyer != null;
            ViewBag.IsSeller = existingSeller != null;
            ViewBag.ShowRoleSelection = !ViewBag.IsBuyer && !ViewBag.IsSeller;

            var couponId = db.Users
                .Where(u => u.Id == parsedUserId)
                .Select(u => u.Code)
                .FirstOrDefault();

            if (couponId != 0) // Ensure the coupon ID exists
            {
                ViewBag.Coupon = db.Coupons
                    .Include("CouponProducts")
                    .FirstOrDefault(c => c.Id == couponId);
            }

            return View(user); // Pass user to the view
        }

        // POST: SelectRole (Assign user to Buyer or Seller with default data)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectRole(string role)
        {
            var userId = Session["UserId"];

            if (userId == null)
            {
                return RedirectToAction("Error");
            }

            int parsedUserId;
            if (!int.TryParse(userId.ToString(), out parsedUserId))
            {
                return RedirectToAction("Error"); // Invalid session data
            }
            Debug.WriteLine(parsedUserId + "      user id ");

            var user = db.Users.FirstOrDefault(u => u.Id == parsedUserId);
            if (user == null)
            {
                return RedirectToAction("Error");
            }

            // Ensure the user has not already selected a role
            var existingBuyer = db.Buyers.FirstOrDefault(b => b.Id == parsedUserId);
            var existingSeller = db.Sellers.FirstOrDefault(s => s.Id == parsedUserId);

            if (existingBuyer != null || existingSeller != null)
            {
                return RedirectToAction("View"); // Already has a role, redirect to profile
            }

            // Add the user to the appropriate table based on the selected role
            if (role == "Buyer")
            {
                var buyer = new Buyers
                {
                    Id = parsedUserId,
                    Birthday = GenerateRandomDate(),
                    Place = "Default Place",
                    Credit = 1000.0,
                    Gender = 1, // Default gender, adjust as needed
                    Code = parsedUserId,
                };
                db.Buyers.Add(buyer);
                db.SaveChanges();
            }
            else if (role == "Seller")
            {
                var seller = new Sellers
                {
                    Id = parsedUserId,
                    Place = "Default Place"
                };
                db.Sellers.Add(seller);
                db.SaveChanges();
            }

            return RedirectToAction("View");
        }

        // Helper method to generate a random DateTime within a range
        private DateTime GenerateRandomDate()
        {
            Random rand = new Random();
            int year = rand.Next(1950, 2000); // Random year between 1950 and 2000
            int month = rand.Next(1, 13); // Random month between 1 and 12
            int day = rand.Next(1, 29); // Random day between 1 and 28 (to avoid issues with different month lengths)

            return new DateTime(year, month, day);
        }
    }
}
