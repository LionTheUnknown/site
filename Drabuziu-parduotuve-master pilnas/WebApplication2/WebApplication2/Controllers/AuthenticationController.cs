using System;
using System.Web.Mvc;
using WebApplication2.Models;
using System.Linq;
using System.Web.Security;
using System.Data.Entity;
using WebApplication2.Data;

namespace WebApplication2.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly DatabaseContext db = new DatabaseContext();

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the phone number is already registered in the Users table
                if (db.Users.Any(u => u.PhoneNumber == model.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "This phone number is already registered.");
                    return View(model);
                }

                // Create the new user and add it to the Users table
                var user = new Users
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = model.Password, // Hash the password in a real application
                    PhoneNumber = model.PhoneNumber,
                };

                db.Users.Add(user);
                db.SaveChanges(); // Save to get the generated Id

                if (!db.Administrators.Any()) // If no admins exist
                {
                    // Register the new user as an admin
                    var newAdmin = new Administrator
                    {
                        Id = user.Id,
                        Chief = true,
                        Salary = 0,
                        Card = "0000" // Example card number
                    };

                    db.Administrators.Add(newAdmin);
                    db.SaveChanges();
                }
                else if (Session["UserId"] != null)
                {
                    var currentLoggedInUserId = (int)Session["UserId"];
                    var currentUserAdmin = db.Administrators.FirstOrDefault(a => a.Id == currentLoggedInUserId);

                    if (currentUserAdmin != null && currentUserAdmin.Chief)
                    {
                        // Register the new user as an admin
                        var newAdmin = new Administrator
                        {
                            Id = user.Id,
                            Chief = true,
                            Salary = 0,
                            Card = "0000" // Example card number
                        };

                        db.Administrators.Add(newAdmin);
                        db.SaveChanges();
                    }
                }

                // Redirect to the login page after successful registration
                return RedirectToAction("Login");
            }

            return View(model);
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.FirstName == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    // Store the user ID in the session
                    Session["UserId"] = user.Id;

                    // Check if the user is an admin
                    var admin = db.Administrators.FirstOrDefault(a => a.Id == user.Id);
                    ViewBag.IsAdmin = admin != null && admin.Chief;

                    // Pass admin status to the view
                    TempData["IsAdmin"] = ViewBag.IsAdmin;
                    if (db.Administrators.Find(user.Id) != null && db.Administrators.Find(user.Id).Chief == true)
                    {
                        Session["IsAdmin"] = true;
                    }
                    else
                    {
                        Session["IsAdmin"] = false;
                    }

                    if (db.Sellers.Find(user.Id) != null)
                    {
                        Session["IsSeller"] = true;
                    }
                    else
                    {
                        Session["IsSeller"] = false;
                    }

                    // Redirect to the home page
                    return Redirect("/");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear(); // Clear the session to log out
            return Redirect("/"); // Redirect to the home page or login page
        }
    }
}
