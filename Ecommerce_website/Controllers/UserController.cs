using Ecommerce_website.Migrations;
using Ecommerce_website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using static NuGet.Packaging.PackagingConstants;

namespace Ecommerce_website.Controllers
{
    public class UserController : Controller
    {
        private readonly MyContext _context;
        private readonly IWebHostEnvironment _env;

        public UserController(MyContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ====================== Cutomer Section Start   ====================== 

        // ================= SIGNUP   =================

        public IActionResult SignUp() => View();


        [HttpPost]
        public IActionResult SignUp(Models.Customer customer, IFormFile customer_image)
        {
            //  EMAIL UNIQUE CHECK FIRST

            bool existingCustomerEmail = _context.Tbl_Customer
                .Any(c => c.customer_email == customer.customer_email);

            bool existingCustomerPhone = _context.Tbl_Customer
             .Any(c => c.customer_phone == customer.customer_phone);

            ModelState.Remove("customer_image");

            if (existingCustomerEmail)
            {
                ModelState.AddModelError("customer_email", "Email is already registered.");
            }

            if (existingCustomerPhone)
            {
                ModelState.AddModelError("customer_phone", "Phone number is already registered.");
            }

            //  NOW VALIDATION
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            // ===== IMAGE SAVE (OPTIONAL) =====

            if (customer_image != null && customer_image.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "Customer_images");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(customer_image.FileName);
                string filePath = Path.Combine(folder, fileName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    customer_image.CopyTo(fs);
                }

                customer.customer_image = fileName;
            }
            else
            {
                customer.customer_image = "default.png";
            }

            _context.Tbl_Customer.Add(customer);
            _context.SaveChanges();

            //  Redirect to login 
            TempData["success"] = "Account created successfully. Please login.";
            return RedirectToAction("SignIn");
        }


        // ====================== SIGNIN  ======================

        public IActionResult SignIn() => View();

        [HttpPost]
        public IActionResult SignIn(string customer_email, string customer_password, string returnUrl = null)
        {
            var customer = _context.Tbl_Customer
                .FirstOrDefault(c => c.customer_email == customer_email &&
                c.customer_password == customer_password);

            if (customer == null)
            {
                ViewBag.message = "Invalid email or password.";
                return View();
            }
            // Set session

            HttpContext.Session.SetString("User_Session", customer.customer_id.ToString());

            //redirect back to original page
         
     

            return RedirectToAction("index");

        }


        // ====================== Profile  ======================
        public IActionResult Profile()
        {
            string userid = HttpContext.Session.GetString("User_Session");

            if (userid == null)
                return RedirectToAction("SignIn");

            int id = Convert.ToInt32(userid);

            var customer = _context.Tbl_Customer.FirstOrDefault(c => c.customer_id == id);

            return View(customer);
        }


        // ====================== EditCustomer  ======================
        public IActionResult EditCustomer()
        {
            string userid = HttpContext.Session.GetString("User_Session");
            if (userid == null)
                return RedirectToAction("SignIn");

            int id = int.Parse(userid);

            var customer = _context.Tbl_Customer
                .FirstOrDefault(c => c.customer_id == id);

            if (customer == null)
                return RedirectToAction("Index");

            return View(customer);
        }

        [HttpPost]
        public IActionResult EditCustomer(Models.Customer model, IFormFile customer_image)
        {
            // Remove Image Validation

            ModelState.Remove("customer_image");

            if (!ModelState.IsValid)
                return View(model);

            string userid = HttpContext.Session.GetString("User_Session");

            if (userid == null)
                return RedirectToAction("SignIn");

            int id = int.Parse(userid);

            var customer = _context.Tbl_Customer
                .FirstOrDefault(c => c.customer_id == id);

            if (customer == null)
                return RedirectToAction("Index");

            // ===== TEXT FIELDS UPDATE =====

            customer.customer_name = model.customer_name;
            customer.customer_email = model.customer_email;
            customer.customer_phone = model.customer_phone;
            customer.customer_country = model.customer_country;
            customer.customer_city = model.customer_city;
            customer.customer_address = model.customer_address;
            customer.customer_gender = model.customer_gender;
            customer.customer_password = model.customer_password;


            // ===== IMAGE OPTIONAL UPDATE =====

            if (customer_image != null && customer_image.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "Customer_images");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                // old image delete (except default)
                if (!string.IsNullOrEmpty(customer.customer_image) &&
                    customer.customer_image != "default.png")
                {
                    string oldPath = Path.Combine(folder, customer.customer_image);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(customer_image.FileName);
                string filePath = Path.Combine(folder, fileName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    customer_image.CopyTo(fs);
                }

                customer.customer_image = fileName;
            }

            _context.SaveChanges();

            TempData["success"] = "Profile updated successfully";
            return RedirectToAction("Profile");
        }


        // ====================== Logout  ======================
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User_Session");
            return RedirectToAction("Index");
        }

        // ====================== Cutomer Section End   ====================== 




        // ====================== Navbar Section Start   ====================== 


        // Home Page Start
     
        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                Categories = _context.Tbl_Category.ToList(),   //  categories
                TopProducts = _context.Tbl_Product.Where(p => p.is_top_product).Take(8).ToList() //   products
            };

            return View(model);
        }

        public IActionResult CategoryProducts(int id, int page = 1)
        {
            int pageSize = 12;   // 1 page par kitne products
            int skip = (page - 1) * pageSize;

            // Sirf is category ke products

            var products = _context.Tbl_Product
                .Where(p => p.category_id == id)
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            // Total products count (pagination ke liye)
            int totalProducts = _context.Tbl_Product
                .Count(p => p.category_id == id);

            ViewBag.TotalPages =
                (int)Math.Ceiling((double)totalProducts / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.CategoryId = id;

            return View(products);
        }

        public IActionResult ProductDetails(int id)
        {
            var product = _context.Tbl_Product
                .Include(p => p.category)
                .FirstOrDefault(p => p.product_id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // AddToCart 

        public IActionResult AddToCart(int id)
        {
            // login check

            string userid = HttpContext.Session.GetString("User_Session");
            if (userid == null)
                return RedirectToAction("SignIn");


            int customerId = Convert.ToInt32(userid);

            // product check

            var product = _context.Tbl_Product.Find(id);
            if (product == null)
                return NotFound();

            // check already in cart

            var existingCart = _context.Tbl_Cart
           .FirstOrDefault(c => c.product_id == id
             && c.customer_id == customerId
             && c.product_status == 0);

            if (existingCart != null)
            {
                // quantity +1
                existingCart.product_quantity += 1;
                _context.SaveChanges();
            }
            else
            {
                // new add

                Models.Cart cart = new Models.Cart()
                {
                    product_id = id,
                    customer_id = customerId,
                    product_quantity = 1,
                    product_status = 0
                };

                _context.Tbl_Cart.Add(cart);
            }
            _context.SaveChanges();

            return RedirectToAction("CartPage");

        }

       public IActionResult CartPage ()
            {
           
            string userid = HttpContext.Session.GetString("User_Session");
            if (userid == null)
                return RedirectToAction("SignIn");

            int customerId = Convert.ToInt32(userid);

            var CartItems = _context.Tbl_Cart
                .Include(c => c.product)
                .Include(c => c.customer)
                .Where(c => c.customer_id == customerId && c.product_status==0)
                .ToList();

             return View(CartItems); 
        
        }

        public IActionResult CheckoutAll ()
        {
            string userid = HttpContext.Session.GetString("User_Session");
            if (userid == null)
                return RedirectToAction("SignIn");

            int customerId = Convert.ToInt32(userid);

            var cartItems = _context.Tbl_Cart
             .Include(c => c.product)
             .Include(c => c.customer)
             .Where(c => c.customer_id == customerId && c.product_status == 0)
             .ToList();

            // customer
            var customer = _context.Tbl_Customer
                .FirstOrDefault(c => c.customer_id == customerId);

            ViewBag.cart = cartItems;

            return View(customer);  // checkout page open
        }


        public IActionResult ConfirmOrderAll()
        {
            string userid = HttpContext.Session.GetString("User_Session");
            if (userid == null)
                return RedirectToAction("SignIn");

            int customerId = Convert.ToInt32(userid);

            var customer = _context.Tbl_Customer
             .FirstOrDefault(c => c.customer_id == customerId
             );
            if (customer == null)
                return NotFound();


            var cartItems = _context.Tbl_Cart
                .Where(c => c.customer_id == customerId && c.product_status == 0)
                .ToList();

            foreach (var item in cartItems)
            {
                Order order = new Order()
                {
                    product_id = item.product_id,
                    customer_id = customerId,
                    quantity = item.product_quantity,
                    order_date = DateTime.Now
                };

                _context.Tbl_Order.Add(order);

                // cart empty
                item.product_status = 1;
            }

            _context.SaveChanges();

            return RedirectToAction("ThankYou");
        }

        // Increase qty
        public IActionResult IncreaseQty(int id)
        {
            var cart = _context.Tbl_Cart.Find(id);
            if (cart != null)
            {
                cart.product_quantity += 1;
                _context.SaveChanges();
            }

            return RedirectToAction("CartPage");
        }

        // Decrease qty
        public IActionResult DecreaseQty(int id)
        {
            var cart = _context.Tbl_Cart.Find(id);
            if (cart != null)
            {
                if (cart.product_quantity > 1)
                    cart.product_quantity -= 1;

                _context.SaveChanges();
            }

            return RedirectToAction("CartPage");
        }



        public IActionResult RemoveCart(int id)
        {
            var cart = _context.Tbl_Cart.Find(id);
            _context.Tbl_Cart.Remove(cart);
            _context.SaveChanges();

            return RedirectToAction("CartPage");
        }


        // BuyNow 
        public IActionResult BuyNow(int id)
        {
            string userid = HttpContext.Session.GetString("User_Session");
            if (userid == null)
                return RedirectToAction("SignIn");

            int customerId = Convert.ToInt32(userid);


            var product = _context.Tbl_Product
                .Include(p => p.category)
                .FirstOrDefault(p => p.product_id == id);

            if (product == null)
            {
                return NotFound();
            }
        ;

            var customer = _context.Tbl_Customer.FirstOrDefault(c => c.customer_id == customerId);

            if (customer == null)
            {
                return NotFound();
            }

            ViewBag.product = product; // product details ke liye view me bhej rahe hain
                                       // viewbag use kren ge udher BuyNow per

            return View(customer);
        }


        public IActionResult ConfirmOrder(int product_id, int quantity)
        {
            //  login check
            string userid = HttpContext.Session.GetString("User_Session");
            if (userid == null)
                return RedirectToAction("SignIn");

            int customerId = Convert.ToInt32(userid);

            //  product check

            var product = _context.Tbl_Product
                .FirstOrDefault(p => p.product_id == product_id);

            if (product == null)
                return NotFound();

                
            //  customer profile fetch
             
            var customer = _context.Tbl_Customer
                .FirstOrDefault(c => c.customer_id == customerId
                );

            if (customer == null)
                return NotFound();

            //  quantity check

            if (quantity <= 0)
                quantity = 1;

            //  order save

            Order order = new Order()
            {
                product_id = product_id,
                customer_id = customerId,
                quantity = quantity,
                order_date = DateTime.Now
            };

            _context.Tbl_Order.Add(order);
            _context.SaveChanges();

            return RedirectToAction("ThankYou");
        }

        public IActionResult ThankYou()
        {
            return View();
        }

        // Check Cart for header cart icon 

        public IActionResult CheckCart()
        {
            string userid = HttpContext.Session.GetString("User_Session");
            if (userid == null)
              return RedirectToAction("SignIn");

            int customerId = Convert.ToInt32(userid);

            //check pending cart items

            var Pendingcart = _context.Tbl_Cart
                .Where(c => c.customer_id == customerId && c.product_status == 0)
                .ToList();

            if (Pendingcart.Count > 0)
            {
                return RedirectToAction("CartPage");
            }

            //cart empty → check order exist

            var orders = _context.Tbl_Order
                .Where(o => o.customer_id == customerId)
                .OrderByDescending(o => o.order_date)
                .ToList();

            if (orders.Count > 0)
            {
                return RedirectToAction("MyOrders"); // order page
            }

            // default
            return RedirectToAction("CartPage");
        }

        //  Order section 

        public IActionResult FetchOrders()
        {
            string userid = HttpContext.Session.GetString("User_Session");
            if (userid == null)
                return RedirectToAction("SignIn");

            int customerId = Convert.ToInt32(userid);

            var orders = _context.Tbl_Order
                .Include(o => o.product)
                .Include(o => o.customer)
                .OrderByDescending(o => o.order_date)
                .ToList();

            return View(orders);

        }

        // Home Page End   



        //  Abous us Page  Start
            public IActionResult AboutUs()
            {
                return View();
            }

        //  Abous us Page End

        //Shop Page Start

        public IActionResult Shop(int? id, string sort, string searchTerm)
        {
            var products = _context.Tbl_Product.AsQueryable();

            // CATEGORY FILTER

            if (id != null)
            {
                products = products.Where(p => p.category_id == id);
            }


            // 2️⃣ SEARCH FILTER
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.product_name.Contains(searchTerm));
            }

            // SORTING

            if (sort == "low")
            {
                products = products.OrderBy(p => p.product_price);
            }
            else if (sort == "high")
            {
                products = products.OrderByDescending(p => p.product_price);
            }
            else // default latest
            {
                products = products.OrderByDescending(p => p.product_id);
            }


            var productList = products.ToList();

            if (!productList.Any())
            {
                ViewBag.EmptyMessage = "No products found in this category.";
            }

            var totalProducts = products.Count();

            var ShopViewModel = new ShopViewModel
            {
                Categories = _context.Tbl_Category.ToList(),
                Products = products.ToList(),
                SelectedCategoryId = id,
                TotalProducts = totalProducts
            };

            return View(ShopViewModel);
        }


        // Shop Page End

        public IActionResult Experience()
        {
            ExperienceViewModel vm = new ExperienceViewModel();

            vm.GamingProducts = _context.Tbl_Product
                .Where(p => p.show_in_experience == true
                         && p.ExperienceType == "Gaming")
                .OrderBy(p => p.ExperienceOrder)
                .ToList();

            vm.WorkProducts = _context.Tbl_Product
                .Where(p => p.show_in_experience == true
                         && p.ExperienceType == "WorkFromHome")
                .OrderBy(p => p.ExperienceOrder)
                .ToList();

            vm.SmartProducts = _context.Tbl_Product
                .Where(p => p.show_in_experience == true
                         && p.ExperienceType == "SmartLiving")
                .OrderBy(p => p.ExperienceOrder)
                .ToList();

            vm.EntertainmentProducts = _context.Tbl_Product
                .Where(p => p.show_in_experience == true
                         && p.ExperienceType == "Entertainment")
                .OrderBy(p => p.ExperienceOrder)
                .ToList();

            return View(vm);
        }

        [HttpPost]
        public IActionResult AddExperienceOrder(List<int> SelectedProducts)
        {
            string userid = HttpContext.Session.GetString("User_Session");
            if (userid == null)
                return RedirectToAction("SignIn");

            int customerId = Convert.ToInt32(userid);

            if (SelectedProducts == null || !SelectedProducts.Any())
            {
                TempData["Error"] = "Please select at least one product!";
                return RedirectToAction("Experience");
            }

            // Add all selected products to cart (status 0 = pending)
            foreach (var prodId in SelectedProducts)
            {
                var existingCart = _context.Tbl_Cart
                    .FirstOrDefault(c => c.product_id == prodId
                        && c.customer_id == customerId
                        && c.product_status == 0);

                if (existingCart != null)
                {
                    existingCart.product_quantity += 1;
                }
                else
                {
                    _context.Tbl_Cart.Add (new Models.Cart
                    {
                        product_id = prodId,
                        customer_id = customerId,
                        product_quantity = 1,
                        product_status = 0
                    });
                }
            }

            _context.SaveChanges();

            // Redirect to cart page
            return RedirectToAction("CartPage");
        }



        // Contact us Page Start

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult SendFeedback (Models.Feedback feedback) 
        {

            string userid = HttpContext.Session.GetString("User_Session");
            if (userid == null)
                return RedirectToAction("SignIn");


            int customerId = Convert.ToInt32(userid);

            feedback.customer_id = customerId;

            _context.Tbl_Feedback.Add(feedback);
            _context.SaveChanges();
             TempData["success"] = "Feedback sent successfully!";

            return RedirectToAction("ContactUs");
        }

        // Contact us page end 


        // ====================== Navbar Section End   ====================== 





    }
}
