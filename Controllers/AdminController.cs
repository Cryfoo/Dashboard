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
    public class AdminController : Controller
    {
        private MyContext _context;

        public AdminController(MyContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("dashboard/admin")]
        public IActionResult Dashboard() {
            if (HttpContext.Session.GetInt32("id") != null) {
                int id = (int)HttpContext.Session.GetInt32("id");
                var user = _context.Users.SingleOrDefault(u => u.id == id);
                if (user.user_level != 9) {
                    return RedirectToAction("Dashboard", "User");
                }
            } else {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.loggedin = true;
            return View(_context.Users);
        }

        [HttpGet]
        [Route("users/new")]
        public IActionResult New() {
            if (HttpContext.Session.GetInt32("id") != null) {
                int id = (int)HttpContext.Session.GetInt32("id");
                var user = _context.Users.SingleOrDefault(u => u.id == id);
                if (user.user_level != 9) {
                    return RedirectToAction("Dashboard", "User");
                }
            } else {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.loggedin = true;
            ViewBag.invalidEmail = false;
            return View();
        }

        [HttpPost]
        [Route("users/new")]
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
                user.user_level = 0;
                user.created_at = DateTime.Now;
                user.updated_at = DateTime.Now;
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            } else {
                ViewBag.loggedin = true;
                return View("New", user);
            }
        }

        [HttpGet]
        [Route("users/edit/{edit_id}")]
        public IActionResult Edit(int edit_id) {
            if (HttpContext.Session.GetInt32("id") != null) {
                int id = (int)HttpContext.Session.GetInt32("id");
                var user = _context.Users.SingleOrDefault(u => u.id == id);
                if (user.user_level != 9) {
                    return RedirectToAction("Dashboard", "User");
                }
            } else {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.loggedin = true;
            ViewBag.invalidEmail = false;
            return View(_context.Users.SingleOrDefault(u => u.id == edit_id));
        }

        [HttpPost]
        [Route("users/edit/{edit_id}/info")]
        public IActionResult UpdateInfo(int edit_id, User user) {
            var oldUser = _context.Users.SingleOrDefault(u => u.id == edit_id);
            var sameEmail = _context.Users.Where(u => u.email != oldUser.email).Where(u => u.email == user.email);
            ViewBag.invalidEmail = false;
            if (sameEmail.Count() != 0) {
                ViewBag.invalidEmail = true;
            }
            user.id = oldUser.id;
            if (ModelState.IsValid && sameEmail.Count() == 0) {
                oldUser.email = user.email;
                oldUser.first_name = user.first_name;
                oldUser.last_name = user.last_name;
                oldUser.user_level = user.user_level;
                oldUser.updated_at = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            } else {
                ViewBag.loggedin = true;
                return View("Edit", user);
            }
        }

        [HttpPost]
        [Route("users/edit/{edit_id}/pwd")]
        public IActionResult UpdatePwd(int edit_id, User user) {
            ViewBag.invalidEmail = false;
            var oldUser = _context.Users.SingleOrDefault(u => u.id == edit_id);
            user.id = oldUser.id;
            user.email = oldUser.email;
            user.first_name = oldUser.first_name;
            user.last_name = oldUser.last_name;
            user.user_level = oldUser.user_level;
            ModelState["email"].Errors.Clear();
            ModelState["first_name"].Errors.Clear();
            ModelState["last_name"].Errors.Clear();
            string pwError = ModelState["password"].ValidationState.ToString();
            string cpError = ModelState["confirm"].ValidationState.ToString();            
            if (pwError == "Valid" && cpError == "Valid") {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                oldUser.password = Hasher.HashPassword(user, user.password);
                oldUser.confirm = oldUser.password;
                oldUser.updated_at = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            } else {
                ViewBag.loggedin = true;
                return View("Edit", user);
            }
        }

        [HttpGet]
        [Route("users/{delete_id}")]
        public IActionResult Delete(int delete_id) {
            if (HttpContext.Session.GetInt32("id") != null) {
                int id = (int)HttpContext.Session.GetInt32("id");
                var user = _context.Users.SingleOrDefault(u => u.id == id);
                if (user.user_level != 9) {
                    return RedirectToAction("Dashboard", "User");
                }
            } else {
                return RedirectToAction("Index", "Home");
            }
            var deleteUser = _context.Users.SingleOrDefault(u => u.id == delete_id);
            _context.Users.Remove(deleteUser);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}
