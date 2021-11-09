using GeneralStore.MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GeneralStore.MVC.Controllers
{
    public class CustomerController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Customer
        public ActionResult Index()
        {
            List<Customer> customerList = _db.Customers.ToList();
            List<Customer> orderedList = customerList.OrderBy(prod => prod.LastName).ToList();
            return View(_db.Customers.ToList());
        }

        //Get: Customer
        public ActionResult Create()
        {
            return View();
        }

        //Post: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        //Get: Delete
        //Customer/Delete/{id}
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = _db.Customers.Find(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        //Post: Delete
        //Customer/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var customer = _db.Customers.Find(id);
            _db.Customers.Remove(customer);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Get: Edit
        //Customer/Edit/{id}
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = _db.Customers.Find(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        //Post: edit
        //Customer/Edit/{id}
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(customer).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

    }
}