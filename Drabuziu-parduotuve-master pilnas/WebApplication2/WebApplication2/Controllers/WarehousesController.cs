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
    public class WarehousesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Warehouses
        public ActionResult Index()
        {
            return View(db.Warehouses.ToList());
        }

        // GET: Warehouses/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse warehouse = db.Warehouses.Find(id);
            var products_temp = new List<Product>();
            foreach (var prwr in warehouse.WarehouseProducts)
            {
                foreach (var product in db.Products.ToList())
                {
                    if (prwr.ProductId == product.id)
                    {
                        prwr.Product = product;
                    }
                }
            }
                
                if (warehouse == null)
                {
                    return HttpNotFound();
                }
                return View(warehouse);
            }
        

        // GET: Warehouses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Warehouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,place")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                db.Warehouses.Add(warehouse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(warehouse);
        }

        // GET: Warehouses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // POST: Warehouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,place")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(warehouse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(warehouse);
        }

        // GET: Warehouses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // POST: Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Warehouse warehouse = db.Warehouses.Find(id);
            db.Warehouses.Remove(warehouse);
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
        // GET: Warehouses/AddExistingProduct/5
        public ActionResult AddExistingProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }

            ViewBag.WarehouseId = id;
            ViewBag.ProductId = new SelectList(db.Products, "id", "name"); // Populate dropdown with existing products
            return View();
        }

        // POST: Warehouses/AddExistingProduct/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExistingProduct(int id, int productId, int count)
        {
            if (count <= 0)
            {
                ModelState.AddModelError("count", "Count must be greater than zero.");
            }

            if (ModelState.IsValid)
            {
                // Check if the product is already associated with this warehouse
                var existingEntry = db.WarehouseProducts.FirstOrDefault(wp => wp.WarehouseId == id && wp.ProductId == productId);

                if (existingEntry != null)
                {
                    // Update count for existing product
                    existingEntry.Count += count;
                }
                else
                {
                    // Add new product to warehouse
                    var warehouseProduct = new WarehouseProduct
                    {
                        WarehouseId = id,
                        ProductId = productId,
                        Count = count
                    };
                    db.WarehouseProducts.Add(warehouseProduct);
                }

                db.SaveChanges();
                return RedirectToAction("Details", new { id });
            }

            ViewBag.WarehouseId = id;
            ViewBag.ProductId = new SelectList(db.Products, "id", "name", productId);
            return View();
        }
    }
}
