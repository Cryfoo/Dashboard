using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Models;
using System.Linq;
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
            return View();
        }

        [HttpGet]
        [Route("users/edit")]
        public IActionResult Edit() {
           ViewBag.loggedin = true;
           return View();
        }
    }
}
