using Mysqlx.Crud;
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
    public class SalariesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Salaries
        public ActionResult Index()
        {
            int loggedInUserId = (int)Session["UserId"];
            // Find the logged-in admin in the database
            var loggedInAdmin = db.Administrators.FirstOrDefault(a => a.Id == loggedInUserId);

            // If the logged-in admin is not found or is not a chief, restrict access
            if (loggedInAdmin == null || !loggedInAdmin.Chief)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Access Denied");
            }

            // Get all administrators excluding the logged-in admin
            List<Administrator> admins = db.Administrators
                .Where(a => a.Id != loggedInUserId)
                .ToList();

            // Pass the filtered administrators to the view
            ViewBag.Administrators = admins;
            return View();
        }


        // Set: Salaries/Index
        public ActionResult Payout(int adminId, string adminName, float adminSalary, float bonus)
        {
            int currentUserId = (int)Session["UserId"];
            var payout = new Salary
            {
                FkAdministratorius = currentUserId,
                bonus = bonus,
                FkAdministratorius1 = adminId,
                cost = adminSalary
            };

            db.Salaries.Add(payout);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
