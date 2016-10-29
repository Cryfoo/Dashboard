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
    public class UserController : Controller
    {
        private MyContext _context;

        public UserController(MyContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard() {
            if (HttpContext.Session.GetInt32("id") != null) {
                int id = (int)HttpContext.Session.GetInt32("id");
                var user = _context.Users.SingleOrDefault(u => u.id == id);
                if (user.user_level == 9) {
                    return RedirectToAction("Dashboard", "Admin");
                }
            } else {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.loggedin = true;
            return View(_context.Users);
        }

        [HttpGet]
        [Route("users/edit")]
        public IActionResult Edit() {
            if (HttpContext.Session.GetInt32("id") == null) {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.loggedin = true;
            ViewBag.invalidEmail = false;
            int id = (int)HttpContext.Session.GetInt32("id");
            return View(_context.Users.SingleOrDefault(u => u.id == id));
        }

        [HttpPost]
        [Route("users/edit/info")]
        public IActionResult UpdateInfo(User user) {
            int id = (int)HttpContext.Session.GetInt32("id");
            var oldUser = _context.Users.SingleOrDefault(u => u.id == id);
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
                oldUser.updated_at = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            } else {
                ViewBag.loggedin = true;
                return View("Edit", user);
            }
        }

        [HttpPost]
        [Route("users/edit/pwd")]
        public IActionResult UpdatePwd(User user) {
            ViewBag.invalidEmail = false;
            int id = (int)HttpContext.Session.GetInt32("id");
            var oldUser = _context.Users.SingleOrDefault(u => u.id == id);
            user.id = oldUser.id;
            user.email = oldUser.email;
            user.first_name = oldUser.first_name;
            user.last_name = oldUser.last_name;
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

        [HttpPost]
        [Route("users/edit/desc")]
        public IActionResult UpdateDesc(User user) {
            int id = (int)HttpContext.Session.GetInt32("id");
            var oldUser = _context.Users.SingleOrDefault(u => u.id == id);
            oldUser.description = user.description;
            oldUser.updated_at = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("users/show/{id}")]
        public IActionResult Show(int id) {
            ViewBag.loggedin = true;
            ViewBag.user = _context.Users.SingleOrDefault(u => u.id == id);
            return View();
        }

        [HttpPost]
        [Route("users/createMsg")]
        public IActionResult CreateMessage(string msg, int id) {
            int current_id = (int)HttpContext.Session.GetInt32("id");
            var currentUser = _context.Users.SingleOrDefault(u => u.id == current_id);
            Message newMessage = new Message {
                message = msg,
                creator.id = (int)currentUser.id,
                recipient.id = id,
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };
            _context.Messages.Add(newMessage);
            _context.SaveChanges();
            return RedirectToAction("Show", new {id = id});
        }
    }
}
