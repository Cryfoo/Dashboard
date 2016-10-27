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
        [Route("dashboard/admin")]
        public IActionResult Dashboard() {
            ViewBag.loggedin = true;
            return View();
        }

        [HttpGet]
        [Route("users/new")]
        public IActionResult New() {
            ViewBag.loggedin = true;
            return View();
        }

        [HttpGet]
        [Route("users/show/1")]
        public IActionResult Show() {
           ViewBag.loggedin = true;
           return View();
        }

        [HttpGet]
        [Route("users/edit/1")]
        public IActionResult Edit() {
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
