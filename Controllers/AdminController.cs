using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Models;
using System.Linq;

namespace Dashboard.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard() {
            ViewBag.loggedin = true;
            return View();
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout() {
           return RedirectToAction("Index", "Home");
        }
    }
}
