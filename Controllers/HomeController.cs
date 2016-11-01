using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        public HomeController(MyContext context)
        {
            _context = context;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.loggedin = false;
            if (HttpContext.Session.GetInt32("id") != null) {
                ViewBag.loggedin = true;
            }
            return View();
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login() {
            if (HttpContext.Session.GetInt32("id") != null) {
                int id = (int)HttpContext.Session.GetInt32("id");
                var user = _context.Users.SingleOrDefault(u => u.id == id);
                if (user.user_level == 9) {
                    return RedirectToAction("Dashboard", "Admin");
                } else {
                    return RedirectToAction("Dashboard", "User");
                }
            }
            ViewBag.loggedin = false;
            ViewBag.invalid = false;
            if (TempData["invalid"] != null && (bool)TempData["invalid"]) {
                ViewBag.invalid = true;
            }
            return View();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string email, string password) {
            var user = _context.Users.SingleOrDefault(u => u.email == email);
            if (user == null) {
                TempData["invalid"] = true;
                return RedirectToAction("Login");
            } else {
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(user, user.password, password)) {
                    HttpContext.Session.SetInt32("id", user.id);
                    if (user.user_level == 9) {
                        return RedirectToAction("Dashboard", "Admin");
                    } else {
                        return RedirectToAction("Dashboard", "User");
                    }
                } else {
                    TempData["invalid"] = true;
                    return RedirectToAction("Login");
                }
            }
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register() {
            if (HttpContext.Session.GetInt32("id") != null) {
                int id = (int)HttpContext.Session.GetInt32("id");
                var user = _context.Users.SingleOrDefault(u => u.id == id);
                if (user.user_level == 9) {
                    return RedirectToAction("Dashboard", "Admin");
                } else {
                    return RedirectToAction("Dashboard", "User");
                }
            }
            ViewBag.loggedin = false;
            ViewBag.invalidEmail = false;
            return View();
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(User user) {
            var sameEmail = _context.Users.Where(u => u.email == user.email);
            ViewBag.invalidEmail = false;
            if (sameEmail.Count() != 0) {
                ViewBag.invalidEmail = true;
            }
            if (ModelState.IsValid && sameEmail.Count() == 0) {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.password = Hasher.HashPassword(user, user.password);
                user.confirm = user.password;
                if (_context.Users.Count() == 0) {
                    user.user_level = 9;
                    user.created_at = DateTime.Now;
                    user.updated_at = DateTime.Now;
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    var userSaved = _context.Users.SingleOrDefault(u => u.email == user.email);
                    HttpContext.Session.SetInt32("id", userSaved.id);
                    return RedirectToAction("Dashboard", "Admin");
                } else {
                    user.user_level = 0;
                    user.created_at = DateTime.Now;
                    user.updated_at = DateTime.Now;
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    var userSaved = _context.Users.SingleOrDefault(u => u.email == user.email);
                    HttpContext.Session.SetInt32("id", userSaved.id);
                    return RedirectToAction("Dashboard", "User");
                }
            } else {
                ViewBag.loggedin = false;
                return View("register", user);
            }
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
