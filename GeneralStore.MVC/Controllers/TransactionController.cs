using GeneralStore.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GeneralStore.MVC.Controllers
{
    public class TransactionController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Transaction
        public ActionResult Index()
        {
            //use this instead
            //var transactions = _db.Transactions.ToList().OrderBy(t => t.TransactionId);
            //return View(transcations)
            List<Transaction> transactionList = _db.Transactions.ToList();
            List<Transaction> orderedList = transactionList.OrderBy(prod => prod.DateOfTransaction).ToList();
            return View(_db.Transactions.ToList());
        }

        //Get: Transaction
        //viewbag - https://techfunda.com/howto/136/views-of-model-having-primary-and-foreign-key-relationship
        public ActionResult Create()
        {
            //View.Customers = _db.Customers.Select(cust => new SelectListItem {Value = cust.CustomerId.ToString(), Text = cust.FirstName}).ToList();
            //ViewBag.Products = _dbProducts.Select(cust => new SelectListItem {Value = prod.ProductId.ToString(), Text = prod.Name }).ToList();

            ViewBag.CustomerId = new SelectList(_db.Customers, "CustomerId", "FullName");
            ViewBag.ProductId = new SelectList(_db.Products, "ProductId", "Name");
            return View();
        }

        private void PopulateDropDownLists()
        {
            ViewBag.Customers = _db.Customers.ToList().Select(cust => new SelectListItem { Value = cust.CustomerId.ToString(), Text = cust.FullName }).ToList();
            ViewBag.Products = _db.Products.Select(prod => new SelectListItem { Value = prod.ProductId.ToString(), Text = prod.Name }).ToList();
        }

        //Post: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.DateOfTransaction = DateTime.Now;//set the date of the transaction

                var product = _db.Products.Find(transaction.ProductId);//find the product
                if (product == null)
                    return HttpNotFound();

                //check if in stock
                if (product.InventoryCount < 1)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No inventory in stock.");

                //Enough product for order
                if (product.InventoryCount < transaction.ItemCount)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Not enough in stock");

                //if transaction is made deduct from stock
                product.InventoryCount -= transaction.ItemCount;

                _db.Transactions.Add(transaction);
                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.CustomerID = new SelectList(_db.Customers, "CustomerID", "FullName", transaction.CustomerId);
            ViewBag.ProductID = new SelectList(_db.Products, "ProductID", "Name", transaction.ProductId);

            return View(transaction);
        }
    }
}