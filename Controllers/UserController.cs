using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Models;
using System.Linq;

namespace Dashboard.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard() {
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
