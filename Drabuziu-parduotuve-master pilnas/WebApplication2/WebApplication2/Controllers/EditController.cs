using System.Linq;
using System.Web.Mvc;
using WebApplication2.Models;
using WebApplication2.Data;

namespace WebApplication2.Controllers
{
    public class EditController : Controller
    {
        private readonly DatabaseContext _context;

        public EditController()
        {
            _context = new DatabaseContext();
        }

        // GET: Edit/Index
        public ActionResult Index()
        {
            // Check if the user is logged in by checking the session
            var userId = Session["UserId"];
            if (userId == null)
            {
                return RedirectToAction("Login", "Authentication"); // Redirect to login if not logged in
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == (int)userId);
            if (user == null)
            {
                return HttpNotFound(); // If no user found, return 404
            }

            return View(user); // Pass the user data to the view
        }

        // POST: Edit/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Users user)
        {
            if (ModelState.IsValid)
            {
                // Check if the user is logged in by checking the session
                var userId = Session["UserId"];
                if (userId == null)
                {
                    return RedirectToAction("Login", "Authentication"); // Redirect to login if not logged in
                }

                // Retrieve the existing user from the database
                var existingUser = _context.Users.FirstOrDefault(u => u.Id == (int)userId);
                if (existingUser == null)
                {
                    return HttpNotFound(); // If no user found, return 404
                }

                // Update the user details
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.PhoneNumber = user.PhoneNumber;

                // Save changes to the database
                _context.SaveChanges();

                // Redirect to the profile page after saving changes
                return RedirectToAction("Index", "Profile");
            }

            // If model validation fails, return the view with validation errors
            return View(user);
        }
    }
}
