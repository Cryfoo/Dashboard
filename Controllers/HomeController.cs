using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Models;
using System.Linq;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.loggedin = false;
            return View();
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login() {
            ViewBag.loggedin = false;
            return View();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string email, string password) {
            Console.WriteLine(email);
            Console.WriteLine(password);
            return RedirectToAction("Dashboard", "Admin");
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register() {
            ViewBag.loggedin = false;
            return View();
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(User user) {
            Console.WriteLine(user.email);
            Console.WriteLine(user.first_name);
            Console.WriteLine(user.last_name);
            Console.WriteLine(user.password);
            Console.WriteLine(user.confirm);
            return RedirectToAction("Dashboard", "Admin");
        }
    }
}
