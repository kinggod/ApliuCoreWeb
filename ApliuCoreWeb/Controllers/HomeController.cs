﻿using ApliuCoreWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ApliuCoreWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TestView()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }
    }
}
