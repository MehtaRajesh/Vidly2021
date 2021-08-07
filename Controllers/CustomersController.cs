using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;
        public CustomersController()
        {
            _context = new ApplicationDbContext();        
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        //public ViewResult Index()
        //{
        //    var customer = _context.Customers;
        //    return View(customer);
        //}
        // GET: Customers
        public ViewResult New()
        {
            var membershipTypes = _context.MembershipTypes.ToList();
            var viewModel = new CustomerFormViewModel()
            {
                Customer = new Customer(),
                MembershipTypes = membershipTypes
            };
            return View("CustomerForm",viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(CustomerFormViewModel viewModel)
        public ActionResult Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel()
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList()
                };
                return View("CustomerForm", viewModel);
            }
            if(customer.id == 0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                var _customerInDb = _context.Customers.Single(c => c.id == customer.id);
                _customerInDb.Name = customer.Name;
                _customerInDb.Birthdate = customer.Birthdate;
                _customerInDb.MembershipTypeId = customer.MembershipTypeId;
                _customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
                //TryUpdateModel(_customerInDb);
            }

            _context.SaveChanges();
            return RedirectToAction("Index","Customers");
            //return View(viewModel);

        }
        public ViewResult Index()
        {
            var membershipTyoes = _context.MembershipTypes.ToList();
            var customers = _context.Customers.Include(c => c.MembershipType).ToList();
            return View(customers);
            #region old code
            //var customers = GetCustomers();
            //var customers = new List<Customer>()
            //{
            //    new Customer(){id = 1, Name ="Customer 11"}
            //    ,new Customer(){id = 2, Name ="Customer 12"}
            //};
            //var viewmodel = new ViewModels.CustomerViewModel()
            //{
            //    Customers = customers
            //};
            //return View(customers);
            #endregion
        }
        #region Old code
        //public ViewResult Index()
        //{
        //    var customers = new List<Customer>()
        //    {
        //        new Customer(){id = 1, Name ="Customer 11"}
        //        ,new Customer(){id = 2, Name ="Customer 12"}
        //    };
        //    var viewmodel = new ViewModels.CustomerViewModel()
        //    {
        //        Customers = customers
        //    };
        //    return View(viewmodel);
        //}
        #endregion
        public ActionResult Details (int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.id == id);
            //var customer = GetCustomers().SingleOrDefault(c => c.id == id);
            if(customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.id == id);
            if (customer == null)
                return HttpNotFound();
            var viewModel = new CustomerFormViewModel()
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };
            return View("CustomerForm",viewModel);
        }
        private IEnumerable<Customer> GetCustomers()
        {
            return new List<Customer>
            {
                new Customer{id = 1,Name = "Johm Smit"},
                new Customer{id = 2,Name = "Mary Willians"}
            };
        }
    }
}