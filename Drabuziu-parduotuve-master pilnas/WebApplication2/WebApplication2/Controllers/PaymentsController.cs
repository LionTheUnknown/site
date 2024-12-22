using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;
using static Mysqlx.Expect.Open.Types.Condition.Types;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace WebApplication2.Controllers
{
    public class PaymentsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public float CalculateCOGSForCurrentMonth()
        {
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            var beginningInventory = db.Orders
                .Where(i => i.start < startDate)
                .OrderByDescending(i => i.start)
                .Select(i => i.itemCount)
                .FirstOrDefault();

            var totalPurchases = db.Payments
                .Join(db.Orders, p => p.FkUzsakymas, o => o.id, (p, o) => new { Payment = p, Order = o })
                .Where(x => x.Order.start >= startDate && x.Order.end <= endDate)
                .Sum(x => (float?)x.Payment.cost) ?? 0;

            var endingInventory = db.Orders
            .Where(i => i.end <= endDate)
            .OrderByDescending(i => i.end)
            .Select(i => (float)i.itemCount)
            .FirstOrDefault();

            var cogs = beginningInventory + totalPurchases - endingInventory;

            return cogs;
        }
        public ActionResult Index()
        {
            // Step 4: Calculate total revenue from the mokestis column
            var totalRevenue = db.Payments.Sum(m => m.cost);

            // Step 5: Calculate gross profit (assume COGS logic exists)
            var totalCOGS = CalculateCOGSForCurrentMonth();
            var grossProfit = totalRevenue - totalCOGS;

            // Step 6: Calculate operating profit
            var totalOperatingExpenses = db.Salaries.Sum(e => e.cost); // Replace with additional warehouse costs if applicable
            var operatingProfit = grossProfit - totalOperatingExpenses;

            // Step 7: Calculate profit margin
            var profitMargin = (operatingProfit / totalRevenue) * 100;

            // Step 8: Calculate total expenses
            var totalExpenses = totalOperatingExpenses + totalCOGS;

            // Step 9: Calculate net profit or loss
            var netProfit = totalRevenue - (totalExpenses + totalCOGS);

            var topClothes = db.OrderProducts
            .Join(db.Products, up => up.ProductId, p => p.id, (up, p) => new { Product = p, Quantity = up.cost })
            .GroupBy(x => x.Product.name)
            .Select(g => new
            {
                ProductName = g.Key,
                TotalSold = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.TotalSold)
            .Take(5)
            .ToList();

            // Create lists for the view
            ViewBag.TopClothesNames = topClothes.Select(x => x.ProductName).ToList();
            ViewBag.TopClothesSold = topClothes.Select(x => x.TotalSold).ToList();

            // Category-Wise Revenue Distribution
            var topCategories = db.Products
                .SelectMany(p => p.ProductCategories)
                .GroupBy(c => c.Category)
                .Select(g => new
                {
                    Category = g.Key.name,
                    TotalSold = g.Count()
                })
                .ToList();


            ViewBag.TopClothesCategories = topCategories.Select(tc => tc.Category).ToList();
            ViewBag.TopCategoriesSold = topCategories.Select(tc => tc.TotalSold).ToList();

            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.GrossProfit = grossProfit;
            ViewBag.OperatingProfit = operatingProfit;
            ViewBag.ProfitMargin = profitMargin;
            ViewBag.TotalExpenses = totalExpenses;
            ViewBag.NetProfit = netProfit;

            return View();
        }

        [ActionName("CancelOrderPayment")]
        public ActionResult CancelOrderPayment(int orderId)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Locate the associated payment for the order
                    var payment = db.Payments.FirstOrDefault(p => p.FkUzsakymas == orderId);

                    if (payment == null)
                    {
                        // If no payment exists for the order, inform the user
                        TempData["ErrorMessage"] = "No payment found for the specified order.";
                        return RedirectToAction("Index", "Orders");
                    }

                    // Remove the payment from the database
                    db.Payments.Remove(payment);

                    // Save changes to persist the deletion
                    db.SaveChanges();

                    // Commit the transaction
                    transaction.Commit();

                    // Success message
                    TempData["Message"] = "Payment successfully canceled.";
                }
                catch (Exception ex)
                {
                    // Rollback the transaction in case of an error
                    transaction.Rollback();
                    TempData["ErrorMessage"] = "An error occurred while canceling the payment. Please try again.";
                }
            }

            // Redirect back to the Orders Index page
            return RedirectToAction("Index", "Orders");
        }

        public ActionResult GenerateAndDownloadReceipt(int orderId)
        {
            // Get order details (dummy data here)
            var order = db.Orders.Find(orderId);
            var receiptData = new
            {
                OrderId = orderId,
                TotalAmount = order.cost,
                CustomerName = db.Users.Find(order.buyerId).FirstName + " " + db.Users.Find(order.buyerId).LastName,
                Date = order.start
            };

            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Receipt";

            // Create an empty page
            PdfPage page = document.AddPage();

            // Create a drawing object to draw on the page
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font to use in the PDF
            XFont font = new XFont("Verdana", 12);  // No need for XFontStyle.Regular
            XFont headerFont = new XFont("Verdana", 16);  // Larger font for the header

            // Add a logo (ensure logo path is correct)
            string logoPath = Server.MapPath("~/logo.png"); // Adjust the path as necessary
            if (System.IO.File.Exists(logoPath))
            {
                XImage logo = XImage.FromFile(logoPath);
                gfx.DrawImage(logo, 50, 20, 100, 100); // Adjust the position and size of the logo
            }

            // Set up margins and positions
            int leftMargin = 50;
            int topMargin = 140; // Adjust to place text below the logo
            int lineSpacing = 20; // Space between lines

            // Draw the content on the page
            gfx.DrawString("Sąskaita", headerFont, XBrushes.Black, new XPoint(260, topMargin)); // Title

            // Order details
            gfx.DrawString($"Užsakymo nr.: {receiptData.OrderId}", font, XBrushes.Black, new XPoint(leftMargin, topMargin + lineSpacing * 2));
            gfx.DrawString($"Pirkėjo vardas/pavardė: {receiptData.CustomerName}", font, XBrushes.Black, new XPoint(leftMargin, topMargin + lineSpacing * 3));
            gfx.DrawString($"Pirkinio suma: ${receiptData.TotalAmount}", font, XBrushes.Black, new XPoint(leftMargin, topMargin + lineSpacing * 4));
            gfx.DrawString($"Nusipirkimo data: {receiptData.Date}", font, XBrushes.Black, new XPoint(leftMargin, topMargin + lineSpacing * 5));

            // Save the PDF to a memory stream
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                ms.Position = 0; // Rewind the memory stream

                // Return the PDF as a file download
                return File(ms.ToArray(), "application/pdf", "receipt.pdf");
            }
        }


    }
}
