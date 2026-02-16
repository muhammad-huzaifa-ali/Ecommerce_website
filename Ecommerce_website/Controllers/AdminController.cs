using Ecommerce_website.Migrations;
using Ecommerce_website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace Ecommerce_website.Controllers
{
    public class AdminController : Controller
    {
        private readonly MyContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(MyContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ================= Admin Dashboard =================
        public IActionResult Index()
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
            {
                return RedirectToAction("Admin_Login");
            }
            return View();
        }

        // ================= Admin Login / Logout =================
        public IActionResult Admin_Login() => View();

        [HttpPost]
        public IActionResult Admin_Login(string Adminemail, string Adminpassword)
        {
            var admin = _context.Tbl_Admin.FirstOrDefault(a => a.admin_email == Adminemail &&
            a.admin_password == Adminpassword);

            if (admin != null)
            {
                HttpContext.Session.SetString("Admin_Session", admin.admin_id.ToString());
                return RedirectToAction("Index");
            }
            ViewBag.message = "Invalid Email or Password";
            return View();
        }

        public IActionResult Admin_Logout()
        {
            HttpContext.Session.Remove("Admin_Session");
            return RedirectToAction("Admin_Login");
        }

        // ================= Admin Profile =================
        public IActionResult Profile()
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
            {
                return RedirectToAction("Admin_Login");
            }
            int adminId = int.Parse(adminSession);
            var admin = _context.Tbl_Admin.FirstOrDefault(a => a.admin_id == adminId);
            return View(admin);
        }

        [HttpPost]
        public IActionResult Profile(Models.Admin model)
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
            {
                return RedirectToAction("Admin_Login");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            int adminId = int.Parse(adminSession);
            var admin = _context.Tbl_Admin.FirstOrDefault(a => a.admin_id == adminId);
            if (admin == null) return RedirectToAction("Admin_Login");

            admin.admin_name = model.admin_name;
            admin.admin_email = model.admin_email;
            admin.admin_password = model.admin_password;

            _context.Tbl_Admin.Update(admin);
            _context.SaveChanges();

            ViewBag.message = "Profile Updated Successfully";
            return View(admin);
        }

        [HttpPost]
        public IActionResult Admin_image(IFormFile admin_image)
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");

            if (adminSession == null)
                return RedirectToAction("Admin_Login");

            if (admin_image == null || admin_image.Length == 0)
                return RedirectToAction("Profile");

            int adminId = int.Parse(adminSession);
            var admin = _context.Tbl_Admin.FirstOrDefault(a => a.admin_id == adminId);

            if (admin == null)
                return RedirectToAction("Admin_Login");

            string folderPath = Path.Combine(_env.WebRootPath, "admin_image");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            //  STEP 1: Purani image delete
            if (!string.IsNullOrEmpty(admin.admin_image))
            {
                string oldImagePath = Path.Combine(folderPath, admin.admin_image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            //  STEP 2: Nayi image save
            string fileName = Guid.NewGuid() + Path.GetExtension(admin_image.FileName);
            string newImagePath = Path.Combine(folderPath, fileName);

            using (FileStream fs = new FileStream(newImagePath, FileMode.Create))
            {
                admin_image.CopyTo(fs);
            }

            //  STEP 3: DB update
            admin.admin_image = fileName;
            _context.SaveChanges();

            return RedirectToAction("Profile");
        }

        // ================= Customer Section Start =================
        public IActionResult fetchCustomer()
        {
            var customers = _context.Tbl_Customer.ToList();
            return View(customers);
        }

        public IActionResult CutomerDetails(int id)
        {
            var customer = _context.Tbl_Customer.FirstOrDefault(c => c.customer_id == id);
            return View(customer);
        }

        public IActionResult EditCustomer(int id)
        {
            var customer = _context.Tbl_Customer.Find(id);
            return View(customer);
        }

        [HttpPost]
        public IActionResult EditCustomer(Models.Customer model)
        {
            ModelState.Remove("customer_email");
            ModelState.Remove("customer_password");
            ModelState.Remove("customer_image");

            if (!ModelState.IsValid) return View(model);

            var customer = _context.Tbl_Customer.FirstOrDefault(c => c.customer_id == model.customer_id);
            if (customer == null) return RedirectToAction("fetchCustomer");

            customer.customer_name = model.customer_name;
            customer.customer_phone = model.customer_phone;
            customer.customer_country = model.customer_country;
            customer.customer_city = model.customer_city;
            customer.customer_address = model.customer_address;
            customer.customer_gender = model.customer_gender;

            _context.Tbl_Customer.Update(customer);
            _context.SaveChanges();

            ViewBag.message = "Customer Updated Successfully";
            return RedirectToAction("FetchCustomer");
        }



        public IActionResult DeleteCustomer(int id)
        {
            var customer = _context.Tbl_Customer.FirstOrDefault(c => c.customer_id == id);
            if (customer != null)
            {
                _context.Tbl_Customer.Remove(customer);
                _context.SaveChanges();
            }
            return RedirectToAction("fetchCustomer");
        }

        // =================== Customer section End ===================

        // =================== Category section Start ===================

        public IActionResult FetchCategory()
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
                return RedirectToAction("Admin_Login");

            var categories = _context.Tbl_Category.ToList();
            return View(categories);
        }

        public IActionResult AddCategory()
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
                return RedirectToAction("Admin_Login");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(Models.Category category)
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
                return RedirectToAction("Admin_Login");

            if (!ModelState.IsValid)
                return View(category);

            _context.Tbl_Category.Add(category);
            _context.SaveChanges();

            TempData["message"] = "Category added successfully!";
            return RedirectToAction("AddCategory");
        }

        public IActionResult EditCategory(int id)
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
                return RedirectToAction("Admin_Login");

            var category = _context.Tbl_Category.Find(id);
            if (category == null)
                return RedirectToAction("FetchCategory");

            return View(category);
        }

        [HttpPost]
        public IActionResult EditCategory(Models.Category category)
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
                return RedirectToAction("Admin_Login");

            if (!ModelState.IsValid)
                return View(category);

            _context.Tbl_Category.Update(category);
            _context.SaveChanges();

            TempData["message"] = "Category updated successfully!";
            return RedirectToAction("EditCategory");
        }

        public IActionResult DeleteCategory(int id)
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
                return RedirectToAction("Admin_Login");

            var category = _context.Tbl_Category.Find(id);
            if (category != null)
            {
                _context.Tbl_Category.Remove(category);
                _context.SaveChanges();
                TempData["message"] = "Category deleted successfully!";
            }

            return RedirectToAction("FetchCategory");
        }

        // =================== Category section End ===================

        // =================== Product section Start ===================

        // FetchProduct

        public IActionResult FetchProduct()
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
                return RedirectToAction("Admin_Login");


            var products = _context.Tbl_Product.Include(p => p.category).ToList();

            return View(products);
        }

        public IActionResult ProductDetails(int id)
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
                return RedirectToAction("Admin_Login");

            var product = _context.Tbl_Product
                .Include(p => p.category)
                .FirstOrDefault(p => p.product_id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }


        //  ADD (GET) 
        public IActionResult AddProduct()
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
                return RedirectToAction("Admin_Login");

            ViewBag.CategoryList = new SelectList(
                _context.Tbl_Category,
                "category_id",
                "category_name"
            );
            return View();
        }

        //  ADD (Post) 


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(Models.Product product)
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
                return RedirectToAction("Admin_Login");

         
            ModelState.Remove("ExperienceType");
            ModelState.Remove("ExperienceOrder");

            if (string.IsNullOrEmpty(product.ExperienceType))
                product.ExperienceType = "None"; // ya "None"

            if (product.ExperienceOrder == null || product.ExperienceOrder == 0)
                product.ExperienceOrder = 1;  // default order


            if (!ModelState.IsValid)
                return View(product);



            var file = Request.Form.Files["product_image"];

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("product_image", "Product image is required");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.CategoryList = new SelectList(
                    _context.Tbl_Category,
                    "category_id",
                    "category_name"
                );
                return View(product);
            }

            string folder = Path.Combine(_env.WebRootPath, "Product_images");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            product.product_image = fileName;

            _context.Tbl_Product.Add(product);
            _context.SaveChanges();

            return RedirectToAction("FetchProduct");

        }


        //  EDIT (GET) 
        public IActionResult EditProduct(int id)
        {

            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
                return RedirectToAction("Admin_Login");

            var product = _context.Tbl_Product.Find(id);
            if (product == null)
                return NotFound();

            ViewBag.CategoryList = new SelectList(
                _context.Tbl_Category,
                "category_id",
                "category_name",
                product.category_id
            );

            return View(product);
        }

        //  EDIT (POST) 

        [HttpPost]
        public IActionResult EditProduct(Models.Product product, IFormFile imgfile)
        {

            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
                return RedirectToAction("Admin_Login");

            ModelState.Remove("product_image");
            ModelState.Remove("imgfile");

            if (!ModelState.IsValid)
                return View(product);

            var oldProduct = _context.Tbl_Product
                .AsNoTracking()
                .FirstOrDefault(p => p.product_id == product.product_id);

            if (oldProduct == null)
                return RedirectToAction("FetchProduct");

            if (!ModelState.IsValid)
            {
                ViewBag.CategoryList = new SelectList(
                    _context.Tbl_Category,
                    "category_id",
                    "category_name",
                    product.category_id
                );
                return View(product);
            }

            if (imgfile != null && imgfile.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "Product_images");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(imgfile.FileName);
                string path = Path.Combine(folder, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    imgfile.CopyTo(stream);
                }

                // delete old image
                if (!string.IsNullOrEmpty(oldProduct.product_image))
                {
                    string oldPath = Path.Combine(folder, oldProduct.product_image);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                product.product_image = fileName;
            }
            else
            {
                product.product_image = oldProduct.product_image;
            }

            _context.Tbl_Product.Update(product);
            _context.SaveChanges();

            return RedirectToAction("FetchProduct");
        }


        //  DeleteProduct  

        public IActionResult DeleteProduct(int id)
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
                return RedirectToAction("Admin_Login");

            var product = _context.Tbl_Product.Find(id);
            if (product != null)
            {
                _context.Tbl_Product.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("FetchProduct");
        }

        // =================== Product section End ===================


        // =================== Order section Start ===================


        public IActionResult FetchOrders()
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
                return RedirectToAction("Admin_Login");

            var orders = _context.Tbl_Order
                .Include(o => o.product)
                .Include(o => o.customer)
                .ToList();

            return View(orders);
        }



        public IActionResult OrderDetails(int id)
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
                return RedirectToAction("Admin_Login");

            var order = _context.Tbl_Order
                .Include(o => o.product)
                .Include(o => o.customer)
                .FirstOrDefault(o => o.order_id == id);

            return View(order);
        }


        public IActionResult DeleteOrder(int id)
        {
            var order = _context.Tbl_Order.Find(id);

            if (order != null)
            {
                _context.Tbl_Order.Remove(order);
                _context.SaveChanges();
            }

            return RedirectToAction("Orders");
        }

        // CLEAR ALL ORDERS
        public IActionResult ClearAllOrders()
        {
            var orders = _context.Tbl_Order.ToList();

            _context.Tbl_Order.RemoveRange(orders);
            _context.SaveChanges();

            return RedirectToAction("FetchOrders");
        }

        // =================== Order section End ===================


        public IActionResult Feedback()
        {
            string adminSession = HttpContext.Session.GetString("Admin_Session");
            if (adminSession == null)
                return RedirectToAction("Admin_Login");


            var feedbacks = _context.Tbl_Feedback.ToList();

            return View(feedbacks);



        }

        public IActionResult DeleteFeedback(int id)
        {
            var feedback = _context.Tbl_Feedback.Find(id);

            if (feedback != null)
            {
                _context.Tbl_Feedback.Remove(feedback);
                _context.SaveChanges();
            }

            return RedirectToAction("Feedback");

        }
    }
}


